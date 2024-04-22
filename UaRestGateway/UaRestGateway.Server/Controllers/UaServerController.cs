using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Opc.Ua;
using UaRestGateway.Server.Service;
using StatusCodes = Opc.Ua.StatusCodes;
using ISession = Opc.Ua.Client.ISession;
using UaRestGateway.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Opc.Ua.Server;
using System.IO.Compression;

namespace UaRestGateway.Server.Controllers
{
    [ApiController]
    [Route("opcua")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UaServerController : CommonController
    {
        private readonly object m_lock = new();
        private readonly IMemoryCache m_cache;
        private readonly IUACommunicationService m_communicationService;
        private readonly Dictionary<string, ISession> m_sessions = new ();
        private NodeId m_authenticationToken;

        public UaServerController(
            IConfiguration configuration,
            ILogger<UaServerController> logger,
            DatabaseContext context,
            IMemoryCache cache,
            IUACommunicationService communicationService)
        :
            base(configuration, logger, context)
        {
            m_cache = cache;
            m_communicationService = communicationService;
        }

        class CacheSessions
        {
            public string SecureChannelId { get; set; }

            public Dictionary<string, string> AuthenticationTokens { get; set; }
        }

        private static async Task<byte[]> Compress(MemoryStream istrm)
        {
            using (var ostrm = new MemoryStream())
            {
                using (var zstrm = new GZipStream(ostrm, CompressionMode.Compress))
                {
                    await istrm.CopyToAsync(zstrm);
                    zstrm.Close();
                    return ostrm.ToArray();
                }
            }
        }

        private static async Task<MemoryStream> Decompress(Stream istrm)
        {
            using (var zstrm = new GZipStream(istrm, CompressionMode.Decompress))
            {
                var ostrm = new MemoryStream();
                await zstrm.CopyToAsync(ostrm);
                ostrm.Position = 0;
                return ostrm;
            }
        }

        private bool IsRequestCompressed()
        {
            if (Request.Headers.TryGetValue("Content-Encoding", out var header))
            {
                var token = String.Join(" ", header).Trim();

                if (token.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<T> Decode<T>(IServiceMessageContext context) where T : IEncodeable, IServiceRequest, new()
        {
            Stream stream = Request.Body;

            if (IsRequestCompressed())
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    stream = await Decompress(stream);
                }
            }

            using (var reader = new StreamReader(stream))
            {
                var json = await reader.ReadToEndAsync();

                using (var decoder = new JsonDecoder(json, context))
                {
                    decoder.UpdateNamespaceTable = true;
                    SessionLessServiceMessage message = new SessionLessServiceMessage();

                    // message.NamespaceUris = new NamespaceTable(decoder.ReadStringArray(nameof(SessionLessServiceMessage.NamespaceUris)));
                    // message.ServerUris = new StringTable(decoder.ReadStringArray(nameof(SessionLessServiceMessage.ServerUris)));
                    message.LocaleIds = new StringTable(decoder.ReadStringArray(nameof(SessionLessServiceMessage.LocaleIds)));
                    decoder.SetMappingTables(message.NamespaceUris, message.ServerUris);
                    message.Message = decoder.ReadEncodeable("Body", typeof(T));

                    var request = (T)message.Message;

                    if (NodeId.IsNull(request.RequestHeader.AuthenticationToken))
                    {
                        request.RequestHeader.AuthenticationToken = m_authenticationToken;
                    }

                    return request;
                }
            }
        }

        private async Task<IActionResult> Encode<T>(IServiceMessageContext context, T response) where T : IEncodeable
        {
            var stream = new MemoryStream();

            using (var encoder = new JsonEncoder(context, true, stream: stream, leaveOpen: true))
            {
                encoder.UseStringNodeIds = true;
                encoder.ForceNamespaceUri = true;
                encoder.ForceNamespaceUriForIndex1 = true;

                // encoder.WriteStringArray(nameof(SessionLessServiceMessage.NamespaceUris), context.NamespaceUris.ToArray());
                // encoder.WriteStringArray(nameof(SessionLessServiceMessage.ServerUris), context.ServerUris.ToArray());
                encoder.WriteStringArray(nameof(SessionLessServiceMessage.LocaleIds), null);
                encoder.WriteEncodeable("Body", response, typeof(T));

                encoder.Close();
                stream.Position = 0;

                if (IsRequestCompressed())
                {
                    Response.Headers.Append("Content-Encoding", "gzip");
                    var compressed = await Compress(stream);
                    return File(compressed, "application/json");
                }

                return File(stream, "application/json");
            }
        }

        private async Task<IActionResult> Fault(Exception e)
        {
            Logger.LogWarning(e, "Fault calling Service.");

            IServiceMessageContext context = m_communicationService?.ServerApi?.MessageContext ?? ServiceMessageContext.GlobalContext;

            using (var encoder = new JsonEncoder(context, true))
            {
                encoder.UseStringNodeIds = true;
                encoder.ForceNamespaceUri = true;
                encoder.ForceNamespaceUriForIndex1 = true;

                var sr = new ServiceResult(e, StatusCodes.BadUnexpectedError);

                ServiceFault fault = new ServiceFault()
                {
                    ResponseHeader = new ResponseHeader()
                    {
                        Timestamp = DateTime.UtcNow,
                        ServiceResult = sr.StatusCode,
                        StringTable = new StringCollection(new string[] { sr.ToString() }),
                        ServiceDiagnostics = new DiagnosticInfo() { LocalizedText = 0 }
                    }
                };

                return await Encode<ServiceFault>(context, fault);
            }
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<ReadRequest>(server.MessageContext);

                if (NodeId.IsNull(request.RequestHeader.AuthenticationToken))
                {
                    request.RequestHeader.AuthenticationToken = m_authenticationToken;
                }

                var responseHeader = server.Read(
                    request.RequestHeader,
                    request.MaxAge,
                    request.TimestampsToReturn,
                    request.NodesToRead,
                    out DataValueCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new ReadResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("write")]
        public async Task<IActionResult> Write()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<WriteRequest>(server.MessageContext);

                var responseHeader = server.Write(
                    request.RequestHeader,
                    request.NodesToWrite,
                    out StatusCodeCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new WriteResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("call")]
        public async Task<IActionResult> Call()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<CallRequest>(server.MessageContext);

                var responseHeader = server.Call(
                    request.RequestHeader,
                    request.MethodsToCall,
                    out CallMethodResultCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new CallResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("browse")]
        public async Task<IActionResult> Browse()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<BrowseRequest>(server.MessageContext);

                var responseHeader = server.Browse(
                    request.RequestHeader,
                    request.View,
                    request.RequestedMaxReferencesPerNode,
                    request.NodesToBrowse,
                    out BrowseResultCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new BrowseResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("browsenext")]
        public async Task<IActionResult> BrowseNext()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<BrowseNextRequest>(server.MessageContext);

                var responseHeader = server.BrowseNext(
                    request.RequestHeader,
                    request.ReleaseContinuationPoints,
                    request.ContinuationPoints,
                    out BrowseResultCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new BrowseNextResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Translate()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<TranslateBrowsePathsToNodeIdsRequest>(server.MessageContext);

                var responseHeader = server.TranslateBrowsePathsToNodeIds(
                    request.RequestHeader,
                    request.BrowsePaths,
                    out BrowsePathResultCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new TranslateBrowsePathsToNodeIdsResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("historyread")]
        public async Task<IActionResult> HistoryRead()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<HistoryReadRequest>(server.MessageContext);

                var responseHeader = server.HistoryRead(
                    request.RequestHeader,
                    request.HistoryReadDetails,
                    request.TimestampsToReturn,
                    request.ReleaseContinuationPoints,
                    request.NodesToRead,
                    out HistoryReadResultCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new HistoryReadResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("historyupdate")]
        public async Task<IActionResult> HistoryUpdate()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<HistoryUpdateRequest>(server.MessageContext);

                var responseHeader = server.HistoryUpdate(
                    request.RequestHeader,
                    request.HistoryUpdateDetails,
                    out HistoryUpdateResultCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new HistoryUpdateResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        private StandardServer GetServer()
        {
            LoadWebSession();

            var sessions = m_cache.GetOrCreate<CacheSessions>("CS", (ii) =>
            {
                return new CacheSessions() {
                    SecureChannelId = Guid.NewGuid().ToString(),
                    AuthenticationTokens = new Dictionary<string, string>()
                };
            });

            var server = m_communicationService.ServerApi;
            var ed = server.GetEndpoints().Where(x => x.TransportProfileUri == Profiles.HttpsJsonTransport).First();
            ed.EndpointUrl = $"https://{HttpContext.Request.Host}/opcua";

            SecureChannelContext.Current = new SecureChannelContext(
                sessions.SecureChannelId,
                ed,
                RequestEncoding.Json);

            var accessToken = "";

            if (Request.Headers.TryGetValue("Authorization", out var header))
            {
                accessToken = String.Join(" ", header).Trim();

                if (accessToken.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                {
                    accessToken = accessToken.Substring(JwtBearerDefaults.AuthenticationScheme.Length).Trim();
                }
            }

            lock (m_lock)
            {
                if (!sessions.AuthenticationTokens.TryGetValue(accessToken, out var authenticationToken))
                {
                    try
                    {
                        authenticationToken = CreateDefaultSession(accessToken);
                        sessions.AuthenticationTokens[accessToken] = authenticationToken;
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e, "CreateDefaultSession failed.");
                    }
                }

                m_authenticationToken = NodeId.Parse(server.CurrentInstance.MessageContext, authenticationToken);

                if (server.CurrentInstance.SessionManager.GetSession(m_authenticationToken) == null)
                {
                    try
                    {
                        authenticationToken = CreateDefaultSession(accessToken);
                        sessions.AuthenticationTokens[accessToken] = authenticationToken;
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(e, "CreateDefaultSession failed.");
                    }

                    m_authenticationToken = NodeId.Parse(server.CurrentInstance.MessageContext, authenticationToken);
                }
            }

            return server;
        }

        private string CreateDefaultSession(string accessToken)
        {
            var server = m_communicationService.ServerApi;

            var request = new CreateSessionRequest()
            {
                RequestHeader = new RequestHeader()
                {
                    Timestamp = DateTime.UtcNow,
                    RequestHandle = 0,
                    ReturnDiagnostics = 0,
                    TimeoutHint = 60000
                },
                ClientDescription = new ApplicationDescription()
                {
                    ApplicationUri = $"urn:{System.Net.Dns.GetHostName().ToLower()}:UA:UaRestGateway:Client",
                    ProductUri = "UaRestGateway",
                    ApplicationName = new LocalizedText("UaRestGateway"),
                    ApplicationType = ApplicationType.Client
                },
                ServerUri = server.CurrentInstance.NamespaceUris.GetString(1),
                EndpointUrl = "opc.tcp://localhost:46040",
                SessionName = "UaRestGateway:InternalClient",
                RequestedSessionTimeout = 30000,
                MaxResponseMessageSize = 0
            };

            var responseHeader = server.CreateSession(
                request.RequestHeader,
                request.ClientDescription,
                request.ServerUri,
                request.EndpointUrl,
                request.SessionName,
                request.ClientNonce,
                request.ClientCertificate,
                request.RequestedSessionTimeout,
                request.MaxResponseMessageSize,
                out NodeId sessionId,
                out NodeId authenticationToken,
                out double revisedSessionTimeout,
                out byte[] serverNonce,
                out byte[] serverCertificate,
                out EndpointDescriptionCollection serverEndpoints,
                out SignedSoftwareCertificateCollection serverSoftwareCertificates,
                out SignatureData serverSignature,
                out uint maxRequestMessageSize);

            var userIdentity = (!String.IsNullOrEmpty(accessToken))
                ? new ExtensionObject(new IssuedIdentityToken()
                {
                    PolicyId = SecureChannelContext.Current.EndpointDescription.UserIdentityTokens.Where(x => x.TokenType == UserTokenType.IssuedToken).FirstOrDefault()?.PolicyId,
                    DecryptedTokenData = new UTF8Encoding(false).GetBytes(accessToken)
                })
                : null;

            var request2 = new ActivateSessionRequest()
            {
                RequestHeader = new RequestHeader()
                {
                    Timestamp = DateTime.UtcNow,
                    RequestHandle = 0,
                    ReturnDiagnostics = 0,
                    TimeoutHint = 60000,
                    AuthenticationToken = authenticationToken
                },
                ClientSignature = new SignatureData(),
                ClientSoftwareCertificates = new SignedSoftwareCertificateCollection(),
                LocaleIds = new StringCollection(),
                UserTokenSignature = new SignatureData()
            };

            responseHeader = server.ActivateSession(
                request2.RequestHeader,
                request2.ClientSignature,
                request2.ClientSoftwareCertificates,
                request2.LocaleIds,
                userIdentity,
                request2.UserTokenSignature,
                out serverNonce,
                out StatusCodeCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

            var response = new ActivateSessionResponse()
            {
                ResponseHeader = responseHeader,
                ServerNonce = serverNonce,
                Results = results,
                DiagnosticInfos = diagnosticInfos
            };

            return authenticationToken.Format(server.CurrentInstance.MessageContext, true);
        }

        [HttpPost]
        [Route("createsession")]
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<CreateSessionRequest>(server.MessageContext);

                // Logger.LogError("CreateSession.Current: {0} {1}", Session.SecureChannelId, SecureChannelContext.Current?.SecureChannelId ?? "null"); 

                var responseHeader = server.CreateSession(
                    request.RequestHeader,
                    request.ClientDescription,
                    request.ServerUri,
                    request.EndpointUrl,
                    request.SessionName,
                    request.ClientNonce,
                    request.ClientCertificate,
                    request.RequestedSessionTimeout,
                    request.MaxResponseMessageSize,
                    out NodeId sessionId,
                    out NodeId authenticationToken,
                    out double revisedSessionTimeout,
                    out byte[] serverNonce,
                    out byte[] serverCertificate,
                    out EndpointDescriptionCollection serverEndpoints,
                    out SignedSoftwareCertificateCollection serverSoftwareCertificates,
                    out SignatureData serverSignature,
                    out uint maxRequestMessageSize);

                serverEndpoints = new EndpointDescriptionCollection(new EndpointDescription[] { SecureChannelContext.Current.EndpointDescription });

                var response = new CreateSessionResponse()
                {
                    ResponseHeader = responseHeader,
                    SessionId = sessionId,
                    AuthenticationToken = authenticationToken,
                    RevisedSessionTimeout = revisedSessionTimeout,
                    ServerNonce = serverNonce,
                    ServerCertificate = serverCertificate,
                    ServerEndpoints = serverEndpoints,
                    ServerSoftwareCertificates = serverSoftwareCertificates,
                    ServerSignature = serverSignature,
                    MaxRequestMessageSize = maxRequestMessageSize
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("activatesession")]
        public async Task<IActionResult> ActivateSession()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<ActivateSessionRequest>(server.MessageContext);

                // Logger.LogError("ActivateSession.Current: {0} {1}", Session.SecureChannelId, SecureChannelContext.Current?.SecureChannelId ?? "null");

                var accessToken = "";

                if (Request.Headers.TryGetValue("Authorization", out var header))
                {
                    accessToken = String.Join(" ", header).Trim();

                    if (accessToken.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                    {
                        accessToken = accessToken.Substring(JwtBearerDefaults.AuthenticationScheme.Length).Trim();
                    }
                }

                var userIdentity = (!String.IsNullOrEmpty(accessToken))
                    ? new ExtensionObject(new IssuedIdentityToken()
                    {
                        PolicyId = SecureChannelContext.Current.EndpointDescription.UserIdentityTokens.Where(x => x.TokenType == UserTokenType.IssuedToken).FirstOrDefault()?.PolicyId,
                        DecryptedTokenData = new UTF8Encoding(false).GetBytes(accessToken)
                    })
                    : null;

                var responseHeader = server.ActivateSession(
                    request.RequestHeader,
                    request.ClientSignature,
                    request.ClientSoftwareCertificates,
                    request.LocaleIds,
                    userIdentity,
                    request.UserTokenSignature,
                    out byte[] serverNonce,
                    out StatusCodeCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new ActivateSessionResponse()
                {
                    ResponseHeader = responseHeader,
                    ServerNonce = serverNonce,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("createsubscription")]
        public async Task<IActionResult> CreateSubscription()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<CreateSubscriptionRequest>(server.MessageContext);

                // Logger.LogError("CreateSubscription.Current: {0} {1}", Session.SecureChannelId, SecureChannelContext.Current?.SecureChannelId ?? "null");

                var responseHeader = server.CreateSubscription(
                    request.RequestHeader,
                    request.RequestedPublishingInterval,
                    request.RequestedLifetimeCount,
                    request.RequestedMaxKeepAliveCount,
                    request.MaxNotificationsPerPublish,
                    request.PublishingEnabled,
                    request.Priority,
                    out uint subscriptionId,
                    out double revisedPublishingInterval,
                    out uint revisedLifetimeCount,
                    out uint revisedMaxKeepAliveCount);

                var response = new CreateSubscriptionResponse()
                {
                     ResponseHeader = responseHeader,
                     SubscriptionId = subscriptionId,
                     RevisedPublishingInterval = revisedPublishingInterval,
                     RevisedLifetimeCount = revisedLifetimeCount,
                     RevisedMaxKeepAliveCount = revisedMaxKeepAliveCount
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("deletesubscriptions")]
        public async Task<IActionResult> DeleteSubscriptions()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<DeleteSubscriptionsRequest>(server.MessageContext);

                // Logger.LogError("DeleteSubscriptions.Current: {0} {1}", Session.SecureChannelId, SecureChannelContext.Current?.SecureChannelId ?? "null");

                var responseHeader = server.DeleteSubscriptions(
                    request.RequestHeader,
                    request.SubscriptionIds,
                    out StatusCodeCollection results,
                    out DiagnosticInfoCollection diagnosticInfos);

                var response = new DeleteSubscriptionsResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("publish")]
        public async Task<IActionResult> Publish()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<PublishRequest>(server.MessageContext);

                // Logger.LogError("Publish.Current: {0} {1}", Session.SecureChannelId, SecureChannelContext.Current?.SecureChannelId ?? "null");

                var responseHeader = server.Publish(
                    request.RequestHeader,
                    request.SubscriptionAcknowledgements,
                    out var subscriptionId,
                    out var availableSequenceNumbers,
                    out var moreNotifications,
                    out var notificationMessage,
                    out var results,
                    out var diagnosticInfos);

                var response = new PublishResponse()
                {
                    ResponseHeader = responseHeader,
                    SubscriptionId = subscriptionId,
                    AvailableSequenceNumbers = availableSequenceNumbers,
                    MoreNotifications = moreNotifications,
                    NotificationMessage = notificationMessage,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("createmonitoreditems")]
        public async Task<IActionResult> CreateMonitoredItems()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<CreateMonitoredItemsRequest>(server.MessageContext);

                var responseHeader = server.CreateMonitoredItems(
                    request.RequestHeader,
                    request.SubscriptionId,
                    request.TimestampsToReturn,
                    request.ItemsToCreate,
                    out var results,
                    out var diagnosticInfos);

                var response = new CreateMonitoredItemsResponse()
                {
                    ResponseHeader = responseHeader,
                    Results = results,
                    DiagnosticInfos = diagnosticInfos
                };

                return await Encode(server.MessageContext, response);
            }
            catch (Exception e)
            {                
                return await Fault(e);
            }
        }
    }
}
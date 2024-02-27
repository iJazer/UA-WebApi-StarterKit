using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Opc.Ua;
using UaRestGateway.Server.Service;
using StatusCodes = Opc.Ua.StatusCodes;
using ISession = Opc.Ua.Client.ISession;
using UaRestGateway.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;
using Opc.Ua.Server;

namespace UaRestGateway.Server.Controllers
{
    [ApiController]
    [Route("opcua")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UaServerController : CommonController
    {
        private readonly IMemoryCache m_cache;
        private readonly IUACommunicationService m_communicationService;

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

        private async Task<T> Decode<T>(IServiceMessageContext context) where T : IEncodeable, new()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var json = await reader.ReadToEndAsync();

                using (var decoder = new JsonDecoder(json, context))
                {
                    decoder.UpdateNamespaceTable = true;
                    SessionLessServiceMessage message = new SessionLessServiceMessage();

                    message.NamespaceUris = new NamespaceTable(decoder.ReadStringArray(nameof(SessionLessServiceMessage.NamespaceUris)));
                    message.ServerUris = new StringTable(decoder.ReadStringArray(nameof(SessionLessServiceMessage.ServerUris)));
                    message.LocaleIds = new StringTable(decoder.ReadStringArray(nameof(SessionLessServiceMessage.LocaleIds)));
                    message.UrisVersion = decoder.ReadUInt32(nameof(SessionLessServiceMessage.UrisVersion));

                    decoder.SetMappingTables(message.NamespaceUris, message.ServerUris);

                    message.Message = decoder.ReadEncodeable("Body", typeof(T));

                    return (T)message.Message;
                }
            }
        }

        private Task<IActionResult> Encode<T>(IServiceMessageContext context, T response) where T : IEncodeable
        {
            using (var encoder = new JsonEncoder(context, true))
            {
                encoder.UseStringNodeIds = true;
                encoder.ForceNamespaceUri = true;
                encoder.ForceNamespaceUriForIndex1 = true;

                encoder.WriteStringArray(nameof(SessionLessServiceMessage.NamespaceUris), context.NamespaceUris.ToArray());
                encoder.WriteStringArray(nameof(SessionLessServiceMessage.ServerUris), context.ServerUris.ToArray());
                encoder.WriteStringArray(nameof(SessionLessServiceMessage.LocaleIds), null);
                encoder.WriteUInt32(nameof(SessionLessServiceMessage.UrisVersion), 0);
                encoder.WriteUInt32("ServiceId", (uint)response.TypeId.Identifier);
                encoder.WriteEncodeable("Body", response, typeof(T));

                var json = encoder.CloseAndReturnText();
                return Task.FromResult<IActionResult>(Content(json, "application/json"));
            }
        }

        private Task<IActionResult> Fault(Exception e)
        {
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

                return Encode<ServiceFault>(context, fault);
            }
        }

        private async Task<ISession> GetSession(string serverId)
        {
            var token = "";

            if (Request.Headers.TryGetValue("Authorization", out var header))
            {
                token = String.Join(" ", header).Trim();

                if (token.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                {
                    token = token.Substring(JwtBearerDefaults.AuthenticationScheme.Length).Trim();
                }
            }

            var session = await m_communicationService.FindSession(serverId, token);

            if (session == null)
            {
                throw new UnauthorizedAccessException();
            }

            return session;
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<ReadRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.ReadAsync(
                    request.RequestHeader,
                    request.MaxAge,
                    request.TimestampsToReturn,
                    request.NodesToRead,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("write")]
        public async Task<IActionResult> Write([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<WriteRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.WriteAsync(
                    request.RequestHeader,
                    request.NodesToWrite,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("call")]
        public async Task<IActionResult> Call([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<CallRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.CallAsync(
                    request.RequestHeader,
                    request.MethodsToCall,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("browse")]
        public async Task<IActionResult> Browse([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<BrowseRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.BrowseAsync(
                    request.RequestHeader,
                    request.View,
                    request.RequestedMaxReferencesPerNode,
                    request.NodesToBrowse,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("browsenext")]
        public async Task<IActionResult> BrowseNext([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<BrowseNextRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.BrowseNextAsync(
                    request.RequestHeader,
                    request.ReleaseContinuationPoints,
                    request.ContinuationPoints,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Translate([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<TranslateBrowsePathsToNodeIdsRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.TranslateBrowsePathsToNodeIdsAsync(
                    request.RequestHeader,
                    request.BrowsePaths,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("historyread")]
        public async Task<IActionResult> HistoryRead([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<HistoryReadRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.HistoryReadAsync(
                    request.RequestHeader,
                    request.HistoryReadDetails,
                    request.TimestampsToReturn,
                    request.ReleaseContinuationPoints,
                    request.NodesToRead,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("historyupdate")]
        public async Task<IActionResult> HistoryUpdate([FromQuery] string serverId = null)
        {
            try
            {
                var session = await GetSession(serverId);
                var request = await Decode<HistoryUpdateRequest>(session.MessageContext);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.HistoryUpdateAsync(
                    request.RequestHeader,
                    request.HistoryUpdateDetails,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        private StandardServer GetServer()
        {
            LoadWebSession();

            var server = m_communicationService.ServerApi;
            var ed = server.GetEndpoints().Where(x => x.TransportProfileUri == Profiles.HttpsJsonTransport).First();
            ed.EndpointUrl = $"https://{HttpContext.Request.Host}/opcua";

            SecureChannelContext.Current = new SecureChannelContext(
                Session.SecureChannelId,
                ed,
                RequestEncoding.Json);

            return server;
        }

        [HttpPost]
        [Route("createsession")]
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var server = GetServer();
                var request = await Decode<CreateSessionRequest>(server.MessageContext);

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
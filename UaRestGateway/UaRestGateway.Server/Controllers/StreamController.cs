using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using UaRestGateway.Server.Service;
using Microsoft.Extensions.Caching.Memory;
using UaRestGateway.Server.Model;
using Opc.Ua;
using Opc.Ua.Server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Runtime.CompilerServices;

namespace UaRestGateway.Server.Controllers
{
    [Route("[controller]")]
    public class StreamController : CommonController
    {
        private readonly object m_lock = new();
        private readonly IMemoryCache m_cache;
        private readonly IUACommunicationService m_communicationService;
        private readonly Dictionary<string, ISession> m_sessions = new();
        private NodeId m_authenticationToken;

        public StreamController(
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

        // GET api/values
        [HttpGet]
        public async Task Get()
        {
            try
            {
                var context = ControllerContext.HttpContext;
                var isSocketRequest = context.WebSockets.IsWebSocketRequest;

                if (isSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await ProcessMessages(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in StreamController");
            }
        }

        private StandardServer GetServer(HttpContext context)
        {
            LoadWebSession();

            var server = m_communicationService.ServerApi;

            var sessions = m_cache.GetOrCreate<CacheSessions>("CS", (ii) =>
            {
                return new CacheSessions()
                {
                    AuthenticationTokens = new Dictionary<string, string>()
                };
            });

            var accessToken = "";

            if (context.Request.Headers.TryGetValue("Authorization", out var header))
            {
                accessToken = String.Join(" ", header).Trim();

                if (accessToken.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                {
                    accessToken = accessToken.Substring(JwtBearerDefaults.AuthenticationScheme.Length).Trim();
                }
            }

            lock (m_lock)
            {
                var ed = server.GetEndpoints().Where(x => x.TransportProfileUri == Profiles.HttpsJsonTransport).First();
                ed.EndpointUrl = $"https://{context.Request.Host}/opcua";

                SessionContext sessionContext = new SessionContext()
                {
                    ChannelContext = new SecureChannelContext(
                        Guid.NewGuid().ToString(),
                        ed,
                        RequestEncoding.Json)
                };

                if (!sessions.AuthenticationTokens.TryGetValue(accessToken, out var authenticationToken))
                {
                    try
                    {
                        authenticationToken = MessageUtils.CreateDefaultSession(sessionContext, m_communicationService.ServerApi, accessToken);
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
                        authenticationToken = MessageUtils.CreateDefaultSession(sessionContext, m_communicationService.ServerApi, accessToken);
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

        private async Task ProcessMessages(HttpContext httpContext, WebSocket webSocket)
        {
            var server = GetServer(httpContext);

            var ed = server.GetEndpoints().Where(x => x.TransportProfileUri == Profiles.HttpsJsonTransport).First();
            ed.EndpointUrl = $"https://{httpContext.Request.Host}/opcua";

            SessionContext sessionContext = new SessionContext()
            {
                ChannelContext = new SecureChannelContext(
                    Guid.NewGuid().ToString(),
                    ed,
                    RequestEncoding.Json)
            };

            while (webSocket.State == WebSocketState.Open)
            {
                await DispatchMessage(server, httpContext, sessionContext, webSocket);
            }
        }

        private async Task SendResponse(WebSocket webSocket, IServiceMessageContext context, IServiceResponse response)
        {
            try
            {
                using (var ostrm = await MessageUtils.Encode(context, response))
                {
                    ostrm.Close();
                    await webSocket.SendAsync(ostrm.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error sending response.");
            }
        }

        private async Task DispatchMessage(
            StandardServer server,
            HttpContext httpContext,
            SessionContext sessionContext,
            WebSocket webSocket)
        {
            var istrm = new MemoryStream();
            var buffer = new byte[1024];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            istrm.Write(buffer, 0, result.Count);

            while (!result.CloseStatus.HasValue && !result.EndOfMessage)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                istrm.Write(buffer, 0, result.Count);
            }

            if (result.CloseStatus.HasValue)
            {
                return;
            }

            istrm.Position = 0;
            MemoryStream stream = istrm;

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                stream = await MessageUtils.Decompress(istrm);
            }

            IServiceRequest request = null;
            IServiceResponse response = null;

            try
            {
                request = await MessageUtils.Decode(server.MessageContext, stream);
            }
            catch (Exception e)
            {
                response = MessageUtils.Fault(request, e);
                await SendResponse(webSocket, server.MessageContext, response);
                return;
            }

            if (request is CreateSessionRequest csr)
            {
                lock (sessionContext)
                {
                    if (sessionContext.AuthenticationToken != null)
                    {
                        server.CloseSession(
                            new RequestHeader()
                            {
                                Timestamp = DateTime.UtcNow,
                                TimeoutHint = 60000,
                                AuthenticationToken = sessionContext.AuthenticationToken
                            },
                            true);

                        sessionContext.AuthenticationToken = null;
                    }
                }

                response = await MessageUtils.CreateSession(sessionContext, server, csr);

                lock (sessionContext)
                {
                    sessionContext.AuthenticationToken = ((CreateSessionResponse)response).AuthenticationToken;
                }

                await SendResponse(webSocket, server.MessageContext, response);
                return;
            }

            if (sessionContext.AuthenticationToken == null || sessionContext.AuthenticationToken != request.RequestHeader.AuthenticationToken)
            {
                response = MessageUtils.Fault(request, new ServiceResultException(Opc.Ua.StatusCodes.BadSessionIdInvalid, "Session not created."));
                await SendResponse(webSocket, server.MessageContext, response);
                Logger.LogError($"BadSessionIdInvalid {request.RequestHeader.RequestHandle}");
                return;
            }

            if (request is ActivateSessionRequest asr)
            {
                response = await MessageUtils.ActivateSession(sessionContext, server, asr);
                sessionContext.IsActivated = true;
                await SendResponse(webSocket, server.MessageContext, response);
                return;
            }
            else if (!sessionContext.IsActivated)
            {
                response = MessageUtils.Fault(request, new ServiceResultException(Opc.Ua.StatusCodes.BadSessionNotActivated, "Session not activated."));
                await SendResponse(webSocket, server.MessageContext, response);
                Logger.LogError($"BadSessionNotActivated {request.RequestHeader.RequestHandle}");
                return;
            }

            var task = Task.Run(async () =>
            {
                await ProcessRequest(webSocket, stream, sessionContext, server, request);
            });
        }

        private async Task ProcessRequest(
            WebSocket webSocket,
            Stream stream, 
            SessionContext sessionContext,
            StandardServer server,
            IServiceRequest request)
        {
            using (stream)
            {
                IServiceResponse response = null;

                try
                {
                    switch (request)
                    {
                        case CloseSessionRequest input:
                            response = await MessageUtils.CloseSession(sessionContext, server, input);
                            break;

                        case ReadRequest input:
                            response = await MessageUtils.Read(sessionContext, server, input);
                            break;

                        case WriteRequest input:
                            response = await MessageUtils.Write(sessionContext, server, input);
                            break;

                        case CallRequest input:
                            response = await MessageUtils.Call(sessionContext, server, input);
                            break;

                        case BrowseRequest input:
                            response = await MessageUtils.Browse(sessionContext, server, input);
                            break;

                        case BrowseNextRequest input:
                            response = await MessageUtils.BrowseNext(sessionContext, server, input);
                            break;

                        case TranslateBrowsePathsToNodeIdsRequest input:
                            response = await MessageUtils.TranslateBrowsePathsToNodeIds(sessionContext, server, input);
                            break;

                        case HistoryReadRequest input:
                            response = await MessageUtils.HistoryRead(sessionContext, server, input);
                            break;

                        case HistoryUpdateRequest input:
                            response = await MessageUtils.HistoryUpdate(sessionContext, server, input);
                            break;

                        case CreateSubscriptionRequest input:
                            response = await MessageUtils.CreateSubscription(sessionContext, server, input);
                            break;

                        case DeleteSubscriptionsRequest input:
                            response = await MessageUtils.DeleteSubscriptions(sessionContext, server, input);
                            break;

                        case PublishRequest input:
                            response = await MessageUtils.Publish(sessionContext, server, input);
                            break;

                        case CreateMonitoredItemsRequest input:
                            response = await MessageUtils.CreateMonitoredItems(sessionContext, server, input);
                            break;

                        case ModifyMonitoredItemsRequest input:
                            response = await MessageUtils.ModifyMonitoredItems(sessionContext, server, input);
                            break;

                        case DeleteMonitoredItemsRequest input:
                            response = await MessageUtils.DeleteMonitoredItems(sessionContext, server, input);
                            break;

                        default:
                            throw new ServiceResultException(Opc.Ua.StatusCodes.BadNotSupported, "Request not supported.");
                    }
                }
                catch (Exception e)
                {
                    response = MessageUtils.Fault(request, e);
                    Logger.LogError(e, "Error processing message.");
                }

                var ostrm = await MessageUtils.Encode(server.MessageContext, response);
                ostrm.Close();
                await webSocket.SendAsync(ostrm.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}

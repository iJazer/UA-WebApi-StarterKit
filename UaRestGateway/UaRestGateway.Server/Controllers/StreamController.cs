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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UaRestGateway.Server.Controllers
{
    [Route("[controller]")]
    public class StreamController : CommonController
    {
        public StreamController(
            IConfiguration configuration,
            ILogger<UaServerController> logger,
            DatabaseContext context,
            IMemoryCache cache,
            IUACommunicationService communicationService)
        :
            base(configuration, logger, context, cache, communicationService)
        {
        }

        private ClaimsPrincipal VerifyToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                ValidIssuer = "https://opcfoundation.org",
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    var cache = HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

                    if (!cache.TryGetValue(nameof(TokenValidationParameters.IssuerSigningKey), out JsonWebKey key))
                    {
                        using (var client = new HttpClient())
                        {
                            var response = client.GetAsync(parameters.ValidIssuer + "/.well-known/keys").ConfigureAwait(false).GetAwaiter().GetResult();
                            response.EnsureSuccessStatusCode();

                            string json = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                            var keyset = new JsonWebKeySet(json);
                            key = keyset.Keys.FirstOrDefault();

                            cache.Set(
                                nameof(TokenValidationParameters.IssuerSigningKey),
                                key,
                                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1)));
                        }
                    }

                    return new[] { key };
                }
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
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
                    var protocols = context.WebSockets.WebSocketRequestedProtocols;

                    string selectedProtocol = "opcua+uajson";

                    if (protocols == null || !protocols.Contains(selectedProtocol))
                    {
                        selectedProtocol = null;
                    }

                    var accessToken = protocols.Where(p => p.StartsWith("opcua+token+")).FirstOrDefault();  
                    
                    if (accessToken != null)
                    {
                        accessToken = accessToken.Substring("opcua+token+".Length);

                        try
                        {
                            var principal = VerifyToken(accessToken);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(e, "Error verifying token.");
                            accessToken = null;
                        }
                    }

                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync(selectedProtocol);
                    await ProcessMessages(context, webSocket, accessToken);
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

        private async Task ProcessMessages(HttpContext httpContext, WebSocket webSocket, string accessToken)
        {
            var sessionContext = GetSessionContext(httpContext, accessToken);

            while (webSocket.State == WebSocketState.Open)
            {
                await DispatchMessage(Server, httpContext, sessionContext, webSocket);
            }
        }

        private async Task SendResponse(WebSocket webSocket, IServiceMessageContext context, IServiceResponse response, bool compress)
        {
            try
            {
                using (var ostrm = await MessageUtils.Encode(context, response, compress, envelopeRequired: true))
                {
                    ostrm.Close();

                    await webSocket.SendAsync(
                        ostrm.ToArray(),
                        (compress) ? WebSocketMessageType.Binary : WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
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
            bool compress = false;

            if (result.MessageType == WebSocketMessageType.Binary)
            {
                stream = await MessageUtils.Decompress(istrm);
                compress = true;
            }

            IServiceRequest request = null;
            IServiceResponse response = null;

            try
            {
                request = await MessageUtils.Decode<IServiceRequest>(server.MessageContext, stream, envelopeExpected: true);
            }
            catch (Exception e)
            {
                response = MessageUtils.Fault(request, e);
                await SendResponse(webSocket, server.MessageContext, response, false);
                return;
            }

            if (request is CreateSessionRequest csr)
            {
                lock (sessionContext)
                {
                    if (sessionContext.AuthenticationToken != null)
                    {
                        try
                        {
                            server.CloseSession(
                                new RequestHeader()
                                {
                                    Timestamp = DateTime.UtcNow,
                                    TimeoutHint = 60000,
                                    AuthenticationToken = sessionContext.AuthenticationToken
                                },
                                true);
                        }
                        catch (Exception)
                        {
                            // ignore
                        }
                        finally
                        {
                            sessionContext.AuthenticationToken = null;
                        }
                    }
                }

                response = await MessageUtils.CreateSession(sessionContext, server, csr);

                lock (sessionContext)
                {
                    sessionContext.AuthenticationToken = ((CreateSessionResponse)response).AuthenticationToken;
                }

                await SendResponse(webSocket, server.MessageContext, response, compress);
                return;
            }

            if (sessionContext.AuthenticationToken == null || sessionContext.AuthenticationToken != request.RequestHeader.AuthenticationToken)
            {
                response = MessageUtils.Fault(request, new ServiceResultException(Opc.Ua.StatusCodes.BadSessionIdInvalid, "Session not created."));
                await SendResponse(webSocket, server.MessageContext, response, compress);
                Logger.LogError($"BadSessionIdInvalid {request.RequestHeader.RequestHandle}");
                return;
            }

            if (request is ActivateSessionRequest asr)
            {
                response = await MessageUtils.ActivateSession(sessionContext, server, asr);
                sessionContext.IsActivated = true;
                await SendResponse(webSocket, server.MessageContext, response, compress);
                return;
            }
            else if (!sessionContext.IsActivated)
            {
                response = MessageUtils.Fault(request, new ServiceResultException(Opc.Ua.StatusCodes.BadSessionNotActivated, "Session not activated."));
                await SendResponse(webSocket, server.MessageContext, response, compress);
                Logger.LogError($"BadSessionNotActivated {request.RequestHeader.RequestHandle}");
                return;
            }

            var task = Task.Run(async () =>
            {
                await ProcessRequest(webSocket, stream, sessionContext, server, request, result.MessageType == WebSocketMessageType.Binary);
            });
        }

        private async Task ProcessRequest(
            WebSocket webSocket,
            Stream stream, 
            SessionContext sessionContext,
            StandardServer server,
            IServiceRequest request,
            bool compress)
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

                var ostrm = await MessageUtils.Encode(server.MessageContext, response, compress, envelopeRequired: true);
                ostrm.Close();

                await webSocket.SendAsync(
                    ostrm.ToArray(),
                    compress ? WebSocketMessageType.Binary : WebSocketMessageType.Text, 
                    endOfMessage: true,
                    CancellationToken.None);
            }
        }
    }
}

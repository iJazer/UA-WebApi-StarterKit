using AasCore.Aas3_0;
using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua.Server;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using UaRestGateway.Server.Model;
using UaRestGateway.Server.Service;
using UaRestGateway.Server.Service.AAS;

namespace UaRestGateway.Server.Controllers
{
    [Route("[controller]")]
    public class StreamController : CommonController
    {
        private readonly IAASCommunicationService m_aasCommunicationService;

        public StreamController(
            IConfiguration configuration,
            ILogger<UaServerController> logger,
            ILogger<AasController> aaslogger,
            DatabaseContext context,
            IMemoryCache cache,
            IUACommunicationService communicationService,
            IAASCommunicationService aasCommunicationService)
        :
            base(configuration, logger, context, cache, communicationService)
        {
            m_aasCommunicationService = aasCommunicationService;
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
                        using (var client = CreateClient())
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
                var sessionContext = GetSessionContext(HttpContext);
                var httpContext = ControllerContext.HttpContext;
                var isSocketRequest = httpContext.WebSockets.IsWebSocketRequest;

                if (isSocketRequest)
                {
                    var protocols = httpContext.WebSockets.WebSocketRequestedProtocols;

                    string selectedProtocol = "aas+opcua+uajson";

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

                    WebSocket webSocket = await httpContext.WebSockets.AcceptWebSocketAsync(selectedProtocol);
                    await ProcessMessages(httpContext, sessionContext, webSocket, accessToken);
                }
                else
                {
                    httpContext.Response.StatusCode = 400;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error in StreamController");
            }
        }

        private async Task ProcessMessages(HttpContext httpContext, SessionContext sessionContext, WebSocket webSocket, string accessToken)
        {
            while (webSocket.State == WebSocketState.Open)
            {
                await DispatchMessage(Server, Client, httpContext, sessionContext, webSocket);
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
            UAClient client,
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

            byte[] rawBytes = stream.ToArray(); // snapshot before any reads
            string rawText = null;
            try
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    rawText = Encoding.UTF8.GetString(rawBytes);

                    var jsonDoc = System.Text.Json.JsonDocument.Parse(rawText);
                    if (jsonDoc.RootElement.TryGetProperty("ServiceId", out var serviceId) &&
                        serviceId.GetString() == "AASRequest")
                    {
                        await HandleAASRequest(jsonDoc.RootElement, webSocket);
                        return;
                    }
                }

                // Otherwise: decode as OPC UA
                // Reset a fresh stream for OPC UA decode
                using var freshStream = new MemoryStream(rawBytes);
                request = await MessageUtils.Decode<IServiceRequest>(server.MessageContext, freshStream, envelopeExpected: true);
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
                await ProcessRequest(webSocket, stream, sessionContext, server, client, request, result.MessageType == WebSocketMessageType.Binary);
            });
        }

        private async Task HandleAASRequest(JsonElement root, WebSocket webSocket)
        {
            try
            {
                var requestHeader = root.GetProperty("Body").GetProperty("RequestHeader");
                var method = root.GetProperty("Body").GetProperty("Method").GetString();
                var path = root.GetProperty("Body").GetProperty("Path").GetString();
                var body = root.GetProperty("Body").TryGetProperty("Body", out var b) ? b : default;
                var requestHandle = requestHeader.GetProperty("AASRequestHandle").GetInt32();

                if (method == "GET" && path != null)
                {
                    var match = Regex.Match(path, @"^/shells/(?<aasId>[^/]+)/submodels/(?<submodelId>[^/]+)/submodel-elements/(?<elementPath>.+)$");
                    if (match.Success)
                    {
                        var aasIdEncoded = match.Groups["aasId"].Value;
                        var submodelIdEncoded = match.Groups["submodelId"].Value;
                        var elementPath = match.Groups["elementPath"].Value;

                        // Decode Base64 URL-safe encoded IDs
                        var aasId = DecodeBase64Url(aasIdEncoded);
                        var submodelId = DecodeBase64Url(submodelIdEncoded);

                        var result = m_aasCommunicationService.GetSubmodelElementByPathWithinAAS(aasId, submodelId, elementPath);

                        var response = new
                        {
                            ServiceId = "AASResponse",
                            Body = new
                            {
                                RequestHeader = new { AASRequestHandle = requestHandle },
                                Result = Jsonization.Serialize.ToJsonObject(result)
                            }
                        };

                        var jsonResponse = JsonSerializer.Serialize(response);
                        var buffer = Encoding.UTF8.GetBytes(jsonResponse);
                        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        return;
                    }
                }

                // Fallback: unsupported AAS path
                await SendError(webSocket, requestHandle, $"Unsupported AAS path: {path}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error handling AASRequest");
                await SendError(webSocket, -1, "Internal Server Error");
            }
        }

        private async Task SendError(WebSocket socket, int requestHandle, string message)
        {
            var errorResponse = new
            {
                ServiceId = "AASResponse",
                Body = new
                {
                    RequestHeader = new { AASRequestHandle = requestHandle },
                    Error = message
                }
            };

            var json = JsonSerializer.Serialize(errorResponse);
            var buffer = Encoding.UTF8.GetBytes(json);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private string DecodeBase64Url(string encoded)
        {
            encoded = encoded.Replace('-', '+').Replace('_', '/');
            switch (encoded.Length % 4)
            {
                case 2: encoded += "=="; break;
                case 3: encoded += "="; break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }

        private async Task<object> CallAASController(string method, string path, JsonElement body)
        {
            return new
            {
                echo = new
                {
                    Method = method,
                    Path = path,
                    Body = body.ToString()
                }
            };
        }

        private async Task ProcessRequest(
            WebSocket webSocket,
            Stream stream, 
            SessionContext sessionContext,
            StandardServer server,
            UAClient client,
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
                            response = await MessageUtils.CloseSession_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.CloseSession(sessionContext, server, input);
                            break;

                        case ReadRequest input:
//#ifdef ClientAccess
                            response = await MessageUtils.Read_with_Client(sessionContext, client, input);
//#endif
//#ifdef ServerAccess
                            //response = await MessageUtils.Read(sessionContext, server, input);
//#endif
                            break;

                        case WriteRequest input:
                            response = await MessageUtils.Write_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.Write(sessionContext, server, input);
                            break;

                        case CallRequest input:
                            response = await MessageUtils.Call_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.Call(sessionContext, server, input);
                            break;

                        case BrowseRequest input:
                            response = await MessageUtils.Browse_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.Browse(sessionContext, server, input);
                            break;

                        case BrowseNextRequest input:
                            response = await MessageUtils.BrowseNext_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.BrowseNext(sessionContext, server, input);
                            break;

                        case TranslateBrowsePathsToNodeIdsRequest input:
                            response = await MessageUtils.TranslateBrowsePathsToNodeIds_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.TranslateBrowsePathsToNodeIds(sessionContext, server, input);
                            break;

                        case HistoryReadRequest input:
                            response = await MessageUtils.HistoryRead(sessionContext, server, input);
                            break;

                        case HistoryUpdateRequest input:
                            response = await MessageUtils.HistoryUpdate(sessionContext, server, input);
                            break;

                        case CreateSubscriptionRequest input:
                            response = await MessageUtils.CreateSubscription_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.CreateSubscription(sessionContext, server, input);
                            break;

                        case DeleteSubscriptionsRequest input:
                            response = await MessageUtils.DeleteSubscriptions_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.DeleteSubscriptions(sessionContext, server, input);
                            break;

                        case ModifySubscriptionRequest input:
                            response = await MessageUtils.ModifySubscription_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.DeleteSubscriptions(sessionContext, server, input);
                            break;

                        case PublishRequest input:
                            response = await MessageUtils.Publish_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.Publish(sessionContext, server, input);
                            break;

                        case CreateMonitoredItemsRequest input:
                            response = await MessageUtils.CreateMonitoredItems_with_Client(sessionContext, client, input);
                           // response = await MessageUtils.CreateMonitoredItems(sessionContext, server, input);
                            break;

                        case ModifyMonitoredItemsRequest input:
                            response = await MessageUtils.ModifyMonitoredItems_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.ModifyMonitoredItems(sessionContext, server, input);
                            break;

                        case DeleteMonitoredItemsRequest input:
                            response = await MessageUtils.DeleteMonitoredItems_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.DeleteMonitoredItems(sessionContext, server, input);
                            break;

                        case SetPublishingModeRequest input:
                            response = await MessageUtils.SetPublishingMode_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.DeleteMonitoredItems(sessionContext, server, input);
                            break;

                        case CreateSessionRequest input:
                            response = await MessageUtils.CreateSession_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.CreateSession(sessionContext, server, input);
                            break;

                        case ActivateSessionRequest input:
                            response = await MessageUtils.ActivateSession_with_Client(sessionContext, client, input);
                            //response = await MessageUtils.ActivateSession(sessionContext, server, input);
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Opc.Ua;
using UaRestGateway.Server.Service;
using StatusCodes = Opc.Ua.StatusCodes;
using ISession = Opc.Ua.Client.ISession;
using UaRestGateway.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        private async Task<T> Decode<T>(ISession session)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var json = await reader.ReadToEndAsync();

                using (var decoder = new JsonDecoder(json, session.MessageContext))
                {
                    decoder.ExtractUrisFromData(true);
                    return (T)decoder.ReadEncodeable("Body", typeof(T));
                }
            }
        }

        private Task<IActionResult> Encode<T>(ISession session, T response) where T : IEncodeable
        {
            using (var encoder = new JsonEncoder(session.MessageContext, true))
            {
                encoder.UseStringNodeIds = true;
                encoder.ForceNamespaceUriForIndex1 = true;
                encoder.WriteEncodeable("Body", response, null);

                var json = encoder.CloseAndReturnText();
                return Task.FromResult<IActionResult>(Content(json, "application/json"));
            }
        }

        private Task<IActionResult> Fault(Exception e)
        {
            using (var encoder = new JsonEncoder(ServiceMessageContext.GlobalContext, true))
            {
                encoder.UseStringNodeIds = true;
                encoder.ForceNamespaceUriForIndex1 = true;
                encoder.UseStringNodeIds = true;

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

                encoder.WriteEncodeable("Body", fault, null);
                var json = encoder.CloseAndReturnText();
                return Task.FromResult<IActionResult>(Content(json, "application/json"));
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
                var request = await Decode<ReadRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.ReadAsync(
                    request.RequestHeader,
                    request.MaxAge,
                    request.TimestampsToReturn,
                    request.NodesToRead,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<WriteRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.WriteAsync(
                    request.RequestHeader,
                    request.NodesToWrite,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<CallRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.CallAsync(
                    request.RequestHeader,
                    request.MethodsToCall,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<BrowseRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.BrowseAsync(
                    request.RequestHeader,
                    request.View,
                    request.RequestedMaxReferencesPerNode,
                    request.NodesToBrowse,                     
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<BrowseNextRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.BrowseNextAsync(
                    request.RequestHeader,
                    request.ReleaseContinuationPoints,
                    request.ContinuationPoints,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<TranslateBrowsePathsToNodeIdsRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.TranslateBrowsePathsToNodeIdsAsync(
                    request.RequestHeader,
                    request.BrowsePaths,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<HistoryReadRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.HistoryReadAsync(
                    request.RequestHeader,
                    request.HistoryReadDetails,
                    request.TimestampsToReturn,
                    request.ReleaseContinuationPoints,
                    request.NodesToRead,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
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
                var request = await Decode<HistoryUpdateRequest>(session);
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await session.HistoryUpdateAsync(
                    request.RequestHeader,
                    request.HistoryUpdateDetails,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(session, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }
    }
}
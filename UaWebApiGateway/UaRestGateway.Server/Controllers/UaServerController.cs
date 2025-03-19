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
        public UaServerController(
            IConfiguration configuration,
            ILogger<UaServerController> logger,
            DatabaseContext context,
            IMemoryCache cache,
            IUACommunicationService communicationService)
        :
            base(configuration, logger, context, cache, communicationService)
        {
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

        private async Task<T> Decode<T>(IServiceMessageContext context) where T : IServiceRequest, new()
        {
            return (T)await MessageUtils.Decode<T>(context, Request.Body, compressed: IsRequestCompressed(), envelopeExpected: false);
        }

        private async Task<IActionResult> Encode<T>(IServiceMessageContext context, T response) where T : IEncodeable
        {
            var compressed = IsRequestCompressed();

            var stream = await MessageUtils.Encode<T>(context, response, compressed);

            if (compressed)
            {
                Response.Headers.Append("Content-Encoding", "gzip");
            }

            return File(stream, "application/json");
        }

        private async Task<IActionResult> Fault(Exception e)
        {
            Logger.LogWarning(e, "Fault calling Service.");

            IServiceMessageContext context = Server.MessageContext ?? ServiceMessageContext.GlobalContext;

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

            var stream = await MessageUtils.Encode<ServiceFault>(context, fault, false);

            return File(stream, "application/json");
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<ReadRequest>(Server.MessageContext);
                var response = await MessageUtils.Read_with_Client(context, Client, request);
                //var response = await MessageUtils.Read(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<WriteRequest>(Server.MessageContext);
                var response = await MessageUtils.Write(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<CallRequest>(Server.MessageContext);
                var response = await MessageUtils.Call(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<BrowseRequest>(Server.MessageContext);
                //var response = await MessageUtils.Browse(context, Server, request);
                var response = await MessageUtils.Browse_with_Client(context, Client, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<BrowseNextRequest>(Server.MessageContext);
                var response = await MessageUtils.BrowseNext_with_Client(context, Client, request);
                //var response = await MessageUtils.BrowseNext(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<TranslateBrowsePathsToNodeIdsRequest>(Server.MessageContext);
                var response = await MessageUtils.TranslateBrowsePathsToNodeIds(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<HistoryReadRequest>(Server.MessageContext);
                var response = await MessageUtils.HistoryRead(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<HistoryUpdateRequest>(Server.MessageContext);
                var response = await MessageUtils.HistoryUpdate(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }
    }
}
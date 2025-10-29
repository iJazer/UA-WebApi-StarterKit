using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Opc.Ua;
using UaRestGateway.Server.Service;
using StatusCodes = Opc.Ua.StatusCodes;
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
                var response = await MessageUtils.Write_with_Client(context, Client, request);
                //var response = await MessageUtils.Write(context, Server, request);
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
                var response = await MessageUtils.Call_with_Client(context, Client, request);
                //var response = await MessageUtils.Call(context, Server, request);
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
                var response = await MessageUtils.Browse_with_Client(context, Client, request);
                //var response = await MessageUtils.Browse(context, Server, request);
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
                var response = await MessageUtils.TranslateBrowsePathsToNodeIds_with_Client(context, Client, request);
                //var response = await MessageUtils.TranslateBrowsePathsToNodeIds(context, Server, request);
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

        [HttpPost]
        [Route("createsession")]
        public async Task<IActionResult> CreateSession()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<CreateSessionRequest>(Server.MessageContext);
                var response = await MessageUtils.CreateSession_with_Client(context, Client, request);
                //var response = await MessageUtils.CreateSession(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<ActivateSessionRequest>(Server.MessageContext);
                var response = await MessageUtils.ActivateSession_with_Client(context, Client, request);
                //var response = await MessageUtils.ActivateSession(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("closesession")]
        public async Task<IActionResult> CloseSession()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<CloseSessionRequest>(Server.MessageContext);
                var response = await MessageUtils.CloseSession_with_Client(context, Client, request);
                //var response = await MessageUtils.CloseSession(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("publish")]
        public async Task<IActionResult> publish()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<PublishRequest>(Server.MessageContext);
                var response = await MessageUtils.Publish_with_Client(context, Client, request);
                //var response = await MessageUtils.Publish(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        //--> TODO
        
        [HttpPost]
        [Route("setpublishingmode")]
        public async Task<IActionResult> SetPublishingMode()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<SetPublishingModeRequest>(Server.MessageContext);
                var response = await MessageUtils.SetPublishingMode_with_Client(context, Client, request);
                //var response = await MessageUtils.SetPublishingMode(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<CreateSubscriptionRequest>(Server.MessageContext);
                var response = await MessageUtils.CreateSubscription_with_Client(context, Client, request);
                //var response = await MessageUtils.CreateSubscription(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("deletesubscription")]
        public async Task<IActionResult> DeleteSubscription()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<DeleteSubscriptionsRequest>(Server.MessageContext);
                var response = await MessageUtils.DeleteSubscriptions_with_Client(context, Client, request);
                //var response = await MessageUtils.DeleteSubscriptions(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("modifysubscription")]
        public async Task<IActionResult> ModifySubscription()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<ModifySubscriptionRequest>(Server.MessageContext);
                var response = await MessageUtils.ModifySubscription_with_Client(context, Client, request);
                //var response = await MessageUtils.ModifySubscription(context, Server, request);
                return await Encode(Server.MessageContext, response);
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
                var context = GetSessionContext(HttpContext);
                var request = await Decode<CreateMonitoredItemsRequest>(Server.MessageContext);
                var response = await MessageUtils.CreateMonitoredItems_with_Client(context, Client, request);
                //var response = await MessageUtils.CreateMonitoredItems(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("deletemonitoreditems")]
        public async Task<IActionResult> DeleteMonitoredItems()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<DeleteMonitoredItemsRequest>(Server.MessageContext);
                var response = await MessageUtils.DeleteMonitoredItems_with_Client(context, Client, request);
                //var response = await MessageUtils.DeleteMonitoredItems(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("modifymonitoreditems")]
        public async Task<IActionResult> ModifyMonitoredItems()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<ModifyMonitoredItemsRequest>(Server.MessageContext);
                var response = await MessageUtils.ModifyMonitoredItems_with_Client(context, Client, request);
                //var response = await MessageUtils.ModifyMonitoredItems(context, Server, request);
                return await Encode(Server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }
    }
}
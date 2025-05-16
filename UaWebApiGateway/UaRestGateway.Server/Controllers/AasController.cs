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
    [Route("aas")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AasController : CommonController
    {
        public AasController(
            IConfiguration configuration,
            ILogger<AasController> logger,
            DatabaseContext context,
            IMemoryCache cache,
            IUACommunicationService communicationService)
        :
            base(configuration, logger, context, cache, communicationService)
        {
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                // Implement your AAS-specific read logic here
                return Ok(new { Message = "AAS Read operation successful" });
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
                // Implement your AAS-specific write logic here
                return Ok(new { Message = "AAS Write operation successful" });
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
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
    }
}

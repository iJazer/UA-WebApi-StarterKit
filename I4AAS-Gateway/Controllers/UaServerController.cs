using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Threading;
using I4AAS_Gateway.Server;
using Microsoft.Extensions.Caching.Memory;
using Opc.Ua;
using Opc.Ua.Client;

namespace I4AAS_Gateway.Controllers
{
    [ApiController]
    [Route("opcua")]
    [ResponseCache(NoStore = true)]
    public class UaServerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UaServerController> _logger;
        private readonly IMemoryCache _cache;

        private ISession m_server;

        public UaServerController(
            IConfiguration configuration,
            ILogger<UaServerController> logger, 
            IMemoryCache cache)
        {
            _configuration = configuration;
            _logger = logger;
            _cache = cache;
            
            if (!_cache.TryGetValue<ISession>(nameof(UAServerService), out m_server))
            {
                _logger.LogError("UAServerService not found in cache.");
            }
        }

        [HttpGet]
        [Route("")]
        public  async Task<string> List(int? start = 0, int? count = 1000)
        {
            try
            {
                await Task.Delay(100);
                return "";
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                ReadRequest request = null;

                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();

                    using (var decoder = new JsonDecoder(body, m_server.MessageContext))
                    {
                        decoder.ExtractUrisFromData(true);
                        request = (ReadRequest)decoder.ReadEncodeable("Body", typeof(ReadRequest));
                    }
                }

                var response = await m_server.ReadAsync(
                    request.RequestHeader,
                    request.MaxAge,
                    request.TimestampsToReturn,
                    request.NodesToRead,
                    CancellationToken.None);

                using (var encoder = new JsonEncoder(m_server.MessageContext, true))
                {
                    encoder.UseStringNodeIds = true;
                    encoder.ForceNamespaceUriForIndex1 = true;
                    encoder.WriteEncodeable("Body", response, null);

                    var json = encoder.CloseAndReturnText();
                    return Content(json, "application/json");
                }
            }
            catch (Exception e)
            {
                using (var encoder = new JsonEncoder(m_server.MessageContext, true))
                {
                    encoder.UseStringNodeIds = true;

                    ServiceFault fault = new ServiceFault()
                    {
                        ResponseHeader = new ResponseHeader()
                        {
                            Timestamp = DateTime.UtcNow,
                            ServiceResult = new ServiceResult(e, StatusCodes.BadUnexpectedError).StatusCode
                        }
                    };

                    encoder.ForceNamespaceUriForIndex1 = true;
                    encoder.WriteEncodeable("Body", fault, null);
                    var json = encoder.CloseAndReturnText();
                    return Content(json, "application/json");
                }
            }
        }

        [HttpPost]
        [Route("browse")]
        public async Task<IActionResult> Browse()
        {
            try
            {
                BrowseRequest request = null;

                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();

                    using (var decoder = new JsonDecoder(body, m_server.MessageContext))
                    {
                        decoder.ExtractUrisFromData(true);
                        request = (BrowseRequest)decoder.ReadEncodeable("Body", typeof(BrowseRequest));
                    }
                }

                var response = await m_server.BrowseAsync(
                    request.RequestHeader,
                    request.View,
                    request.RequestedMaxReferencesPerNode,
                    request.NodesToBrowse,
                    CancellationToken.None); 

                using (var encoder = new JsonEncoder(m_server.MessageContext, true))
                {
                    encoder.UseStringNodeIds = true;
                    encoder.ForceNamespaceUriForIndex1 = true;
                    encoder.WriteEncodeable("Body", response, null);

                    var json = encoder.CloseAndReturnText();
                    return Content(json, "application/json");
                }
            }
            catch (Exception e)
            {
                using (var encoder = new JsonEncoder(m_server.MessageContext, true))
                {
                    encoder.UseStringNodeIds = true;

                    ServiceFault fault = new ServiceFault()
                    {
                        ResponseHeader = new ResponseHeader()
                        {
                            Timestamp = DateTime.UtcNow,
                            ServiceResult = new ServiceResult(e, StatusCodes.BadUnexpectedError).StatusCode
                        }
                    };

                    encoder.ForceNamespaceUriForIndex1 = true;
                    encoder.WriteEncodeable("Body", fault, null);
                    var json = encoder.CloseAndReturnText();
                    return Content(json, "application/json");
                }
            }
        }
    }
}
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

        private async Task<T> Decode<T>()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var json = await reader.ReadToEndAsync();

                using (var decoder = new JsonDecoder(json, m_server.MessageContext))
                {
                    decoder.ExtractUrisFromData(true);
                    return (T)decoder.ReadEncodeable("Body", typeof(T));
                }
            }
        }

        private Task<IActionResult> Encode<T>(T response) where T : IEncodeable
        {
            using (var encoder = new JsonEncoder(m_server.MessageContext, true))
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
            using (var encoder = new JsonEncoder(m_server.MessageContext, true))
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

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                var request = await Decode<ReadRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.ReadAsync(
                    request.RequestHeader,
                    request.MaxAge,
                    request.TimestampsToReturn,
                    request.NodesToRead,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<WriteRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.WriteAsync(
                    request.RequestHeader,
                    request.NodesToWrite,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<CallRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.CallAsync(
                    request.RequestHeader,
                    request.MethodsToCall,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<BrowseRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.BrowseAsync(
                    request.RequestHeader,
                    request.View,
                    request.RequestedMaxReferencesPerNode,
                    request.NodesToBrowse,                     
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<BrowseNextRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.BrowseNextAsync(
                    request.RequestHeader,
                    request.ReleaseContinuationPoints,
                    request.ContinuationPoints,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<TranslateBrowsePathsToNodeIdsRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.TranslateBrowsePathsToNodeIdsAsync(
                    request.RequestHeader,
                    request.BrowsePaths,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<HistoryReadRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.HistoryReadAsync(
                    request.RequestHeader,
                    request.HistoryReadDetails,
                    request.TimestampsToReturn,
                    request.ReleaseContinuationPoints,
                    request.NodesToRead,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
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
                var request = await Decode<HistoryUpdateRequest>();
                uint requestHandle = request.RequestHeader.RequestHandle;

                var response = await m_server.HistoryUpdateAsync(
                    request.RequestHeader,
                    request.HistoryUpdateDetails,
                    CancellationToken.None);

                response.ResponseHeader.RequestHandle = requestHandle;
                return await Encode(response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }
    }
}
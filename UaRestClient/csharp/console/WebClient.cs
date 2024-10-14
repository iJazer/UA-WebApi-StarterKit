using System.Text;
using Newtonsoft.Json;
using Opc.Ua.WebApi;
using Opc.Ua.WebApi.Model;

namespace UaRestClient
{
    internal class OpcUaClient : IDisposable
    {
        private HttpClient m_client;

        public OpcUaClient()
        {
            m_client = new HttpClient();
        }

        public void Dispose()
        {
            if (m_client != null)
            {
                m_client.Dispose();
                m_client = null;
            }
        }

        public Uri BaseUrl { get; set; }

        public void SetUserName(string userName, string password)
        {
            var utf8 = new UTF8Encoding(false).GetBytes($"{userName}:{password}");
            m_client.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(utf8)}");
        }

        public async Task<TResponse> PostAsync<TRequest,TResponse>(string path, TRequest request)
        {
            string jsonOut = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(jsonOut, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await m_client.PostAsync(BaseUrl.ToString() + path, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                var jsonIn = JsonConvert.DeserializeObject<TResponse>(responseContent);
                return jsonIn;
            }
            else
            {
                throw new HttpRequestException($"Unexpected error sending {request.GetType().Name}.", null, statusCode: response.StatusCode);
            }
        }

        public List<T> Deserialize<T>(Variant value) where T : class
        {
            if (value.Type != (int)BuiltInType.ExtensionObject)
            {
                return default;
            }

            if (value.Body is Newtonsoft.Json.Linq.JArray)
            {
                var array = JsonConvert.DeserializeObject<ExtensionObject[]>(value.Body.ToString());

                List<T> items = new();

                foreach (var eo in array)
                {
                    items.Add(Deserialize<T>(eo));
                }

                return items;
            }

            return default;
        }

        public T Deserialize<T>(ExtensionObject eo) where T : class
        {
            if (eo == null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(eo.Body.ToString());
        }

        public async Task<List<ReferenceDescription>> BrowseChildren(string nodeId)
        {
            var message = new BrowseRequest()
            {
                RequestHeader = new RequestHeader()
                {
                    TimeoutHint = 100000,
                    Timestamp = DateTime.UtcNow
                },
                NodesToBrowse = new List<BrowseDescription>()
                {
                    new BrowseDescription()
                    {
                        NodeId = nodeId,
                        BrowseDirection = (int)BrowseDirection.Forward,
                        ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                        IncludeSubtypes = true,
                        NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable | NodeClass.Method),
                        ResultMask = 63
                    }
                }
            };

            var response = await PostAsync<BrowseRequest, BrowseResponse>("browse", message);

            if (response.ResponseHeader.ServiceResult != 0)
            {
                throw new Exception($"Browse failed with status code {StatusCodes.ToName(response.ResponseHeader.ServiceResult)}.");
            }

            return response.Results[0].References;
        }

        public async Task<List<DataValue>> ReadValues(IList<string> nodeIds)
        {
            var message = new ReadRequest()
            {
                RequestHeader = new RequestHeader()
                {
                    TimeoutHint = 100000,
                    Timestamp = DateTime.UtcNow
                },
                MaxAge = 0,
                NodesToRead = nodeIds.Select(x => new ReadValueId()
                {
                    NodeId = x,
                    AttributeId = Attributes.Value
                }).ToList()
            };

            var response = await PostAsync<ReadRequest, ReadResponse>("read", message);

            if (response.ResponseHeader.ServiceResult != 0)
            {
                throw new Exception($"Read failed with status code {StatusCodes.ToName(response.ResponseHeader.ServiceResult)}.");
            }

            return response.Results;
        }

        public async Task<List<long>> WriteValues(IList<WriteValue> valuesToWrite)
        {
            var message = new WriteRequest()
            {
                RequestHeader = new RequestHeader()
                {
                    TimeoutHint = 100000,
                    Timestamp = DateTime.UtcNow
                },
                NodesToWrite = new(valuesToWrite)
            };

            var response = await PostAsync<WriteRequest, WriteResponse>("write", message);

            if (response.ResponseHeader.ServiceResult != 0)
            {
                throw new Exception($"Write failed with status code {StatusCodes.ToName(response.ResponseHeader.ServiceResult)}.");
            }

            return response.Results;
        }

        public async Task<List<Variant>> Call(string objectId, string methodId, List<Variant> inputs)
        {
            var message = new CallRequest()
            {
                RequestHeader = new RequestHeader()
                {
                    TimeoutHint = 100000,
                    Timestamp = DateTime.UtcNow
                },
                MethodsToCall = new List<CallMethodRequest>()
                {
                    new CallMethodRequest()
                    {
                        ObjectId = objectId,
                        MethodId = methodId,
                        InputArguments = inputs
                    }
                }
            };

            var response = await PostAsync<CallRequest, CallResponse>("call", message);

            if (response.ResponseHeader.ServiceResult != 0)
            {
                throw new Exception($"Call failed with status code {StatusCodes.ToName(response.ResponseHeader.ServiceResult)}.");
            }

            if (response.Results[0].StatusCode != 0)
            {
                throw new Exception($"Method return failed status {StatusCodes.ToName(response.Results[0].StatusCode)}.");
            }

            return response.Results[0].OutputArguments;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Opc.Ua;
using Opc.Ua.Server;
using ISession = Opc.Ua.Server.ISession;
using UaRestGateway.Server.Model;
using UaRestGateway.Server.Service;

namespace UaRestGateway.Server.Controllers
{
    public class CommonController : ControllerBase
    {
        private readonly static object m_lock = new();
        private readonly IMemoryCache m_cache;
        private readonly IUACommunicationService m_communicationService;
        private readonly Dictionary<string, ISession> m_sessions = new();
 
        protected IConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        protected WebSession Session { get; private set; }

        protected StandardServer Server => m_communicationService.ServerApi;

        protected string ConnectionString { get; private set; }

        protected DatabaseContext DB { get; private set; }

        public CommonController(
            IConfiguration configuration, 
            ILogger logger, 
            DatabaseContext db,
            IMemoryCache cache,
            IUACommunicationService communicationService)
        {
            Configuration = configuration;
            Logger = logger;
            ConnectionString = Configuration.GetConnectionString("DatabaseContext");
            DB = db;
            m_cache = cache;
            m_communicationService = communicationService;
        }

        protected void LoadWebSession()
        {
            if (HttpContext.Session.TryGetValue("SD", out byte[] data))
            {
                Session = WebSession.FromBytes(data);
            }
            else
            {
                Session = new WebSession() { AuthenticationTokens = new(), SecureChannelId = Guid.NewGuid().ToString() };
            }
        }

        protected void SaveWebSession()
        {
            var bytes = Session.ToBytes();
            HttpContext.Session.Set("SD", bytes);
        }

        protected HttpClient CreateClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    if (policyErrors != System.Net.Security.SslPolicyErrors.None)
                    {
                        Logger.LogError($"{cert.Subject} has errors: {policyErrors}");
                    }

                    return policyErrors == System.Net.Security.SslPolicyErrors.None;
                };

           var client = new HttpClient(handler);
           client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgents.Default);
           return client;
        }

        protected ApiResponse<T> ReturnError<T>(string code, string text) where T : class
        {
            return new ApiResponse<T>()
            {
                Failed = true,
                ErrorCode = code,
                ErrorText = text
            };
        }

        protected ApiResponse<T> ReturnError<T>(Exception exception, string defaultCode = nameof(ErrorCodes.UnexpectedError)) where T : class
        {
            string code;
            string text;

            if (exception is ApiResponseException are)
            {
                code = are.ErrorCode;
                text = are.ErrorText;
            }
            else
            {
                code = defaultCode;
                text = exception.Message;
                Logger.LogError(exception, code);
            }

            return new ApiResponse<T>()
            {
                Failed = true,
                ErrorCode = code,
                ErrorText = text
            };
        }

        protected string GetAccessToken(HttpContext context)
        {
            var accessToken = "";

            if (context.Request.Headers.TryGetValue("Authorization", out var header))
            {
                accessToken = String.Join(" ", header).Trim();
            }

            return accessToken;
        }

        protected CachedSessions GetCacheSessions(HttpContext context)
        {
            LoadWebSession();

            var sessions = m_cache.GetOrCreate<CachedSessions>("CS", (ii) =>
            {
                return new CachedSessions()
                {
                    SecureChannelId = Guid.NewGuid().ToString(),
                    AuthenticationTokens = new Dictionary<string, string>()
                };
            });

            return sessions;
        }

        protected SessionContext GetSessionContext(HttpContext context, string accessToken = null)
        {
            lock (m_lock)
            {
                accessToken = (accessToken != null) ? accessToken : GetAccessToken(context);

                var endpoints = Server.GetEndpoints();

                if (endpoints == null || endpoints.Count == 0)
                {
                    var status = Server.CurrentState;
                    throw new ApiResponseException(ErrorCodes.ServerNotRunning, $"{ErrorCodes.ServerNotRunning}. State={status}");
                }

                var ed = Server.GetEndpoints().Where(x => x.TransportProfileUri == Profiles.HttpsJsonTransport).First();
                ed.EndpointUrl = $"https://{context.Request.Host}/opcua";

                var sessions = GetCacheSessions(context);

                SessionContext sessionContext = new SessionContext()
                {
                    ChannelContext = new SecureChannelContext(
                        sessions.SecureChannelId,
                        ed,
                        RequestEncoding.Json)
                };

                // see if a session for the specified access token already exists.
                if (!sessions.AuthenticationTokens.TryGetValue(accessToken, out var authenticationToken))
                {
                    try
                    {
                        authenticationToken = MessageUtils.CreateDefaultSession(sessionContext, m_communicationService.ServerApi, accessToken);
                        sessions.AuthenticationTokens[accessToken] = authenticationToken;
                        SaveWebSession();
                    }
                    catch (Exception e)
                    {
                        var sre = e as ServiceResultException;

                        if (sre != null)
                        {
                            switch (sre.StatusCode)
                            {
                                case Opc.Ua.StatusCodes.BadIdentityTokenInvalid:
                                case Opc.Ua.StatusCodes.BadIdentityTokenRejected:
                                {
                                    throw sre;
                                }
                            }
                        }

                        Logger.LogError(e, "CreateDefaultSession failed.");
                    }
                }

                var nodeId = NodeId.Parse(Server.CurrentInstance.MessageContext, authenticationToken);

                // verify that the previously created session is still valid.
                if (Server.CurrentInstance.SessionManager.GetSession(nodeId) == null)
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

                    nodeId = NodeId.Parse(Server.CurrentInstance.MessageContext, authenticationToken);
                }

                sessionContext.AuthenticationToken = nodeId;
                return sessionContext;
            }
        }
    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(Exception e, string defaultCode = null)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            Failed = true;

            if (e is ApiResponseException are)
            {
                ErrorCode = are.ErrorCode;
                ErrorText = are.ErrorText;
            }
            else
            {
                ErrorCode = (String.IsNullOrEmpty(defaultCode)) ? ErrorCodes.UnexpectedError : defaultCode;
                ErrorText = $"[{e.GetType().Name}] {e.Message}";
            }
        }

        public ApiResponse(T result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            Result = result;
        }


        public bool Failed { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorText { get; set; }

        public T Result { get; set; }

        public override string ToString()
        {
            if (Failed)
            {
                return $"[{ErrorCode}] '{ErrorText}'";
            }

            return $"{Result}";
        }
    }

    public class ApiResponseException : ApplicationException
    {
        public ApiResponseException(string code, string text) : base(text)
        {
            ErrorCode = code;
        }

        public ApiResponseException(string code, string text, Exception e) : base(text, e)
        {
            ErrorCode = code;
        }

        public string ErrorCode { get; }

        public string ErrorText { get { return base.Message; } }
    }
}
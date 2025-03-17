using System.Text;
using Opc.Ua;
using ISession = Opc.Ua.Client.ISession;
using Opc.Ua.Server;

namespace UaRestGateway.Server.Service
{
    public interface IUACommunicationService : IHostedService
    {
        Task<ISession> FindSession(string id, string accessToken = "");

        StandardServer ServerApi { get; }

        UAClient ClientAPI { get; }
    }

    public class UACommunicationService : BackgroundService, IUACommunicationService
    {
        private readonly Dictionary<string, UAClient> m_sessions = new();
        private readonly GatewayServer m_server;
        private readonly UAClient m_client;
        private CancellationToken m_stoppingToken;

        protected IConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        public override void Dispose()
        {
            Utils.SilentDispose(m_server);
            //Utils.SilentDispose(m_client);
            base.Dispose();
        }

        public StandardServer ServerApi { get { return m_server.ServerApi; } }

        public UAClient ClientAPI { get { return m_client; } }

        public async Task<ISession> FindSession(string id, string accessToken = "")
        {
            UAClient client;

            lock (m_sessions)
            {
                if (m_sessions.TryGetValue((id ?? "") + accessToken, out client))
                {
                    return client.Session;
                }
            }

            UAClientSettings settings = new UAClientSettings()
            {
                ServerUrl = m_server.ServerEndpoints.OrderByDescending(x => x.SecurityLevel).Select(x => x.EndpointUrl).First(),
                AutoAccept = true,
                NoSecurity = false,
                UserIdentity = (!String.IsNullOrEmpty(accessToken)) 
                    ? new UserIdentity(
                        new IssuedIdentityToken()
                        {
                            IssuedTokenType = IssuedTokenType.JWT,
                            DecryptedTokenData = new UTF8Encoding(false).GetBytes(accessToken)
                        }) 
                    : new UserIdentity()
            };

            client = await UAClient.Run(Logger, settings, m_stoppingToken).ConfigureAwait(false);

            lock (m_sessions)
            {
                m_sessions[(id ?? "") + accessToken] = client;
            }

            return client.Session;
        }

        public UACommunicationService(
            IConfiguration configuration,
            ILogger<UACommunicationService> logger)
        {
            Configuration = configuration;
            Logger = logger;
            m_server = new GatewayServer();
            m_client = new UAClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                m_stoppingToken = stoppingToken;
                await m_server.Start(Logger, stoppingToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error starting OPC UA server.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

            try
            {
                var clients = new List<UAClient>();

                lock (m_sessions)
                {
                    clients.AddRange(m_sessions.Values);
                    m_sessions.Clear();
                }   

                foreach (var client in clients)
                {
                    client.Disconnect();
                    Utils.SilentDispose(client);
                }

                await m_server.Stop().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error stopping OPC UA server.");
                return;
            }
        }
    }
}

using Opc.Ua;
using Opc.Ua.Configuration;
using System.Reflection;

namespace UaRestGateway.Server.Service
{
    public class GatewayServer
    {
        private ApplicationInstance m_application;
        private GatewayServerManager m_server;

        public ApplicationDescription ApplicationDescription { get; private set; }

        public StringCollection ServerCapabilities { get; private set; }

        public EndpointDescriptionCollection ServerEndpoints { get; private set; }

        public GatewayServerManager ServerApi { get { return m_server; } }

        public async Task Start(ILogger logger, CancellationToken stoppingToken)
        {
            m_application = new ApplicationInstance
            {
                ApplicationName = "OPC UA WebApi Gateway",
                ApplicationType = ApplicationType.Server
            };

            // load the application configuration.
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configurationFile = Path.Combine(folder, "config", "uaserver-configuration.xml");
            var config = await m_application.LoadApplicationConfiguration("config/uaserver-configuration.xml", false).ConfigureAwait(false);

            // check the application certificate.
            bool haveAppCertificate = await m_application.CheckApplicationInstanceCertificate(
                false,
                CertificateFactory.DefaultKeySize,
                CertificateFactory.DefaultLifeTime).ConfigureAwait(false);

            if (!haveAppCertificate)
            {
                throw new Exception("Application instance certificate invalid!");
            }

            if (!config.SecurityConfiguration.AutoAcceptUntrustedCertificates)
            {
                config.CertificateValidator.CertificateValidation 
                    += new CertificateValidationEventHandler(
                        CertificateValidator_CertificateValidation);
            }

            // start the server.
            m_server = new GatewayServerManager();
            await m_application.Start(m_server).ConfigureAwait(false);

            // print endpoint info
            var endpoints = m_application.Server.GetEndpoints();

            ServerEndpoints = new EndpointDescriptionCollection();

            foreach (var endpoint in endpoints)
            {
                if (ApplicationDescription == null)
                {
                    ApplicationDescription = new ApplicationDescription()
                    {
                        ApplicationName = endpoint.Server.ApplicationName,
                        ApplicationUri = endpoint.Server.ApplicationUri,
                        ApplicationType = endpoint.Server.ApplicationType,
                        ProductUri = endpoint.Server.ProductUri,
                        DiscoveryProfileUri = endpoint.Server.DiscoveryProfileUri,
                        GatewayServerUri = endpoint.Server.GatewayServerUri,
                        DiscoveryUrls = endpoint.Server.DiscoveryUrls
                    };

                    ServerCapabilities = config.ServerConfiguration.ServerCapabilities;
                }

                ServerEndpoints.Add(endpoint);
            }
        }

        private void CertificateValidator_CertificateValidation(CertificateValidator sender, CertificateValidationEventArgs e)
        {
            e.Accept = true;
        }

        public Task Stop()
        {
            m_server.Stop();
            return Task.FromResult(0);
        }
    }
}

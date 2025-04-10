
using AasCore.Aas3_0;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace UaRestGateway.Server.Service.AAS
{
    public interface IAASCommunicationService: IHostedService
    {
        List<IAssetAdministrationShell> AssetAdministrationShells { get;}
        List<ISubmodel> Submodels { get; }
        List<IConceptDescription> ConceptDescriptions { get; }
    }
    public class AASCommunicationService : BackgroundService, IAASCommunicationService
    {
        private CancellationToken m_stoppingToken;
        protected IConfiguration Configuration { get; private set; }
        protected ILogger Logger { get; private set; }

        List<IAssetAdministrationShell> m_AssetAdministrationShells { get; set; }
        List<ISubmodel> m_Submodels { get; set; }
        List<IConceptDescription> m_ConceptDescriptions { get; set; }

        public List<IAssetAdministrationShell> AssetAdministrationShells { get { return m_AssetAdministrationShells; }}

        public List<ISubmodel> Submodels { get { return m_Submodels; } }

        public List<IConceptDescription> ConceptDescriptions { get { return m_ConceptDescriptions; } }

        public AASCommunicationService(IConfiguration configuration,
            ILogger<AASCommunicationService> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                m_stoppingToken = stoppingToken;
                Logger.LogDebug("AASCommunication Scoped Service Hosted Service running.");
                await LoadAASXFile();
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task LoadAASXFile()
        {
            Logger.LogDebug("AASCommunication Scoped Service Hosted Service is working");
            // load the application configuration.
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var aasxFile = Path.Combine(folder, "Resources", "TemplateAAS.aasx");

            var packaging = new AasCore.Aas3.Package.Packaging();
            using var pkgOrErr = packaging.OpenRead(aasxFile);

            if (pkgOrErr.MaybeException != null)
            {
                throw new ArgumentException(
                    "something went wrong", pkgOrErr.MaybeException);
            }

            var pkg = pkgOrErr.Must();
            // Read the specs
            var specsByContentType = pkg.SpecsByContentType();
            if (!specsByContentType.ContainsKey("text/xml"))
            {
                throw new ArgumentException("No json specs");
            }
            var spec = specsByContentType["text/xml"].First();
            byte[] specContent = spec.ReadAllBytes();

            AasCore.Aas3_0.Environment env;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            settings.CheckCharacters = false;

            using (var memoryStream = new MemoryStream(specContent))
            using (XmlReader xr = XmlReader.Create(memoryStream, settings))
            {
                env = Xmlization.Deserialize.EnvironmentFrom(xr);
                m_AssetAdministrationShells = env.AssetAdministrationShells;
                m_Submodels = env.Submodels;
                m_ConceptDescriptions = env.ConceptDescriptions;
                Logger.LogInformation("Total AASs found " + m_AssetAdministrationShells.Count);
            }
            
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("AASCommunication Scoped Service Hosted Service is starting");
            return base.StartAsync(cancellationToken);
        }
    }
}

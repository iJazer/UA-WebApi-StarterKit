
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
using UaRestGateway.Server.Exceptions.AAS;
using static System.Net.Mime.MediaTypeNames;

namespace UaRestGateway.Server.Service.AAS
{
    public interface IAASCommunicationService : IHostedService
    {
        List<IAssetAdministrationShell> AssetAdministrationShells { get; }
        List<ISubmodel> Submodels { get; }
        List<IConceptDescription> ConceptDescriptions { get; }

        IAssetAdministrationShell GetAssetAdministrationShellById(string decodedAasIdentifier);
        ISubmodel GetSubmodelByIdWithinAAS(string aasIdentifier, string submodelIdentifier);
        ISubmodelElement GetSubmodelElementByPathWithinAAS(string aasIdentifier, string submodelIdentifier, string idShortPath);
    }
    public class AASCommunicationService : BackgroundService, IAASCommunicationService
    {
        private CancellationToken m_stoppingToken;
        protected IConfiguration Configuration { get; private set; }
        protected ILogger Logger { get; private set; }

        List<IAssetAdministrationShell> m_AssetAdministrationShells { get; set; }
        List<ISubmodel> m_Submodels { get; set; }
        List<IConceptDescription> m_ConceptDescriptions { get; set; }

        public List<IAssetAdministrationShell> AssetAdministrationShells { get { return m_AssetAdministrationShells; } }

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

        public ISubmodel GetSubmodelByIdWithinAAS(string aasIdentifier, string submodelIdentifier)
        {
            var aas = m_AssetAdministrationShells.Find(a => a.Id.Equals(aasIdentifier));
            if (aas == null)
            {
                throw new NotFoundException($"AssetAdministrationShell with id {aasIdentifier} NOT found");
            }

            bool isSmPresentInAas = false;
            if (aas.Submodels != null)
            {
                isSmPresentInAas = aas.Submodels.Exists(s => s.Keys.First().Value.Equals(submodelIdentifier));
            }

            if (!isSmPresentInAas)
            {
                throw new NotFoundException($"No submodel with id {submodelIdentifier} present in AAS with id {aasIdentifier}");
            }

            var submodel = m_Submodels.Find(s => s.Id.Equals(submodelIdentifier));
            if (submodel == null)
            {
                throw new NotFoundException($"Submodel with id {submodelIdentifier} NOT found");
            }
            return submodel;
        }

        public IAssetAdministrationShell GetAssetAdministrationShellById(string aasIdentifier)
        {
            var aas = m_AssetAdministrationShells.Find(a => a.Id.Equals(aasIdentifier));
            if (aas == null)
            {
                throw new NotFoundException($"AssetAdministrationShell with id {aasIdentifier} NOT found");
            }
            return aas;
        }

        public ISubmodelElement GetSubmodelElementByPathWithinAAS(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            ISubmodelElement output = null;
            var submodel = GetSubmodelByIdWithinAAS(aasIdentifier, submodelIdentifier);

            

            if (submodel.SubmodelElements != null)
            {
                output = GetSubmodelElement(submodel.SubmodelElements, idShortPath);
                
            }

            if(output == null)
            {
                throw new NotFoundException($"Submodel Element with idShortPath {idShortPath} NOT found.");
            }

            return output;
        }

        private ISubmodelElement GetSubmodelElement(List<ISubmodelElement> submodelElements, string idShortPath)
        {
            ISubmodelElement output = null;
            if (!idShortPath.Contains("."))
            {
                //Submodel element on the hierarchy level 1
                output = submodelElements.Find(sme => sme.IdShort.Equals(idShortPath));
            }
            else
            {
                var splitIdShorts = idShortPath.Split('.', 2);
                var parentIdShort = splitIdShorts[0];
                var childIdShort = splitIdShorts[1];
                var parent = submodelElements.Find(sme => sme.IdShort.Equals(parentIdShort));
                if (parent != null)
                {
                    if(parent is SubmodelElementCollection parentSmc)
                    {
                        output = GetSubmodelElement(parentSmc.Value, childIdShort);
                    }
                    else
                    {
                        throw new Exception("Parent SubmodelElement not supported.");
                    }
                }
            }

            if (output == null)
            {
                throw new NotFoundException($"Submodel Element with idShortPath {idShortPath} NOT found.");
            }

            return output;
        }
    }
}

using AasCore.Aas3_0;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using UaRestGateway.Server.Exceptions.AAS;
using UaRestGateway.Server.Model;
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
        Task<ISubmodelElement> GetSubmodelElementByPathWithinAASAsync(string aasIdentifier, string submodelIdentifier, string idShortPath);
        Task<(ISubmodelElement Element, bool IsOpcUaBacked, NodeId NodeId)> GetSubmodelElementInfoAsync(string aasIdentifier, string submodelIdentifier, string idShortPath);
    }
    public class AASCommunicationService : BackgroundService, IAASCommunicationService
    {
        Random random = new Random();
        private CancellationToken m_stoppingToken;
        private readonly IUACommunicationService m_uaCommunicationService;
        private readonly IUACommunicationService uaCommunicationService;
        private UAClient OpcUaClient => m_uaCommunicationService.ClientAPI;

        protected IConfiguration Configuration { get; private set; }
        protected ILogger Logger { get; private set; }

        List<IAssetAdministrationShell> m_AssetAdministrationShells { get; set; }
        List<ISubmodel> m_Submodels { get; set; }
        List<IConceptDescription> m_ConceptDescriptions { get; set; }

        public List<IAssetAdministrationShell> AssetAdministrationShells { get { return m_AssetAdministrationShells; } }

        public List<ISubmodel> Submodels { get { return m_Submodels; } }

        public List<IConceptDescription> ConceptDescriptions { get { return m_ConceptDescriptions; } }

        public AASCommunicationService(IConfiguration configuration,
            ILogger<AASCommunicationService> logger,
            IUACommunicationService uaCommunicationService)
        {
            Configuration = configuration;
            Logger = logger;
            m_uaCommunicationService = uaCommunicationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                m_stoppingToken = stoppingToken;
                Logger.LogDebug("AASCommunication Scoped Service Hosted Service running.");
                await LoadAASXFile().ConfigureAwait(false);
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
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var aasxFile = Path.Combine(folder, "Resources", "TemplateAAS.aasx");

            var packaging = new AasCore.Aas3.Package.Packaging();
            using var pkgOrErr = await Task.Run(() => packaging.OpenRead(aasxFile));

            if (pkgOrErr.MaybeException != null)
            {
                throw new ArgumentException("something went wrong", pkgOrErr.MaybeException);
            }

            var pkg = pkgOrErr.Must();
            var specsByContentType = pkg.SpecsByContentType();
            if (!specsByContentType.ContainsKey("text/xml"))
            {
                throw new ArgumentException("No json specs");
            }
            var spec = specsByContentType["text/xml"].First();
            byte[] specContent = await Task.Run(() => spec.ReadAllBytes());

            AasCore.Aas3_0.Environment env;

            XmlReaderSettings settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true,
                CheckCharacters = false
            };

            using (var memoryStream = new MemoryStream(specContent))
            using (XmlReader xr = XmlReader.Create(memoryStream, settings))
            {
                env = await Task.Run(() => Xmlization.Deserialize.EnvironmentFrom(xr));
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

        public async Task<ISubmodelElement> GetSubmodelElementByPathWithinAASAsync(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var (element, isOpcUa, nodeId) = await GetSubmodelElementInfoAsync(aasIdentifier, submodelIdentifier, idShortPath).ConfigureAwait(false);

            if (element == null)
            {
                throw new NotFoundException($"Submodel Element with idShortPath {idShortPath} NOT found.");
            }

            if (isOpcUa && element is Property prop)
            {
                var value = GetOpcUaNodeValue(nodeId);
                prop.Value = value;
            }

            return element;
        }

        public async Task<(ISubmodelElement Element, bool IsOpcUaBacked, NodeId NodeId)> GetSubmodelElementInfoAsync(
            string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var submodel = GetSubmodelByIdWithinAAS(aasIdentifier, submodelIdentifier);

            if (submodel.SubmodelElements == null)
                throw new NotFoundException("Submodel has no elements");

            var element = GetSubmodelElement(submodel.SubmodelElements, idShortPath);
            if (element == null)
                throw new NotFoundException($"Element not found: {idShortPath}");

            bool isOpcUa = GetExtensionValue(element);
            NodeId nodeId = null;

            if (isOpcUa)
            {
                // Await the task to retrieve the NodeId before assigning it
                nodeId = await FindOpcUaNodeId(submodel.IdShort, idShortPath).ConfigureAwait(false);
            }

            return (element, isOpcUa, nodeId);
        }


        private ISubmodelElement GetSubmodelElement(List<ISubmodelElement> submodelElements, string idShortPath)
        {
            ISubmodelElement output = null;
            if (!idShortPath.Contains("."))
            {
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

        private string GetOpcUaNodeValue(NodeId smeNodeId)
        {
            return ReadOpcUaNodeValue(smeNodeId);
        }

        private async Task<NodeId> FindOpcUaNodeId(string submodelIdShort, string idShortPath)
        {
            if (OpcUaClient == null)
            {
                throw new Exception("OpcUaClient is not initialized. Cannot get OpcUa node value.");
            }

            var session = OpcUaClient.Session;

            if(session == null  || !session.Connected)
            {     
                bool connected = await OpcUaClient.ConnectAsync(m_stoppingToken).ConfigureAwait(false);
                if(connected)
                    session = OpcUaClient.Session;
                else
                    throw new Exception("OpcUa session is not connected. Cannot get OpcUa node value.");
            }

            //Get AAS Environment root node
            NodeId aasEnvNodeId = GetNodeIdByTypeDefinition((Session)session, ObjectIds.ObjectsFolder, AasToOpcUaTypeDefinitions.Mappings["Environment"]);

            //Get AAS node
            if (aasEnvNodeId == null)
            {
                throw new NotFoundException("AAS Environment node not found.");
            }

            NodeId aasNodeId = GetNodeIdByTypeDefinition((Session)session, aasEnvNodeId, AasToOpcUaTypeDefinitions.Mappings["AssetAdministrationShell"]);
            if (aasNodeId == null)
            {
                throw new NotFoundException("AAS node not found.");
            }

            NodeId submodelNodeId = GetNodeIdByTypeDefinition((Session)session, aasNodeId, AasToOpcUaTypeDefinitions.Mappings[submodelIdShort]);
            if (submodelNodeId == null)
            {
                throw new NotFoundException("Submodel node not found.");
            }

            //Browse OpcUa Node for the given idShortPath
            NodeId smeNodeId = GetSmeNodeIdByIdShortPath((Session)session, submodelNodeId, idShortPath);
            if (smeNodeId == null)
            {
                throw new NotFoundException($"OPC UA Node corresponding to Submodel Element with idShortPath {idShortPath} NOT found.");
            }

            return smeNodeId;
        }

        private string ReadOpcUaNodeValue(NodeId nodeId)
        {
            if (OpcUaClient == null)
            {
                throw new Exception("OpcUaClient is not initialized. Cannot get OpcUa node value.");
            }

            var session = OpcUaClient.Session;
            var value = session.ReadValue(nodeId).Value;
            if (value == null)
            {
                throw new NotFoundException($"Value for OpcUa Node {nodeId} not found.");
            }
            return value.ToString();
        }

        private NodeId GetSmeNodeIdByIdShortPath(Session session, NodeId parentNodeId, string idShortPath)
        {
            NodeId outputNodeId = null;
            if (!idShortPath.Contains("."))
            {
                //As this is the last part of the idShortPath, we can directly get the nodeId based on BrowseName
                outputNodeId = GetNodeIdByBrowseName(session, parentNodeId, idShortPath);
            }
            else
            {
                var splitIdShorts = idShortPath.Split('.', 2);
                var parentIdShort = splitIdShorts[0];
                var childIdShort = splitIdShorts[1];

                parentNodeId = GetNodeIdByTypeDefinition(session, parentNodeId, AasToOpcUaTypeDefinitions.Mappings[parentIdShort]);
                if(parentNodeId == null)
                {
                    throw new NotFoundException($"ParentNode for SubmodelElement with idShort {parentIdShort} NOT found.");
                }

                outputNodeId = GetSmeNodeIdByIdShortPath(session, parentNodeId, childIdShort);

            }

            return outputNodeId; 
        }

        private NodeId GetNodeIdByBrowseName(Session session, NodeId root, string idShortPath)
        {
            ReferenceDescriptionCollection refs;
            byte[] cp;
            session.Browse(
                null,
                null,
                root,
                0u,
                BrowseDirection.Forward,
                ReferenceTypeIds.HierarchicalReferences,
                true,
                0, // All NodeClasses
                out cp,
                out refs);

            foreach (var rd in refs)
            {
                NodeId rdNodeId = ExpandedNodeId.ToNodeId(rd.NodeId, session.NamespaceUris);
                //Check BrowseName for idShortPath
                if (rd.BrowseName != null && rd.BrowseName.Name.Equals(idShortPath))
                {
                    //Console.WriteLine($"Found required node: {rd.DisplayName.Text}");
                    return rdNodeId; // Return the AAS node ID
                }
            }
            return null; // If not found, return null
        }

        private NodeId GetNodeIdByTypeDefinition(Session session, NodeId root, NodeId typeDefId)
        {
            ReferenceDescriptionCollection refs;
            byte[] cp;
            session.Browse(
                null,
                null,
                root,
                0u,
                BrowseDirection.Forward,
                ReferenceTypeIds.HierarchicalReferences,
                true,
                0, // All NodeClasses
                out cp,
                out refs);

            foreach (var rd in refs)
            {
                NodeId rdNodeId = ExpandedNodeId.ToNodeId(rd.NodeId, session.NamespaceUris);
                // ✅ Use HasTypeDefinition to check type
                NodeId typeDef = GetTypeDefinition(session, rdNodeId);
                if (typeDef != null && NodeId.Equals(typeDef, typeDefId))
                {
                    //Console.WriteLine($"Found required node: {rd.DisplayName.Text}");
                    return rdNodeId; // Return the AAS node ID
                }
            }
            return null; // If not found, return null
        }

        private NodeId GetTypeDefinition(Session session, NodeId nodeId)
        {
            session.Browse(
                null,
                null,
                nodeId,
                0u,
                BrowseDirection.Forward,
                ReferenceTypeIds.HasTypeDefinition,
                true,
                0u, // No node class filtering!
                out byte[] cp,
                out ReferenceDescriptionCollection refs);

            if (refs != null && refs.Count > 0)
            {
                return ExpandedNodeId.ToNodeId(refs[0].NodeId, session.NamespaceUris);
            }

            return null;
        }

        private bool GetExtensionValue(ISubmodelElement output)
        {
            if(output.Extensions == null || output.Extensions.Count == 0)
            {
                return false;
            }

            foreach (var ext in output.Extensions)
            {
                if (ext.Name.Equals("OpcUa"))
                {
                    if (ext.Value.Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if (ext.Value.Equals("False", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    else
                    {
                        throw new Exception($"Invalid OpcUa extension value: {ext.Value}");
                    }
                }
            }

            return false;
        }
    }
}

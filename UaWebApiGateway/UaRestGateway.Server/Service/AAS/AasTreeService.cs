using AasCore.Aas3_0; // Import AAS model
using System.Collections.Generic;
using System.Linq;
using UaRestGateway.Server.Model.AAS;
using static Opc.Ua.RelativePathFormatter;
namespace UaRestGateway.Server.Service.AAS
{
    public interface IAasTreeService
    {
        AasTreeNode ConvertAasToTree(AssetAdministrationShell aas);
    }

    public class AasTreeService : IAasTreeService
    {
        private readonly IAASCommunicationService _aasCommunicationService;
        private readonly ILogger<AasTreeService> _logger;

        public AasTreeService(IAASCommunicationService aasCommunicationService, ILogger<AasTreeService> logger)
        {
            _aasCommunicationService = aasCommunicationService;
            _logger = logger;
        }
        public AasTreeNode ConvertAasToTree(AssetAdministrationShell aas)
        {
            var rootNode = new AasTreeNode
            {
                Id = aas.Id,
                Name = $"AAS: {aas.IdShort}",
                Link = aas.Id // If there's a link for AAS
            };

            // Convert Submodels
            if (aas.Submodels != null)
            {
                List<AasTreeNode> smTreeNodes = new List<AasTreeNode>();
                foreach (var smReference in aas.Submodels)
                {
                    var submodel = _aasCommunicationService.Submodels.Find(s => smReference.Keys.First().Value.Equals(s.Id));
                    smTreeNodes.Add(ConvertSubmodelToTree(submodel));
                }
                rootNode.Children = smTreeNodes;
            }

            return rootNode;
        }

        private AasTreeNode ConvertSubmodelToTree(ISubmodel submodel)
        {
            var submodelNode = new AasTreeNode
            {
                Id = submodel.Id,
                Name = $"{GetModelTypeAbbriviations(submodel.GetType().Name)}: {submodel.IdShort}",
                Link = submodel.Id
            };

            // Convert Submodel Elements (Properties, References, etc.)
            if (submodel.SubmodelElements != null)
            {
                submodelNode.Children = submodel.SubmodelElements
                    .Select(element => ConvertElementToTree(element))
                    .ToList();
            }

            return submodelNode;
        }

        private AasTreeNode ConvertElementToTree(ISubmodelElement element)
        {
            var elementNode = new AasTreeNode
            {
                Id = element.IdShort,
                Name = $"{GetModelTypeAbbriviations(element.GetType().Name)}: {element.IdShort}"
            };

            // If element is a Collection, add children recursively
            if (element is SubmodelElementCollection collection && collection.Value != null)
            {
                elementNode.Children = collection.Value
                    .Select(subElement => ConvertElementToTree(subElement))
                    .ToList();
            }

            return elementNode;
        }

        private string GetModelTypeAbbriviations(string modelType)
        {
            switch (modelType)
            {
                case "Submodel": return "SM";
                case "Property": return "Prop";
                case "MultiLanguageProperty": return "MLP";
                case "Range": return "Range";
                case "Blob": return "Blob";
                case "File": return "File";
                case "ReferenceElement": return "Ref";
                case "RelationshipElement": return "Rel";
                case "AnnotatedRelationshipElement": return "RelA";
                case "BasicEventElement": return "Evt";
                case "Capability": return "Cap";
                case "Operation": return "Opr";
                case "SubmodelElementCollection": return "SMC";
                case "SubmodelElementList": return "SML";
                case "Entity": return "Ent";
                default:
                    {
                        _logger.LogError("Abbreiviation for AAS ModelType " + modelType + "not supported");
                        return "Unk";
                    }
            }
        }
    }

}

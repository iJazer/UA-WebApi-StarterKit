using Opc.Ua;

namespace UaRestGateway.Server.Service.AAS
{
    public static class AasToOpcUaTypeDefinitions
    {
        public static readonly Dictionary<string, NodeId> Mappings = new Dictionary<string, NodeId>()
            {
                { "Environment",  new NodeId(1004, 4)},
                { "AssetAdministrationShell",  new NodeId(1006, 4)},
                { "Submodel",  new NodeId(1012, 4)},
                { "CarbonFootprint",  new NodeId(1003, 2)},
                { "ProductCarbonFootprint",  new NodeId(1002, 2)}
            };
    }
}

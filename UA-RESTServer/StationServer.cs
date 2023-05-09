
namespace Ua.Rest.Server
{
    using Opc.Ua;
    using Opc.Ua.Server;
    using System.Collections.Generic;

    public partial class FactoryStationServer : StandardServer
    {
        public static StationNodeManager? NodeManagerInstance { get; private set; }

        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            List<INodeManager> nodeManagers = new List<INodeManager>();

            NodeManagerInstance = new StationNodeManager(server, configuration);

            nodeManagers.Add(NodeManagerInstance);

            return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
        }

        protected override ServerProperties LoadServerProperties()
        {
            ServerProperties properties = new ServerProperties
            {
                ManufacturerName = "OPC Foundation",
                ProductName = "Factory Station Simulation",
                ProductUri = "",
                SoftwareVersion = Utils.GetAssemblySoftwareVersion(),
                BuildNumber = Utils.GetAssemblyBuildNumber(),
                BuildDate = Utils.GetAssemblyTimestamp()
            };

            return properties;
        }
    }
}


namespace Ua.Rest.Server
{
    using Opc.Ua;
    using Opc.Ua.Export;
    using Opc.Ua.Server;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    public enum StationStatus : int
    {
        Ready = 0,
        WorkInProgress = 1,
        Done = 2,
        Discarded = 3,
        Fault = 4
    }

    public class StationNodeManager : CustomNodeManager2
    {
        private const ulong c_idealCycleTimeDefault = 5 * 1000; // [ms]
        private const ulong c_pressureDefault = 2500;           // [mbar]
        private DateTime m_cycleStartTime = DateTime.UtcNow;

        private Stopwatch m_faultClock = new Stopwatch();
        private Timer m_simulationTimer;

        private Random m_random = new Random();

        private ushort m_namespaceIndex;
        private long m_lastUsedId;

        public Dictionary<string, BaseDataVariableState> UANodes { get; private set; } = new();

        public StationNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            List<string> namespaceUris = new List<string>
            {
                "http://opcfoundation.org/UA/Station/"
            };

            NamespaceUris = namespaceUris;

            m_namespaceIndex = Server.NamespaceUris.GetIndexOrAppend(namespaceUris[0]);
            m_lastUsedId = 0;

            m_simulationTimer = new Timer(SimulationFinished, null, (int)c_idealCycleTimeDefault, (int)c_idealCycleTimeDefault);
            m_faultClock.Reset();
        }

        public override NodeId New(ISystemContext context, NodeState node)
        {
            return new NodeId(Utils.IncrementIdentifier(ref m_lastUsedId), m_namespaceIndex);
        }

        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                IList<IReference>? references = null;
                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                ImportNodeset2Xml(externalReferences, "Station.NodeSet2.xml");

                AddReverseReferences(externalReferences);
            }

            // set initial values
            UANodes["ProductSerialNumber"].Value = 1;
            UANodes["NumberOfManufacturedProducts"].Value = 0;
            UANodes["NumberOfDiscardedProducts"].Value = 0;
            UANodes["OverallRunningTime"].Value = TimeSpan.Zero;
            UANodes["FaultyTime"].Value = TimeSpan.Zero;
            UANodes["Status"].Value = StationStatus.Ready;
            UANodes["EnergyConsumption"].Value = 0.0f;
            UANodes["Pressure"].Value = c_pressureDefault;
            UANodes["IdealCycleTime"].Value = c_idealCycleTimeDefault;
            UANodes["ActualCycleTime"].Value = c_idealCycleTimeDefault;
    }

        // Nodeset2 files can be edited using e.g. the SIEMENS OPC UA Modeling Editor (SiOME)
        // see https://support.industry.siemens.com/cs/document/109755133/siemens-opc-ua-modeling-editor-(siome)-for-implementing-opc-ua-companion-specification
        private void ImportNodeset2Xml(IDictionary<NodeId, IList<IReference>> externalReferences, string resourcepath)
        {
            NodeStateCollection predefinedNodes = new NodeStateCollection();

            Stream stream = new FileStream(resourcepath, FileMode.Open);
            UANodeSet nodeSet = UANodeSet.Read(stream);

            foreach (string namespaceUri in nodeSet.NamespaceUris)
            {
                SystemContext.NamespaceUris.GetIndexOrAppend(namespaceUri);
            }

            nodeSet.Import(SystemContext, predefinedNodes);

            for (int i = 0; i < predefinedNodes.Count; i++)
            {
                AddPredefinedNode(SystemContext, predefinedNodes[i]);
            }
        }

        protected override NodeState AddBehaviourToPredefinedNode(ISystemContext context, NodeState predefinedNode)
        {
            // add behaviour to our methods
            MethodState? methodState = predefinedNode as MethodState;
            if (( methodState != null) && (methodState.ModellingRuleId == null))
            {
                if (methodState.DisplayName == "OpenPressureReleaseValve")
                {
                    methodState.OnCallMethod = new GenericMethodCalledEventHandler(OpenPressureReleaseValve);
                }
            }

            // also capture the nodeIDs of our instance variables (i.e. NOT the model!)
            BaseDataVariableState? variableState = predefinedNode as BaseDataVariableState;
            if ((variableState != null) && (variableState.ModellingRuleId == null))
            {
                UANodes.Add(variableState.DisplayName.Text, variableState);
            }

            return predefinedNode;
        }

        private void SimulationFinished(object? state)
        {
            CalculateSimulationResult();

            UpdateNodeValues();

            m_cycleStartTime = DateTime.UtcNow;
        }

        private ServiceResult OpenPressureReleaseValve(ISystemContext context, MethodState method, IList<object> inputArguments, IList<object> outputArguments)
        {
            UANodes["Pressure"].Value = c_pressureDefault;

            UpdateNodeValues();

            return ServiceResult.Good;
        }

        private void UpdateNodeValues()
        {
            UANodes["OverallRunningTime"].Value = (TimeSpan)UANodes["OverallRunningTime"].Value + TimeSpan.FromMilliseconds((ulong)UANodes["ActualCycleTime"].Value);

            foreach (BaseDataVariableState variable in UANodes.Values)
            {
                variable.Timestamp = DateTime.UtcNow;
                variable.ClearChangeMasks(SystemContext, false);
            }
        }

        public virtual void CalculateSimulationResult()
        {
            bool productDiscarded = (NormalDistribution(m_random, 0.0, 1.0) > 2.0);

            if (productDiscarded)
            {
                UANodes["Status"].Value = StationStatus.Discarded;
                UANodes["NumberOfDiscardedProducts"].Value = (int)UANodes["NumberOfDiscardedProducts"].Value + 1;
            }
            else
            {
                UANodes["Status"].Value = StationStatus.Done;
                UANodes["NumberOfManufacturedProducts"].Value = (int)UANodes["NumberOfManufacturedProducts"].Value + 1;
            }

            UANodes["ActualCycleTime"].Value = (ulong)(DateTime.UtcNow - m_cycleStartTime).TotalMilliseconds;

            // The power consumption of the station increases exponentially if the ideal cycle time is reduced below the default ideal cycle time
            double cycleTimeModifier = (1 / Math.E) * (1 / Math.Exp(-(double)c_idealCycleTimeDefault / c_idealCycleTimeDefault));
            double powerConsumption = 2000 * cycleTimeModifier;

            // assume the station consumes only power during the active cycle
            // energy consumption [kWh] = (PowerConsumption [kW] * actualCycleTime [s]) / 3600
            UANodes["EnergyConsumption"].Value = powerConsumption * ((ulong)UANodes["ActualCycleTime"].Value / 1000.0) / 3600.0;

            // slowly increase pressure until c_pressureHigh is reached
            UANodes["Pressure"].Value = (ulong)((ulong)UANodes["Pressure"].Value + Math.Abs(NormalDistribution(m_random, (cycleTimeModifier - 1.0) * 10.0, 10.0)));

            // keep pressure within our bounds
            if ((ulong)UANodes["Pressure"].Value < c_pressureDefault)
            {
                UANodes["Pressure"].Value = c_pressureDefault;
            }
        }

        private double NormalDistribution(Random rand, double mean, double stdDev)
        {
            // convert a generic normal distribution function f(x) to a standard
            // normal distribution (a normal distribution with mean=0 and stdDev=1) with the
            // following formula:
            //
            //  z = (x - mean) / stdDev
            //
            // then with z value you can retrieve the probability value P(X>x) from the standard
            // normal distribution table

            // these are uniform(0,1) random doubles
            double u1 = rand.NextDouble();
            double u2 = rand.NextDouble();

            // random normal(0,1)
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            // random normal(mean,stdDev^2)
            return mean + stdDev * randStdNormal;
        }
    }
}

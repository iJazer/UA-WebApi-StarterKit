/* ========================================================================
 * Copyright (c) 2005-2023 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

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
        private const UInt64 c_idealCycleTimeDefault = 5 * 1000; // [ms]
        private const double c_pressureDefault = 2500;           // [mbar]
        private DateTime m_cycleStartTime = DateTime.UtcNow;

        private Stopwatch m_faultClock = new Stopwatch();
        private Timer m_simulationTimer;
        private Random m_random = new Random();
        private long m_lastUsedId = 0;

        public Dictionary<string, BaseDataVariableState> UAVariableNodes { get; private set; } = new();
        public Dictionary<string, MethodState> UAMethodNodes { get; private set; } = new();

        public List<NodeState>? NodeStates { get; private set; } = null;

        public NodeState? RootNode { get; private set; } = null;

        public StationNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            Server.NamespaceUris.Append("http://opcfoundation.org/UA/Station/");
            NamespaceUris = Server.NamespaceUris.ToArray();

            m_simulationTimer = new Timer(SimulationFinished, null, (int)c_idealCycleTimeDefault, (int)c_idealCycleTimeDefault);
            m_faultClock.Reset();
        }

        public override NodeId New(ISystemContext context, NodeState node)
        {
            return new NodeId(Utils.IncrementIdentifier(ref m_lastUsedId), (ushort)Server.NamespaceUris.GetIndex("http://opcfoundation.org/UA/Station/"));
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

            NodeStates = PredefinedNodes.Values.ToList();

            // capture root node
            foreach (NodeState predefinedNode in PredefinedNodes.Values)
            {
                BaseObjectState? folder = predefinedNode as BaseObjectState;
                if (folder != null)
                {
                    Dictionary<NodeId, string> hierarchy = new();
                    List<NodeStateHierarchyReference> references = new();
                    folder.GetHierarchyReferences(Server.DefaultSystemContext, "", hierarchy, references);
                    foreach (NodeStateHierarchyReference reference in references)
                    {
                        if (reference.TargetId == ObjectIds.ObjectsFolder)
                        {
                            RootNode = folder;
                        }
                    }
                }
            }

            // set initial values
            UAVariableNodes["ProductSerialNumber"].Value = (UInt64)1;
            UAVariableNodes["NumberOfManufacturedProducts"].Value = (UInt64)0;
            UAVariableNodes["NumberOfDiscardedProducts"].Value = (UInt64)0;
            UAVariableNodes["OverallRunningTime"].Value = (UInt64)0;
            UAVariableNodes["FaultyTime"].Value = (UInt64)0;
            UAVariableNodes["Status"].Value = StationStatus.Ready;
            UAVariableNodes["EnergyConsumption"].Value = 0.0;
            UAVariableNodes["Pressure"].Value = c_pressureDefault;
            UAVariableNodes["IdealCycleTime"].Value = c_idealCycleTimeDefault;
            UAVariableNodes["ActualCycleTime"].Value = c_idealCycleTimeDefault;
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
                UAMethodNodes.Add(methodState.DisplayName.Text, methodState);

                // hook up our method
                if (methodState.DisplayName == "OpenPressureReleaseValve")
                {
                    methodState.OnCallMethod = new GenericMethodCalledEventHandler(OpenPressureReleaseValve);
                }
            }

            // capture the nodeIDs of our instance variables (i.e. NOT the model!)
            BaseDataVariableState? variableState = predefinedNode as BaseDataVariableState;
            if ((variableState != null) && (variableState.ModellingRuleId == null))
            {
                UAVariableNodes.Add(variableState.DisplayName.Text, variableState);
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
            UAVariableNodes["Pressure"].Value = c_pressureDefault;

            UpdateNodeValues();

            return ServiceResult.Good;
        }

        private void UpdateNodeValues()
        {
            UAVariableNodes["OverallRunningTime"].Value = (UInt64)UAVariableNodes["OverallRunningTime"].Value + (UInt64)UAVariableNodes["ActualCycleTime"].Value;

            foreach (BaseDataVariableState variable in UAVariableNodes.Values)
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
                UAVariableNodes["Status"].Value = StationStatus.Discarded;
                UAVariableNodes["NumberOfDiscardedProducts"].Value = (UInt64)UAVariableNodes["NumberOfDiscardedProducts"].Value + 1;
            }
            else
            {
                UAVariableNodes["Status"].Value = StationStatus.Done;
                UAVariableNodes["NumberOfManufacturedProducts"].Value = (UInt64)UAVariableNodes["NumberOfManufacturedProducts"].Value + 1;
            }

            UAVariableNodes["ActualCycleTime"].Value = (UInt64)(DateTime.UtcNow - m_cycleStartTime).TotalMilliseconds;

            // The power consumption of the station increases exponentially if the ideal cycle time is reduced below the default ideal cycle time
            double cycleTimeModifier = (1 / Math.E) * (1 / Math.Exp(-(double)c_idealCycleTimeDefault / c_idealCycleTimeDefault));
            double powerConsumption = 2000 * cycleTimeModifier;

            // assume the station consumes only power during the active cycle
            // energy consumption [kWh] = (PowerConsumption [kW] * actualCycleTime [s]) / 3600
            UAVariableNodes["EnergyConsumption"].Value = powerConsumption * ((UInt64)UAVariableNodes["ActualCycleTime"].Value / 1000.0) / 3600.0;

            // slowly increase pressure until c_pressureHigh is reached
            UAVariableNodes["Pressure"].Value = (double)((double)UAVariableNodes["Pressure"].Value + Math.Abs(NormalDistribution(m_random, (cycleTimeModifier - 1.0) * 10.0, 10.0)));

            // keep pressure within our bounds
            if ((double)UAVariableNodes["Pressure"].Value < c_pressureDefault)
            {
                UAVariableNodes["Pressure"].Value = c_pressureDefault;
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

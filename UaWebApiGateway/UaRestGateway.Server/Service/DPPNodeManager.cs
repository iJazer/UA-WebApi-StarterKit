/* ========================================================================
 * Copyright (c) 2005-2020 The OPC Foundation, Inc. All rights reserved.
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

using BatteryPassport;
using Opc.Ua;
using Opc.Ua.Server;
using System.Reflection;
using StatusCodes = Opc.Ua.StatusCodes;

namespace UaRestGateway.Server.Service
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class DPPNodeManager : CustomNodeManager2
    {
        #region Private Fields
        private Dictionary<NodeId, FileManager> m_fileManagers = new();
        private ushort InstanceNamespaceIndex = 0;
        private uint m_counter = 0;
        private Timer m_timer = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public DPPNodeManager(IServerInternal server, ApplicationConfiguration configuration)
            :
            base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;
            SetNamespaces([BatteryPassport.Namespaces.BatteryPassport]);
            InstanceNamespaceIndex = NamespaceIndexes[0];

            Server.MessageContext.Factory.AddEncodeableTypes(typeof(BatteryPassport.BatteryPassportDataType).GetTypeInfo().Assembly);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (Lock)
                {
                    foreach (var manager in m_fileManagers.Values)
                    {
                        manager.Dispose();
                    }

                    m_fileManagers.Clear();
                    m_timer?.Dispose();
                }
            }
        }
        #endregion

        #region INodeIdFactory Members
        /// <summary>
        /// Creates the NodeId for the specified node.
        /// </summary>
        public override NodeId New(ISystemContext context, NodeState node)
        {
            BaseInstanceState instance = node as BaseInstanceState;

            if (instance != null && instance.Parent != null)
            {
                string id = instance.Parent.NodeId.Identifier as string;

                if (id != null)
                {
                    return new NodeId(id + "_" + instance.SymbolicName, instance.Parent.NodeId.NamespaceIndex);
                }
                else
                {
                    return new NodeId(++m_counter, instance.Parent.NodeId.NamespaceIndex);
                }
            }

            return node.NodeId;
        }
        #endregion

        #region INodeManager Members
        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <remarks>
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.  
        /// </remarks>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                LoadPredefinedNodes(SystemContext, externalReferences);

                var root = Find(ExpandedNodeId.ToNodeId(BatteryPassport.ObjectIds.Batteries, Server.NamespaceUris));

                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var password = CreatePassport(root as BaseObjectState, Path.Join(folder, "Data"));

                if (m_timer != null)
                {
                    m_timer.Dispose();
                    m_timer = null;
                }

                m_timer = new Timer(DoSimulation, null, 1000, 1000);
            }
        }

        private void DoSimulation(object state)
        {

            try
            {
                lock (Lock)
                {
                    var root = Find(ExpandedNodeId.ToNodeId(BatteryPassport.ObjectIds.Batteries, Server.NamespaceUris));

                    List<BaseInstanceState> children = new();
                    root.GetChildren(SystemContext, children);

                    foreach (var child in children)
                    {
                        var battery = child as BatteryState;

                        if (battery == null)
                        {
                            continue;
                        }

                        battery.CurrentState.Voltage.Value += (Random.Shared.NextDouble() - 0.05); 
                        battery.CurrentState.PowerIn.Value += 0;
                        battery.CurrentState.PowerOut.Value += (Random.Shared.NextDouble() - 0.05);
                        battery.CurrentState.ChargeRemaining.Value += (Random.Shared.NextDouble() - 0.05);
                        battery.CurrentState.Temperature.Value += (Random.Shared.NextDouble() - 0.05);
                        battery.CurrentState.TimeRemaining.Value += (Random.Shared.NextDouble() - 0.05);

                        if (battery.CurrentState.ChargeRemaining.Value > 100)
                        {
                            battery.CurrentState.ChargeRemaining.Value = 75;
                        }

                        // notifies any monitored items that the value has changed.
                        battery.ClearChangeMasks(SystemContext, true);
                    }
                }

            }
            catch (Exception e)
            { 
               Utils.Trace($"Error in simulation [{e.GetType().Name}] {e.Message}");
            }
        }

        private BatteryState CreatePassport(
            BaseObjectState root,
            string rootFolder)
        {
            var passportFilePath = Path.Join(
                rootFolder,
                "BatteryPassportSample.json");

            BatteryPassportDataType passport = new();
            var json = File.ReadAllText(passportFilePath);

            ServiceMessageContext context = new ServiceMessageContext();
            context.Factory = Server.Factory;

            using (var decoder = new JsonDecoder(json, context))
            {
                passport.Decode(decoder);
            }

            var name = passport.GeneralBatteryAndManufacturerInformation.BatteryUniqueIdentifier;

            int index = name.LastIndexOf(':');
            if (index > 0)
            {
                name = name.Substring(index + 1);
            }

            var battery = new BatteryState(root);

            battery.Create(
                SystemContext,
                new NodeId(++m_counter, InstanceNamespaceIndex),
                new QualifiedName(name, InstanceNamespaceIndex),
                null,
                true);

            battery.CurrentState.Voltage.Value = 1250;
            battery.CurrentState.PowerIn.Value = 0.00;
            battery.CurrentState.PowerOut.Value = 25.000;
            battery.CurrentState.ChargeRemaining.Value = 75.000;
            battery.CurrentState.Temperature.Value = 25.0;
            battery.CurrentState.TimeRemaining.Value = 34.000;

            InitializeChildFromData(passport, battery.Passport, rootFolder);
            root.AddChild(battery);

            FileManager manager = new FileManager(rootFolder + ".aasx", battery.Package);
            m_fileManagers.Add(battery.Package.NodeId, manager);

            // store it and all of its children in the pre-defined nodes dictionary for easy look up.
            AddPredefinedNode(SystemContext, battery);

            return battery;
        }

        private void InitializeChildFromData(IEncodeable instance, NodeState node, string rootFolder)
        {
            foreach (var property in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var nodeProperty = node.GetType().GetProperty(property.Name);

                if (nodeProperty == null)
                {
                    continue;
                }

                var value = property.GetValue(instance);

                if (value == null)
                {
                    continue;
                }

                if (value is LocalizedText text && LocalizedText.IsNullOrEmpty(text))
                {
                    continue;
                }

                var child = nodeProperty.GetValue(node);

                if (child == null)
                {
                    if (!nodeProperty.PropertyType.IsSubclassOf(typeof(BaseInstanceState)))
                    {
                        continue;
                    }

                    var newChild = Activator.CreateInstance(nodeProperty.PropertyType, node) as BaseInstanceState;

                    newChild.Create(
                        SystemContext,
                        new NodeId(property.Name, InstanceNamespaceIndex),
                        new QualifiedName(property.Name, InstanceNamespaceIndex),
                        null,
                        true);

                    node.AddChild(newChild);
                    child = newChild;
                }

                var variable = child as BaseVariableState;

                if (variable != null)
                {
                    variable.Value = property.GetValue(instance);
                    continue;
                }

                var file = child as FileState;

                if (file != null)
                {
                    FileManager manager = new FileManager(Path.Join(rootFolder, value as string), file);
                    m_fileManagers.Add(file.NodeId, manager);
                }

                var @object = child as BaseObjectState;

                if (@object != null && value is IEncodeable encodeable)
                {
                    InitializeChildFromData(encodeable, @object, rootFolder);
                    continue;
                }
            }
        }

        private class FileManager : IDisposable
        {
            private readonly FileInfo m_fileInfo;
            private readonly FileState m_file;
            private readonly Dictionary<uint, Handle> m_handles = new();
            private uint m_nextHandle = 1;

            private class Handle
            {
                public NodeId SessionId; 
                public FileStream Stream;
                public uint Position;
            }

            public FileManager(string path, FileState file)
            {
                m_fileInfo = new FileInfo(path);
                m_file = file;

                if (m_fileInfo.Exists)
                {
                    m_file.Size.Value = (ulong)m_fileInfo.Length;
                    if (m_file.LastModifiedTime != null) m_file.LastModifiedTime.Value = m_fileInfo.LastWriteTimeUtc;

                    if (m_file.MimeType != null)
                    {
                        var extension = Path.GetExtension(path)[1..^0];

                        switch (extension)
                        {
                            case "bmp":
                                m_file.MimeType.Value = "image/bmp";
                                break;
                            case "csv":
                                m_file.MimeType.Value = "text/csv";
                                break;
                            case "ico":
                                m_file.MimeType.Value = "image/x-icon";
                                break;
                            case "gif":
                                m_file.MimeType.Value = "image/gif";
                                break;
                            case "jpg":
                            case "jpeg":
                                m_file.MimeType.Value = "image/jpeg";
                                break;
                            case "png":
                                m_file.MimeType.Value = "image/png";
                                break;
                            case "pdf":
                                m_file.MimeType.Value = "application/pdf";
                                break;
                            case "txt":
                                m_file.MimeType.Value = "text/plain";
                                break;
                            default:
                                m_file.MimeType.Value = $"application/{extension}";
                                break;
                        }
                    }
                }

                m_file.Writable.Value = false;
                m_file.UserWritable.Value = false;
                m_file.OpenCount.Value = 0;
                if (m_file.MaxByteStringLength != null) m_file.MaxByteStringLength.Value = UInt16.MaxValue;
                m_file.Open.OnCall = new OpenMethodStateMethodCallHandler(OnOpen);
                m_file.Close.OnCall = new CloseMethodStateMethodCallHandler(OnClose);
                m_file.Read.OnCall = new ReadMethodStateMethodCallHandler(OnRead);
                m_file.Write.OnCall = new WriteMethodStateMethodCallHandler(OnWrite);
                m_file.GetPosition.OnCall = new GetPositionMethodStateMethodCallHandler(OnGetPosition);
                m_file.SetPosition.OnCall= new SetPositionMethodStateMethodCallHandler(OnSetPosition);
            }

            private Handle Find(ISystemContext _context, uint fileHandle)
            {
                Handle handle;

                lock (m_handles)
                {
                    if (!m_handles.TryGetValue(fileHandle, out handle))
                    {
                        throw new ServiceResultException(StatusCodes.BadInvalidArgument);
                    }

                    if (handle.SessionId != _context.SessionId)
                    {
                        throw new ServiceResultException(StatusCodes.BadUserAccessDenied);
                    }
                }

                return handle;
            }

            private ServiceResult OnSetPosition(
                ISystemContext _context, 
                MethodState _method, 
                NodeId _objectId,
                uint fileHandle,
                ulong position)
            {
                Handle handle = Find(_context, fileHandle);
                handle.Stream.Seek((long)position, SeekOrigin.Begin);
                return StatusCodes.Good;
            }

            private ServiceResult OnGetPosition(
                ISystemContext _context,
                MethodState _method, 
                NodeId _objectId, 
                uint fileHandle, 
                ref ulong position)
            {
                Handle handle = Find(_context, fileHandle);
                position = (ulong)handle.Stream.Position;
                return StatusCodes.Good;
            }

            private ServiceResult OnWrite(
                ISystemContext _context,
                MethodState _method, 
                NodeId _objectId, 
                uint fileHandle, 
                byte[] data)
            {
                Handle handle = Find(_context, fileHandle);
                return StatusCodes.BadInvalidState;
            }

            private ServiceResult OnRead(
                ISystemContext _context, 
                MethodState _method, 
                NodeId _objectId,
                uint fileHandle, 
                int length, 
                ref byte[] data)
            {
                Handle handle = Find(_context, fileHandle);
                data = new byte[length];
                int bytesReed = handle.Stream.Read(data, 0, length);
                if (bytesReed <= 0)
                {
                    data = new byte[0];
                }
                return StatusCodes.Good;
            }

            private ServiceResult OnClose(
                ISystemContext _context, 
                MethodState _method, 
                NodeId _objectId, 
                uint fileHandle)
            {
                Handle handle;

                lock (m_handles)
                {
                    if (!m_handles.TryGetValue(fileHandle, out handle))
                    {
                        return StatusCodes.BadInvalidArgument;
                    }

                    if (handle.SessionId != _context.SessionId)
                    {
                        return StatusCodes.BadUserAccessDenied;
                    }

                    m_handles.Remove(fileHandle);
                    m_file.OpenCount.Value = (ushort)m_handles.Count;
                }

                handle.Stream.Close();
                handle.Stream.Dispose();

                return ServiceResult.Good;
            }

            private ServiceResult OnOpen(
                ISystemContext _context,
                MethodState _method,
                NodeId _objectId,
                byte mode,
                ref uint fileHandle)
            {
                if (mode != 1)
                {
                    return StatusCodes.BadNotWritable;
                }

                if (m_handles.Count >= 10)
                {
                    return StatusCodes.BadTooManyOperations;
                }

                var handle = new Handle
                {
                    SessionId = _context.SessionId,
                    Stream = m_fileInfo.Open(FileMode.Open, FileAccess.Read),
                    Position = 0
                };

                lock (m_handles)
                {
                    fileHandle = ++m_nextHandle;
                    m_handles.Add(fileHandle, handle);
                    m_file.OpenCount.Value = (ushort)m_handles.Count;
                }

                return ServiceResult.Good;
            }

            public void Dispose()
            {
                lock (m_handles)
                {
                    foreach (var handle in m_handles.Values)
                    {
                        handle.Stream.Close();
                        handle.Stream.Dispose();
                    }

                    m_handles.Clear();
                }
            }
        }

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public override void DeleteAddressSpace()
        {
            lock (Lock)
            {
                base.DeleteAddressSpace();
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Loads a node set from a file or resource and addes them to the set of predefined nodes.
        /// </summary>
        protected override NodeStateCollection LoadPredefinedNodes(ISystemContext context)
        {
            NodeStateCollection predefinedNodes = new NodeStateCollection();

            foreach (var name in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (name.EndsWith("BatteryPassport.PredefinedNodes.uanodes"))
                {
                    predefinedNodes.LoadFromBinaryResource(context, name, Assembly.GetExecutingAssembly(), true);
                    break;
                }
            }

            return predefinedNodes;
        }
        #endregion
    }
}

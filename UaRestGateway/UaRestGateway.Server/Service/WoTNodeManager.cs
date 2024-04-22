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
using Newtonsoft.Json.Linq;
using Opc.Ua;
using Opc.Ua.Server;
using Opc.Ua.WotCon;
using System.Reflection;
using System.Text;
using StatusCodes = Opc.Ua.StatusCodes;
using Wot = Opc.Ua.WotCon;

namespace UaRestGateway.Server.Service
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class WoTNodeManager : CustomNodeManager2
    {
        #region Private Fields
        private Dictionary<NodeId, FileManager> m_fileManagers = new();
        private ushort InstanceNamespaceIndex = 0;
        private ushort TypeNamespaceIndex = 0;
        private uint m_counter = 0;
        private Timer m_timer = null;
        private Wot.WoTAssetConnectionManagementState m_management;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public WoTNodeManager(IServerInternal server, ApplicationConfiguration configuration)
            :
            base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;
            SetNamespaces([Wot.Namespaces.WotCon, "tag:opcfoundation.org,2024-04:wot-connection:"]);
            TypeNamespaceIndex = NamespaceIndexes[0];
            InstanceNamespaceIndex = NamespaceIndexes[1];
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
                    return new NodeId(++m_counter, InstanceNamespaceIndex);
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

                var root = Find(ExpandedNodeId.ToNodeId(Wot.ObjectIds.WoTAssetConnectionManagement, Server.NamespaceUris));

                // find the untyped Boiler1 node that was created when the model was loaded.
                BaseObjectState passiveNode = (BaseObjectState)FindPredefinedNode(new NodeId(Wot.Objects.WoTAssetConnectionManagement, TypeNamespaceIndex), typeof(BaseObjectState));

                // convert the untyped node to a typed node that can be manipulated within the server.
                m_management = new Wot.WoTAssetConnectionManagementState(null);
                m_management.Create(SystemContext, passiveNode);

                m_management.CreateAsset.OnCall = new Wot.CreateAssetMethodStateMethodCallHandler(OnCreateAsset);
                m_management.DeleteAsset.OnCall = new Wot.DeleteAssetMethodStateMethodCallHandler(OnDeleteAsset);

                // replaces the untyped predefined nodes with their strongly typed versions.
                AddPredefinedNode(SystemContext, m_management);
            }
        }

        private ServiceResult OnCreateAsset(
            ISystemContext _context,
            MethodState _method,
            NodeId _objectId,
            string assetName,
            ref NodeId assetId)
        {
            if (String.IsNullOrEmpty(assetName))
            {
                return StatusCodes.BadInvalidArgument;
            }

            lock (Lock)
            {
                var root = FindPredefinedNode(new NodeId(Wot.Objects.WoTAssetConnectionManagement, TypeNamespaceIndex), typeof(Wot.WoTAssetConnectionManagementState));

                var browser = root.CreateBrowser(
                    SystemContext,
                    null,
                    ReferenceTypeIds.HasComponent,
                    false,
                    BrowseDirection.Forward,
                    new QualifiedName(assetName, InstanceNamespaceIndex),
                    null,
                    true);

                if (browser.Next() != null)
                {
                    return StatusCodes.BadBrowseNameDuplicated;
                }

                var asset = new Wot.IWoTAssetState(root);

                asset.Create(
                    SystemContext,
                    new NodeId(++m_counter, InstanceNamespaceIndex),
                    new QualifiedName(assetName, InstanceNamespaceIndex),
                    null,
                    true);

                var fileManager = new FileManager(this, asset.WoTFile);

                root.AddChild(asset);
                m_fileManagers.Add(asset.NodeId, fileManager);

                AddPredefinedNode(SystemContext, asset);
                assetId = asset.NodeId;

                return ServiceResult.Good;
            }
        }

        private ServiceResult OnDeleteAsset(
            ISystemContext _context,
            MethodState _method,
            NodeId _objectId,
            NodeId assetId)
        {
            lock (Lock)
            {
                var asset = FindPredefinedNode(assetId, typeof(Wot.IWoTAssetState));

                if (asset == null)
                {
                    return StatusCodes.BadNodeIdUnknown;
                }

                m_fileManagers.Remove(assetId);
                DeleteNode(Server.DefaultSystemContext, assetId);

                return ServiceResult.Good;
            }
        }

        internal NodeState CreateAssetProperty(NodeState asset, string name)
        {
            lock (Lock)
            {
                var property = new BaseDataVariableState<double>(asset);

                property.Create(
                    SystemContext,
                    new NodeId(++m_counter, InstanceNamespaceIndex),
                    new QualifiedName(name, InstanceNamespaceIndex),
                    null,
                    true);

                property.Value = 1;

                asset.AddChild(property);

                AddPredefinedNode(SystemContext, property);

                return property;
            }
        }

        private class FileManager : IDisposable
        {
            private readonly WoTNodeManager m_nodeManager;
            private readonly WoTAssetFileState m_file;
            private readonly Dictionary<uint, Handle> m_handles = new();
            private bool m_writing = false;
            private uint m_nextHandle = 1;

            private class Handle
            {
                public NodeId SessionId;
                public MemoryStream Stream;
                public uint Position;
                public bool Writing;
            }

            public FileManager(WoTNodeManager nodeManager, WoTAssetFileState file)
            {
                m_nodeManager = nodeManager;
                m_file = file;
                m_file.Size.Value = 0;
                if (m_file.MaxByteStringLength != null) m_file.LastModifiedTime.Value = DateTime.MinValue;
                m_file.Writable.Value = false;
                m_file.UserWritable.Value = false;
                m_file.OpenCount.Value = 0;
                if (m_file.MaxByteStringLength != null) m_file.MaxByteStringLength.Value = UInt16.MaxValue;
                m_file.Open.OnCall = new OpenMethodStateMethodCallHandler(OnOpen);
                m_file.Close.OnCall = new CloseMethodStateMethodCallHandler(OnClose);
                m_file.Read.OnCall = new ReadMethodStateMethodCallHandler(OnRead);
                m_file.Write.OnCall = new WriteMethodStateMethodCallHandler(OnWrite);
                m_file.GetPosition.OnCall = new GetPositionMethodStateMethodCallHandler(OnGetPosition);
                m_file.SetPosition.OnCall = new SetPositionMethodStateMethodCallHandler(OnSetPosition);
                m_file.CloseAndUpdate.OnCall = new Wot.CloseAndUpdateMethodStateMethodCallHandler(OnCloseAndUpdate);
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
                lock (m_handles)
                {
                    var handle = Find(_context, fileHandle);

                    if (!handle.Writing)
                    {
                        return StatusCodes.BadInvalidState;
                    }

                    if (data != null && data.Length > 0)
                    {
                        handle.Stream.Write(data, 0, data.Length);
                    }
                }

                return StatusCodes.Good;
            }

            private ServiceResult OnRead(
                ISystemContext _context,
                MethodState _method,
                NodeId _objectId,
                uint fileHandle,
                int length,
                ref byte[] data)
            {
                lock (m_handles)
                {
                    var handle = Find(_context, fileHandle);

                    if (handle.Writing)
                    {
                        return StatusCodes.BadInvalidState;
                    }

                    if (data != null && data.Length > 0)
                    {
                        byte[] buffer = new byte[data.Length];
                        handle.Stream.Read(data, 0, data.Length);
                        data = buffer;
                    }
                    else
                    {
                        data = new byte[0];
                    }
                }

                return StatusCodes.Good;
            }

            private ServiceResult OnCloseAndUpdate(
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

                    m_writing = false;
                    m_handles.Remove(fileHandle);
                    m_file.OpenCount.Value = (ushort)m_handles.Count;
                }

                try
                {
                    handle.Stream.Close();
                    var json = Encoding.UTF8.GetString(handle.Stream.ToArray());

                    JObject jsonObject = JObject.Parse(json);
                    JObject properties = (JObject)jsonObject["properties"];

                    foreach (JProperty property in properties.Properties())
                    {
                        m_nodeManager.CreateAssetProperty(m_file.Parent, property.Name);
                    }
                }
                catch (Exception)
                {
                    return StatusCodes.BadDecodingError;
                }
                finally
                {
                    handle.Stream.Dispose();
                }

                return ServiceResult.Good;
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

                    m_writing = false;
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
                if (mode != 1 && mode != 6)
                {
                    return StatusCodes.BadNotSupported;
                }

                lock (m_handles)
                {
                    if (m_handles.Count >= 10)
                    {
                        return StatusCodes.BadTooManyOperations;
                    }
                    
                    if (m_writing && mode != 1)
                    {
                        return StatusCodes.BadInvalidState;
                    }

                    var handle = new Handle
                    {
                        SessionId = _context.SessionId,
                        Stream = new MemoryStream(),
                        Position = 0
                    };

                    if (mode == 6)
                    {
                        m_writing = handle.Writing = true;
                    }

                    lock (m_handles)
                    {
                        fileHandle = ++m_nextHandle;
                        m_handles.Add(fileHandle, handle);
                        m_file.OpenCount.Value = (ushort)m_handles.Count;
                    }
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
                if (name.EndsWith("Opc.Ua.WotCon.PredefinedNodes.uanodes"))
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

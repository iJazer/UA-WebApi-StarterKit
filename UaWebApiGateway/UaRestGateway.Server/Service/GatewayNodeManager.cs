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

using Opc.Ua;
using Opc.Ua.Server;
using StatusCodes = Opc.Ua.StatusCodes;

namespace UaRestGateway.Server.Service
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class GatewayNodeManager : CustomNodeManager2
    {
        #region Private Fields
        private Dictionary<NodeId, FileManager> m_fileManagers = new();
        private ushort MeasurementsNamespaceIndex => (NamespaceIndexes?.Count > 0) ? NamespaceIndexes[0] : (ushort)0;
        private Timer m_simulationTimer;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public GatewayNodeManager(IServerInternal server, ApplicationConfiguration configuration)
            :
            base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;
            string[] namespaceUrls = new string[1];
            namespaceUrls[0] = Measurements.Namespaces.Measurements;
            SetNamespaces(namespaceUrls);
            SystemContext.EncodeableFactory.AddEncodeableTypes(typeof(Measurements.OrientationDataType).Assembly);
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
                if (m_simulationTimer != null)
                {
                    m_simulationTimer.Dispose();
                    m_simulationTimer = null;
                }

                lock (Lock)
                {
                    foreach (var manager in m_fileManagers.Values)
                    {
                        manager.Dispose();
                    }

                    m_fileManagers.Clear();
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
            NodeStateCollection predefinedNodes = LoadPredefinedNodes(context);
            BaseInstanceState instance = node as BaseInstanceState;

            if (instance != null && instance.Parent != null)
            {
                string id = instance.Parent.NodeId.Identifier as string;

                if (id != null)
                {
                    return new NodeId(id + "_" + instance.SymbolicName, instance.Parent.NodeId.NamespaceIndex);
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

                // link root to objects folder.
                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(Opc.Ua.ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[Opc.Ua.ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                var tests = CreateMeasurementsFolder();
                references.Add(new NodeStateReference(Opc.Ua.ReferenceTypeIds.Organizes, false, tests.NodeId));
            }
        }

        private bool HasEngineerAccess(ISystemContext context)
        {
            OperationContext oc = (context as SystemContext)?.OperationContext as OperationContext;

            if (oc != null)
            {
                IUserIdentity user = context.UserIdentity;

                if (user?.GrantedRoleIds.Contains(ObjectIds.WellKnownRole_AuthenticatedUser) == true)
                {
                    return true;
                }
            }

            return false;
        }

        protected override ReferenceDescription GetReferenceDescription(ServerSystemContext context, Dictionary<NodeId, NodeState> cache, IReference reference, ContinuationPoint continuationPoint)
        {
            var result = base.GetReferenceDescription(context, cache, reference, continuationPoint);

            if (result != null)
            {
                if (result.NodeId?.Identifier is string id)
                {
                    if (id.Contains(Measurements.BrowseNames.Reset) == true)
                    {
                        if (!HasEngineerAccess(context))
                        {
                            return null;
                        }
                    }
                }
            }

            return result;
        }

        public override NodeMetadata GetPermissionMetadata(
            OperationContext context,
            object targetHandle,
            BrowseResultMask resultMask,
            Dictionary<NodeId, List<object>> uniqueNodesServiceAttributesCache,
            bool permissionsOnly)
        {
            var metadata = base.GetPermissionMetadata(
                context,
                targetHandle,
                resultMask,
                uniqueNodesServiceAttributesCache,
                permissionsOnly);

            if (metadata != null)
            {
                if (metadata.NodeId.Identifier is string id && id.Contains(Measurements.BrowseNames.Reset) == true)
                {
                    if (metadata.RolePermissions == null)
                    {
                        metadata.RolePermissions = new(new RolePermissionType[]
                        {
                            new RolePermissionType()
                            {
                                RoleId = ObjectIds.WellKnownRole_Anonymous,
                                Permissions = (uint)(PermissionType.None)
                            },
                            new RolePermissionType()
                            {
                                RoleId = ObjectIds.WellKnownRole_AuthenticatedUser,
                                Permissions = (uint)(PermissionType.Browse | PermissionType.Read | PermissionType.Call)
                            },
                            new RolePermissionType()
                            {
                                RoleId = ObjectIds.WellKnownRole_Engineer,
                                Permissions = (uint)(PermissionType.Browse | PermissionType.Read | PermissionType.Call)
                            }
                        });
                    }
                }
            }

            return metadata;
        }

        private BaseObjectState CreateMeasurementsFolder()
        {
            var container = new Measurements.MeasurementContainerState(null);

            container.Create(
                SystemContext,
                new NodeId("Measurements", MeasurementsNamespaceIndex),
                new QualifiedName("Measurements", MeasurementsNamespaceIndex),
                null,
                true);

            AddPredefinedNode(SystemContext, container);

            container.Temperature.Value = 25.0;
            container.Temperature.AccessLevel = container.Temperature.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            container.Pressure.Value = 103.0;
            container.Pressure.AccessLevel = container.Pressure.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            container.Orientation.Value = new Measurements.OrientationDataType()
            {
                ProfileName = "Profile 1",
                X = 10.0,
                Y = 20.0,
                Rotation = 180.0
            };

            container.Orientation.OnWriteValue += OrientationOnWrite;
            container.Orientation.AccessLevel = container.Orientation.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            container.Reset.OnCallMethod2 += (context, methodToCall, objectId, inputArguments, outputArguments) =>
            {
                if (inputArguments.Count != 2 || outputArguments.Count != 2)
                {
                    return StatusCodes.BadArgumentsMissing;
                }

                lock (Lock)
                {
                    double oldTemperature = container.Temperature.Value;
                    double oldPressure = container.Pressure.Value;

                    container.Temperature.Value = (double)inputArguments[0];
                    container.Pressure.Value = (double)inputArguments[1];

                    outputArguments[0] = oldTemperature;
                    outputArguments[1] = oldPressure;
                }

                return ServiceResult.Good;
            };

            m_simulationTimer = new Timer(
                (state) =>
                {
                    lock (Lock)
                    {
                        container.Temperature.Value = Math.Round(Math.Round(container.Temperature.Value, 0) + Random.Shared.NextDouble() - 0.5, 2);
                        container.Pressure.Value = Math.Round(Math.Round(container.Pressure.Value, 0) + Random.Shared.NextDouble() - 0.5, 2);
                    }

                    container.Temperature.ClearChangeMasks(SystemContext, false);
                    container.Pressure.ClearChangeMasks(SystemContext, false);
                },
                null,
                1000,
                1000);

            return container;
        }

        private ServiceResult OrientationOnWrite(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            if (value is ExtensionObject eo)
            {
                if (eo.Body is Measurements.OrientationDataType orientation)
                {
                    value = orientation;
                    return ServiceResult.Good;
                }
            }

            return StatusCodes.BadTypeMismatch;
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
                m_file.SetPosition.OnCall = new SetPositionMethodStateMethodCallHandler(OnSetPosition);
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
        protected override NodeStateCollection LoadPredefinedNodes(ISystemContext context)
        {
            NodeStateCollection predefinedNodes = new NodeStateCollection();

            var assembly = typeof(GatewayNodeManager).Assembly;

            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (name.EndsWith("uanodes"))
                {
                    if (name.Contains("Measurements"))
                    {
                        predefinedNodes.LoadFromBinaryResource(context, name, assembly, true);
                        break;
                    }
                }
            }

            return predefinedNodes;
        }
        #endregion
    }
}

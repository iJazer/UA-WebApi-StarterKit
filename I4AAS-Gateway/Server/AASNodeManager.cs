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

using I4AAS.Submodels;
using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace I4AAS_Gateway.Server
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class AASNodeManager : CustomNodeManager2
    {
        #region Private Fields
        private Dictionary<NodeId, FileManager> m_fileManagers = new();
        private ushort InstanceNamespaceIndex = 0;
        private ushort TestNamespaceIndex = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public AASNodeManager(IServerInternal server, ApplicationConfiguration configuration)
            :
            base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            string[] namespaceUrls = new string[4];
            namespaceUrls[0] = I4AAS.Submodels.Namespaces.I4AAS;
            namespaceUrls[1] = I4AAS.IRDI.Namespaces.IRDI;
            namespaceUrls[2] = I4AAS.Submodels.Namespaces.I4AAS + "instances";
            namespaceUrls[3] = "tag:opcua-is-great.net,2023:testing";
            SetNamespaces(namespaceUrls);
            
            InstanceNamespaceIndex = NamespaceIndexes[2];
            TestNamespaceIndex = NamespaceIndexes[3];

            Server.MessageContext.Factory.AddEncodeableTypes(typeof(I4AAS.Submodels.SubmodelDataType).GetTypeInfo().Assembly);
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

                string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var asset = CreateAsset(Path.Join(folder, "Data", "ProductCarbonFootprint_NCSU Pulp & Paper batch 2023-10-12"));

                // link root to objects folder.
                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(Opc.Ua.ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[Opc.Ua.ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                references.Add(new NodeStateReference(Opc.Ua.ReferenceTypeIds.Organizes, false, asset.NodeId));

                var tests = CreateTestFolder();
                references.Add(new NodeStateReference(Opc.Ua.ReferenceTypeIds.Organizes, false, tests.NodeId));
            }
        }

        private FolderState CreateTestFolder()
        {
            var folder = new FolderState(null);

            folder.Create(
                SystemContext,
                new NodeId("Tests", TestNamespaceIndex),
                new QualifiedName("Tests", TestNamespaceIndex),
                null,
                true);

            AddPredefinedNode(SystemContext, folder);

            var variable = new AnalogItemState<double>(folder);

            variable.Create(
                SystemContext,
                new NodeId("Temperature", TestNamespaceIndex),
                new QualifiedName("Temperature", TestNamespaceIndex),
                null,
                true);

            variable.Definition = null;
            variable.InstrumentRange = null;
            variable.ValuePrecision = null;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            variable.Value = 100.0;

            variable.EngineeringUnits.Value = new EUInformation()
            {
                DisplayName = "°C",
                Description = "Celsius",
                UnitId = 4408652,
                NamespaceUri = "http://www.opcfoundation.org/UA/units/un/cefact"
            };

            variable.EURange.Value = new Opc.Ua.Range(-263, 400);

            folder.AddChild(variable);
            AddPredefinedNode(SystemContext, variable);

            var method = new MethodState(folder);
            method.ReferenceTypeId = ReferenceTypeIds.HasComponent;

            method.Create(
                SystemContext,
                new NodeId("Reset", TestNamespaceIndex),
                new QualifiedName("Reset", TestNamespaceIndex),
                null,
                true);

            method.OnCallMethod = new GenericMethodCalledEventHandler(OnReset);

            folder.AddChild(method);
            AddPredefinedNode(SystemContext, method);

            var argument = new PropertyState<Argument[]>(method);
            argument.ReferenceTypeId = ReferenceTypeIds.HasProperty;

            argument.Create(
                SystemContext,
                new NodeId(Opc.Ua.BrowseNames.InputArguments, TestNamespaceIndex),
                Opc.Ua.BrowseNames.InputArguments,
                null,
                true);

            argument.Value = new Argument[]
            {
                new()
                {
                    Name = "NewValue",
                    Description = "Sets the temperature to a new value.",
                    DataType = Opc.Ua.DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar
                }
            };

            method.AddChild(argument);
            method.InputArguments = argument;
            AddPredefinedNode(SystemContext, argument);

            argument = new PropertyState<Argument[]>(method);
            argument.ReferenceTypeId = ReferenceTypeIds.HasProperty;

            argument.Create(
                SystemContext,
                new NodeId(Opc.Ua.BrowseNames.OutputArguments, TestNamespaceIndex),
                Opc.Ua.BrowseNames.OutputArguments,
                null,
                true);

            argument.Value = new Argument[]
            {
                new()
                {
                    Name = "OldValue",
                    Description = "The previous value for the temperature.",
                    DataType = Opc.Ua.DataTypeIds.Double,
                    ValueRank = ValueRanks.Scalar
                }
            };

            method.AddChild(argument);
            method.OutputArguments = argument;
            AddPredefinedNode(SystemContext, argument);

            return folder;
        }

        private ServiceResult OnReset(
            ISystemContext context, 
            MethodState method, 
            IList<object> inputArguments, 
            IList<object> outputArguments)
        {
            var variable = method.Parent.FindChild(
                context,
                new QualifiedName("Temperature", InstanceNamespaceIndex)) as AnalogItemState<double>;

            if (variable == null)
            {
                return StatusCodes.BadInvalidState;
            }

            if (inputArguments[0] is double value)
            {
                outputArguments[0] = variable.Value;
                variable.Value = value;
                return ServiceResult.Good;
            }

            return StatusCodes.BadInvalidArgument;
        }

        private AssetModelState CreateAsset(string rootFolder)
        {
            var assetFilePath = Path.Join(
                rootFolder,
                "aasx",
                "NCSU_ProductCarbonFootprint",
                "NCSU_ProductCarbonFootprint.opcua.json");

            AssetDescriptionFileDataType product = new();
            var json = File.ReadAllText(assetFilePath);

            using (var decoder = new JsonDecoder(json, Server.MessageContext))
            {
                product.Namespaces = decoder.ReadStringArray(nameof(product.Namespaces));
            }

            ServiceMessageContext context = new ServiceMessageContext();
            product.Namespaces.ForEach(x => context.NamespaceUris.Append(x));
            context.Factory = Server.Factory;

            using (var decoder = new JsonDecoder(json, context))
            {
                product.Decode(decoder);
            }

            var node = new AssetModelState(null);

            node.Create(
                SystemContext,
                new NodeId(product.ModelIdShort, InstanceNamespaceIndex),
                new QualifiedName(product.ModelIdShort, InstanceNamespaceIndex),
                null,
                true);

            FileManager manager = new FileManager(rootFolder + ".aasx", node.AssetFile);
            m_fileManagers.Add(node.AssetFile.NodeId, manager);

            foreach (var submodel in product.Submodels)
            {
                if (submodel.Body is NameplateSubmodelDataType nameplate)
                {
                    InitializeChildFromData(nameplate, node.Nameplate, rootFolder);
                    continue;
                }

                if (submodel.Body is SubmodelDataType instance)
                {
                    SubmodelState child;

                    switch (instance)
                    {
                        case ProductCarbonFootprintDataType:
                            child = new ProductCarbonFootprintSubmodelState(node);
                            break;
                        default:
                            child = new SubmodelState(node);
                            break;
                    };

                    child.Create(
                        SystemContext,
                        new NodeId(instance.ModelIdShort, InstanceNamespaceIndex),
                        new QualifiedName(instance.ModelIdShort, InstanceNamespaceIndex),
                        null,
                        true);

                    node.AddChild(child);

                    InitializeChildFromData(instance, child, rootFolder);
                    continue;
                }
            }

            // store it and all of its children in the pre-defined nodes dictionary for easy look up.
            AddPredefinedNode(SystemContext, node);

            return node;
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

            Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .Where(x => x.EndsWith(".uanodes"))
                .ToList().ForEach(name =>
                {
                    predefinedNodes.LoadFromBinaryResource(
                        context,
                        name,
                        Assembly.GetExecutingAssembly(),
                        true);
                });

            return predefinedNodes;
        }
        #endregion
    }
}

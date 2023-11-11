import * as React from 'react';
import axios from 'axios';
import * as OpcUa from "../opcua/api";
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import LabelOutlinedIcon from '@mui/icons-material/LabelOutlined';
import SaveAltOutlinedIcon from '@mui/icons-material/SaveAltOutlined';

const RequestTimeout = 300000;
const FileType = 'i=11575';
const AssetModelType = 'nsu=http://opcfoundation.org/UA/I4AAS/;i=209';
const ObjectsFolder = 'i=85';
const HierarchicalReferences = 'i=33';

enum NodeClass {
   Unspecified = 0,
   Object = 1,
   Variable = 2,
   Method = 4,
   ObjectType = 8,
   VariableType = 16,
   ReferenceType = 32,
   DataType = 64,
   View = 128
};

enum BuiltInType {
   Null = 0,
   Boolean = 1,
   SByte = 2,
   Byte = 3,
   Int16 = 4,
   UInt16 = 5,
   Int32 = 6,
   UInt32 = 7,
   Int64 = 8,
   UInt64 = 9,
   Float = 10,
   Double = 11,
   String = 12,
   DateTime = 13,
   Guid = 14,
   ByteString = 15,
   XmlElement = 16,
   NodeId = 17,
   ExpandedNodeId = 18,
   StatusCode = 19,
   QualifiedName = 20,
   LocalizedText = 21,
   ExtensionObject = 22,
   DataValue = 23,
   Variant = 24,
   DiagnosticInfo = 25,
   Number = 26,
   Integer = 27,
   UInteger = 28,
   Enumeration = 29
};

interface TreeNode {
   'Id': number,
   'Reference': OpcUa.ReferenceDescription;
   'Value'?: string;
   'Children'?: TreeNode[];
}

let counter: number = 1;

function toString(value?: OpcUa.Variant): string {
   if (!value?.Body === undefined) {
      return '';
   }
   const isArray = Array.isArray(value?.Body);
   if (isArray) {
      const array = value?.Body as any[];
      return "[" + array.map((x) => {
         return toString({ Type: value?.Type, Body: x });
      }).join(', ') + "]";
   }
   const body = value?.Body ?? '';
   switch (value?.Type) {
      case BuiltInType.Null: return '';
      case BuiltInType.LocalizedText: {
         const lt: OpcUa.LocalizedText = body as OpcUa.LocalizedText;
         return lt.Text ?? '';
      }
      case BuiltInType.ExtensionObject: {
         const eo: OpcUa.ExtensionObject = body as OpcUa.ExtensionObject;
         return (eo.Body) ? JSON.stringify(eo.Body) : '';
      }
      case BuiltInType.Variant: {
         const variant: OpcUa.Variant = body as OpcUa.Variant;
         return toString(variant);
      }
      case BuiltInType.StatusCode: {
         return "0x" + body.toString(16).padStart(8, '0');
      }
      default:
         return body.toString();
   }
}

async function browse(nodeId: string, requestHandle: number): Promise<TreeNode[] | null> {
   let request: OpcUa.BrowseRequestMessage = {
      "Body": {
         "RequestHeader": {
            "Timestamp": new Date().toISOString(),
            "TimeoutHint": RequestTimeout,
            "RequestHandle": requestHandle
         },
         "NodesToBrowse": [
            {
               "NodeId": nodeId,
               "BrowseDirection": 0,
               "ReferenceTypeId": HierarchicalReferences,
               "IncludeSubtypes": true,
               "ResultMask": 63
            }
         ]
      }
   };
   var response = await axios.post('/opcua/browse', request, { timeout: RequestTimeout });
   if (!response) {
      return null;
   }
   var serviceResult: OpcUa.BrowseResponse = response.data?.Body;
   if (serviceResult?.ResponseHeader?.ServiceResult) {
      console.warn(`Browse call failed: ${serviceResult?.ResponseHeader?.ServiceResult}`);
      return null;
   }
   var browseResult: OpcUa.BrowseResult | undefined = serviceResult?.Results?.[0];
   if (browseResult?.StatusCode) {
      console.warn(`Browse Node '${nodeId}' failed: ${browseResult?.StatusCode}`);
      return null;
   }
   var nodes: TreeNode[] = [];
   browseResult?.References?.forEach((x) => {
      if (x.NodeClass === NodeClass.Object || x.NodeClass === NodeClass.Variable) {
         nodes.push({ Id: counter++, Reference: x });
      }
   });
   return nodes;
}

async function read(nodes: TreeNode[], requestHandle: number): Promise<TreeNode[]> {
   let request: OpcUa.ReadRequestMessage = {
      "Body": {
         "RequestHeader": {
            "Timestamp": new Date().toISOString(),
            "TimeoutHint": RequestTimeout,
            "RequestHandle": requestHandle
         },
         "NodesToRead": []
      }
   };
   nodes.forEach((x) => {
      if (x.Reference.NodeClass === NodeClass.Variable) {
         request?.Body?.NodesToRead?.push({
            "NodeId": x.Reference.NodeId,
            "AttributeId": 13
         });
      }
   });
   if (!request?.Body?.NodesToRead?.length) {
      return nodes;
   }
   var response = await axios.post('/opcua/read', request, { timeout: RequestTimeout });
   if (!response) {
      return nodes;
   }
   var serviceResult: OpcUa.ReadResponse = response.data?.Body;
   if (serviceResult?.ResponseHeader?.ServiceResult) {
      console.warn(`Read call failed: ${serviceResult?.ResponseHeader?.ServiceResult}`);
      return nodes;
   }
   let ii = 0;
   nodes.forEach((node) => {
      if (node.Reference.NodeClass === NodeClass.Variable) {
         var dv: OpcUa.DataValue | undefined = serviceResult?.Results?.[ii++];
         if (dv?.StatusCode) {
            node.Value = "0x" + dv?.StatusCode?.toString(16).padStart(8, '0');
         }
         else {
            node.Value = toString(dv?.Value);
         }
      }
   });
   return nodes;
}

interface NodeTagProps {
   node: TreeNode
}

const tagStyle = {
   backgroundColor: 'blue',
   color: 'white',
   fontWeight: 'bolder',
   padding: '0px 3px 0px 3px',
   borderRadius: '2px',
   marginBottom: '5px'
};

const NodeTag = ({ node }: NodeTagProps) => {
   if (!node.Reference) {
      return null;
   }
   if (node.Reference.TypeDefinition === AssetModelType) {
      return <div style={tagStyle}>ASS</div>;
   }
   if (node.Reference.TypeDefinition === FileType) {
      return <div style={tagStyle}>File</div>;
   }
   if (node.Reference.NodeClass === NodeClass.Variable) {
      return <div style={tagStyle}>Prop</div>;
   }
   return <div style={tagStyle}>Sub</div>;
}

interface NodeWidgetProps {
   node: TreeNode,
   offset: number
}

const NodeWidget = ({ node, offset }: NodeWidgetProps) => {
   const [count, setCount] = React.useState<number>(1);

   async function handleClick(e: React.MouseEvent<HTMLDivElement, MouseEvent>) {
      e.stopPropagation();
      if (node.Reference.NodeId && node.Reference.NodeClass === NodeClass.Object) {
         let nodes = await browse(node.Reference.NodeId, counter++);
         if (nodes) {
            nodes = await read(nodes, counter++);
            node.Children = nodes;
            setCount(count + 1);
         }
      }
   }

   return (
      <div style={{ marginLeft: `${offset}px` }} onClick={(e) => handleClick(e)}>
         <div style={{ display: 'flex' }}>
            <div style={{ marginBottom: '5px' }}>{
               (FileType === node.Reference?.TypeDefinition)
                  ? <SaveAltOutlinedIcon />
                  : (NodeClass.Variable === node.Reference?.NodeClass)
                     ? <LabelOutlinedIcon />
                     : <AddCircleOutlineIcon />
            }
            </div>
            <NodeTag node={node} />
            <div style={{ marginLeft: `5px`, color: 'green', fontWeight: 'bolder', marginBottom: '5px' }}>{node.Reference.DisplayName?.Text}</div>
            <div style={{ marginLeft: `10px`, marginBottom: '5px' }}>{node.Value}</div>
         </div>
         {node.Children?.map(x => {
            if (x.Reference.NodeClass === NodeClass.Object || x.Value) {
               return <NodeWidget key={x.Id} node={x} offset={offset + 10} />
            }
            return null;
         })}
      </div>
   );
}

export function Home() {
   const [roots, setRoots] = React.useState<TreeNode[]>([]);

   React.useEffect(() => {
      fetch();
      async function fetch() {
         let nodes = await browse(ObjectsFolder, counter++);
         if (nodes) {
            nodes = await read(nodes, counter++);
            setRoots(nodes);
         }
      }
   }, []);


   return (
      <div>
         {roots.map(x => {
            if (x.Reference.TypeDefinition === AssetModelType) {
               return <NodeWidget key={x.Id} node={x} offset={0} />;
            }
            return null;
         })}
      </div>
   );
}

export default Home;

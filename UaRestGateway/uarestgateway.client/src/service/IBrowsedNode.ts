import * as OpcUa from '../opcua';

export interface IBrowsedNode {
   id: string,
   reference: OpcUa.ReferenceDescription;
   value?: OpcUa.DataValue;
   parentId?: string;
   lastUpdated?: Date
}
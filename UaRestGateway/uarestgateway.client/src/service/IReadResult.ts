import * as OpcUa from '../opcua';
export interface IReadResult {
   id: number,
   nodeId: string,
   attributeId?: number
   value?: OpcUa.DataValue;
}

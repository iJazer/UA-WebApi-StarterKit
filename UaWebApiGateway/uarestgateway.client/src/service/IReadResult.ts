import * as OpcUa from 'opcua-webapi';
export interface IReadResult {
   id: number,
   nodeId: string,
   attributeId?: number
   value?: OpcUa.DataValue;
}

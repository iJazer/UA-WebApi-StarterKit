import * as OpcUa from 'opcua-webapi';
export default class WebClient {
   requestHandle: number;
   configuration: OpcUa.Configuration;
   api: OpcUa.DefaultApi;

   constructor(configuration: OpcUa.Configuration) {
      this.configuration = configuration;
      this.requestHandle = 1;
      this.api = new OpcUa.DefaultApi(configuration);
   }

   async browseChildren(nodeId: string): Promise<OpcUa.ReferenceDescription[] | undefined> {
      const request = OpcUa.BrowseRequestFromJSON({
         RequestHeader: {
            RequestHandle: this.requestHandle++,
            TimeoutHint: 100000,
            Timestamp: new Date().toISOString()
         },
         NodesToBrowse: [
            {
               NodeId: nodeId,
               BrowseDirection: OpcUa.BrowseDirection.Forward,
               ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
               IncludeSubtypes: true,
               NodeClassMask: Number(OpcUa.NodeClass.Object | OpcUa.NodeClass.Variable | OpcUa.NodeClass.Method),
               ResultMask: 63
            }
         ]
      });

      const response = await this.api.browse({ browseRequest: request });

      if (OpcUa.StatusUtils.isBad(response?.ResponseHeader?.ServiceResult?.Code)) {
         throw new Error(`Browse failed with status code ${OpcUa.StatusCodes[response?.ResponseHeader?.ServiceResult?.Code ?? 0]}.`);
      }

      return response.Results?.[0].References;
   }

   async readValues(nodeIds: string[]): Promise<OpcUa.DataValue[] | undefined> {
      const request = OpcUa.ReadRequestFromJSON({
         RequestHeader: {
            RequestHandle: this.requestHandle++,
            TimeoutHint: 100000,
            Timestamp: new Date().toISOString()
         },
         NodesToRead: nodeIds.map(x => OpcUa.ReadValueIdFromJSON({ NodeId: x, AttributeId: OpcUa.Attributes.Value }))
      });

      const response = await this.api.read({ readRequest: request });

      if (OpcUa.StatusUtils.isBad(response?.ResponseHeader?.ServiceResult?.Code)) {
         throw new Error(`Read failed with status code ${OpcUa.StatusCodes[response?.ResponseHeader?.ServiceResult?.Code ?? 0]}.`);
      }

      return response.Results;
   }

   async writeValues(nodesToWrite: OpcUa.WriteValue[]): Promise<OpcUa.StatusCode[] | undefined>  {
      const request = OpcUa.WriteRequestFromJSON({
         RequestHeader: {
            RequestHandle: this.requestHandle++,
            TimeoutHint: 100000,
            Timestamp: new Date().toISOString()
         },
         NodesToWrite: nodesToWrite
      });

      const response = await this.api.write({ writeRequest: request });

      if (OpcUa.StatusUtils.isBad(response?.ResponseHeader?.ServiceResult?.Code)) {
         throw new Error(`Write failed with status code ${OpcUa.StatusCodes[response?.ResponseHeader?.ServiceResult?.Code ?? 0]}.`);
      }

      return response.Results;
   }

   async call(objectId: string, methodId: string, inputArguments: OpcUa.Variant[]): Promise<OpcUa.Variant[] | undefined>  {
      const request = OpcUa.CallRequestFromJSON({
         RequestHeader: {
            RequestHandle: this.requestHandle++,
            TimeoutHint: 100000,
            Timestamp: new Date().toISOString()
         },
         MethodsToCall: [
            {
               ObjectId: objectId,
               MethodId: methodId,
               InputArguments: inputArguments
            }
         ]
      });

      const response = await this.api.call({ callRequest: request });

      if (OpcUa.StatusUtils.isBad(response?.ResponseHeader?.ServiceResult?.Code)) {
         throw new Error(`Call failed with status code ${OpcUa.StatusCodes[response?.ResponseHeader?.ServiceResult?.Code ?? 0]}.`);
      }

      if (OpcUa.StatusUtils.isBad(response?.Results?.[0].StatusCode?.Code)) {
         throw new Error(`Method return failed status ${OpcUa.StatusCodes[response?.Results?.[0].StatusCode?.Code ?? 0]}.`);
      }

      return response.Results?.[0].OutputArguments;
   }
}

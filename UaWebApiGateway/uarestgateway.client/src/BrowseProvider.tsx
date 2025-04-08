import * as React from 'react';
import * as OpcUa from 'opcua-webapi';
import { SessionContext } from './SessionContext';
import { HandleFactory } from './service/HandleFactory';
import { IRequestMessage } from './service/IRequestMessage';
import { IBrowsedNode } from './service/IBrowsedNode';
import { BrowseContext } from './BrowseContext';
import { IReadValueId } from './service/IReadValueId';
import { IReadResult } from './service/IReadResult';
import { IRequestBody } from './service/IRequestBody';

interface IBrowseResult {
   callerHandle: number,
   children?: IBrowsedNode[],
   values?: IReadResult[]
}

export interface IBrowseContext {
   stateChangeCount: number,
   nodes: Map<string, IBrowsedNode>,
   visibleNodes: string[],
   setVisibleNodes: (items: string[]) => void,
   lastCompletedRequest?: IBrowseResult,
   browseChildren: (requestId: number, parentId: string) => Promise<void>,
   readValues: (requestId: number, nodesToRead: IReadValueId[]) => Promise<void>
}

interface BrowseProviderProps {
   children?: React.ReactNode
}

interface InternalRequest {
   callerHandle: number,
   internalHandle: number,
   serviceId?: string,
   children?: IBrowsedNode[],
   nodesToTranslate?: IReadValueId[]
   nodesToRead?: IReadValueId[]
}

interface BrowseInternals {
   stateChangeCount: number,
   nodes: Map<string, IBrowsedNode>,
   lastCompletedRequest?: IBrowseResult,
   requests: Map<number,InternalRequest>
}

export const BrowseProvider = ({ children }: BrowseProviderProps) => {
   const [visibleNodes, setVisibleNodes] = React.useState<string[]>([]);
   const [lastCompletedBrowse, setLastCompletedBrowse] = React.useState<IBrowseResult | undefined>();

   const {
      sendRequest,
      lastCompletedRequest,
      sessionState
   } = React.useContext(SessionContext);

   const m = React.useRef<BrowseInternals>({
      stateChangeCount: 0,
      nodes: new Map<string, IBrowsedNode>(),
      lastCompletedRequest: undefined,
      requests: new Map<number,InternalRequest>()
   });

   React.useEffect(() => {
      m.current.stateChangeCount++;
   }, [sessionState]);

   /**
   * prepareAndSend: Function to handle a specific request function (like Browse or Read).
   * 
   * @param serviceId - The ID of the service to be requested.
   * @param request - The request body containing the details of the request.
   * @param state - The internal request state containing details of the current operation (optional).
   * @param callerHandle - The handle of the caller initiating the request (optional).
   * @param callerId - The ID of the caller / technology initiating the request (optional).
   * 
   * If the state is undefined, it will be set to an internal request with the callerHandle and the serviceId.
   * After creating the RequestMessage, the state will be set into a request map.
   * This function sends the request using the sendRequest function from the SessionContext.
   */
   const prepareAndSend = React.useCallback((serviceId: string, request: IRequestBody, state?: InternalRequest, callerHandle?: number) => {
      if (!state) {
         state = {
            callerHandle: callerHandle,
            internalHandle: HandleFactory.increment(),
            serviceId: serviceId
         } as InternalRequest;
      }
       const message: IRequestMessage = {
         ServiceId: serviceId,
         Body: request
      };
      console.error("requests(internal=" + state.internalHandle + ", caller=" + state.callerHandle + ")");
      m.current.requests.set(state.internalHandle, state);
      sendRequest(message, state.internalHandle);
   }, [sendRequest]);

  /**
  * browseChildren: Function to handle a browse operation in OPC UA.
  * 
  * @param callerHandle - The handle of the caller initiating the browse operation.
  * @param parentId - The NodeId of the parent node to browse.
  * 
  * This function prepares an OPC UA BrowseRequest and sends it using the prepareAndSend function.
  * If the parentId is not provided, it sets the last completed browse result with the caller handle.
  */
   const browseChildren = React.useCallback(async (callerHandle: number, parentId: string): Promise<void> => {
      if (!parentId) {
         setLastCompletedBrowse({ callerHandle: callerHandle });
         return;
      }
      const request: OpcUa.BrowseRequest = {
         RequestedMaxReferencesPerNode: 20,
         NodesToBrowse: [
            {
               NodeId: parentId,
               BrowseDirection: OpcUa.BrowseDirection.Forward,
               ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
               IncludeSubtypes: true,
               ResultMask: 63
            }
         ]
      };
      prepareAndSend(OpcUa.DataTypeIds.BrowseRequest, request, undefined, callerHandle);
   }, [prepareAndSend]);

  /**
  * browseNext: Function to handle the continuation of a browse operation in OPC UA.
  * 
  * @param state - The internal request state containing details of the current browse operation.
  * @param continuationPoint - The continuation point to resume the browse operation.
  * @param releaseContinuationPoints - A boolean indicating whether to release the continuation points after the browse operation.
  * 
  * This function prepares an OPC UA BrowseNextRequest and sends it using the prepareAndSend function.
  */
   const browseNext = React.useCallback(async (state: InternalRequest, continuationPoint: string, releaseContinuationPoints: boolean): Promise<void> => {
      const request: OpcUa.BrowseNextRequest = {
         ReleaseContinuationPoints: releaseContinuationPoints,
         ContinuationPoints: [ continuationPoint ]
      };
       prepareAndSend(OpcUa.DataTypeIds.BrowseNextRequest, request, state, undefined);
   }, [prepareAndSend]);

   /**
   * read: Function to handle a read operation in OPC UA.
   * 
   * @param state - The internal request state containing details of the current read operation.
   * 
   * This function prepares an OPC UA ReadRequest using the nodes to read from the state.
   * It then sends the request using the prepareAndSend function.
   */
   const read = React.useCallback(async (state: InternalRequest): Promise<void> => {
      const valuesToRead: OpcUa.ReadValueId[] = [];
      state.nodesToRead?.forEach((item) => {
         valuesToRead.push({
            NodeId: item.resolvedNodeId,
            AttributeId: item.attributeId ?? OpcUa.Attributes.Value
         });
      });
      const request: OpcUa.ReadRequest = {
         MaxAge: 0,
         NodesToRead: valuesToRead
      };
       prepareAndSend(OpcUa.DataTypeIds.ReadRequest, request, state, undefined);
   }, [prepareAndSend]);

   /**
   * readValues: Function to handle reading values of nodes found during browsing in OPC UA.
   *
   * @param callerHandle - The handle of the caller initiating the read operation.
   * @param nodesToRead - An array of nodes to read, each containing details like nodeId, attributeId, and path.
   * 
   * This function prepares an OPC UA ReadRequest or TranslateBrowsePathsToNodeIdsRequest based on the nodes to read.
   * If the nodes have unresolved node IDs and paths, it prepares a TranslateBrowsePathsToNodeIdsRequest to resolve them.
   * Otherwise, it directly prepares a ReadRequest to read the values of the nodes.
   * The request is sent using the prepareAndSend function.
   */
   const readValues = React.useCallback(async (callerHandle: number, nodesToRead: IReadValueId[]): Promise<void> => {
      if (!nodesToRead.length) {
         setLastCompletedBrowse({ callerHandle: callerHandle });
         return;
      }
      const browsePaths: OpcUa.BrowsePath[] = [];
      const nodesToTranslate: IReadValueId[] = [];
      const values: IReadResult[] = [];
      nodesToRead.forEach((item) => {
         values.push({
            id: item.id ?? values.length,
            nodeId: item.resolvedNodeId ?? item.nodeId,
            attributeId: item.attributeId ?? OpcUa.Attributes.Value,
            value: { StatusCode: { Code: OpcUa.StatusCodes.BadNodeIdUnknown } }
         } as IReadResult);
         if (item.resolvedNodeId) {
            return;
         }
         if (!item.path?.length) {
            item.resolvedNodeId = item.nodeId;
            return;
         }
         browsePaths.push({
            StartingNode: item.nodeId,
            RelativePath: {
               Elements: item.path?.map((path) => {
                  return {
                     ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
                     IsInverse: false,
                     IncludeSubtypes: true,
                     TargetName: path
                  } as OpcUa.ReferenceDescription
               })
            }
         });
         nodesToTranslate.push(item);
      });
      const state = {
         callerHandle: callerHandle,
         internalHandle: HandleFactory.increment(),
         nodesToRead: nodesToRead
      } as InternalRequest;
      if (browsePaths.length) {
         const request: OpcUa.TranslateBrowsePathsToNodeIdsRequest = {
            BrowsePaths: browsePaths
         };
         state.nodesToTranslate = nodesToTranslate;
          prepareAndSend(OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsRequest, request, state, undefined);
         return;
      }
      read(state);
   }, [prepareAndSend, read]);


   /*
   const getSubmodel = React.useCallback(async (callerHandle: number, parentId: string): Promise<void> => {
        
      const request: AAS.GetSubmodelRequest = {
            
      };
      prepareAndSend(AAS.DataTypeIds.GetSubmodelRequest, request, undefined, callerHandle, "aas");
   }, [prepareAndSend]);
   */

  /**
   * Response handling
   * 
   * 
   */
   React.useEffect(() => {
      const request = m.current.requests.get(lastCompletedRequest?.callerHandle ?? 0);
       console.error("lastCompletedRequest(internal=" + (lastCompletedRequest?.callerHandle ?? 0) + ", caller=" + (request?.callerHandle ?? 0) + ")");
       //Check if response is from OPC UA or AAS
       if (lastCompletedRequest?.response?.Body?.ResponseHeader?.RequestHandle) {
           if (request) {
               if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.BrowseResponse) {
                   const children: IBrowsedNode[] = request.children = [];
                   const response = lastCompletedRequest.response?.Body as OpcUa.BrowseResponse;
                   response?.Results?.forEach((result) => {
                       if (!result?.StatusCode) {
                           result?.References?.forEach((reference) => {
                               children.push({
                                   id: reference.NodeId ?? '',
                                   reference: reference
                               });
                           });
                           if (result?.ContinuationPoint) {
                               console.error("browseNext[" + children.length + "]: " + request.callerHandle);
                               browseNext(request, result?.ContinuationPoint, false);
                           }
                           else {
                               children.forEach((child) => m.current.nodes.set(child.id, child));
                               console.error("BrowseResponse[" + children.length + "]: " + request.callerHandle);
                               setLastCompletedBrowse({ callerHandle: request.callerHandle, children: children });
                           }
                       }
                   });
               }
               else if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.BrowseNextResponse) {
                   const children: IBrowsedNode[] = request.children ?? [];
                   const response = lastCompletedRequest.response?.Body as OpcUa.BrowseNextResponse;
                   response?.Results?.forEach((result) => {
                       if (!result?.StatusCode) {
                           result?.References?.forEach((reference) => {
                               children.push({
                                   id: reference.NodeId ?? '',
                                   reference: reference
                               });
                           });
                           if (result?.ContinuationPoint) {
                               console.error("browseNext[" + children.length + "]: " + request.callerHandle);
                               browseNext(request, result?.ContinuationPoint, false);
                           }
                           else {
                               children.forEach((child) => m.current.nodes.set(child.id, child));
                               console.error("BrowseNextResponse[" + children.length + "]: " + request.callerHandle);
                               setLastCompletedBrowse({ callerHandle: request.callerHandle, children: children });
                           }
                       }
                   });
               }
               else if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsResponse) {
                   const nodesToTranslate: IReadValueId[] = request.nodesToTranslate ?? [];
                   const response = lastCompletedRequest.response?.Body as OpcUa.TranslateBrowsePathsToNodeIdsResponse;
                   response.Results?.forEach((item, index) => {
                       if (!item.StatusCode && item?.Targets?.at(0)?.RemainingPathIndex === 4294967295) {
                           nodesToTranslate[index].resolvedNodeId = item.Targets[0].TargetId;
                       }
                   });
                   console.error("read[" + nodesToTranslate.length + "]: " + request.callerHandle);
                   read(request);
               }
               else if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.ReadResponse) {
                   const values: IReadResult[] = [];
                   const nodesToRead: IReadValueId[] = request.nodesToRead ?? [];
                   const response = lastCompletedRequest.response?.Body as OpcUa.ReadResponse;
                   response.Results?.forEach((item, index) => {
                       values.push({
                           id: nodesToRead[index].id,
                           nodeId: nodesToRead[index].nodeId,
                           attributeId: nodesToRead[index].attributeId,
                           value: item
                       });
                       if (nodesToRead[index].attributeId == OpcUa.Attributes.Value) {
                           const node = m.current.nodes.get(nodesToRead[index].nodeId);
                           if (node) {
                               node.value = item;
                           }
                       }
                   });
                   console.error("ReadResponse[" + values.length + "]: " + request.callerHandle);
                   setLastCompletedBrowse({ callerHandle: request.callerHandle, values: values });
               }
           }
       }
       else if (lastCompletedRequest?.response?.Body?.ResponseHeader?.AASRequestHandle) { // --> for Juliee 
           
       }
   }, [lastCompletedRequest, setLastCompletedBrowse, browseNext, read]);
   
   const browseContext = {
      stateChangeCount: m.current.stateChangeCount,
      nodes: m.current.nodes,
      visibleNodes: visibleNodes,
      setVisibleNodes: setVisibleNodes,
      lastCompletedRequest: lastCompletedBrowse,
      browseChildren: browseChildren,
      readValues: readValues
   } as IBrowseContext;

   return (
      <BrowseContext.Provider value={browseContext}>
         {children}
      </BrowseContext.Provider>
   );
};

export default BrowseProvider;
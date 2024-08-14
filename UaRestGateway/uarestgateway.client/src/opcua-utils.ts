  /* eslint-disable @typescript-eslint/no-explicit-any */
import * as OpcUa from './opcua';
import * as pako from 'pako';
import { Account } from './user/Account';
import { IResponseMessage } from './service/IResponseMessage';
import { ICompletedRequest } from './service/ICompletedRequest';
import { IBrowsedNode } from './service/IBrowsedNode';
import { IReadResult } from './service/IReadResult';
import { HandleFactory } from './service/HandleFactory';
import { IReadValueId } from './service/IReadValueId';

function toFault(request: ICompletedRequest, response?: IResponseMessage): ICompletedRequest | null {
   if (response?.Body?.ResponseHeader) {
      const responseHeader = response.Body.ResponseHeader;
      if (!responseHeader.ServiceResult || responseHeader.ServiceResult === 0) {
         return null;
      }
      const stringTable = response.Body?.ResponseHeader?.StringTable;
      const serviceDiagnostics = response.Body?.ResponseHeader?.ServiceDiagnostics;
      if (!stringTable?.length || !serviceDiagnostics) {
         return { ...request, code: responseHeader.ServiceResult };
      }
      let message = '';
      if ((serviceDiagnostics?.SymbolicId && serviceDiagnostics?.SymbolicId > 0) || serviceDiagnostics?.SymbolicId === 0) {
         message += `[${stringTable?.[serviceDiagnostics?.SymbolicId]}] `;
      }
      if ((serviceDiagnostics?.LocalizedText && serviceDiagnostics?.LocalizedText > 0) || serviceDiagnostics?.LocalizedText === 0) {
         message += `'${stringTable?.[serviceDiagnostics?.LocalizedText]}'`;
      }
      return {
         ...request,
         code: responseHeader.ServiceResult,
         message: message
      }
   }
   return {
      ...request,
      code: OpcUa.StatusCodes.BadUnknownResponse
   }
}

function stringToUtf8ByteArray(str: string): Uint8Array {
   const encoder = new TextEncoder();
   return encoder.encode(str);
}

async function gzip(data: string): Promise<Uint8Array> {
   return new Promise((resolve) => {
      const encoder = new TextEncoder();
      const compressedData = pako.gzip(encoder.encode(data));
      resolve(compressedData);
   });
}

async function readResponseBody(url: string, response: Response) {
   const content = response.headers.get("Content-Type");
   if (content && content.indexOf("json") < 0) {
      console.error("UnexpectedResponse: " + await response.text());
      return null;
   }
   return await response.clone().json();

   //const data = await response.arrayBuffer();
   //const view = new DataView(data);
   //console.log("URL: " + url + "[Size=" + view.byteLength + "]");

   //if (view.byteLength < 2) {
   //   console.error("EmptyBody!");
   //   return null;
   //}

   //let json = undefined;
   //if (view.getUint8(0) === 0x1f && view.getUint8(1) === 0x8b) {
   //   const decompressed = pako.deflate(new Uint8Array(data));
   //   json = new TextDecoder().decode(decompressed);
   //}
   //else {
   //   json = new TextDecoder().decode(data);
   //}

   //return JSON.parse(json);
}

export async function call(
   url: string,
   result: ICompletedRequest,
   controller?: AbortController,
   user?: Account,
   compress?: boolean) {
   const request = result.request;
   const timeoutId = (request?.Body?.RequestHeader?.TimeoutHint && controller)
      ? setTimeout(() => controller.abort(), request?.Body?.RequestHeader?.TimeoutHint)
      : null;
   try {
      const json = JSON.stringify(request);
      const body = (compress) ? await gzip(json) : stringToUtf8ByteArray(json);
      const requestOptions = {
         method: 'POST',
         mode: 'cors',
         cache: 'no-cache',
         headers: {
            ...(compress ? { 'Content-Encoding': 'gzip' } : {}),
            'Content-Type': 'application/json',
            'Authorization': (user?.accessToken) ? `Bearer ${user?.accessToken}` : ''
         },
         credentials: 'include',
         body: body,
         signal: controller?.signal
      } as RequestInit;
      const response = await fetch(url, requestOptions);
      if (timeoutId) clearTimeout(timeoutId);
      if (response.ok) {
         const body = await readResponseBody(url, response);
         const fault = toFault(result, body);
         if (fault) {
            return fault;
         }
         return body;
      }
      else {
         console.info(`call: ${response.status} ${response.statusText}`);
         return {
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: `HTTP ${response.status} ${response.statusText}`
         };
      }
   }
   catch (exception: any) {
      if (timeoutId) clearTimeout(timeoutId);
      if (exception.code) {
         console.info(`call: ${exception.code} ${exception.message}`);
         return exception;
      } else {
         console.info(`call: BadUnexpectedError ${exception.toString()}`);
         return {
            code: OpcUa.StatusCodes.BadUnexpectedError,
            message: exception.toString()
         };
      }
   }
}

export async function browseChildren(
   nodeId: string,
   requestTimeout: number = 120000,
   user?: Account
): Promise<IBrowsedNode[] | undefined> {

   const controller: AbortController = new AbortController();
   const request: OpcUa.BrowseRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout
         },
         RequestedMaxReferencesPerNode: 20,
         NodesToBrowse: [
            {
               NodeId: nodeId,
               BrowseDirection: OpcUa.BrowseDirection.Forward,
               ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
               IncludeSubtypes: true,
               ResultMask: 63
            }
         ]
      }
   };
   const result: ICompletedRequest = { clientHandle: HandleFactory.increment(), request };
   const response = await call(`/opcua/browse`, result, controller, user, true);
   if (!response) {
      return undefined;
   }
   if (response.code) {
      console.warn(`Browse call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return undefined;
   }
   const nodes: IBrowsedNode[] = [];
   let continuationPoints: string[] = [];
   let body: OpcUa.BrowseResponse = response.Body;
   do {
      continuationPoints = [];
      body.Results?.forEach(x => x.References?.forEach((y) => {
         if (OpcUa.StatusCode.isGood(x.StatusCode)) {
            nodes.push({
               id: `${y.NodeId}`,
               reference: y
            });
            if (x.ContinuationPoint) {
               continuationPoints.push(x.ContinuationPoint);
            }
         }
      }));
      if (continuationPoints.length > 0) {
         const request: OpcUa.BrowseNextRequestMessage = {
            Body: {
               RequestHeader: {
                  Timestamp: new Date(),
                  TimeoutHint: requestTimeout
               },
               ReleaseContinuationPoints: false,
               ContinuationPoints: continuationPoints
            }
         };
         result.request = request;
         const response = await call(`/opcua/browsenext`, result, controller, user, true);
         if (!response) {
            return undefined;
         }
         if (response.code) {
            console.warn(`BrowseNext call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
            return undefined;
         }
         body = response.Body;
      }
   } while (continuationPoints.length > 0);

   return nodes;
}

export async function readAttributes(
   reference: OpcUa.ReferenceDescription,
   requestTimeout: number = 120000,
   user?: Account
): Promise<IReadResult[] | null> {

   const controller: AbortController = new AbortController();
   const request: OpcUa.ReadRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout
         },
         MaxAge: 0,
         TimestampsToReturn: OpcUa.TimestampsToReturn.Server,
         NodesToRead: []
      }
   };
   for (const id in OpcUa.Attributes) {
      request.Body?.NodesToRead?.push(
         {
            NodeId: reference.NodeId,
            AttributeId: Number(OpcUa.Attributes[id])
         });
   }
   const response = await call(`/opcua/read`, { clientHandle: HandleFactory.increment(), request }, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`Read call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }
   const body: OpcUa.ReadResponse = response.Body
   const values: IReadResult[] = [];
   if (body.Results && request.Body?.NodesToRead) {
      for (let ii = 0; ii < body.Results?.length; ii++) {
         const x = body.Results[ii];
         if (x.StatusCode !== OpcUa.StatusCodes.BadAttributeIdInvalid) {
            values.push({
               id: HandleFactory.increment(),
               nodeId: reference.NodeId ?? '',
               attributeId: request.Body.NodesToRead[ii].AttributeId ?? 0,
               value: x
            });
         }
      }
   }
   return values;
}

export async function readValues(
   variableIds: string[],
   requestTimeout: number,
   user?: Account
): Promise<IReadResult[] | null> {

   const controller: AbortController = new AbortController();
   const request: OpcUa.ReadRequestMessage = {
      ServiceId: OpcUa.ReadRequestMessageServiceIdEnum.NUMBER_629,
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout
         },
         MaxAge: 0,
         TimestampsToReturn: OpcUa.TimestampsToReturn.Both,
         NodesToRead: []
      }
   };
   variableIds.map(ii => {
      request.Body?.NodesToRead?.push(
         {
            NodeId: ii,
            AttributeId: OpcUa.Attributes.Value
         });
      return null;
   });
   const values: IReadResult[] = [];
   if (request.Body?.NodesToRead?.length) {
      const response = await call(`/opcua/read`, { clientHandle: HandleFactory.increment(), request }, controller, user, true);
      if (!response) {
         return null;
      }
      if (response.code) {
         console.warn(`Read call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
         return null;
      }
      const body: OpcUa.ReadResponse = response.Body
      if (body.Results && request.Body?.NodesToRead) {
         for (let ii = 0; ii < body.Results?.length; ii++) {
            const item = body.Results[ii];
            values.push({
               id: HandleFactory.increment(),
               nodeId: variableIds[ii] ?? '',
               attributeId: request.Body.NodesToRead[ii].AttributeId ?? 0,
               value: item
            });
         }
      }
   }
   return values;
}

export async function translateAndReadValues(
   nodesToRead: IReadValueId[],
   requestTimeout: number,
   user?: Account
): Promise<IReadResult[] | null> {
   const controller: AbortController = new AbortController();
   const browsePaths: OpcUa.BrowsePath[] = [];
   const nodesToTranslate: IReadValueId[] = [];
   const values: IReadResult[] = [];
   nodesToRead.forEach((item) => {
      values.push({
         id: item.id ?? values.length,
         nodeId: item.resolvedNodeId ?? item.nodeId,
         attributeId: item.attributeId ?? OpcUa.Attributes.Value,
         value: { StatusCode: OpcUa.StatusCodes.BadNodeIdUnknown }
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
               }
            })
         }
      });
      nodesToTranslate.push(item);
   });
   if (browsePaths.length) {
      const request: OpcUa.TranslateBrowsePathsToNodeIdsRequestMessage = {
         ServiceId: OpcUa.TranslateBrowsePathsToNodeIdsRequestMessageServiceIdEnum.NUMBER_552,
         Body: {
            RequestHeader: {
               Timestamp: new Date(),
               TimeoutHint: requestTimeout
            },
            BrowsePaths: browsePaths
         }
      };
      const response= await call(`/opcua/translate`, { clientHandle: HandleFactory.increment(), request }, controller, user, true);
      if (!response) {
         return null;
      }
      if (response.code) {
         console.warn(`Translate call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
         return null;
      }
      const body : OpcUa.TranslateBrowsePathsToNodeIdsResponse = response.Body;
      body.Results?.forEach((item, index) => {
         if (!item.StatusCode && item?.Targets?.at(0)?.RemainingPathIndex === 4294967295) {
            nodesToTranslate[index].resolvedNodeId = item.Targets[0].TargetId;
         }
      });
   }
   const valuesToRead: OpcUa.ReadValueId[] = [];
   const subsetOfResults: (IReadResult | undefined)[] = []; 
   nodesToRead.forEach((item, index) => {
      if (!item.resolvedNodeId) {
         return;
      }
      valuesToRead.push({
         NodeId: item.resolvedNodeId,
         AttributeId: item.attributeId ?? OpcUa.Attributes.Value
      });
      item.id = valuesToRead.length;
      subsetOfResults.push(values.at(index));
   });
   if (valuesToRead.length) {
      const request: OpcUa.ReadRequestMessage = {
         ServiceId: OpcUa.ReadRequestMessageServiceIdEnum.NUMBER_629,
         Body: {
            RequestHeader: {
               Timestamp: new Date(),
               TimeoutHint: requestTimeout
            },
            MaxAge: 0,
            NodesToRead: valuesToRead
         }
      };
      const response = await call(`/opcua/read`, { clientHandle: HandleFactory.increment(), request }, controller, user, true);
      if (!response) {
         return null;
      }
      if (response.code) {
         console.warn(`Read call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
         return null;
      }
      const body: OpcUa.ReadResponse = response.Body;
      subsetOfResults.forEach((item, index) => {
         if (item?.value) {
            item.value = body.Results?.at(index);
         }
      });
   }
   return values;
}
/* eslint-disable @typescript-eslint/no-explicit-any */
import { IBrowseTreeNode } from './ApplicationProvider';
import { IUserContext } from './UserProvider';
import { Account } from './api';
import * as OpcUa from './opcua';
import * as pako from 'pako';

export class HandleFactory {
   private static counter: number = 0;
   public static increment(): number {
      return ++this.counter;
   }
}
export enum SessionState {
   Closed = 0,
   Opening = 1,
   Open = 2
}

export interface IMonitoredItem {
   path: string[],
   name: string,
   nodeId: string,
   browsePath: string[],
   resolvedNodeId: string,
   clientHandle: number,
   serverHandle: number,
   attributeId: number,
   value: OpcUa.DataValue
}

export interface ISession {
   state: SessionState,
   sessionId?: string,
   serverNonce?: string,
   authenticationToken?: string,
   subscriptionId?: number,
   lastSequenceNumber?: number,
   acknowledgements?: OpcUa.SubscriptionAcknowledgement[],
   monitoredItems?: IMonitoredItem[]
}

export interface NodeAttributeValue {
   id: number,
   reference: OpcUa.ReferenceDescription;
   attributeId: OpcUa.Attributes;
   value?: OpcUa.DataValue;
}

export interface IServiceFault {
   code?: number;
   message?: string;
}

interface ServiceFault {
   ResponseHeader?: OpcUa.ResponseHeader;
}

interface IServiceFaultMessage {
   NamespaceUris?: Array<string>;
   ServerUris?: Array<string>;
   ServiceId?: number;
   Body: ServiceFault;
}

interface IServiceRequestMessage {
   NamespaceUris?: Array<string>;
   ServerUris?: Array<string>;
   ServiceId?: number;
   Body?: any;
}

function toFault(response?: IServiceFaultMessage): IServiceFault | null {
   if (response?.Body?.ResponseHeader) {
      const responseHeader = response.Body.ResponseHeader;
      if (!responseHeader.ServiceResult || responseHeader.ServiceResult === 0) {
         return null;
      }
      const stringTable = response.Body?.ResponseHeader?.StringTable;
      const serviceDiagnostics = response.Body?.ResponseHeader?.ServiceDiagnostics;
      if (!stringTable?.length || !serviceDiagnostics) {
         return { code: responseHeader.ServiceResult };
      }
      let message = '';
      if ((serviceDiagnostics?.SymbolicId && serviceDiagnostics?.SymbolicId > 0) || serviceDiagnostics?.SymbolicId === 0) {
         message += `[${stringTable?.[serviceDiagnostics?.SymbolicId]}] `;
      }
      if ((serviceDiagnostics?.LocalizedText && serviceDiagnostics?.LocalizedText > 0) || serviceDiagnostics?.LocalizedText === 0) {
         message += `'${stringTable?.[serviceDiagnostics?.LocalizedText]}'`;
      }
      return {
         code: responseHeader.ServiceResult,
         message: message
      }
   }
   return {
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

async function readResponseBody(url: string, response: any) {
   console.log("URL: " + url);
   const content = response.headers.get("Content-Type");
   if (content && content.indexOf("json") < 0) {
      console.error("UnexpectedResponse: " + await response.text());
      return null;
   }
   // the fetch library automatically uncompresses gzipped content.
   return await response.json();
}

export async function call(
   url: string,
   request: IServiceRequestMessage,
   controller?: AbortController,
   user?: Account,
   compress?: boolean) {

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
         const fault = toFault(body);
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
   controller?: AbortController,
   user?: Account
): Promise<IBrowseTreeNode[] | undefined> {
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
   const response = await call(`/opcua/browse`, request, controller, user, true);
   if (!response) {
      return undefined;
   }
   if (response.code) {
      console.warn(`Browse call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return undefined;
   }
   const nodes: IBrowseTreeNode[] = [];
   let continuationPoints: string[] = [];
   let result: OpcUa.BrowseResponse = response.Body;

   do {
      continuationPoints = [];
      result.Results?.forEach(x => x.References?.forEach((y) => {
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
         const response = await call(`/opcua/browsenext`, request, controller, user, true);
         if (!response) {
            return undefined;
         }
         if (response.code) {
            console.warn(`BrowseNext call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
            return undefined;
         }
         result = response.Body;
      }
   } while (continuationPoints.length > 0);

   return nodes;
}

export async function readAttributes(
   reference: OpcUa.ReferenceDescription,
   requestTimeout: number = 120000,
   controller?: AbortController,
   user?: Account
): Promise<NodeAttributeValue[] | null> {

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

   const response = await call(`/opcua/read`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`Read call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   const result: OpcUa.ReadResponse = response.Body
   const values: NodeAttributeValue[] = [];

   if (result.Results && request.Body?.NodesToRead) {
      for (let ii = 0; ii < result.Results?.length; ii++) {
         const x = result.Results[ii];
         if (x.StatusCode !== OpcUa.StatusCodes.BadAttributeIdInvalid) {
            values.push({
               id: HandleFactory.increment(),
               reference: reference,
               attributeId: request.Body.NodesToRead[ii].AttributeId ?? 0,
               value: x
            });
         }
      }
   }

   return values;
}

export async function readValues(
   variables: IBrowseTreeNode[],
   requestTimeout: number = 120000,
   controller?: AbortController,
   user?: Account
): Promise<IBrowseTreeNode[] | null> {

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

   variables.map(ii => {
      request.Body?.NodesToRead?.push(
         {
            NodeId: ii.id,
            AttributeId: OpcUa.Attributes.Value
         });
      return null;
   });

   const response = await call(`/opcua/read`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`Read call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   const result: OpcUa.ReadResponse = response.Body
   const values: IBrowseTreeNode[] = [];

   if (result.Results && request.Body?.NodesToRead) {
      for (let ii = 0; ii < result.Results?.length; ii++) {
         const item = result.Results[ii];
         if (OpcUa.StatusCode.isBad(item.StatusCode)) {
            values.push({ ...variables[ii], value: JSON.stringify(item.StatusCode) });
         }
         else {
            if (item.Value?.Type === 21) { // LocalizedText
               values.push({ ...variables[ii], value: item.Value?.Body?.Text });
            }
            else if (item.Value?.Type === 12) { // String
               values.push({ ...variables[ii], value: item.Value?.Body });
            }
            else {
               values.push({ ...variables[ii], value: JSON.stringify(item.Value?.Body) });
            }
         }
      }
   }

   return values;
}
async function createSession(
   user: Account,
   requestTimeout: number,
   controller?: AbortController
): Promise<ISession | null> {
   const uuid = window.crypto.randomUUID();

   const request: OpcUa.CreateSessionRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout
         },
         ClientDescription: {
            ApplicationUri: 'urn:localhost:UA:uarestgateway.client',
            ProductUri: 'uarestgateway.client',
            ApplicationName: { Text: 'uarestgateway.client' },
            ApplicationType: OpcUa.ApplicationType.Client
         },
         EndpointUrl: window.location.href.split('?')[0],
         SessionName: uuid,
         ClientNonce: undefined,
         ClientCertificate: undefined,
         RequestedSessionTimeout: 120000,
         MaxResponseMessageSize: 8 * 1014 * 1024
      }
   };

   const response = await call(`/opcua/createsession`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`CreateSession call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   response.Body.ServerNonce = response.Body.ServerNonce ?? new Uint8Array(0);

   return {
      sessionId: response.Body.SessionId,
      serverNonce: response.Body.ServerNonce.toString(),
      authenticationToken: response.Body.AuthenticationToken
   } as ISession;
}

async function activateSession(
   user: Account,
   session: ISession,
   language: string,
   requestTimeout: number,
   controller?: AbortController
): Promise<ISession | null> {
   const request: OpcUa.ActivateSessionRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout,
            AuthenticationToken: session.authenticationToken
         },
         LocaleIds: [language ?? " en"]
      }
   };

   const response = await call(`/opcua/activatesession`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`ActivateSession call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   return {
      ...session,
      serverNonce: response.Body.ServerNonce.toString(),
   } as ISession;
}

async function createSubscription(
   user: Account,
   session: ISession,
   requestTimeout: number,
   controller?: AbortController
): Promise<ISession | null> {

   const request: OpcUa.CreateSubscriptionRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout,
            AuthenticationToken: session.authenticationToken
         },
         RequestedPublishingInterval: 1000,
         RequestedLifetimeCount: 60,
         RequestedMaxKeepAliveCount: 10,
         MaxNotificationsPerPublish: 1000,
         PublishingEnabled: true,
         Priority: 100,
      }
   };

   const response = await call(`/opcua/createsubscription`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`CreateSubscription call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   return {
      ...session,
      subscriptionId: response.Body.SubscriptionId ?? 0
   } as ISession;
}

export async function publish(
   user: Account,
   session: ISession,
   monitoredItems: Map<number, IMonitoredItem>,
   requestTimeout: number,
   controller?: AbortController
): Promise<{ session: ISession, monitoredItems: { clientHandle: number, value?: OpcUa.DataValue }[] } | null> {

   const request: OpcUa.PublishRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout,
            AuthenticationToken: session.authenticationToken
         },
         SubscriptionAcknowledgements: session.acknowledgements ?? [],
      }
   };

   const response = await call(`/opcua/publish`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`Publish call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   const acknowledgements = [];

   // console.warn(`SequenceNumber ${response.Body.NotificationMessage.SequenceNumber}`);

   const updates: { clientHandle: number, value?: OpcUa.DataValue }[] = [];
   const message = response.Body.NotificationMessage;
   if (message) {
      const notifications: Array<OpcUa.ExtensionObject> = message.NotificationData;
      if (notifications) {
         notifications.forEach(x => {
            if (x.TypeId === OpcUa.DataTypeIds.DataChangeNotification) {
               const dataChange = x.Body as OpcUa.DataChangeNotification;
               if (dataChange.MonitoredItems) {
                  dataChange.MonitoredItems.forEach(y => {
                     if (y.ClientHandle) {
                        updates.push({ clientHandle: y.ClientHandle ?? 0, value: y.Value });
                     }
                  });
               }
            }
         });
      }

      acknowledgements.push({ SubscriptionId: response.Body.SubscriptionId, SequenceNumber: message.SequenceNumber });

      return {
         session: {
            ...session,
            lastSequenceNumber: response.Body.NotificationMessage.SequenceNumber,
            acknowledgements: acknowledgements
         } as ISession,
         monitoredItems: updates
      };
   }

   return { session: session, monitoredItems: updates };
}

export async function readInitialValues(
   user: Account,
   monitoredItems: IMonitoredItem[],
   requestTimeout: number,
   controller?: AbortController
): Promise<void | null> {

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

   monitoredItems.map(ii => {
      request.Body?.NodesToRead?.push(
         {
            NodeId: ii.resolvedNodeId ?? ii.nodeId,
            AttributeId: OpcUa.Attributes.Value
         });
      return null;
   });

   const response = await call(`/opcua/read`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`Read call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   const result: OpcUa.ReadResponse = response.Body

   if (result.Results && request.Body?.NodesToRead) {
      for (let ii = 0; ii < result.Results?.length; ii++) {
         monitoredItems[ii].value = result.Results[ii];
      }
   }
}

async function createMonitoredItems(
   user: Account,
   session: ISession,
   requestTimeout: number,
   monitoredItems: IMonitoredItem[],
   controller?: AbortController
): Promise<IMonitoredItem[] | null> {

   const request: OpcUa.CreateMonitoredItemsRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout,
            AuthenticationToken: session.authenticationToken
         },
         SubscriptionId: session.subscriptionId ?? 0,

         TimestampsToReturn: OpcUa.TimestampsToReturn.Both,
         ItemsToCreate: monitoredItems.map(item => {
            if (!item.clientHandle) item.clientHandle = HandleFactory.increment();
            return {
               ItemToMonitor: {
                  NodeId: item.resolvedNodeId ?? item.nodeId,
                  AttributeId: item.attributeId ?? OpcUa.Attributes.Value
               },
               MonitoringMode: OpcUa.MonitoringMode.Reporting,
               RequestedParameters: {
                  ClientHandle: item.clientHandle,
                  SamplingInterval: 1000,
                  QueueSize: 0,
                  DiscardOldest: true
               }
            } as OpcUa.MonitoredItemCreateRequest;
         })
      }
   };

   const response = await call(`/opcua/createmonitoreditems`, request, controller, user, true);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`CreateMonitoredItems call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   response.Body.Results.forEach((result: OpcUa.MonitoredItemCreateResult, index: number) => {
      if (OpcUa.StatusCode.isGood(result.StatusCode)) {
         monitoredItems[index].serverHandle = result.MonitoredItemId ?? 0;
      }
   });

   return monitoredItems;
}

export function stripUris(input: string[]): string[] {
   const output: string[] = [];
   if (!input?.length) {
      return output;
   }
   input.forEach(x => {
      const parts = x.split(';');
      output.push(parts[parts.length - 1]);
   });
   return output;
}

export async function translateAndSubscribe(
   user: Account,
   session: ISession,
   requestTimeout: number,
   monitoredItems: IMonitoredItem[],
   controller?: AbortController
): Promise<IMonitoredItem[] | null> {

   const request: OpcUa.TranslateBrowsePathsToNodeIdsRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout,
            AuthenticationToken: session.authenticationToken
         },
         BrowsePaths: []
      }
   };

   const itemsToTranslate: number[] = [];

   monitoredItems.forEach((monitoredItem: IMonitoredItem, index: number) => {
      if (monitoredItem.browsePath?.length) {
         const elements: OpcUa.RelativePathElement[] = [];
         monitoredItem.browsePath.forEach(element => {
            elements.push({
               ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
               IsInverse: false,
               IncludeSubtypes: true,
               TargetName: element
            });
         });
         itemsToTranslate.push(index);
         request.Body?.BrowsePaths?.push({
            StartingNode: monitoredItem.nodeId,
            RelativePath: {
               Elements: elements
            }
         });
      }
   });

   if (itemsToTranslate.length) {
      const response = await call(`/opcua/translate`, request, controller, user, true);
      if (!response) {
         return null;
      }
      if (response.code) {
         console.warn(`TranslateBrowsePathsToNodeIds call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
         return null;
      }
      response.Body.Results.forEach((result: OpcUa.BrowsePathResult, index: number) => {
         if (OpcUa.StatusCode.isGood(result.StatusCode) && result.Targets?.length) {
            if (result.Targets[0].TargetId && (result.Targets[0].RemainingPathIndex ?? 0) > 255) {
               const item = monitoredItems[itemsToTranslate[index]];
               item.resolvedNodeId = result.Targets[0].TargetId;
               item.attributeId = OpcUa.Attributes.Value;
               item.serverHandle = 0;
               item.value = { StatusCode: OpcUa.StatusCodes.BadWaitingForInitialData };
            }
         }
      });
   }

   await readInitialValues(user, monitoredItems, requestTimeout, controller);

   if (session.state === SessionState.Open) {
      return await createMonitoredItems(user, session, requestTimeout, monitoredItems, controller);
   }

   return monitoredItems;
}

export async function connect(
   context: IUserContext,
   session: ISession,
   requestTimeout: number,
   controller?: AbortController
): Promise<ISession | null> {

   let result = await createSession(context.user, requestTimeout, controller);
   if (result) {
      session = result;
      await activateSession(context.user, result, context.language, requestTimeout, controller);
      if (result) {
         session = result;
         result = await createSubscription(context.user, result, requestTimeout, controller);
         if (result) {
            return {
               ...session,
               subscriptionId: result.subscriptionId,
               state: SessionState.Open,
               monitoredItems: []
            } as ISession;
         }
      }
   }
   return null;
}

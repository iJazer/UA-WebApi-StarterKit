/* eslint-disable @typescript-eslint/no-explicit-any */
import { IBrowseTreeNode } from './ApplicationProvider';
import { IUserContext } from './UserProvider';
import { Account } from './api';
import * as OpcUa from './opcua';

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

async function readResponseBody(url: string, response: any) {
   const content = response.headers.get("Content-Type");

   if (content && content.indexOf("json") < 0) {
      console.error("URL: " + url);
      console.error("UnexpectedResponse: " + await response.text());
      return null;
   }

   return await response.json();
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
export async function call(
   url: string,
   request: IServiceRequestMessage,
   controller?: AbortController,
   user?: Account) {

   const timeoutId = (request?.Body?.RequestHeader?.TimeoutHint && controller)
      ? setTimeout(() => controller.abort(), request?.Body?.RequestHeader?.TimeoutHint)
      : null;
   try {
      const requestOptions = {
         method: 'POST',
         headers: {
            'Content-Type': 'application/json',
            'Authorization': (user?.accessToken)?`Bearer ${user?.accessToken}`:''
         },
         body: JSON.stringify(request),
         signal: controller?.signal
      };
      // console.info(`call: ${url}`);
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
   serverId?: string,
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
   const response = await call(`/opcua/browse${(serverId) ? `?serverId=${serverId}` : ''}`, request, controller, user);
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
               reference: y,
               children: []
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
         const response = await call(`/opcua/browsenext${(serverId) ? `?serverId=${serverId}` : ''}`, request, controller, user);
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
   serverId?: string,
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

   const response = await call(`/opcua/read${(serverId) ? `?serverId=${serverId}` : ''}`, request, controller, user);
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

   const response = await call(`/opcua/createsession`, request, controller, user);
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

   const response = await call(`/opcua/activatesession`, request, controller, user);
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

   const response = await call(`/opcua/createsubscription`, request, controller, user);
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
   requestTimeout: number,
   controller?: AbortController
): Promise<ISession | null> {

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

   const response = await call(`/opcua/publish`, request, controller, user);
   if (!response) {
      return null;
   }
   if (response.code) {
      console.warn(`Publish call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return null;
   }

   const acknowledgements = session.acknowledgements ?? [];

   // console.warn(`SequenceNumber ${response.Body.NotificationMessage.SequenceNumber}`);

   const message = response.Body.NotificationMessage;
   if (message) {
      const notifications: Array<OpcUa.ExtensionObject> = message.NotificationData;
      if (notifications) {
         notifications.forEach(x => {
            if (x.TypeId === OpcUa.DataTypeIds.DataChangeNotification) {
               const dataChange = x.Body as OpcUa.DataChangeNotification;
               if (dataChange.MonitoredItems) {
                  dataChange.MonitoredItems.forEach(y => {
                     const item = session.monitoredItems?.find(z => z.clientHandle === y.ClientHandle);
                     if (item && y.Value) {
                        item.value = y.Value;
                     }
                  });
               }
            }
         });
      }

      acknowledgements.push({ SubscriptionId: message.SubscriptionId, SequenceNumber: message.SequenceNumber });
   }

   return {
      ...session,
      lastSequenceNumber: response.Body.NotificationMessage.SequenceNumber,
      acknowledgements: acknowledgements
   } as ISession;
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
            return {
               ItemToMonitor: {
                  NodeId: item.nodeId,
                  AttributeId: item.attributeId
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

   const response = await call(`/opcua/createmonitoreditems`, request, controller, user);
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
            let monitoredItems: IMonitoredItem[] | null = [
               {
                  path: ["Server", "ServerStatus", "State"],
                  name: "State",
                  nodeId: OpcUa.VariableIds.Server_ServerStatus_State,
                  attributeId: OpcUa.Attributes.Value,
                  clientHandle: HandleFactory.increment(),
                  serverHandle: 0,
                  value: { StatusCode: OpcUa.StatusCodes.BadWaitingForInitialData }
               },
               {
                  path: ["Server", "ServerStatus", "CurrentTime"],
                  name: "CurrentTime",
                  nodeId: OpcUa.VariableIds.Server_ServerStatus_CurrentTime,
                  attributeId: OpcUa.Attributes.Value,
                  clientHandle: HandleFactory.increment(),
                  serverHandle: 0,
                  value: { StatusCode: OpcUa.StatusCodes.BadWaitingForInitialData }
               }];
            monitoredItems = await createMonitoredItems(
               context.user,
               result,
               requestTimeout,
               monitoredItems,
               controller);
            return {
               ...session,
               state: SessionState.Open,
               monitoredItems: monitoredItems
            } as ISession;
         }
      }
   }
   return null;
}

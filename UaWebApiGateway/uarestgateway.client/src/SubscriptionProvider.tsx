import * as React from 'react';
import * as OpcUa from 'opcua-webapi';
import { SessionContext } from './SessionContext';
import { SubscriptionState } from './service/SubscriptionState';
import { HandleFactory } from './service/HandleFactory';
import { SessionState } from './service/SessionState';
import { IRequestMessage } from './service/IRequestMessage';
import { SubscriptionContext } from './SubscriptionContext';

export interface IMonitoredItem {
   nodeId: string,
   path?: string[],
   resolvedNodeId?: string
   attributeId?: number,
   samplingInterval?: number,
   subscriberHandle?: number,
   itemHandle?: number,
   monitoredItemId?: number,
   value?: OpcUa.DataValue,
   creationError?: OpcUa.StatusCode,
}

export interface ISubscriptionContext {
   publishingInterval?: number,
   setPublishingInterval: (interval: number) => void,
   samplingInterval?: number,
   setSamplingInterval: (interval: number) => void,
   isSubscriptionEnabled: boolean,
   setIsSubscriptionEnabled: (enabled: boolean) => void,
   subscriptionState: SubscriptionState,
   lastSequenceNumber: number,
   subscribe: (items: IMonitoredItem[], clientHandle: number) => void,
   unsubscribe: (items: IMonitoredItem[], clientHandle: number) => void,
   unsubscribeElement: (items: IMonitoredItem[], index:number, clientHandle: number) => void
}

interface SubscriptionProps {
   children?: React.ReactNode
}

interface InternalRequest {
   internalHandle: number,
   serviceId: string,
   items: IMonitoredItem[]
}

interface SubscriptionInternals {
   isEnabled: boolean,
   publishingInterval: number,
   samplingInterval: number,
   subscriptionState: SubscriptionState,
   subscriptionId?: number,
   monitoredItems: Map<number, IMonitoredItem>
   acknowledgements: OpcUa.SubscriptionAcknowledgement[]
   lastPublishTime?: Date,
   requests: InternalRequest[]
}

export const SubscriptionProvider = ({ children }: SubscriptionProps) => {
   const [isSubscriptionEnabled, setIsSubscriptionEnabled] = React.useState<boolean>(false);
   const [publishingInterval, setPublishingInterval] = React.useState<number>(1000);
   const [samplingInterval, setSamplingInterval] = React.useState<number>(1000);
   const [subscriptionState, setSubscriptionState] = React.useState<SubscriptionState>(SubscriptionState.Closed);
   const [lastSequenceNumber, setLastSequenceNumber] = React.useState<number>(0);
   const [publishCount, setPublishCount] = React.useState<number>(0);
   const [componentHandle] = React.useState<number>(HandleFactory.increment());

   const {
      sessionState,
      sendRequest,
      lastCompletedRequest
   } = React.useContext(SessionContext);

   const m = React.useRef<SubscriptionInternals>({
      isEnabled: false,
      publishingInterval: 5000,
      samplingInterval: 1000,
      subscriptionState: SubscriptionState.Closed,
      monitoredItems: new Map<number, IMonitoredItem>(),
      acknowledgements: [],
      requests: []
   });

   const createSubscription = React.useCallback(() => {
      const request: OpcUa.CreateSubscriptionRequest = {
         RequestedPublishingInterval: publishingInterval,
         RequestedLifetimeCount: 180,
         RequestedMaxKeepAliveCount: 3,
         MaxNotificationsPerPublish: 1000,
         PublishingEnabled: true, //toDo --> set on false and set on true if element is in DataView
         Priority: 100
      }
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.CreateSubscriptionRequest,
         Body: request
      };
      console.warn("createSubscription");
      sendRequest(message, componentHandle);
   }, [sendRequest, publishingInterval, componentHandle]);

   const deleteSubscription = React.useCallback((subscriptionId: number) => {
      const request: OpcUa.DeleteSubscriptionsRequest = {
         SubscriptionIds: [subscriptionId]
      }
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.DeleteSubscriptionsRequest,
         Body: request
      };
      console.warn("deleteSubscription");
      sendRequest(message, componentHandle);
   }, [sendRequest, componentHandle]);

   const publish = React.useCallback(() => {
      const request: OpcUa.PublishRequest = {
         SubscriptionAcknowledgements: m.current.acknowledgements,
      }
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.PublishRequest,
         Body: request
      };
      console.warn("publish");
      sendRequest(message, componentHandle);
   }, [sendRequest, componentHandle]);

   const translate = React.useCallback((items: IMonitoredItem[]) => {
      const browsePaths: OpcUa.BrowsePath[] = [];
      const itemsToTranslate: IMonitoredItem[] = [];
      items.forEach((item) => {
         if (item.resolvedNodeId || item.creationError) {
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
         itemsToTranslate.push(item);
      });
      if (itemsToTranslate.length) {
         const request: OpcUa.TranslateBrowsePathsToNodeIdsRequest = {
            BrowsePaths: browsePaths
         }
         const message: IRequestMessage = {
            ServiceId: OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsRequest,
            Body: request
         };
         const state = {
            internalHandle: HandleFactory.increment(),
            serviceId: message.ServiceId ?? '',
            items: itemsToTranslate
         };
         m.current.requests.push(state);
         console.warn("translate");
         sendRequest(message, state.internalHandle);
      }
   }, [sendRequest]);

   const createMonitoredItems = React.useCallback((items: IMonitoredItem[]) => {
      const createRequests: OpcUa.MonitoredItemCreateRequest[] = [];
      const itemsToMonitor: IMonitoredItem[] = [];
      items.forEach((item) => {
         if (item.resolvedNodeId && !item.monitoredItemId && !item.creationError) {
            createRequests.push({
               ItemToMonitor: {
                  NodeId: item.resolvedNodeId,
                  AttributeId: item.attributeId
               },
               MonitoringMode: OpcUa.MonitoringMode.Reporting,
               RequestedParameters: {
                  ClientHandle: item.itemHandle,
                  SamplingInterval: item.samplingInterval ?? m.current.samplingInterval,
                  QueueSize: 1,
                  DiscardOldest: true
               }
            });
            itemsToMonitor.push(item);
         }
      });
      if (createRequests.length) {
         const request: OpcUa.CreateMonitoredItemsRequest = {
            SubscriptionId: m.current.subscriptionId,
            TimestampsToReturn: OpcUa.TimestampsToReturn.Both,
            ItemsToCreate: createRequests
         }
         const message: IRequestMessage = {
            ServiceId: OpcUa.DataTypeIds.CreateMonitoredItemsRequest,
            Body: request
         };
         const state = {
            internalHandle: HandleFactory.increment(),
            serviceId: message.ServiceId ?? '',
            items: itemsToMonitor
         };
         m.current.requests.push(state);
         console.warn("createMonitoredItems");
         sendRequest(message, state.internalHandle);
      }
   }, [sendRequest]);

    const deleteMonitoredItem = React.useCallback((item: IMonitoredItem) => {
        const itemsToDelete: number[] = [];

        if (item.monitoredItemId) {
            itemsToDelete.push(item.monitoredItemId);
            item.monitoredItemId = undefined;
        }

        if (itemsToDelete.length) {
            const request: OpcUa.DeleteMonitoredItemsRequest = {
                SubscriptionId: m.current.subscriptionId,
                MonitoredItemIds: itemsToDelete
            }
            const message: IRequestMessage = {
                ServiceId: OpcUa.DataTypeIds.DeleteMonitoredItemsRequest,
                Body: request
            };
            const state = {
                internalHandle: HandleFactory.increment(),
                serviceId: message.ServiceId ?? '',
                items: []
            } as InternalRequest;
            m.current.requests.push(state);
            console.warn("deleteMonitoredItem");
            sendRequest(message, state.internalHandle);
        }
    }, [sendRequest]);

   const deleteMonitoredItems = React.useCallback((items: IMonitoredItem[]) => {
      const itemsToDelete: number[] = [];
      items.forEach((item) => {
         if (item.monitoredItemId) {
            itemsToDelete.push(item.monitoredItemId);
            item.monitoredItemId = undefined;
         }
      });
      if (itemsToDelete.length) {
         const request: OpcUa.DeleteMonitoredItemsRequest = {
            SubscriptionId: m.current.subscriptionId,
            MonitoredItemIds: itemsToDelete
         }
         const message: IRequestMessage = {
            ServiceId: OpcUa.DataTypeIds.DeleteMonitoredItemsRequest,
            Body: request
         };
         const state = {
            internalHandle: HandleFactory.increment(),
            serviceId: message.ServiceId ?? '',
            items: []
         } as InternalRequest;
         m.current.requests.push(state);
         console.warn("deleteMonitoredItems");
         sendRequest(message, state.internalHandle);
      }
   }, [sendRequest]);

   React.useEffect(() => {
      if (lastCompletedRequest?.callerHandle === componentHandle) {
         if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.CreateSubscriptionResponse) {
            const csrm = lastCompletedRequest?.response?.Body as OpcUa.CreateSubscriptionResponse;
            if (csrm?.ResponseHeader && !csrm?.ResponseHeader.ServiceResult) {
               m.current.subscriptionId = csrm?.SubscriptionId;
               m.current.subscriptionState = SubscriptionState.Open;
               setSubscriptionState(m.current.subscriptionState);
               console.warn("CreateSubscriptionResponse");
               publish();
            } 
         }
         else if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.DeleteSubscriptionsResponse) {
            m.current.subscriptionId = undefined;
            m.current.acknowledgements = [];
            m.current.subscriptionState = SubscriptionState.Closed;
            setSubscriptionState(m.current.subscriptionState);
            setLastSequenceNumber(0);
         }
         else if (lastCompletedRequest?.response?.ServiceId === OpcUa.DataTypeIds.PublishResponse) {
            const prm = lastCompletedRequest.response?.Body as OpcUa.PublishResponse;
            setLastSequenceNumber(() => {
               prm.AvailableSequenceNumbers?.forEach((ii) => {
                  m.current.acknowledgements.push({
                     SubscriptionId: m.current.subscriptionId,
                     SequenceNumber: ii
                  });
               });
               if (prm.NotificationMessage?.NotificationData) {
                  prm.NotificationMessage?.NotificationData.forEach((eo) => {
                     if (eo.UaTypeId === OpcUa.DataTypeIds.DataChangeNotification) {
                        const dcn = eo as OpcUa.DataChangeNotification;
                        dcn.MonitoredItems?.forEach((ii) => {
                           const item = m.current.monitoredItems.get(ii.ClientHandle ?? 0);
                           if (item) {
                              item.value = ii.Value;
                           }
                        });
                     }
                  });
               }
               console.warn("setPublishCount");
               setPublishCount(count => count + 1);
               return prm.NotificationMessage?.SequenceNumber ?? 1;
            });
         }
      }
      const request = m.current.requests.find((r) => r.internalHandle === lastCompletedRequest?.callerHandle);
      if (request) {
         if (lastCompletedRequest?.request?.ServiceId === OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsRequest) {
            const response = lastCompletedRequest.response?.Body as OpcUa.TranslateBrowsePathsToNodeIdsResponse;
            if (response.ResponseHeader?.ServiceResult?.Code) {
               request?.items?.forEach((item) => {
                  item.creationError = response.ResponseHeader?.ServiceResult;
               });
            }
            else {
               response?.Results?.forEach((result, index) => {
                  request.items[index].creationError = result?.StatusCode;
                  if (!result?.StatusCode) {
                     request.items[index].resolvedNodeId = result?.Targets?.at(0)?.TargetId;
                  }
               });
               createMonitoredItems(request.items);
            }
         }
         if (lastCompletedRequest?.request?.ServiceId === OpcUa.DataTypeIds.CreateMonitoredItemsRequest) {
            const response = lastCompletedRequest.response?.Body as OpcUa.CreateMonitoredItemsResponse;
            if (response.ResponseHeader?.ServiceResult?.Code) {
               request?.items?.forEach((item) => {
                  item.creationError = response.ResponseHeader?.ServiceResult;
               });
            }
            else {
               response?.Results?.forEach((result, index) => {
                  request.items[index].creationError = result?.StatusCode;
                  if (!result?.StatusCode) {
                     request.items[index].monitoredItemId = result?.MonitoredItemId;
                  }
               });
            }
         }
      }
   }, [lastCompletedRequest, publish, deleteSubscription, createMonitoredItems, componentHandle]);

   React.useEffect(() => {
      if (publishCount !== 0) {
         if (sessionState === SessionState.SessionActive) {
            if (m.current.isEnabled && m.current.subscriptionState === SubscriptionState.Open) {
               console.warn("Publish " + publishCount);
               publish();
            }
         }
      }
   }, [publishCount, sessionState, publish]);

   React.useEffect(() => {
      switch (sessionState) {
         case SessionState.SessionActive:
            if (m.current.isEnabled && m.current.subscriptionState === SubscriptionState.Closed) {
               createSubscription();
            }
            break;
         default:
            m.current.subscriptionId = undefined;
            m.current.acknowledgements = [];
            m.current.subscriptionState = SubscriptionState.Closed;
            setSubscriptionState(m.current.subscriptionState);
            setLastSequenceNumber(0);
            break;
      }
   }, [sessionState, createSubscription]);

   const subscribe = React.useCallback((items: IMonitoredItem[]) => {
      items.forEach((item) => {
         item.itemHandle = HandleFactory.increment();
         item.subscriberHandle = item.subscriberHandle || item.itemHandle;
         item.attributeId = item.attributeId || OpcUa.Attributes.Value;
         m.current.monitoredItems.set(item.itemHandle, item);
      });
      translate(items);
      createMonitoredItems(items);
   }, [translate, createMonitoredItems]);

    const unsubscribeElement = React.useCallback((items: IMonitoredItem[], index: number) => {
        if (items[index].itemHandle) {
            m.current.monitoredItems.delete(items[index].itemHandle);
        }

        deleteMonitoredItem(items[index]);
    }, [deleteMonitoredItems]);

   const unsubscribe = React.useCallback((items: IMonitoredItem[]) => {
      items.forEach((item) => {
         if (item.itemHandle) {
            m.current.monitoredItems.delete(item.itemHandle);
         }
      });
      deleteMonitoredItems(items);
   }, [deleteMonitoredItems]);

   const setIsSubscriptionEnabledImpl = React.useCallback((value: boolean) => {
      if (m.current.isEnabled === value) {
         return;
      }
      m.current.isEnabled = value;
      setIsSubscriptionEnabled(m.current.isEnabled);
      if (value && sessionState === SessionState.SessionActive && m.current.subscriptionState === SubscriptionState.Closed) {
         createSubscription();
         return;
      }
      if (!value && m.current.subscriptionId && m.current.subscriptionState === SubscriptionState.Open) {
         deleteSubscription(m.current.subscriptionId);
         return;
      }
   }, [createSubscription, deleteSubscription, sessionState]);

   const setPublishingIntervalImpl = React.useCallback((value: number) => {
      m.current.publishingInterval = value;
      setPublishingInterval(m.current.publishingInterval);
   }, []);

   const setSamplingIntervalImpl = React.useCallback((value: number) => {
      m.current.samplingInterval = value;
      setSamplingInterval(m.current.samplingInterval);
   }, []);
   
   const subscriptionContext = {
      isSubscriptionEnabled,
      setIsSubscriptionEnabled: setIsSubscriptionEnabledImpl,
      publishingInterval,
      setPublishingInterval: setPublishingIntervalImpl,
      samplingInterval,
      setSamplingInterval: setSamplingIntervalImpl,
      subscriptionState,
      lastSequenceNumber,
      subscribe: subscribe,
      unsubscribe: unsubscribe,
      unsubscribeElement: unsubscribeElement
   } as ISubscriptionContext;

   return (
      <SubscriptionContext.Provider value={subscriptionContext}>
         {children}
      </SubscriptionContext.Provider>
   );
};

export default SubscriptionProvider;
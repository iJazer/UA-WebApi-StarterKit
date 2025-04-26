import React from 'react';
import { BrowseContext } from '../BrowseContext';
import { HandleFactory } from '../service/HandleFactory';
import { IReadValueId } from '../service/IReadValueId';
import { SubscriptionState } from '../service/SubscriptionState';
import { SubscriptionContext } from '../SubscriptionContext';
import { IMonitoredItem } from '../SubscriptionProvider';

interface TypeDefinitionCardProps {
   children?: React.ReactNode;
   variables: IMonitoredItem[];
   readTrigger?: number;
   onValueUpdate: () => void;
}

interface TypeDefinitionCardInternals {
   internalHandle: number;
   requests: number[];
   monitoredItems: IMonitoredItem[];
   mounted: boolean;
   cleanedUp: boolean;
}

export const TypeDefinitionCard: React.FC<TypeDefinitionCardProps> = ({ children, variables, readTrigger, onValueUpdate }: TypeDefinitionCardProps) => {
   const [counter,setCounter] = React.useState<number>(1);

   const m = React.useRef<TypeDefinitionCardInternals>({
      internalHandle: HandleFactory.increment() + 30000,
      monitoredItems: [],
      requests: [],
      mounted: true,
      cleanedUp: false
   });

   // Set flags to trigger clean up when component is unmounted
   React.useEffect(() => {
      const state = m.current;
      return () => {
         state.mounted = false;
         state.cleanedUp = false;
      };
   }, []);

   // The hook to access active subscription.
   const {
      subscriptionState,
      subscribe,
      unsubscribe,
      lastSequenceNumber
   } = React.useContext(SubscriptionContext);

   const {
      readValues,
      responseCount,
      processResults
   } = React.useContext(BrowseContext);

   // Unsubscribe when component is unmounted
   React.useEffect(() => {
      const state = m.current;
      return () => {
         if (!state.mounted && !state.cleanedUp && state.monitoredItems.length) {
            unsubscribe(state.monitoredItems, state.internalHandle);
            state.cleanedUp = true;
         }
      };
   }, [unsubscribe]);

   // Handle state changes to the subscription.
   React.useEffect(() => {
      if (subscriptionState === SubscriptionState.Open) {
         const itemsToCreate = m.current.monitoredItems.filter(item => !item.itemHandle);
         if (itemsToCreate.length) {
            subscribe(itemsToCreate, m.current.internalHandle);
         }
      }
      else {
         unsubscribe(m.current.monitoredItems, m.current.internalHandle);
         m.current.monitoredItems = [];
      }
   }, [subscriptionState, subscribe, unsubscribe]);

   // Update the monitored items when the monitoredItems prop changes.
   React.useEffect(() => {
      const items: IMonitoredItem[] = m.current.monitoredItems;
      unsubscribe(items, m.current.internalHandle);
      m.current.monitoredItems = variables;
      if (subscriptionState === SubscriptionState.Open) {
         subscribe(m.current.monitoredItems, m.current.internalHandle);
      }
   }, [variables, subscriptionState, unsubscribe, subscribe]);

   // Trigger render when a publish response is received.
   React.useEffect(() => {
      setCounter(counter => counter + 1);
   }, [lastSequenceNumber]);

   // Trigger render when a update is received.
   React.useEffect(() => {
      onValueUpdate();
   }, [counter, onValueUpdate]);

   React.useEffect(() => {
      if (readValues && variables.length) {
         const nodesToRead: IReadValueId[] = [];
         variables.forEach((x) => {
            if (x.nodeId) {
               nodesToRead.push({
                  id: x.subscriberHandle ?? 0,
                  nodeId: x.nodeId,
                  path: x.path,
                  attributeId: x.attributeId
               });
            }
         });
         m.current.internalHandle = HandleFactory.increment() + 20000;
         m.current.requests.push(m.current.internalHandle);
         console.error("ValueList ADD (" + m.current.requests.join(",") + "): " + (m.current.internalHandle ?? 0));
         readValues(m.current.internalHandle, nodesToRead);
      }
   }, [readValues, variables, readTrigger]);

   React.useEffect(() => {
      const results = processResults((result) => {
         return m.current.requests.find(x => x === result.callerHandle) ? true : false;
      });
      if (results) {
         results?.forEach(result => {
            m.current.requests = m.current.requests.filter(x => x !== result.callerHandle);
            if (result.values) {
               result.values.forEach((x) => {
                  const mi = variables.find(y => y.subscriberHandle === x.id)
                  if (mi) {
                     mi.value = x.value;
                  }
               });
               // trigger render after updating the values.
               setCounter(counter => counter + 1);
            }
         });
      }
   }, [responseCount, processResults, variables]);

   return (<React.Fragment>{children}</React.Fragment>);
};

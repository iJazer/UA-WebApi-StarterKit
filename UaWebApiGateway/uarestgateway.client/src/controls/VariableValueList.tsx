import React from 'react';

import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Skeleton, Typography } from '@mui/material';

import * as OpcUa from 'opcua-webapi';

import DataValueDisplay from './DataValueDisplay';
import { IMonitoredItem } from '../SubscriptionProvider';
import { SubscriptionContext } from '../SubscriptionContext';
import { HandleFactory } from '../service/HandleFactory';
import { SubscriptionState } from '../service/SubscriptionState';
import { IReadValueId } from '../service/IReadValueId';

import { BrowseContext } from '../BrowseContext';

interface VariableValueListInternals {
   internalHandle: number,
   monitoredItems: IMonitoredItem[],
   requests: number[],
   mounted: boolean,
   cleanedUp: boolean
}

interface VariableValueListProps {
   rootId?: string
}

interface Row {
   name: string,
   item: IMonitoredItem
}

export const VariableValueList = ({ rootId }: VariableValueListProps) => {
   const [variables, setVariables] = React.useState<Row[]>([]);
   const [counter, setCounter] = React.useState<number>(1);

   const m = React.useRef<VariableValueListInternals>({
      internalHandle: HandleFactory.increment() + 20000,
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
      browseChildren,
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

   // Browse the root node when it changes and subscribe to the variables
   React.useEffect(() => {
      async function changeRoot() {
         const items: IMonitoredItem[] = m.current.monitoredItems;
         unsubscribe(items, m.current.internalHandle);
         m.current.monitoredItems = [];
         setVariables([]);
      }
      async function browse(nodeId: string) {
         await changeRoot();
         if (nodeId) {
            m.current.internalHandle = HandleFactory.increment() + 20000
            m.current.requests.push(m.current.internalHandle);
            browseChildren(m.current.internalHandle, nodeId);
         }
      }
      if (rootId) {
         browse(rootId);
      }
   }, [rootId, browseChildren, subscriptionState, unsubscribe, subscribe]);

   // Trigger render when a publish response is received.
   React.useEffect(() => {
      setCounter(counter => counter + 1);
   }, [lastSequenceNumber]);

   React.useEffect(() => {
      if (readValues && variables.length) {
         const nodesToRead: IReadValueId[] = [];
         variables.forEach((x) => {
            if (x.item.nodeId) {
               nodesToRead.push({
                  id: x.item.subscriberHandle ?? 0,
                  nodeId: x.item.nodeId,
                  path: x.item.path,
                  attributeId: x.item.attributeId
               });
            }
         });
         m.current.internalHandle = HandleFactory.increment() + 20000;
         m.current.requests.push(m.current.internalHandle);
         console.error("ValueList ADD (" + m.current.requests.join(",") + "): " + (m.current.internalHandle ?? 0));
         readValues(m.current.internalHandle, nodesToRead);
      }
   }, [readValues, variables]);

   React.useEffect(() => {
      const results = processResults((result) => {
         return m.current.requests.find(x => x === result.callerHandle) ? true : false;
      });
      if (results) {
         results?.forEach(result => {
            m.current.requests = m.current.requests.filter(x => x !== result.callerHandle);
            if (result.children) {
               const items: IMonitoredItem[] = [];
               const newVariables: Row[] = [];
               result.children.forEach((x) => {
                  if (x?.reference?.NodeClass === OpcUa.NodeClass.Variable && x?.reference?.NodeId) {
                     items.push({
                        nodeId: x.reference.NodeId,
                        subscriberHandle: HandleFactory.increment() + 20000
                     });
                     if (x?.reference?.DisplayName?.Text) {
                        newVariables.push({ name: x?.reference?.DisplayName?.Text, item: items[items.length - 1] });
                     }
                  }
               });
               m.current.monitoredItems = items;
               if (subscriptionState === SubscriptionState.Open) {
                  subscribe(items, m.current.internalHandle);
               }
               setVariables(newVariables);
            }
            else if (result.values) {
               result.values.forEach((x) => {
                  const mi = variables.find(y => y.item.subscriberHandle === x.id)
                  if (mi) {
                     mi.item.value = x.value;
                  }
               });
               // trigger render after updating the values.
               setCounter(counter => counter + 1);
            }
         });
      }
   }, [responseCount, processResults, variables, subscribe, subscriptionState]);
      
   if (!variables?.length || counter === 0) {
      return (
         <Paper elevation={3} sx={{ height: '100%', width: 'auto', p: '4px' }} >
            <Skeleton variant='rounded' height={300}></Skeleton>
         </Paper>
      );
   }

   return (
      <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '100%' }}>
         <Table>
            <TableHead>
               <TableRow>
                  <TableCell><Typography variant='h6'>Name</Typography></TableCell>
                  <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
               </TableRow>
            </TableHead>
            <TableBody>
               {variables?.map((variable) => {
                  if (!variable) return null;
                  return (
                     <TableRow
                        key={variable.name}
                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                     >
                        <TableCell sx={{ width: 'auto' }}>
                           <Typography variant='body1' sx={{ minWidth: '300px' }}>{variable.name}</Typography>
                        </TableCell>
                        <TableCell>
                           <DataValueDisplay value={variable.item.value} />
                        </TableCell>
                     </TableRow>
                  );
               })}
            </TableBody>
         </Table>
      </TableContainer>
   );
}

export default VariableValueList;
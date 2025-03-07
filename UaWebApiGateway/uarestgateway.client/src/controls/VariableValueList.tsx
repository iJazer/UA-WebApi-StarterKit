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
//import { ApplicationContext } from '../ApplicationProvider';
import DataValueDisplay from './DataValueDisplay';
import { IMonitoredItem, SubscriptionContext } from '../SubscriptionProvider';
import { HandleFactory } from '../service/HandleFactory';
import { SubscriptionState } from '../service/SubscriptionState';
import { IReadValueId } from '../service/IReadValueId';
//import { translateAndReadValues } from '../opcua-utils';
import { UserContext } from '../UserProvider';

import { SessionContext } from '../SessionProvider';

interface VariableValueListInternals {
   clientHandle: number,
   monitoredItems: IMonitoredItem[],
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
   //const { browseChildren } = React.useContext(ApplicationContext);
   const { browseChildren } = React.useContext(SessionContext);
   const { translateAndReadValues } = React.useContext(SessionContext);
   const { user } = React.useContext(UserContext);
   const [variables, setVariables] = React.useState<Row[]>([]);
   const [counter, setCounter] = React.useState<number>(1);

   const m = React.useRef<VariableValueListInternals>({
      clientHandle: HandleFactory.increment(),
      monitoredItems: [],
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

   // Unsubscribe when component is unmounted
   React.useEffect(() => {
      const state = m.current;
      return () => {
         if (!state.mounted && !state.cleanedUp && state.monitoredItems.length) {
            unsubscribe(state.monitoredItems, state.clientHandle);
            state.cleanedUp = true;
         }
      };
   }, [unsubscribe]);

   // Handle state changes to the subscription.
   React.useEffect(() => {
      if (subscriptionState === SubscriptionState.Open) {
         const itemsToCreate = m.current.monitoredItems.filter(item => !item.itemHandle);
         if (itemsToCreate.length) {
            subscribe(itemsToCreate, m.current.clientHandle);
         }
      }
      else {
         unsubscribe(m.current.monitoredItems, m.current.clientHandle);
         m.current.monitoredItems = [];
      }
   }, [subscriptionState, subscribe, unsubscribe]);

   // Browse the root node when it changes and subscribe to the variables
   React.useEffect(() => {
      async function changeRoot() {
         const items: IMonitoredItem[] = m.current.monitoredItems;
         unsubscribe(items, m.current.clientHandle);
         m.current.monitoredItems = [];
      }
      async function browse(nodeId: string) {
         await changeRoot();
         const items: IMonitoredItem[] = [];
         const variables : Row[] = [];
         const children = await browseChildren(nodeId, 0);
         children.forEach((x) => {
            if (x?.reference?.NodeClass === OpcUa.NodeClass.Variable && x?.reference?.NodeId) {
               items.push({
                  nodeId: x.reference.NodeId,
                  subscriberHandle: HandleFactory.increment()
               });
               if (x?.reference?.DisplayName?.Text) {
                  variables.push({ name: x?.reference?.DisplayName?.Text, item: items[items.length - 1] });
               }
            }
         });
         m.current.monitoredItems = items;
         if (subscriptionState === SubscriptionState.Open) {
            subscribe(items, m.current.clientHandle);
         }
         setVariables(variables);
      }
      if (rootId) {
         browse(rootId);
      }
   }, [rootId, browseChildren, subscriptionState, unsubscribe, subscribe]);

   // Trigger render when a publish response is received.
   React.useEffect(() => {
      setCounter(counter => counter + 1);
   }, [lastSequenceNumber]);

   // Read the initial value. 
   React.useEffect(() => {
      async function read(variables: Row[]) {
         const nodesToRead: IReadValueId[] = variables.map(x => {
            return {
               id: x.item.subscriberHandle,
               nodeId: x.item.nodeId,
               resolvedNodeId: x.item.nodeId,
               attributeId: OpcUa.Attributes.Value
            } as IReadValueId;
         });
         //const results = await translateAndReadValues(nodesToRead, 60000, user);
         const results = await translateAndReadValues(nodesToRead, 60000);
         if (results) {
            results.forEach((result) => {
               const mi = variables.find(x => x.item.subscriberHandle === result.id)
               if (mi) {
                  mi.item.value = result.value;
               }
            });
            // Trigger render after updating the values.
            setCounter(counter => counter + 1);
         }
      }
      if (variables) {
         read(variables);
      }
   }, [variables, user]);
   
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
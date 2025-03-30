import React from 'react';
import { IMonitoredItem } from '../SubscriptionProvider';
import { SubscriptionContext } from '../SubscriptionContext';
import Card from '@mui/material/Card/Card';
import CardContent from '@mui/material/CardContent/CardContent';
import Table from '@mui/material/Table/Table';
import TableBody from '@mui/material/TableBody/TableBody';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import Typography from '@mui/material/Typography/Typography';
import * as OpcUa from 'opcua-webapi';
import DataValueDisplay from './DataValueDisplay';
import { HandleFactory } from '../service/HandleFactory';
import { SubscriptionState } from '../service/SubscriptionState';
//import { translateAndReadValues } from '../opcua-utils';
import { IReadValueId } from '../service/IReadValueId';
import { UserContext } from '../UserProvider';

import { SessionContext } from '../SessionContext';

interface TypeDefinitionCardProps {
   children?: React.ReactNode,
   variables: IMonitoredItem[],
   onValueUpdate: () => void
}

interface TypeDefinitionCardInternals {
   clientHandle: number,
   monitoredItems: IMonitoredItem[],
   mounted: boolean,
   cleanedUp: boolean
}

const TypeDefinitionCard: React.FC<TypeDefinitionCardProps> = ({ children, variables, onValueUpdate }: TypeDefinitionCardProps) => {
   const { user } = React.useContext(UserContext);

   const m = React.useRef<TypeDefinitionCardInternals>({
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

   // Update the monitored items when the monitoredItems prop changes.
   React.useEffect(() => {
      const items: IMonitoredItem[] = m.current.monitoredItems;
      unsubscribe(items, m.current.clientHandle);
      m.current.monitoredItems = variables;
      if (subscriptionState === SubscriptionState.Open) {
         subscribe(m.current.monitoredItems, m.current.clientHandle);
      }
   }, [variables, subscriptionState, unsubscribe, subscribe]);

   // Trigger render when a publish response is received.
   React.useEffect(() => {
      onValueUpdate();
   }, [lastSequenceNumber, onValueUpdate]);

   // Read the initial value. 
   //React.useEffect(() => {
   //   async function read(variables: IMonitoredItem[]) {
   //      const nodesToRead: IReadValueId[] = variables.map(x => {
   //         return {
   //            id: x.subscriberHandle,
   //            nodeId: x.nodeId,
   //            path: x.path,
   //            attributeId: OpcUa.Attributes.Value
   //         } as IReadValueId;
   //      });
   //       //const results = await translateAndReadValues(nodesToRead, 60000, user);
   //      const results = await translateAndReadValues(nodesToRead, 60000);
   //      if (results) {
   //         results.forEach((result) => {
   //            const mi = variables.find(x => x.subscriberHandle === result.id)
   //            if (mi) {
   //               mi.value = result.value;
   //            }
   //         });
   //         // Trigger render after updating the monitored items.
   //         onValueUpdate();
   //      }
   //   }
   //   if (variables) {
   //      read(variables);
   //   }
   //}, [variables, user, onValueUpdate]);

   return (<React.Fragment>{children}</React.Fragment>);
}

interface ServerStatusCardProps {
   rootId?: string
}

enum ServerStatusField {
   Undefined,
   ProductName,
   ProductUri,
   ManufacturerName,
   SoftwareVersion,
   BuildDate,
   State,
   StartTime,
   CurrentTime,
   StateEnumStrings
}

export const ServerStatusCard = ({ rootId }: ServerStatusCardProps) => {
   const [variables, setVariables] = React.useState<IMonitoredItem[]>([]);
   const [counter, setCounter] = React.useState<number>(1);
   const { translateAndReadValues } = React.useContext(SessionContext);

   React.useEffect(() => {
      const items = [];
      if (rootId) {
         items.push({
            subscriberHandle: ServerStatusField.ProductName,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.BuildInfo, OpcUa.BrowseNames.ProductName]
         });
         items.push({
            subscriberHandle: ServerStatusField.ProductUri,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.BuildInfo, OpcUa.BrowseNames.ProductUri]
         });
         items.push({
            subscriberHandle: ServerStatusField.ManufacturerName,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.BuildInfo, OpcUa.BrowseNames.ManufacturerName]
         });
         items.push({
            subscriberHandle: ServerStatusField.SoftwareVersion,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.BuildInfo, OpcUa.BrowseNames.SoftwareVersion]
         });
         items.push({
            subscriberHandle: ServerStatusField.BuildDate,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.BuildInfo, OpcUa.BrowseNames.BuildDate]
         });
         items.push({
            subscriberHandle: ServerStatusField.State,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.State]
         });
         items.push({
            subscriberHandle: ServerStatusField.StartTime,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.StartTime]
         });
         items.push({
            subscriberHandle: ServerStatusField.CurrentTime,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.CurrentTime]
         });
         items.push({
            subscriberHandle: ServerStatusField.StateEnumStrings,
            nodeId: OpcUa.VariableIds.ServerState_EnumStrings
         });
      }
      setVariables(items);
   }, [rootId]);

   const getValue = React.useCallback((field: ServerStatusField) => {
      return variables.find(x => x.subscriberHandle === field)?.value;
   }, [variables]);

   const valueUpdated = React.useCallback(() => {
      setCounter(counter => counter + 1);
   },[]);

   return (
      <TypeDefinitionCard variables={variables} onValueUpdate={valueUpdated} >
         <Card sx={{ m: 4, mr: 20 }}>
            <CardContent sx={{ p: 0 }}>
               <Table sx={{ m: 0, p: 0 }}>
                  <TableBody>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Product Name</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.ProductName)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Product URI</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.ProductUri)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Manufacturer Name</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.ManufacturerName)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>SoftwareVersion</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.SoftwareVersion)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Build Date</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.BuildDate)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Server State</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.State)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Start Time</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.StartTime)} /></TableCell>
                     </TableRow>
                     <TableRow>
                        <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Server Time</Typography></TableCell>
                        <TableCell><DataValueDisplay value={getValue(ServerStatusField.CurrentTime)} /></TableCell>
                     </TableRow>
                  </TableBody>
               </Table>
            </CardContent>
         </Card>
      </TypeDefinitionCard>
   );
}

export default ServerStatusCard;
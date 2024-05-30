import React from 'react';
import { IMonitoredItem, SubscriptionContext } from '../SubscriptionProvider';
import { HandleFactory, SubscriptionState } from '../opcua-utils';
import Card from '@mui/material/Card/Card';
import CardContent from '@mui/material/CardContent/CardContent';
import Table from '@mui/material/Table/Table';
import TableBody from '@mui/material/TableBody/TableBody';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import Typography from '@mui/material/Typography/Typography';
import * as OpcUa from '../opcua';
import DataValueDisplay from './DataValueDisplay';

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

interface ServerStatusCardInternals {
   clientHandle: number,
   monitoredItems: IMonitoredItem[],
   mounted: boolean,
   disposed: boolean
}

export const ServerStatusCard = ({ rootId }: ServerStatusCardProps) => {
   const m = React.useRef<ServerStatusCardInternals>({
      clientHandle: HandleFactory.increment(),
      monitoredItems: [],
      mounted: true,
      disposed: false
   });

   React.useEffect(() => {
      const state = m.current;
      return () => {
         state.mounted = false;
         state.disposed = false;
      };
   }, []);

   const {
      subscriptionState,
      subscribe,
      unsubscribe
   } = React.useContext(SubscriptionContext);

   React.useEffect(() => {
      const state = m.current;
      return () => {
         if (!state.mounted && !state.disposed && state.monitoredItems.length) {
            unsubscribe(state.monitoredItems, state.clientHandle);
            state.disposed = true;
         }
      };
   }, [unsubscribe]);

   React.useEffect(() => {
      if (m.current.monitoredItems.length) {
         unsubscribe(m.current.monitoredItems, m.current.clientHandle);
         m.current.monitoredItems = [];
      }
      if (rootId) {
         const items = [];
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
         m.current.monitoredItems = items;
      }
   }, [rootId, unsubscribe]);

   React.useEffect(() => {
      if (subscriptionState === SubscriptionState.Open) {
         const itemsToCreate = m.current.monitoredItems.filter(item => !item.itemHandle);
         if (itemsToCreate.length) {
            subscribe(itemsToCreate, m.current.clientHandle);
         }
      }
      else {
         m.current.monitoredItems.forEach(item => {
            item.monitoredItemId = undefined,
               item.creationError = undefined,
               item.value = undefined
         });
      }
   }, [subscriptionState, subscribe]);

   const getValue = React.useCallback((field: ServerStatusField) => {
      return m.current.monitoredItems.find(x => x.subscriberHandle === field)?.value;
   }, []);

   if (!rootId) {
      return null;
   }

   return (
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
   );
}

export default ServerStatusCard;
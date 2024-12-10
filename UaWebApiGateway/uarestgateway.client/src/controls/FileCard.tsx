import React from 'react';
import { IMonitoredItem, SubscriptionContext } from '../SubscriptionProvider';
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
import { translateAndReadValues } from '../opcua-utils';
import { IReadValueId } from '../service/IReadValueId';
import { UserContext } from '../UserProvider';

import DownloadIcon from '@mui/icons-material/Download';
import UploadIcon from '@mui/icons-material/Upload';

interface FileCardProps {
   rootId?: string
}

enum FileCardField {
   Size,
   OpenCount,
   MimeType,
   MaxByteStringLength,
   LastModifiedTime
}

interface FileCardInternals {
   clientHandle: number,
   monitoredItems: IMonitoredItem[],
   mounted: boolean,
   disposed: boolean
}

export const FileCard = ({ rootId }: FileCardProps) => {
   const { user } = React.useContext(UserContext);
   const [counter, setCounter] = React.useState<number>(1);

   const m = React.useRef<FileCardInternals>({
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
            subscriberHandle: FileCardField.Size,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.Size]
         });
         items.push({
            subscriberHandle: FileCardField.OpenCount,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.OpenCount]
         });
         items.push({
            subscriberHandle: FileCardField.MimeType,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.MimeType]
         });
         items.push({
            subscriberHandle: FileCardField.MaxByteStringLength,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.MaxByteStringLength]
         });
         items.push({
            subscriberHandle: FileCardField.LastModifiedTime,
            nodeId: rootId,
            path: [OpcUa.BrowseNames.LastModifiedTime]
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
         const nodesToRead: IReadValueId[] = m.current.monitoredItems.map(x => {
            return {
               nodeId: x.nodeId,
               path: x.path,
               resolvedNodeId: x.resolvedNodeId,
               attributeId: OpcUa.Attributes.Value
            } as IReadValueId;
         });
         translateAndReadValues(nodesToRead, 60000, user).then((results) => {
            if (results) {
               results.forEach((result, index) => {
                  const item = m.current.monitoredItems.at(index)
                  if (item) {
                     item.value = result.value;
                  }
               });
               setCounter(x => x + 1);
            }
         });
      }
   }, [subscriptionState, subscribe, user]);

   const getValue = React.useCallback((field: FileCardField) => {
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
                     <TableCell colSpan={2}><DownloadIcon /><UploadIcon /></TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Media Type</Typography></TableCell>
                     <TableCell><DataValueDisplay value={getValue(FileCardField.MimeType)} /></TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Size (bytes)</Typography></TableCell>
                     <TableCell><DataValueDisplay value={getValue(FileCardField.Size)} /></TableCell>
                  </TableRow>
                  <TableRow>
                     <TableCell><Typography variant="body1" sx={{ fontWeight: 'bolder' }}>Last Modified Time </Typography></TableCell>
                     <TableCell><DataValueDisplay value={getValue(FileCardField.LastModifiedTime)} /></TableCell>
                  </TableRow>
               </TableBody>
            </Table>
         </CardContent>
      </Card>
   );
}

export default FileCard;
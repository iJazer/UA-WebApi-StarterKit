import React from 'react';
import { IMonitoredItem } from '../SubscriptionProvider';
import Card from '@mui/material/Card/Card';
import CardContent from '@mui/material/CardContent/CardContent';
import Table from '@mui/material/Table/Table';
import TableBody from '@mui/material/TableBody/TableBody';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import Typography from '@mui/material/Typography/Typography';
import * as OpcUa from 'opcua-webapi';
import DataValueDisplay from './DataValueDisplay';
import { TypeDefinitionCard } from './TypeDefinitionCard';

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
   const [, setCounter] = React.useState<number>(1);

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
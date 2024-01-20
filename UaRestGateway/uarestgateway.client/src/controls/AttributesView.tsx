import React from 'react';

import { call, HandleFactory } from '../opcua-utils';
import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Skeleton, Typography, useTheme } from '@mui/material';

import * as OpcUa from '../opcua';
import * as Web from '../Web';
import { ApplicationContext } from '../ApplicationProvider';
import { Account } from '../api';

interface NodeAttributeValue {
   id: number,
   reference: OpcUa.ReferenceDescription;
   attributeId: OpcUa.Attributes;
   value?: OpcUa.DataValue;
}

async function readAttributes(
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
interface AttributesViewProps {
   reference?: OpcUa.ReferenceDescription
   serverId?: string
   requestTimeout?: number
}

export const AttributesView = ({ reference, serverId, requestTimeout }: AttributesViewProps) => {
   const [values, setValues] = React.useState<NodeAttributeValue[]>([]);
   const name = reference?.DisplayName?.Text ?? reference?.BrowseName ?? reference?.NodeId;
   const theme = useTheme();
   const context = React.useContext(ApplicationContext);

   React.useEffect(() => {
      const controller = new AbortController();
      if (reference?.NodeId) {
         readAttributes(reference, serverId, requestTimeout, controller, context?.userContext?.user).then((x) => setValues(x ?? []));
      }
      return () => {
         controller.abort();
      }
   }, [serverId, reference, reference?.NodeId, requestTimeout, context?.userContext?.user]);

   if (!reference) {
      return (
         <Paper elevation={3} sx={{ height: '100%', width: 'auto' }} >
            <Skeleton variant='text' sx={{ fontSize: '1.5rem', mx: '4px' }} ></Skeleton>
            <Skeleton variant='text' sx={{ fontSize: '1.5rem', mx: '4px' }} ></Skeleton>
            <Skeleton variant='text' sx={{ fontSize: '1.5rem', mx: '4px' }} ></Skeleton>
            <Skeleton variant='text' sx={{ fontSize: '1.5rem', mx: '4px' }} ></Skeleton>
            <Skeleton variant='text' sx={{ fontSize: '1.5rem', mx: '4px' }} ></Skeleton>
            <Skeleton variant='text' sx={{ fontSize: '1.5rem', mx: '4px' }} ></Skeleton>
         </Paper>
      );
   }

   return (
      <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '100%' }}>
         <Table>
            <TableHead>
               <TableRow>
                  <TableCell colSpan={4} sx={{ width: '100%', backgroundColor: theme.palette.primary.light }}>
                     <Typography variant='h6'>{(reference) ? `${name} [${reference?.NodeId}]` : "   "}</Typography>
                  </TableCell>
               </TableRow>
               <TableRow>
                  <TableCell><Typography variant='h6'>Attribute</Typography></TableCell>
                  <TableCell><Typography variant='h6'>Timestamp</Typography></TableCell>
                  <TableCell ><Typography variant='h6'>StatusCode</Typography></TableCell>
                  <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
               </TableRow>
            </TableHead>
            <TableBody>
               {values?.map((value) => (
                  <TableRow
                     key={value.id}
                     sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                     <TableCell>
                        <Typography variant='body1'>{OpcUa.Attributes[value.attributeId]}</Typography>
                     </TableCell>
                     <TableCell>
                        <Typography variant='body1'>{Web.dateToLocalTime(value.value?.SourceTimestamp ?? value.value?.ServerTimestamp)}</Typography>
                     </TableCell>
                     <TableCell>
                        <Typography variant='body1'>{OpcUa.StatusCodes[value.value?.StatusCode ?? 0]}</Typography>
                     </TableCell>
                     <TableCell sx={{ width: '100%' }}>
                        <Typography variant='body1'>{JSON.stringify(value.value?.Value?.Body)}</Typography>
                     </TableCell>
                  </TableRow>
               ))}
            </TableBody>
         </Table>
      </TableContainer>
   );
}

export default AttributesView;
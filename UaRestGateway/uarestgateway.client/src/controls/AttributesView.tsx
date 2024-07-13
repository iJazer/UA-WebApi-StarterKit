import React from 'react';

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
import { readAttributes } from '../opcua-utils';
import { IReadResult } from '../service/IReadResult';
import { UserContext } from '../UserProvider';

interface AttributesViewProps {
   reference?: OpcUa.ReferenceDescription
   requestTimeout?: number
}

export const AttributesView = ({ reference, requestTimeout }: AttributesViewProps) => {
   const [values, setValues] = React.useState<IReadResult[]>([]);
   const name = reference?.DisplayName?.Text ?? reference?.BrowseName ?? reference?.NodeId;
   const theme = useTheme();
   const { user } = React.useContext(UserContext);

   React.useEffect(() => {
      if (reference?.NodeId) {
         readAttributes(reference, requestTimeout, user).then((x) => setValues(x ?? []));
      }
   }, [reference, reference?.NodeId, requestTimeout, user]);

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
                        <Typography variant='body1'>{OpcUa.Attributes[value.attributeId ?? 0]}</Typography>
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
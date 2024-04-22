import React from 'react';

import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Skeleton, Typography } from '@mui/material';

import * as OpcUa from '../opcua';
import * as Web from '../Web';
import { ApplicationContext } from '../ApplicationProvider';
import { IMonitoredItem } from '../opcua-utils';

interface MonitoredItemValueListProps {
   selection?: string
}

export const MonitoredItemValueList = ({ selection }: MonitoredItemValueListProps) => {
   const [items, setItems] = React.useState<IMonitoredItem[]>([]);
   const context = React.useContext(ApplicationContext);
   const monitoredItems = context.monitoredItems;

   React.useEffect(() => {
      if (monitoredItems) {
         const filtered = Array.from(monitoredItems.values()).map((item) => {
            if (!selection) {
               return item;
            }
            return item.path.join('/').startsWith(selection) ? item : undefined;
         }).filter((x) : x is IMonitoredItem => x !== undefined);
         setItems(filtered);
      }
   }, [monitoredItems, selection]);

   if (!items?.length) {
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
                  <TableCell><Typography variant='h6'>Name</Typography></TableCell>
                  <TableCell><Typography variant='h6'>Timestamp</Typography></TableCell>
                  <TableCell ><Typography variant='h6'>StatusCode</Typography></TableCell>
                  <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
               </TableRow>
            </TableHead>
            <TableBody>
               {items?.map((item) => {
                  if (!item?.serverHandle) return null;
                  return (
                     <TableRow
                        key={item?.serverHandle}
                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                     >
                        <TableCell>
                           <Typography variant='body1'>{item?.path.join('/')}</Typography>
                        </TableCell>
                        <TableCell>
                           <Typography variant='body1'>{Web.dateToLocalTime(item?.value?.SourceTimestamp ?? item?.value?.ServerTimestamp)}</Typography>
                        </TableCell>
                        <TableCell>
                           <Typography variant='body1'>{OpcUa.StatusCodes[item?.value?.StatusCode ?? 0]}</Typography>
                        </TableCell>
                        <TableCell sx={{ width: '100%' }}>
                           <Typography variant='body1'>{JSON.stringify(item?.value?.Value?.Body)}</Typography>
                        </TableCell>
                     </TableRow>
                  );
               })}
            </TableBody>
         </Table>
      </TableContainer>
   );
}

export default MonitoredItemValueList;
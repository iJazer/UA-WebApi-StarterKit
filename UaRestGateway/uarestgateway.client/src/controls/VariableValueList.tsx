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
import { ApplicationContext } from '../ApplicationProvider';
import DataValueDisplay from './DataValueDisplay';
import { IBrowsedNode } from '../service/IBrowsedNode';

interface VariableValueListProps {
   rootId?: string
}

export const VariableValueList = ({ rootId }: VariableValueListProps) => {
   const [variables, setVariables] = React.useState<IBrowsedNode[]>([]);
   const { nodes, lastUpdatedNode, readValues } = React.useContext(ApplicationContext);

   React.useEffect(() => {
      const variableIds: string[] = [];
      if (rootId) {
         const children = Array.from(nodes.values()).filter((x) => x.parentId === rootId);
         if (children) {
            children.forEach((child) => {
               if (child?.reference?.NodeClass === OpcUa.NodeClass.Variable) {
                  variableIds.push(child.id);
               }
            });
         }
         readValues(variableIds).then((values) => setVariables(values ?? []));
      }

   }, [rootId, readValues, lastUpdatedNode, nodes]);

   if (!variables?.length) {
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
                  <TableCell sx={{ width: '100%' }}><Typography variant='h6'>Value</Typography></TableCell>
               </TableRow>
            </TableHead>
            <TableBody>
               {variables?.map((item) => {
                  if (!item) return null;
                  return (
                     <TableRow
                        key={item?.id}
                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                     >
                        <TableCell sx={{ width: 'auto' }}>
                           <Typography variant='body1' sx={{ minWidth: '300px' }}>{item.reference.DisplayName?.Text}</Typography>
                        </TableCell>
                        <TableCell>
                           <DataValueDisplay value={item.value} />
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
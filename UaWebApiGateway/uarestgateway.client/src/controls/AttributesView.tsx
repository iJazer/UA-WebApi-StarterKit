import React from 'react';

import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Skeleton, Typography, useTheme } from '@mui/material';

import * as OpcUa from 'opcua-webapi';
import * as Web from '../Web';
import { readAttributes } from '../opcua-utils';
import { IReadResult } from '../service/IReadResult';
import { UserContext } from '../UserProvider';

import { IReadValueId } from '../service/IReadValueId';
import { HandleFactory } from '../service/HandleFactory';
import { BrowseContext } from '../BrowseContext';

interface VariableValueListInternals {
    internalHandle: number,
    requests: number[]
}

interface AttributesViewProps {
   reference?: OpcUa.ReferenceDescription
   requestTimeout?: number
}

export const AttributesView = ({ reference, requestTimeout }: AttributesViewProps) => {
    const [values, setValues] = React.useState<IReadResult[]>([]);
    const name = reference?.DisplayName?.Text ?? reference?.BrowseName ?? reference?.NodeId;
    const theme = useTheme();
    const { user } = React.useContext(UserContext);

    const m = React.useRef<VariableValueListInternals>({
        internalHandle: HandleFactory.increment(),
        requests: []
    });

    const {
        readValues,
        lastCompletedRequest
    } = React.useContext(BrowseContext);

    React.useEffect(() => {
        if (reference?.NodeId) {
            //readAttributes(reference, requestTimeout, user).then((x) => setValues(x ?? []));
            const nodesToRead: IReadValueId[] = [];

            for (const id in OpcUa.Attributes) {
                nodesToRead.push({
                    id: HandleFactory.increment(),
                    nodeId: reference.NodeId,
                    attributeId: Number(OpcUa.Attributes[id])
                });
            }

            m.current.internalHandle = HandleFactory.increment();
            m.current.requests.push(m.current.internalHandle);
            readValues(m.current.internalHandle, nodesToRead);
        }
    }, [reference, reference?.NodeId, requestTimeout, user, readValues]);

    React.useEffect(() => {
        if (lastCompletedRequest && m.current.requests.find(x => x === lastCompletedRequest.callerHandle)) {
            const values: IReadResult[] = [];
            if (lastCompletedRequest.values) {
                for (let ii = 0; ii < lastCompletedRequest.values.length; ii++) {
                    const x = lastCompletedRequest.values[ii];
                    if (x.value?.StatusCode?.Code !== OpcUa.StatusCodes.BadAttributeIdInvalid) {
                        values.push({
                            id: HandleFactory.increment(),
                            nodeId: x.nodeId ?? '',
                            attributeId: x.attributeId ?? 0,
                            value: x.value
                        });
                    }
                }
            }
            setValues(values ?? []);
        }
    }, [lastCompletedRequest]);


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
                        <Typography variant='body1'>{OpcUa.StatusCodes[value.value?.StatusCode?.Code ?? 0]}</Typography>
                     </TableCell>
                     <TableCell sx={{ width: '100%' }}>
                        <Typography variant='body1'>{JSON.stringify(value.value?.Value)}</Typography>
                     </TableCell>
                  </TableRow>
               ))}
            </TableBody>
         </Table>
      </TableContainer>
   );
}

export default AttributesView;
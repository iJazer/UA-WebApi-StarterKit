import React from 'react';
//import { IMonitoredItem, SubscriptionContext } from '../SubscriptionProvider';
//import Card from '@mui/material/Card/Card';
//import CardContent from '@mui/material/CardContent/CardContent';
import Table from '@mui/material/Table/Table';
import TableBody from '@mui/material/TableBody/TableBody';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import Typography from '@mui/material/Typography/Typography';
import TableContainer from '@mui/material/TableContainer/TableContainer';
import TableHead from '@mui/material/TableHead/TableHead';
import Paper from '@mui/material/Paper/Paper';
//import * as OpcUa from 'opcua-webapi';
//import DataValueDisplay from './DataValueDisplay';
//import { HandleFactory } from '../service/HandleFactory';
//import { SubscriptionState } from '../service/SubscriptionState';
//import { translateAndReadValues } from '../opcua-utils';
//import { IReadValueId } from '../service/IReadValueId';
//import { UserContext } from '../UserProvider';


interface AASSubmodelProps {
    message: string;
}

// Define and export a function
export const updateAAS = (
    setMessage: React.Dispatch<React.SetStateAction<string>>,
    message: string,
) => {
    setMessage(message);
}

export const AASSubmodel: React.FC<AASSubmodelProps> = ({ message }) => {

    return (
       <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '100%' }}>
           <Table>
               <TableHead>
                   <TableRow>
                       <TableCell><Typography variant='h6'>Asset Administration Shell</Typography></TableCell>
                   </TableRow>
               </TableHead>
               <TableBody>
                   <TableRow
                       sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                       <TableCell sx={{ width: 'auto' }}>
                           <Typography variant='body1' sx={{ minWidth: '300px' }}>{message}</Typography>
                       </TableCell>
                   </TableRow> 
               </TableBody>
           </Table>
       </TableContainer>
    );
}

export default AASSubmodel;
import React, { useState } from 'react';
import TableContainer from '@mui/material/TableContainer';
import Table from '@mui/material/Table';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import TableBody from '@mui/material/TableBody';
import Paper from '@mui/material/Paper';
import { Typography } from '@mui/material';
import DataValueDisplay from './DataValueDisplay';
import { IMonitoredItem } from '../SubscriptionProvider';

interface DataAccessViewProps {
    rootId?: string;
}

interface Row {
    name: string;
    item: IMonitoredItem;
}

export const addAccessViewItem = (displayName: string, nodeId: string) => {
    // function implementation
    console.log(displayName);
    console.log(nodeId);
    //setVariables((prev) => [...prev, { name: displayName, item: { value: { value: 'test' } } }]);
};

export const DataAccessView = ({ rootId }: DataAccessViewProps) => {
    const [variables, setVariables] = useState<Row[]>([]);

    return (
        <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '40%', mr: 15 }}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell><Typography variant='h6'>Name</Typography></TableCell>
                        <TableCell sx={{ width: '40%' }}><Typography variant='h6'>Value</Typography></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {variables.map((variable) => (
                        <TableRow key={variable.name} sx={{ '&:last-child td, &:last-child th': { border: 0 } }}>
                            <TableCell sx={{ width: 'auto' }}>
                                <Typography variant='body1' sx={{ minWidth: '300px' }}>{variable.name}</Typography>
                            </TableCell>
                            <TableCell>
                                <DataValueDisplay value={variable.item.value} />
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default DataAccessView;

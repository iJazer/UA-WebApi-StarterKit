import React, { useState, useEffect } from 'react';
import TableContainer from '@mui/material/TableContainer';
import Table from '@mui/material/Table';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import TableCell from '@mui/material/TableCell';
import TableBody from '@mui/material/TableBody';
import Paper from '@mui/material/Paper';
import { Typography } from '@mui/material';

import { BrowseContext } from '../BrowseContext';

interface AccessViewItem {
    displayName: string;
    nodeId: string;
    value?: string;
}

interface DataAccessViewProps {
    items?: AccessViewItem[];
}

const accessViewItems: AccessViewItem[] = [];

export const addAccessViewItem = (displayName: string, nodeId: string) => {
    accessViewItems.push({ displayName, nodeId });

    // Notify the component to re-render
    window.dispatchEvent(new Event('accessViewItemsUpdated'));
};

export const DataAccessView: React.FC<DataAccessViewProps> = () => {
    const [items, setItems] = useState<AccessViewItem[]>([]);

    useEffect(() => {
        const handleItemsUpdated = () => {
            setItems([...accessViewItems]);
        };

        window.addEventListener('accessViewItemsUpdated', handleItemsUpdated);

        return () => {
            window.removeEventListener('accessViewItemsUpdated', handleItemsUpdated);
        };
    }, []);

    const onDeleteItem = (index: number) => {
        accessViewItems.splice(index, 1);
        setItems([...accessViewItems]);
        window.dispatchEvent(new Event('accessViewItemsUpdated'));
    };

    return (
        <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '80%' }}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell><Typography variant='h6'>Name</Typography></TableCell>
                        <TableCell sx={{ width: '40%' }}><Typography variant='h6'>Value</Typography></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {items.map((item, index) => (
                        <TableRow key={index} sx={{ '&:last-child td, &:last-child th': { border: 0 } }} onClick={() => onDeleteItem(index)} >
                            <TableCell sx={{ width: 'auto' }}>
                                <Typography variant='body1' sx={{ minWidth: '300px' }}>{item.displayName}</Typography>
                            </TableCell>
                            <TableCell>
                                <Typography variant='body1'>{item.nodeId}</Typography>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default DataAccessView;

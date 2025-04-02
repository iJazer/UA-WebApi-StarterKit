/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useContext } from 'react'
import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Typography } from '@mui/material';
import { SessionContext } from '../SessionContext';


interface NetworkListenerProps {
}

// Define and export a function
export const setNetworkMessage = (
    setMessage: React.Dispatch<React.SetStateAction<string>>,
    message: string,
) => {
    setMessage(message ? JSON.stringify(JSON.parse(message), null, 2) : "");
}

const NetworkListener: React.FC<NetworkListenerProps> = () => {
    const { message } = useContext(SessionContext);

    return (
        <TableContainer component={Paper} elevation={3} sx={{ height: '100%', width: '100%' }}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell><Typography variant='h6'>Network message</Typography></TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    <TableRow>
                        <TableCell>
                            <pre>{message}</pre>
                        </TableCell>
                    </TableRow>
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default NetworkListener;
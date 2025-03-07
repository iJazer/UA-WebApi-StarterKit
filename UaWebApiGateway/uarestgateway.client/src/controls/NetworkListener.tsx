/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState, useEffect, useMemo } from 'react';
import TableContainer from '@mui/material/TableContainer/TableContainer';
import Table from '@mui/material/Table/Table';
import TableHead from '@mui/material/TableHead/TableHead';
import TableRow from '@mui/material/TableRow/TableRow';
import TableCell from '@mui/material/TableCell/TableCell';
import TableBody from '@mui/material/TableBody/TableBody';
import Paper from '@mui/material/Paper/Paper';
import { Typography } from '@mui/material';

import { SessionContext } from '../SessionProvider';


export const NetworkListener = () => {
    const { message: sessionMessage } = React.useContext(SessionContext);
    const [networkMessage, setMessage] = useState<string>("");

    useEffect(() => {
        setMessage(sessionMessage);
    }, [sessionMessage]);

    const formattedMessage = useMemo(() => {
        return networkMessage ? JSON.stringify(JSON.parse(networkMessage), null, 2) : "";
    }, [networkMessage]);


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
                            <pre>{formattedMessage}</pre>
                        </TableCell>
                    </TableRow>
                </TableBody>
            </Table>
        </TableContainer>
    );
}

export default NetworkListener;
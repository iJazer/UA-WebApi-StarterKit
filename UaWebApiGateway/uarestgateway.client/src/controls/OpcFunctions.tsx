/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box/Box';

//import * as OpcUa from 'opcua-webapi';
//import { IBrowsedNode } from '../service/IBrowsedNode';
//import { SessionContext } from '../SessionProvider';


interface OpcFunctionsProps {
    // Define any props if needed
}

const OpcFunctions: React.FC<OpcFunctionsProps> = () => {
    //const session = useContext(SessionContext);

    return (
        <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
            <Typography variant="h5" component="h2" gutterBottom>
                OPC UA 
            </Typography>
            <Box display="flex" justifyContent="center">
                <Button>
                    Test
                </Button>
            </Box>
        </Paper>
    );
}

export default OpcFunctions;
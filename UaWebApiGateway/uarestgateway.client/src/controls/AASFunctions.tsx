/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box/Box';

//import * as OpcUa from 'opcua-webapi';
//import { IBrowsedNode } from '../service/IBrowsedNode';

interface AASFunctionsProps {
    setMessage: React.Dispatch<React.SetStateAction<string>>;
}

export const AASFunctions: React.FC<AASFunctionsProps> = ({ setMessage }) => {
    const [clickCount, setClickCount] = useState(0);

    const handleButtonClick = async () => {
        const newCount = clickCount + 1;
        setClickCount(newCount);
        setMessage('Error fetching submodel nodes');
    };

    return (
        <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
            <Typography variant="h5" component="h2" gutterBottom>
                AAS
            </Typography>
            <Box display="flex" justifyContent="center">
                <Button sx={{
                    fontSize: '1rem', padding: '10px 20px', backgroundColor: '#333', color: 'white', '&:hover': { color: 'white', backgroundColor: '#555' }
                }} onClick={handleButtonClick}>
                    Get Submodel
                </Button>
            </Box>
        </Paper>
    );
}

export default AASFunctions;
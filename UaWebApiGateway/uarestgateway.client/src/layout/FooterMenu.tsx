import React from 'react';

import { useTheme } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography/Typography';
import Link from '@mui/material/Link/Link';

import { BuildVersion } from '../version';
import { UserContext } from '../UserProvider';

export const Footer = () => {
   const context = React.useContext(UserContext);
   const theme = useTheme();
   return (
      <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px', justifyContent: 'space-between' }}>
         <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', color: theme.palette.text.primary, md: 'flex' } }}>
           
         </Box>
         
      </Toolbar>
   );
}

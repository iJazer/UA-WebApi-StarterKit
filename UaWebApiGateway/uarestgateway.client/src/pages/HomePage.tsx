import React from 'react';
import Box from '@mui/material/Box/Box';

import SessionStatusBar from '../controls/SessionStatusBar';

import NetworkListener from '../controls/NetworkListener';
import { BrowseProvider } from '../BrowseProvider';
import SubscriptionPage from './SubscriptionPage';
import AASTreeView from '../controls/AASTreeView';

export const HomePage = () => {

   return (
       <BrowseProvider>
       <Box display="flex" flexDirection="column" p={2} sx={{ width: '100%' }}>
         <SessionStatusBar />
         <Box display="flex" p={2} pb={4} sx={{ width: '100%', height: '33.33vh'}}>
            <SubscriptionPage />
         </Box>
         <Box display="flex" p={2} pb={4} sx={{ width: '100%', height: '33.33vh' }}>
            <Box flexGrow={0} sx={{ overflow: 'auto' }}>
                 <AASTreeView />
            </Box>
         </Box>
         <Box display="flex" p={2} pb={4} sx={{ width: '100%'}}>
            <Box flexGrow={0} sx={{ width: '100%', overflow: 'auto' }}>
                   <NetworkListener />
            </Box>
         </Box>
       </Box>

           
       </BrowseProvider>
   );
};

export default HomePage;
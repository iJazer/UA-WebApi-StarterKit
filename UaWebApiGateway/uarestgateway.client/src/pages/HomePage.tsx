import React, { useState } from 'react';
import Box from '@mui/material/Box/Box';

import { BrowseTreeView } from '../controls/BrowseTreeView';
import VariableValueList from '../controls/VariableValueList';
import SessionStatusBar from '../controls/SessionStatusBar';

import * as OpcUa from 'opcua-webapi';
import ServerStatusCard from '../controls/ServerStatusCard';
import AASSubmodel from '../controls/AASSubmodel';
import AASFunctions from '../controls/AASFunctions';
import NetworkListener from '../controls/NetworkListener';
import BrowseProvider from '../BrowseProvider';
import SubscriptionPage from './SubscriptionPage';


export const HomePage = () => {
   const [selection, setSelection] = React.useState<OpcUa.ReferenceDescription | undefined>();
   const [message, setMessage] = useState<string>('');

   const selectPanel = React.useCallback((reference?: OpcUa.ReferenceDescription) => {
      if (!reference) {
         return <VariableValueList />;
      }
      switch (reference.TypeDefinition) {
         case OpcUa.VariableTypeIds.ServerStatusType: {
            return <ServerStatusCard rootId={reference.NodeId} />;
         }
         default:
            {
               return <VariableValueList rootId={reference?.NodeId} />;
            }
      }
   }, []);

   //const onSelectionChanged = React.useCallback((x: OpcUa.ReferenceDescription | undefined) => setSelection({ ...x }), []);


   return (
       <BrowseProvider>
       <Box display="flex" flexDirection="column" p={2} sx={{ width: '100%' }}>
         <SessionStatusBar />
         <Box display="flex" p={2} pb={4} sx={{ width: '100%', height: '33.33vh'}}>
            <SubscriptionPage />
         </Box>
         <Box display="flex" p={2} pb={4} sx={{ width: '100%', height: '33.33vh' }}>
               <Box sx={{ width: '20%', overflow: 'auto', mr: 15 }}>
                   <AASFunctions setMessage={setMessage} />
            </Box>
               <Box flexGrow={1} sx={{ width: '80%', overflow: 'auto' }}>
                   <AASSubmodel message={message} />
            </Box>
         </Box>
         <Box display="flex" p={2} pb={4} sx={{ width: '100%'}}>
            <Box flexGrow={0} sx={{ width: '100%'}}>
                   <NetworkListener />
            </Box>
         </Box>
       </Box>

           
       </BrowseProvider>
   );
};

export default HomePage;
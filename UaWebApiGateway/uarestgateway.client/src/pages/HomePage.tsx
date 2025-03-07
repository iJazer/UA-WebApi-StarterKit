import React from 'react';
import Box from '@mui/material/Box/Box';

import { BrowseTreeView } from '../controls/BrowseTreeView';
import VariableValueList from '../controls/VariableValueList';
import SessionStatusBar from '../controls/SessionStatusBar';

import * as OpcUa from 'opcua-webapi';
import ServerStatusCard from '../controls/ServerStatusCard';
import OpcFunctions from '../controls/OpcFunctions';
import CompanionSpec from '../controls/CompanionSpec';
import AASSubmodel from '../controls/AASSubmodel';
import AASFunctions from '../controls/AASFunctions';
import NetworkListener from '../controls/NetworkListener';


export const HomePage = () => {
   const [selection, setSelection] = React.useState<OpcUa.ReferenceDescription | undefined>();

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

   const onSelectionChanged = React.useCallback((x: OpcUa.ReferenceDescription | undefined) => setSelection({ ...x }), []);

   return (
      <Box display="flex" flexDirection="column" p={2} sx={{ width: '100%' }}>
         <SessionStatusBar />
         <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
            <Box flexGrow={0}>
               <BrowseTreeView rootNodeId={OpcUa.ObjectIds.RootFolder} onSelectionChanged={onSelectionChanged} />
            </Box>
            <Box flexGrow={1}>
               {selectPanel(selection)}
            </Box>
         </Box>
           <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
               <Box flexGrow={0}>
                   <OpcFunctions />
               </Box>
               <Box flexGrow={1}>
                   <CompanionSpec />
               </Box>
           </Box>
           <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
               <Box flexGrow={0}>
                   <AASFunctions />
               </Box>
               <Box flexGrow={1}>
                   <AASSubmodel />
               </Box>
           </Box>
           <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
               <NetworkListener />
           </Box>
      </Box>
   );
};

export default HomePage;
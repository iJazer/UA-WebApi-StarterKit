import React from 'react';
import Box from '@mui/material/Box/Box';

import { BrowseTreeView } from '../controls/BrowseTreeView';
import AttributesView from '../controls/AttributesView';

import * as OpcUa from 'opcua-webapi';

export const SubscriptionPage = () => {
   const [selection, setSelection] = React.useState<OpcUa.ReferenceDescription | undefined>();

   return (
      <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
           <Box flexGrow={0} sx={{ width: '60%' }} >
            <BrowseTreeView rootNodeId={OpcUa.ObjectIds.RootFolder} onSelectionChanged={(x) => setSelection({ ...x })} />
         </Box>
           <Box flexGrow={1} sx={{ width: '40%' }} >
            <AttributesView reference={selection} />
         </Box>
      </Box>
   );
};

export default SubscriptionPage;
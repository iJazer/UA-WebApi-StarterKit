import React from 'react';
import Box from '@mui/material/Box/Box';

import { MonitoredItemTreeView } from '../controls/MonitoredItemTreeView';
import MonitoredItemValueList from '../controls/MonitoredItemValueList';
import SessionStatusBar from '../controls/SessionStatusBar';
import { SessionContext } from '../SessionProvider';
import { HandleFactory } from '../opcua-utils';

export const MonitorPage = () => {
   const [selection, setSelection] = React.useState<string | undefined>();


   return (
      <Box sx={{ display: 'flex', flexDirection: 'column', m: 0, p: 0, width: '100%' }}>
         <SessionStatusBar />
         <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
            <Box flexGrow={0}>
               <MonitoredItemTreeView onSelectionChanged={(selection: string) => setSelection(selection)} />
            </Box>
            <Box flexGrow={1}>
               <MonitoredItemValueList selection={selection} />
            </Box>
         </Box>
      </Box>
   );
};

export default MonitorPage;
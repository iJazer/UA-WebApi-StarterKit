import React from 'react';
import Box from '@mui/material/Box/Box';

import { MonitoredItemTreeView } from '../controls/MonitoredItemTreeView';
import { Toolbar, Typography, useTheme } from '@mui/material';
import { ApplicationContext } from '../ApplicationProvider';
import { SessionState } from '../opcua-utils';
import { UserLoginStatus } from '../api';
import MonitoredItemValueList from '../controls/MonitoredItemValueList';

export const MonitorPage = () => {
   const [selection, setSelection] = React.useState<string | undefined>();
   const theme = useTheme();
   const context = React.useContext(ApplicationContext);
   const loginStatus = context?.userContext.loginStatus ?? UserLoginStatus.Unknown;
   const sessionState = context?.session?.state ?? SessionState.Closed;
   const callback = context.connect;

   React.useEffect(() => {
      const controller = new AbortController();
      if (loginStatus === UserLoginStatus.LoggedIn && sessionState === SessionState.Closed) {
         connect();
      }
      async function connect() {
         await callback();
      }
      return () => {
         controller.abort();
      };
   }, [loginStatus, sessionState, callback]);

   return (
      <Box sx={{ display: 'flex', flexDirection: 'column', m: 0, p: 0, width: '100%' }}>
         <Toolbar variant='dense' disableGutters sx={{ py: 0, minHeight: '36px', justifyContent: 'space-between', backgroundColor: theme.palette.primary.light }}>
            <Box ml={6} sx={{ flexGrow: 0, display: { xs: 'none', md: 'flex' } }}>
               <Typography variant='body2' color={theme.palette.primary.dark}>{`Session: ${SessionState[context.session?.state ?? SessionState.Closed]}`}</Typography>
            </Box>
            <Box
               alignContent='right'
               textAlign='right'
               mx={6}
               sx={{
                  display: 'flex',
                  alignItems: 'center'
               }}
            >
               <Typography variant='body2' color={theme.palette.primary.dark}>{`Last Sequence Number: ${context.session?.lastSequenceNumber ?? '---'}`}</Typography>
            </Box>
         </Toolbar>
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
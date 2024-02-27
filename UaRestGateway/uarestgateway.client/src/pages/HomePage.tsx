import React from 'react';
import Box from '@mui/material/Box/Box';

import { BrowseTreeView } from '../controls/BrowseTreeView';
import AttributesView from '../controls/AttributesView';
import { ApplicationContext } from '../ApplicationProvider';

import * as OpcUa from '../opcua';
import { UserLoginStatus } from '../api';
import { SessionState } from '../opcua-utils';

export const HomePage = () => {
   const [selection, setSelection] = React.useState<OpcUa.ReferenceDescription | undefined>();
   const context = React.useContext(ApplicationContext);
   const loginStatus = context?.userContext.loginStatus ?? UserLoginStatus.Unknown;
   const sessionState = context?.session?.state ?? SessionState.Closed;
   const callback = context.connect;

   //React.useEffect(() => {
   //   console.error(`SessionState: ${SessionState[sessionState]}`);
   //}, [sessionState]);

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
      <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
         <Box flexGrow={0}>
            <BrowseTreeView rootNodeId={OpcUa.ObjectIds.RootFolder} onSelectionChanged={(x) => setSelection({ ...x })} />
         </Box>
         <Box flexGrow={1}>
            <AttributesView reference={selection} />
         </Box>
      </Box>
   );
};

export default HomePage;
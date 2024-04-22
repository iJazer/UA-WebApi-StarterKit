import React from 'react';
import Box from '@mui/material/Box/Box';

import { BrowseTreeView } from '../controls/BrowseTreeView';
import VariableValueList from '../controls/VariableValueList';
import { ApplicationContext } from '../ApplicationProvider';

import * as OpcUa from '../opcua';
import { UserLoginStatus } from '../api';
import { SessionState } from '../opcua-utils';

export const HomePage = () => {
   const [selection, setSelection] = React.useState<OpcUa.ReferenceDescription | undefined>();
   const [inProgress, setInProgress] = React.useState<boolean>(false);
   const context = React.useContext(ApplicationContext);
   const loginStatus = context?.userContext.loginStatus ?? UserLoginStatus.Unknown;
   const sessionState = context?.session?.state ?? SessionState.Closed;
   const connect = context.connect;

   React.useEffect(() => {
      //const controller = new AbortController();
      if (!inProgress && loginStatus === UserLoginStatus.LoggedIn && sessionState === SessionState.Closed) {
         setInProgress(true);
         connect().finally(() => setInProgress(false));
      }
      return () => {
         //controller.abort();
      };
   }, [inProgress, loginStatus, sessionState, connect]);

   return (
      <Box display="flex" p={2} pb={4} sx={{ width: '100%' }}>
         <Box flexGrow={0}>
            <BrowseTreeView rootNodeId={OpcUa.ObjectIds.RootFolder} onSelectionChanged={(x) => setSelection({ ...x })} />
         </Box>
         <Box flexGrow={1}>
            <VariableValueList selection={selection?.NodeId} />
         </Box>
      </Box>
   );
};

export default HomePage;
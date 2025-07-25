import React from 'react';
import { SessionState } from './service/SessionState';
import { ISessionContext, DefaultServerUrl } from './SessionProvider';
import { ICompletedRequest } from './service/ICompletedRequest';


export const SessionContext = React.createContext<ISessionContext>({
   serverUrl: DefaultServerUrl,
   setServerUrl: () => { },
   isConnected: false,
   sessionState: SessionState.Disconnected,
   isEnabled: false,
   setIsEnabled: () => { },
   isSessionEnabled: false,
   setIsSessionEnabled: () => { },
   requestTimeout: 60000,
   setRequestTimeout: () => { },
   sendRequest: () => { },
   messageCounter: 0,
   processMessages: () => { return []; }
});
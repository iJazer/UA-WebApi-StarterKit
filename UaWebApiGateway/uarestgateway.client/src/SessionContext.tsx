import React from 'react';
import { SessionState } from './service/SessionState';
import { ISessionContext, DefaultServerUrl } from './SessionProvider';


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
    lastCompletedRequest: undefined,
    message: "",
    getSubmodelNodes: (): Promise<string> => Promise.resolve("")
});

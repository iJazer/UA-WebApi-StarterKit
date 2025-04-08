import React from 'react';
import useWebSocket, { ReadyState } from 'react-use-websocket';
import { UserContext } from './UserProvider';
import * as OpcUa from 'opcua-webapi';
import { UserLoginStatus } from './user';
import { IRequestMessage } from './service/IRequestMessage';
import { IResponseMessage } from './service/IResponseMessage';
import { ICompletedRequest } from './service/ICompletedRequest';
import { SessionState } from './service/SessionState';
import { HandleFactory } from './service/HandleFactory';
import { call } from './opcua-utils';

export const DefaultServerUrl = `wss://${location.host}/stream`;

import { setNetworkMessage } from './controls/NetworkListener';
import { SessionContext } from './SessionContext';

export interface ISessionContext {
   serverUrl: string,
   setServerUrl: (value: string) => void,
   isConnected: boolean,
   sessionState: SessionState,
   isEnabled: boolean,
   setIsEnabled: (value: boolean) => void,
   isSessionEnabled: boolean,
   setIsSessionEnabled: (value: boolean) => void,
   requestTimeout: number,
   setRequestTimeout: (value: number) => void,
   sendRequest: (request: IRequestMessage, clientHandle?: number) => void
   lastCompletedRequest?: ICompletedRequest,
   message: string
}

interface SessionProps {
   children?: React.ReactNode
}

interface SessionInternals {
   serverUrl: string | null,
   isConnected: boolean,
   sessionState: SessionState,
   isEnabled: boolean,
   isSessionEnabled: boolean,
   requestTimeout: number,
   authenticationToken?: string,
   serverNonce?: string,
   requests: Map<number, ICompletedRequest>,
   responses: IResponseMessage[],
   message: string | null,
}

const apiNames = {
   [OpcUa.DataTypeIds.BrowseRequest]: { path: "browse", response: OpcUa.DataTypeIds.BrowseResponse },
   [OpcUa.DataTypeIds.BrowseNextRequest]: { path: "browsenext", response: OpcUa.DataTypeIds.BrowseNextResponse }, 
   [OpcUa.DataTypeIds.ReadRequest]: { path: "read", response: OpcUa.DataTypeIds.ReadResponse },
   [OpcUa.DataTypeIds.WriteRequest]: { path: "write", response: OpcUa.DataTypeIds.WriteResponse },
   [OpcUa.DataTypeIds.CallRequest]: { path: "call", response: OpcUa.DataTypeIds.CallResponse },
   [OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsRequest]: { path: "translate", response: OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsResponse },
   [OpcUa.DataTypeIds.HistoryReadRequest]: { path: "historyread", response: OpcUa.DataTypeIds.HistoryReadResponse }, 
   [OpcUa.DataTypeIds.HistoryUpdateRequest]: { path: "historyupdate", response: OpcUa.DataTypeIds.HistoryUpdateResponse }
};

export const SessionProvider = ({ children }: SessionProps) => {
   const [serverUrl, setServerUrl] = React.useState<string | null>(DefaultServerUrl);
   const [isEnabled, setIsEnabled] = React.useState<boolean>(false);
   const [isSessionEnabled, setIsSessionEnabled] = React.useState<boolean>(false);
   const [sessionState, setSessionState] = React.useState<SessionState>(SessionState.Disconnected);
   const [lastCompletedRequest, setLastCompletedRequest] = React.useState<ICompletedRequest | undefined>();
   const { user, loginStatus } = React.useContext(UserContext);
   const [visibleNodes, setVisibleNodes] = React.useState<string[]>([]);
   const [message, setMessage] = React.useState<string>("");

   const m = React.useRef<SessionInternals>({
      serverUrl: null,
      isConnected: false,
      sessionState: SessionState.Disconnected,
      isEnabled: false,
      isSessionEnabled: false,
      requestTimeout: 60000,
      requests: new Map<number, ICompletedRequest>(),
      responses: [],
      message: null,
   });

   const { sendMessage, lastMessage, readyState } = useWebSocket(
      (isEnabled) ? serverUrl : null,
      {
         share: true,
         protocols: (user?.accessToken) ? ["opcua+uajson", `opcua+token+${user?.accessToken}`] : ["opcua+uajson"],
         shouldReconnect: () => {
            return isEnabled;
         }
      },
   );

   /**
    * processRawResponse: Function which processes the raw response from the server
    * 
    * @param response: IResponseMessage - the response from the server
    * 
    * The function checks the requestHandle of the response and processes the response accordingly
    */
   const processRawResponse = React.useCallback((response: IResponseMessage) => {
      if (response) {
         const requestHandle = response?.Body?.ResponseHeader?.RequestHandle ?? 0;
         const request = m.current.requests.get(requestHandle);
         if (request) {
            m.current.requests.delete(requestHandle);
            setLastCompletedRequest({ ...request, response });
         }
      }
   }, [])

   /**
    * sendRequest is the connection link between the client and the server
    *
    * @parma request: IRequestMessage - the full request message for the server
    * @param callerHandle?: number - The handle of the caller initiating the read operation.
    * 
    * If the readyState is open, a WebSocket is established and the calls are send over WebSocket
    * If the readyState is close, the call is send over REST
    * 
    * For REST, a secound comparison has to be made, if it's a AAS call or an OPC UA call
    */
   const sendRequest = React.useCallback((request: IRequestMessage, callerHandle?: number) => {
      if (!request.Body.RequestHeader) {
         request.Body.RequestHeader = {}
      }
      const requestHeader = request.Body.RequestHeader;
      requestHeader.RequestHandle = HandleFactory.increment();
      if (!requestHeader.Timestamp) {
         requestHeader.Timestamp = new Date();
      }
      if (!requestHeader.TimeoutHint) {
         requestHeader.TimeoutHint = m.current.requestTimeout;
      }
      if (!requestHeader.AuthenticationToken) {
         requestHeader.AuthenticationToken = m.current.authenticationToken;
      }
      m.current.requests.set(
         requestHeader.RequestHandle,
         { callerHandle: callerHandle ?? requestHeader.RequestHandle, request }
      );

      setNetworkMessage(setMessage, JSON.stringify(request));

      try {
          if (readyState === ReadyState.OPEN) {
            console.log("WebSocket call");
            sendMessage(JSON.stringify(request));
         }
         else {
            const clientHandle = requestHeader.RequestHandle;

              if (request.Body.RequestHeader.RequestHandle) { //Erkennung ob es ein OPC UA request ist, da RequestHandle nur für OPC UA calls genutzt werden
                 console.log("OPC UA REST call");
                 call(`/opcua/${apiNames[request.ServiceId ?? ''].path}`,
                     { callerHandle: clientHandle, request: { Body: request.Body } },
                     undefined,
                     user,
                     true)
                     .then(response => {
                         processRawResponse({ ServiceId: apiNames[request.ServiceId ?? ''].response, Body: response });
                     })
                     .catch(error => {
                         console.error('Failed to send request:', error);
                     });
             }
             else if (request.Body.RequestHeader.AASRequestHandle) { //Erkennung ob es ein AAS request ist --> Für Juliee
                 console.log("AAS call");
             }
             else {
                 console.error("Unknown callerId");
             }
         }
      } catch (error) {
         console.error('Failed to send request:', error);
         // Handle the error appropriately
      }
   }, [sendMessage, processRawResponse, readyState, user]);


   /**
    * activateSession: Function which is the second step in the connection process after the createSession call
    * 
    * @param createSessionResponse: OpcUa.CreateSessionResponse - the response from the server after the createSession call
    * 
    * The function searches for the correct endpoint and token to activate the session
    */
   const activateSession = React.useCallback((createSessionResponse: OpcUa.CreateSessionResponse) => {
      const endpoint = createSessionResponse?.ServerEndpoints?.find((endpoint) => {
         if (endpoint.TransportProfileUri === "http://opcfoundation.org/UA-Profile/Transport/wss-uajson") {
            return endpoint;
         }
         return null;
      });
      let token: OpcUa.ExtensionObject | undefined = undefined;
      if (loginStatus === UserLoginStatus.LoggedIn && user.accessToken) {
         const policy = endpoint?.UserIdentityTokens?.find(
            (token) => (token.IssuedTokenType === 'http://opcfoundation.org/UA/UserToken#JWT') ? token : undefined
         );
         token = {
            "@TypeId": OpcUa.DataTypeIds.IssuedIdentityToken,
            PolicyId: policy?.PolicyId,
            TokenData: btoa(user.accessToken ?? '')
         } as OpcUa.ExtensionObject;
      }
      const request: OpcUa.ActivateSessionRequest = {
         RequestHeader: {
            AuthenticationToken: createSessionResponse?.AuthenticationToken
         },
         LocaleIds: ["en"],
         UserIdentityToken: token
      }
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.ActivateSessionRequest,
         Body: request
      };
      console.warn("activateSession");
      sendRequest(message);
   }, [sendRequest, loginStatus, user?.accessToken]);

   /**
    * createSession: Function which is the first step in the connection process
    * 
    * The function creates a session with the server and sets the sessionState to creating
    */
   const createSession = React.useCallback(() => {
      const request: OpcUa.CreateSessionRequest = {
         ClientDescription: {
            ApplicationUri: 'urn:localhost:UA:uarestgateway.client',
            ProductUri: 'uarestgateway.client',
            ApplicationName: { Text: 'uarestgateway.client' },
            ApplicationType: OpcUa.ApplicationType.Client
         },
         EndpointUrl: window.location.href.split('?')[0],
         SessionName: 'uarestgateway.client',
         ClientNonce: undefined,
         ClientCertificate: undefined,
         RequestedSessionTimeout: 120000,
         MaxResponseMessageSize: 8 * 1014 * 1024
      };
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.CreateSessionRequest,
         Body: request
      };
      console.warn("createSession");
      sendRequest(message);
   }, [sendRequest]);

   const deleteSession = React.useCallback(() => {
      const request: OpcUa.CloseSessionRequest = {
         DeleteSubscriptions: true
      };
      const message: IRequestMessage = {
         ServiceId: OpcUa.DataTypeIds.CloseSessionRequest,
         Body: request
      };
      console.warn("deleteSession");
      sendRequest(message);
   }, [sendRequest]);

   React.useEffect(() => {
      switch (readyState) {
         case ReadyState.CONNECTING:
            m.current.sessionState = SessionState.Connecting;
            setSessionState(m.current.sessionState);
            break;
         case ReadyState.OPEN:
            m.current.sessionState = SessionState.NoSession;
            if (m.current.isSessionEnabled) {
               createSession();
               m.current.sessionState = SessionState.Creating;
               setSessionState(m.current.sessionState);
            }
            break;
         case ReadyState.CLOSED:
            if (m.current.sessionState === SessionState.SessionActive) {
               deleteSession();
            }
            m.current.sessionState = SessionState.Disconnected;
            setSessionState(m.current.sessionState);
            break;
         default:
            break;
      }
   }, [readyState, createSession, deleteSession])

   const setMessageImpl = React.useCallback((message: string) => {
      m.current.message = message;
      setMessage(m.current.message);
   }, []);

   const setServerUrlImpl = React.useCallback((value: string) => {
      m.current.serverUrl = value ?? null;
      if (m.current.isEnabled) {
         setServerUrl(m.current.serverUrl);
      }
   }, []);

   const setIsEnabledImpl = React.useCallback((value: boolean) => {
      m.current.isEnabled = value;
      setIsEnabled(m.current.isEnabled);
   }, []);

   const setIsSessionEnabledImpl = React.useCallback((value: boolean) => {
      if (m.current.isSessionEnabled === value) {
         return;
      }
      m.current.isSessionEnabled = value;
      setIsSessionEnabled(m.current.isSessionEnabled);
      if (value && m.current.sessionState === SessionState.NoSession) {
         createSession();
         return;
      }
      if (!value && m.current.sessionState === SessionState.SessionActive) {
         deleteSession();
         return;
      }
   }, [createSession, deleteSession]);

   const sessionContext = {
      serverUrl: m.current.serverUrl,
      setServerUrl: setServerUrlImpl,
      isConnected: readyState === ReadyState.OPEN,
      sessionState,
      isEnabled: m.current.isEnabled,
      setIsEnabled: setIsEnabledImpl,
      isSessionEnabled,
      setIsSessionEnabled: setIsSessionEnabledImpl,
      requestTimeout: m.current.requestTimeout,
      setRequestTimeout: (value: number) => m.current.requestTimeout = value,
      sendRequest,
      lastCompletedRequest: lastCompletedRequest,
      visibleNodes,
      setVisibleNodes,
      message,
      setMessage: setMessageImpl
   } as ISessionContext;

   /**
    * processResponse: Function which processes the response from the server
    * 
    * @param response: IResponseMessage - the response from the server
    * 
    * The function checks the ServiceId of the response and processes the response accordingly
    */
   const processResponse = React.useCallback((response: IResponseMessage) => {
      if (response?.ServiceId === OpcUa.DataTypeIds.CreateSessionResponse) {
         const csr = response.Body as OpcUa.CreateSessionResponse;
         m.current.authenticationToken = csr.AuthenticationToken;
         activateSession(csr);
      }
      else if (response?.ServiceId === OpcUa.DataTypeIds.ActivateSessionResponse) {
         const asr = response.Body as OpcUa.ActivateSessionResponse;
         m.current.serverNonce = asr.ServerNonce;
         m.current.sessionState = SessionState.SessionActive;
         setSessionState(m.current.sessionState);
      }
      else if (response?.ServiceId === OpcUa.DataTypeIds.CloseSessionResponse) {
         m.current.authenticationToken = undefined;
         m.current.serverNonce = undefined;
         m.current.sessionState = SessionState.NoSession;
         setSessionState(m.current.sessionState);
      }
      else if (response) {
         processRawResponse(response);
      }
   }, [activateSession, processRawResponse])

   React.useEffect(() => {
      if (lastMessage?.data) {
         try {
            const message = JSON.parse(lastMessage.data) as IResponseMessage;
            processResponse(message);
         }
         catch (error) {
            // do nothing.
         }
      }
   }, [lastMessage, processResponse])

   return (
      <SessionContext.Provider value={sessionContext}>
         {children}
      </SessionContext.Provider>
   );
};

export default SessionProvider;

 import * as React from 'react';
import useWebSocket, { ReadyState } from 'react-use-websocket';
import { UserContext } from './UserProvider';
import * as OpcUa from './opcua';
import { UserLoginStatus } from './user';
import { IRequestMessage } from './service/IRequestMessage';
import { IResponseMessage } from './service/IResponseMessage';
import { ICompletedRequest } from './service/ICompletedRequest';
import { SessionState } from './service/SessionState';
import { HandleFactory } from './service/HandleFactory';

const DefaultServerUrl = `wss://${location.host}/stream`;

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
   lastCompletedRequest?: ICompletedRequest
}

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
   lastCompletedRequest: undefined
});

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
   lastCompletedRequest?: ICompletedRequest
}

export const SessionProvider = ({ children }: SessionProps) => {
   const [serverUrl, setServerUrl] = React.useState<string | null>(DefaultServerUrl);
   const [isEnabled, setIsEnabled] = React.useState<boolean>(false);
   const [isSessionEnabled, setIsSessionEnabled] = React.useState<boolean>(false);
   const [sessionState, setSessionState] = React.useState<SessionState>(SessionState.Disconnected);
   const { user, loginStatus } = React.useContext(UserContext);

   const m = React.useRef<SessionInternals>({
      serverUrl: null,
      isConnected: false,
      sessionState: SessionState.Disconnected,
      isEnabled: false,
      isSessionEnabled: false,
      requestTimeout: 60000,
      requests: new Map<number, ICompletedRequest>(),
      responses: []
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

   const sendRequest = React.useCallback((request: IRequestMessage, clientHandle?: number) => {
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
         { clientHandle: clientHandle ?? requestHeader.RequestHandle, request }
      );
      sendMessage(JSON.stringify(request));
   }, [sendMessage]);

   const activateSession = React.useCallback((createSessionResponse: OpcUa.CreateSessionResponseMessage) => {
      const endpoint = createSessionResponse?.Body?.ServerEndpoints?.find((endpoint) => {
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
            TypeId: OpcUa.DataTypeIds.IssuedIdentityToken,
            Body: {
               PolicyId: policy?.PolicyId,
               TokenData: btoa(user.accessToken ?? ''),
            }
         } as OpcUa.ExtensionObject;
      }
      const request: OpcUa.ActivateSessionRequestMessage = {
         ServiceId: OpcUa.ActivateSessionRequestMessageServiceIdEnum.NUMBER_465,
         Body: {
            RequestHeader: {
               AuthenticationToken: createSessionResponse?.Body?.AuthenticationToken
            },
            LocaleIds: ["en"],
            UserIdentityToken: token
         }
      }
      sendRequest(request);
   }, [sendRequest, loginStatus, user?.accessToken]);

   const createSession = React.useCallback(() => {
      const request: OpcUa.CreateSessionRequestMessage = {
         ServiceId: OpcUa.CreateSessionRequestMessageServiceIdEnum.NUMBER_459,
         Body: {
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
         }
      };
      sendRequest(request);
   }, [sendRequest]);

   const deleteSession = React.useCallback(() => {
      const request: OpcUa.CloseSessionRequestMessage = {
         ServiceId: OpcUa.CloseSessionRequestMessageServiceIdEnum.NUMBER_471,
         Body: {
            DeleteSubscriptions: true
         }
      };
      sendRequest(request);
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
      lastCompletedRequest: m.current.lastCompletedRequest
   } as ISessionContext;

   const processResponse = React.useCallback((response: IResponseMessage) => {
      if (response?.ServiceId === OpcUa.CreateSessionResponseMessageServiceIdEnum.NUMBER_462) {
         const csr = response as OpcUa.CreateSessionResponseMessage;
         m.current.authenticationToken = csr.Body?.AuthenticationToken;
         activateSession(csr);
      }
      else if (response?.ServiceId === OpcUa.ActivateSessionResponseMessageServiceIdEnum.NUMBER_468) {
         const asr = response as OpcUa.ActivateSessionResponseMessage;
         m.current.serverNonce = asr.Body?.ServerNonce;
         m.current.sessionState = SessionState.SessionActive;
         setSessionState(m.current.sessionState);
      }
      else if (response?.ServiceId === OpcUa.CloseSessionResponseMessageServiceIdEnum.NUMBER_474) {
         m.current.authenticationToken = undefined;
         m.current.serverNonce = undefined;
         m.current.sessionState = SessionState.NoSession;
         setSessionState(m.current.sessionState);
      }
      else if (response) {
         const requestHandle = response?.Body?.ResponseHeader?.RequestHandle ?? 0;
         const request = m.current.requests.get(requestHandle);
         if (request) {
            m.current.requests.delete(requestHandle);
            m.current.lastCompletedRequest = { ...request, response };
         }
      }
   }, [activateSession])

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

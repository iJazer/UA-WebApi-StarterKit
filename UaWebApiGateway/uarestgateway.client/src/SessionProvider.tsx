 import * as React from 'react';
import useWebSocket, { ReadyState } from 'react-use-websocket';
import { UserContext } from './UserProvider';
import * as OpcUa from 'opcua-webapi';
import { UserLoginStatus } from './user';
import { IRequestMessage } from './service/IRequestMessage';
import { IResponseMessage } from './service/IResponseMessage';
import { ICompletedRequest } from './service/ICompletedRequest';
import { SessionState } from './service/SessionState';
import { HandleFactory } from './service/HandleFactory';

const DefaultServerUrl = `wss://${location.host}/stream`;
const DefaultMessage = `No Message send`;

import * as Utils from './opcua-utils';
import { IBrowsedNode } from './service/IBrowsedNode';
import { IReadValueId } from './service/IReadValueId';
import { IReadResult } from './service/IReadResult';


export interface ISessionContext {
   nodes: Map<string, IBrowsedNode>,
   lastUpdatedNode?: string,
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
   browseChildren: (parentId: string, maxAge: number) => Promise<IBrowsedNode[]>,
   visibleNodes: string[],
   setVisibleNodes: (items: string[]) => void,
   translateAndReadValues: (nodesToRead: IReadValueId[], requestTimeout: number) => Promise<IReadResult[] | null>
   //readPathsAndValues: (nodeIds: IReadValueId[]) => Promise<IBrowsedNode[]>,
   message: string,
}

export const SessionContext = React.createContext<ISessionContext>({
   nodes: new Map<string, IBrowsedNode>(),
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
   browseChildren: (): Promise<IBrowsedNode[]> => Promise.resolve([]),
   visibleNodes: [],
   setVisibleNodes: () => { },
   translateAndReadValues: async () => Promise.resolve([]),
   //readPathsAndValues: (): Promise<IBrowsedNode[]> => Promise.resolve([])
    message: DefaultMessage

});

interface SessionProps {
   children?: React.ReactNode
}

interface SessionInternals {
   nodes: Map<string, IBrowsedNode>,
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
   lastCompletedRequest?: ICompletedRequest,
   message: string | null,
}

export const SessionProvider = ({ children }: SessionProps) => {
   const [serverUrl, setServerUrl] = React.useState<string | null>(DefaultServerUrl);
   const [isEnabled, setIsEnabled] = React.useState<boolean>(false);
   const [isSessionEnabled, setIsSessionEnabled] = React.useState<boolean>(false);
   const [sessionState, setSessionState] = React.useState<SessionState>(SessionState.Disconnected);
   const { user, loginStatus } = React.useContext(UserContext);
   const [lastUpdatedNode, setLastUpdatedNode] = React.useState<string>();
   const [visibleNodes, setVisibleNodes] = React.useState<string[]>([]);
   const [message, setMessage] = React.useState<string>("");


   const m = React.useRef<SessionInternals>({
      nodes: new Map<string, IBrowsedNode>(),
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

      //if webSocket is setup --> SendMessage
      //if not use REST
      //if REST create a queue with REST requests
      // Update network message state
      
      setMessage(JSON.stringify(request));
      sendMessage(JSON.stringify(request));
   }, [sendMessage]);

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
      sendRequest(message);
   }, [sendRequest, loginStatus, user?.accessToken]);

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


    /////----------------------------------------Browse and Read----------------------------------------/////
    const browseChildren = React.useCallback(async (parentId: string, maxAge: number): Promise<IBrowsedNode[]> => {
        const cache = Array.from(m.current.nodes.values());
        const node = cache.find((node) => node.id === parentId);
        if (maxAge && node && node.lastUpdated && (new Date().getTime() - node.lastUpdated.getTime()) < maxAge) {
            return cache.filter((node) => node.parentId === parentId);
        }
        if (m.current.sessionState === SessionState.SessionActive) {
            // Construct the BrowseRequest
            const request: OpcUa.BrowseRequest = {
                RequestHeader: {
                    Timestamp: new Date(),
                    TimeoutHint: m.current.requestTimeout,
                    AuthenticationToken: m.current.authenticationToken
                },
                NodesToBrowse: [
                    {
                        NodeId: parentId,
                        BrowseDirection: OpcUa.BrowseDirection.Forward,
                        ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
                        IncludeSubtypes: true,
                        NodeClassMask: 0,
                        ResultMask: 63
                    }
                ]
            };

            const message: IRequestMessage = {
                ServiceId: OpcUa.DataTypeIds.BrowseRequest,
                Body: request
            };

            // Send the request using sendRequest
            sendRequest(message);

            // Wait for the response and process it
            return new Promise<IBrowsedNode[]>((resolve) => {
                const interval = setInterval(() => {
                    const response = m.current.lastCompletedRequest?.response;
                    if (response && response.ServiceId === OpcUa.DataTypeIds.BrowseResponse) {
                        clearInterval(interval);
                        const browseResponse = response.Body as OpcUa.BrowseResponse;
                        const children = browseResponse.Results?.[0]?.References?.map((reference) => ({
                            id: reference.NodeId ?? '',
                            reference,
                            parentId,
                            lastUpdated: new Date()
                        })) ?? [];
                        children.forEach((child) => {
                            if (child.id) {
                                m.current.nodes.set(child.id, child);
                                setLastUpdatedNode(child.id);
                            }
                        });
                        resolve(children.filter(child => child.id));
                    }
                }, 100);
            });

        } else {
            const cache = Array.from(m.current.nodes.values());
            const node = cache.find((node) => node.id === parentId);
            if (maxAge && node && node.lastUpdated && (new Date().getTime() - node.lastUpdated.getTime()) < maxAge) {
                return cache.filter((node) => node.parentId === parentId);
            }
            const children = await Utils.browseChildren(parentId, m.current.requestTimeout, user);
            if (!children) {
                return [];
            }
            const oldChildren = cache.filter((node) => node.parentId === parentId);
            oldChildren.forEach((child) => m.current.nodes.delete(child.id));
            children.forEach((child) => {
                const existing = oldChildren.find((node) => node.id === child.id);
                if (existing) {
                    m.current.nodes.set(child.id, { ...existing, reference: child.reference, parentId, lastUpdated: new Date() });
                }
                else {
                    child.parentId = parentId;
                    child.lastUpdated = new Date();
                    m.current.nodes.set(child.id, child);
                }
                setLastUpdatedNode(child.id);
            });
            return Array.from(m.current.nodes.values()).filter((node) => node.parentId === parentId);
        }
    }, [sendRequest, user, m.current.requestTimeout, m.current.authenticationToken]);

    const translateAndReadValues = React.useCallback(async (
        nodesToRead: IReadValueId[],
        requestTimeout: number,
    ): Promise<IReadResult[] | null> => {
        //const controller: AbortController = new AbortController();
        const browsePaths: OpcUa.BrowsePath[] = [];
        const nodesToTranslate: IReadValueId[] = [];
        const values: IReadResult[] = [];

        if (m.current.sessionState !== SessionState.SessionActive) {
            // Call the utility function if no session is established
            return Utils.translateAndReadValues(nodesToRead, 60000, user);
        }

        nodesToRead.forEach((item) => {
            values.push({
                id: item.id ?? values.length,
                nodeId: item.resolvedNodeId ?? item.nodeId,
                attributeId: item.attributeId ?? OpcUa.Attributes.Value,
                value: { StatusCode: { Code: OpcUa.StatusCodes.BadNodeIdUnknown } }
            } as IReadResult); 
        });


        
        const request: OpcUa.TranslateBrowsePathsToNodeIdsRequest = {
            RequestHeader: {
                Timestamp: new Date(),
                TimeoutHint: requestTimeout
            },
            BrowsePaths: browsePaths
        };
        

        const message: IRequestMessage = {
            ServiceId: OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsRequest,
            Body: request
        };

        sendRequest(message);

        await sendRequest(message);

        const interval = setInterval(() => {
            const response = m.current.lastCompletedRequest?.response;
            if (response && response.ServiceId === OpcUa.DataTypeIds.TranslateBrowsePathsToNodeIdsResponse) {
                clearInterval(interval);
                const body: OpcUa.TranslateBrowsePathsToNodeIdsResponse = response.Body as OpcUa.TranslateBrowsePathsToNodeIdsResponse;
                body.Results?.forEach((item, index) => {
                    if (nodesToTranslate[index] && !item.StatusCode && item?.Targets?.at(0)?.RemainingPathIndex === 4294967295) {
                        nodesToTranslate[index].resolvedNodeId = item.Targets[0].TargetId;
                    }
                });
            }
        }, 100);

        const valuesToRead: OpcUa.ReadValueId[] = [];
        const subsetOfResults: (IReadResult | undefined)[] = [];
        nodesToRead.forEach((item, index) => {
            if (!item.resolvedNodeId) {
                return;
            }
            valuesToRead.push({
                NodeId: item.resolvedNodeId,
                AttributeId: item.attributeId ?? OpcUa.Attributes.Value
            });
            item.id = valuesToRead.length;
            subsetOfResults.push(values.at(index));
        });
        
        const read_request: OpcUa.ReadRequest = {
            RequestHeader: {
                Timestamp: new Date(),
                TimeoutHint: requestTimeout
            },
            MaxAge: 0,
            NodesToRead: valuesToRead
        };
        const read_message: IRequestMessage = {
            ServiceId: OpcUa.DataTypeIds.ReadRequest,
            Body: read_request
        };
        sendRequest(read_message);

        return new Promise<IReadResult[]>((resolve) => {
            const interval = setInterval(() => {
                const response = m.current.lastCompletedRequest?.response;
                if (response && response.ServiceId === OpcUa.DataTypeIds.ReadResponse) {
                    clearInterval(interval);
                    const body: OpcUa.ReadResponse = response.Body as OpcUa.ReadResponse;
                    const values: IReadResult[] = [];
                    if (body.Results && read_request.NodesToRead) {
                        for (let ii = 0; ii < body.Results.length; ii++) {
                            const x = body.Results[ii];
                            if (x.StatusCode?.Code !== OpcUa.StatusCodes.BadAttributeIdInvalid) {
                                if (read_request.NodesToRead[ii] && read_request.NodesToRead[ii].NodeId) {
                                    values.push({
                                        id: HandleFactory.increment(),
                                        nodeId: read_request.NodesToRead[ii].NodeId as string, // Ensure nodeId is a string
                                        attributeId: read_request.NodesToRead[ii].AttributeId ?? 0,
                                        value: { StatusCode: { Code: OpcUa.StatusCodes.BadNodeIdUnknown } }
                                    });
                                } else {
                                    // Handle the case where NodeId is undefined
                                    console.error(`NodeId is undefined for index ${ii}`);
                                }
                            }
                        }
                    }
                    resolve(values);
                }
            }, 100);
        });
        
    }, [sendRequest]);


    const sessionContext = {
      nodes: m.current.nodes,
      lastUpdatedNode,
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
      lastCompletedRequest: m.current.lastCompletedRequest,
      browseChildren,
      visibleNodes,
      setVisibleNodes,
      translateAndReadValues,
      message,
      setMessage: setMessageImpl
   } as ISessionContext;

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

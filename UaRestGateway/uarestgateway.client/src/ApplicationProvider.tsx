import * as React from 'react';
import * as OpcUa from './opcua';
import { IUserContext, UserContext } from './UserProvider';
import { IMonitoredItem, ISession, SessionState, connect, publish } from './opcua-utils';

export interface IBrowseTreeNode {
   id: string,
   reference: OpcUa.ReferenceDescription;
   value?: string;
   children: IBrowseTreeNode[];
}

export interface IApplicationContext {
   userContext: IUserContext,
   serverId?: string,
   requestTimeout: number,
   session?: ISession,
   connect: () => void,
   disconnect: () => void,
   visibleMonitoredItems: string[],
   setVisibleMonitoredItems: (items: string[]) => void,
   visibleNodes: string[],
   setVisibleNodes: (items: string[]) => void,
   nodes: Map<string, IBrowseTreeNode>
   updateNodes: (oldNodes?: IBrowseTreeNode[], newNodes?: IBrowseTreeNode[]) => void,
   monitoredItems?: Map<string, IMonitoredItem>
}

export const ApplicationContext = React.createContext<IApplicationContext>({
   userContext: {} as IUserContext,
   serverId: undefined,
   requestTimeout: 120000,
   session: undefined,
   connect: () => { },
   disconnect: () => { },
   visibleMonitoredItems: [],
   setVisibleMonitoredItems: () => { },
   visibleNodes: [],
   setVisibleNodes: () => { },
   nodes: new Map<string, IBrowseTreeNode>(),
   updateNodes: () => { },
   monitoredItems: new Map<string, IMonitoredItem>()
});

function useInterval(session: ISession, callback: () => void, delay: number) {
   React.useEffect(() => {
      const intervalId = setInterval(callback, delay);
      return () => clearInterval(intervalId);
   }, [callback, delay]);
}

interface ApplicationProviderProps {
   children?: React.ReactNode
}

export const ApplicationProvider = ({ children }: ApplicationProviderProps) => {
   const [session, setSession] = React.useState<ISession>({ state: SessionState.Closed });
   const [visibleMonitoredItems, setVisibleMonitoredItems] = React.useState<string[]>([]);
   const [triggerPublish, setTriggerPublish] = React.useState<boolean>(false);
   const [triggerConnect, setTriggerConnect] = React.useState<boolean>(false);
   const [publishCount, setPublishCount] = React.useState<number>(0);
   const [visibleNodes, setVisibleNodes] = React.useState<string[]>([]);
   const [nodes, setNodes] = React.useState<Map<string, IBrowseTreeNode>>(new Map<string, IBrowseTreeNode>());
   const userContext = React.useContext(UserContext);
   const defaultRequestTimeout = 120000;
   const defaultPublishInterval = 1000;

   React.useEffect(() => {
      setTriggerConnect(current => {
         if (session.state === SessionState.Opening) {
            return true;
         }
         return current;
      });
   }, [session.state]);

   React.useEffect(() => {
      let controller: AbortController | undefined = undefined;
      if (triggerConnect) {
         doConnect();
      }
      async function doConnect() {
         controller = new AbortController();
         setSession(oldSession => {
            connect(userContext, oldSession, defaultRequestTimeout, controller).then(newSession => {
               if (newSession) {
                  setSession(newSession);
               }
            });
            return oldSession;
         });
         setTriggerConnect(false);
      }
      return () => {
        // if (controller) controller.abort();
      }
   }, [triggerConnect, userContext]);

   React.useEffect(() => {
      setTriggerPublish(current => {
         if (session.state === SessionState.Open) {
            return true;
         }
         return current;
      });
   }, [publishCount, session.state]);

   React.useEffect(() => {
      let controller: AbortController | undefined = undefined;
      if (triggerPublish) {
         doPublish();
      }
      async function doPublish() {
         controller = new AbortController();
         publish(userContext?.user, session, defaultRequestTimeout, controller)
            .then(newSession => {
               if (newSession) {
                  setSession(newSession);
               }
            })
            .finally(() => {
               setTriggerPublish(false);
               setPublishCount(current => current - 1);
            });
      }
      return () => {
         // if (controller) controller.abort();
      }
   }, [triggerPublish, session, userContext?.user]);

   useInterval(session, () => setPublishCount(current => current + 1), defaultPublishInterval);

   const context = {
      userContext: userContext,
      serverId: undefined,
      requestTimeout: defaultRequestTimeout,
      session: session,
      connect: () => setSession(oldSession => {
         if (oldSession.state === SessionState.Closed) {
            return { ...oldSession, state: SessionState.Opening };
         }
         return oldSession;
      }),
      disconnect: () => { },
      visibleMonitoredItems: visibleMonitoredItems,
      setVisibleMonitoredItems: (items: string[]) => setVisibleMonitoredItems(items),
      visibleNodes: visibleNodes,
      setVisibleNodes: (items: string[]) => setVisibleNodes(items),
      nodes,
      updateNodes: (oldNodes?: IBrowseTreeNode[], newNodes?: IBrowseTreeNode[]) => {
         setNodes((existing: Map<string, IBrowseTreeNode>) => {
            if (!oldNodes?.length && !newNodes?.length) return existing;
            const updated = new Map(existing);
            if (oldNodes) {
               const nodesToDelete = (newNodes) ? oldNodes.map(x => newNodes?.find(y => y.id === x.id)) : oldNodes;
               nodesToDelete.forEach(x => { if (x) updated.delete(x.id); });
            }
            if (newNodes) {
               const nodesToAdd = (oldNodes) ? newNodes.map(x => oldNodes?.find(y => y.id === x.id)) : newNodes;
               nodesToAdd.forEach(x => { if (x) updated.set(x.id, x); });
            }
            return updated;
         });
      }
   } as IApplicationContext;

   return (
      <ApplicationContext.Provider value={context}>
         {children}
      </ApplicationContext.Provider>
   );
};

export default ApplicationProvider;
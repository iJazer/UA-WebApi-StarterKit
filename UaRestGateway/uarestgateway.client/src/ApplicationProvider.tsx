import * as React from 'react';
import * as OpcUa from './opcua';
import { IUserContext, UserContext } from './UserProvider';
import { HandleFactory, IMonitoredItem, ISession, SessionState, connect, publish, translateAndSubscribe } from './opcua-utils';

export interface IBrowseTreeNode {
   id: string,
   reference: OpcUa.ReferenceDescription;
   value?: string;
   parentId?: string;
}

export interface IApplicationContext {
   userContext: IUserContext,
   serverId?: string,
   requestTimeout: number,
   session?: ISession,
   connect: () => Promise<void>,
   disconnect: () => Promise<void>,
   visibleMonitoredItems: string[],
   setVisibleMonitoredItems: (items: string[]) => void,
   visibleNodes: string[],
   setVisibleNodes: (items: string[]) => void,
   nodes: Map<string, IBrowseTreeNode>
   updateNodes: (parentId? : string, oldNodes?: IBrowseTreeNode[], newNodes?: IBrowseTreeNode[]) => void,
   monitoredItems?: Map<number, IMonitoredItem>,
   createMonitoredItems: (monitoredItem: IMonitoredItem[], controller: AbortController) => Promise<void>
}

export const ApplicationContext = React.createContext<IApplicationContext>({
   userContext: {} as IUserContext,
   serverId: undefined,
   requestTimeout: 120000,
   session: undefined,
   connect: async () => { },
   disconnect: async () => { },
   visibleMonitoredItems: [],
   setVisibleMonitoredItems: () => { },
   visibleNodes: [],
   setVisibleNodes: () => { },
   nodes: new Map<string, IBrowseTreeNode>(),
   updateNodes: () => { },
   monitoredItems: new Map<number, IMonitoredItem>(),
   createMonitoredItems: async () => { }
});

interface ApplicationProviderProps {
   children?: React.ReactNode
}

export const ApplicationProvider = ({ children }: ApplicationProviderProps) => {
   const [session, setSession] = React.useState<ISession>({ state: SessionState.Disconnected });
   const [monitoredItems, setMonitoredItems] = React.useState<Map<number, IMonitoredItem>>(new Map<number, IMonitoredItem>());
   const [visibleMonitoredItems, setVisibleMonitoredItems] = React.useState<string[]>([]);
   const [triggerPublish, setTriggerPublish] = React.useState<boolean>(false);
   const [triggerConnect, setTriggerConnect] = React.useState<boolean>(false);
   const [visibleNodes, setVisibleNodes] = React.useState<string[]>([]);
   const [nodes, setNodes] = React.useState<Map<string, IBrowseTreeNode>>(new Map<string, IBrowseTreeNode>());
   const userContext = React.useContext(UserContext);
   const defaultRequestTimeout = 120000;
   const defaultPublishInterval = 2000;

   React.useEffect(() => {
      setTriggerConnect(current => {
         if (session.state === SessionState.Opening) {
            return true;
         }
         if (session.state === SessionState.Disconnected) {
            setSession({ state: SessionState.Disconnected });
            setNodes(new Map<string, IBrowseTreeNode>());
            setMonitoredItems(new Map<number, IMonitoredItem>());
            return false;
         }
         return current;
      });
   }, [session.state]);

   React.useEffect(() => {
      let interval: NodeJS.Timeout | undefined;
      if (session.state === SessionState.Open) {
         interval = setInterval(() => setTriggerPublish(true), defaultPublishInterval);
      } else if (interval) {
         clearInterval(interval);
      }
      return () => clearInterval(interval);
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
      let controller: AbortController | undefined = undefined;
      if (triggerPublish) {
         doPublish();
      }
      async function doPublish() {
         controller = new AbortController();
         publish(userContext?.user, session, monitoredItems, defaultRequestTimeout, controller)
            .then(result => {
               if (result) {
                  if (result.session) {
                     setSession(result.session);
                  }
                  if (result.monitoredItems) {
                     setMonitoredItems(existing => {
                        const updated = new Map<number, IMonitoredItem>(existing);
                        result.monitoredItems.forEach(ii => {
                           const item = updated.get(ii.clientHandle);
                           if (item) {
                              item.value = ii.value ?? {};
                           }
                        });
                        return updated;
                     });
                  }
               }
            })
            .finally(() => {
               setTriggerPublish(false);
            });
      }
      return () => {
         // if (controller) controller.abort();
      }
   }, [triggerPublish, session, userContext?.user, monitoredItems]);

   const context = {
      userContext: userContext,
      serverId: undefined,
      requestTimeout: defaultRequestTimeout,
      session: session,
      connect: async () => setSession(oldSession => {
         if (oldSession.state === SessionState.Disconnected) {
            return { ...oldSession, state: SessionState.Opening };
         }
         return oldSession;
      }),
      disconnect: async () => { },
      visibleMonitoredItems: visibleMonitoredItems,
      setVisibleMonitoredItems: (items: string[]) => setVisibleMonitoredItems(items),
      visibleNodes: visibleNodes,
      setVisibleNodes: (items: string[]) => setVisibleNodes(items),
      nodes,
      updateNodes: (parentId?: string, oldNodes?: IBrowseTreeNode[], newNodes?: IBrowseTreeNode[]) => {
         setNodes((existing: Map<string, IBrowseTreeNode>) => {
            if (!oldNodes?.length && !newNodes?.length) return existing;
            const updated = new Map(existing);
            if (oldNodes) {
               const nodesToDelete = (newNodes) ? oldNodes.map(x => newNodes?.find(y => y.id === x.id)) : oldNodes;
               nodesToDelete.forEach(x => { if (x) updated.delete(x.id); });
            }
            if (newNodes) {
               const nodesToAdd = (oldNodes) ? newNodes.map(x => oldNodes?.find(y => y.id === x.id)) : newNodes;
               nodesToAdd.forEach(x => {
                  if (x) {
                     updated.set(x.id, { ...x, parentId: parentId });
                  }
               });
            }
            return updated;
         });
      },
      monitoredItems: monitoredItems,
      createMonitoredItems: async (newMonitoredItems: IMonitoredItem[], controller: AbortController) => {
         setMonitoredItems((existing: Map<number, IMonitoredItem>) => {
            const updated = new Map<number, IMonitoredItem>(existing);
            newMonitoredItems.forEach(ii => {
               ii.clientHandle = HandleFactory.increment();
               updated.set(ii.clientHandle, ii);
            });
            return updated;
         });
         const results = await translateAndSubscribe(userContext.user, session, defaultRequestTimeout, newMonitoredItems, controller);
         if (results) {
            setMonitoredItems((existing: Map<number, IMonitoredItem>) => {
               const updated = new Map<number, IMonitoredItem>(existing);
               results.forEach(ii => {
                  const item = updated.get(ii.clientHandle);
                  if (item) {
                     item.serverHandle = ii.serverHandle;
                  }
               });
               // Array.from(updated.values()).forEach(ii => { if (!ii.serverHandle) updated.delete(ii.clientHandle); });
               return updated;
            });
         }
      },
   } as IApplicationContext;

   return (
      <ApplicationContext.Provider value={context}>
         {children}
      </ApplicationContext.Provider>
   );
};

export default ApplicationProvider;
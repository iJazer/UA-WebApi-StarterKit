import * as React from 'react';
import { UserContext } from './UserProvider';
import * as Utils from './opcua-utils';
import { IBrowsedNode } from './service/IBrowsedNode';
import { IReadValueId } from './service/IReadValueId';

export interface IApplicationContext {
   nodes: Map<string, IBrowsedNode>,
   lastUpdatedNode?: string,
   visibleNodes: string[],
   setVisibleNodes: (items: string[]) => void,
   requestTimeout: number,
   setRequestTimeout: (requestTimeout: number) => void,
   browseChildren: (parentId: string, maxAge: number) => Promise<IBrowsedNode[]>,
   readValues: (nodeIds: string[]) => Promise<IBrowsedNode[]>,
   readPathsAndValues: (nodeIds: IReadValueId[]) => Promise<IBrowsedNode[]>
}

export const ApplicationContext = React.createContext<IApplicationContext>({
   nodes: new Map<string, IBrowsedNode>(),
   visibleNodes: [],
   setVisibleNodes: () => { },
   requestTimeout: 120000,
   setRequestTimeout: () => { },
   browseChildren: (): Promise<IBrowsedNode[]> => Promise.resolve([]),
   readValues: (): Promise<IBrowsedNode[]> => Promise.resolve([]),
   readPathsAndValues: (): Promise<IBrowsedNode[]> => Promise.resolve([])
});

interface ApplicationProviderProps {
   children?: React.ReactNode
}

interface ApplicationInternals {
   nodes: Map<string, IBrowsedNode>,
   requestTimeout: number
}

export const ApplicationProvider = ({ children }: ApplicationProviderProps) => {
   const [visibleNodes, setVisibleNodes] = React.useState<string[]>([]);
   const [lastUpdatedNode, setLastUpdatedNode] = React.useState<string>();
   const [requestTimeout, setRequestTimeout] = React.useState<number>(60000);
   const { user } = React.useContext(UserContext);

   const m = React.useRef<ApplicationInternals>({
      nodes: new Map<string, IBrowsedNode>(),
      requestTimeout: 60000
   });

   const browseChildren = React.useCallback(async (parentId: string, maxAge: number): Promise<IBrowsedNode[]> => {
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
   }, [user]);

   const readValues = React.useCallback(async (nodeIds: string[]): Promise<IBrowsedNode[]> => {
      const values = await Utils.readValues(nodeIds, m.current.requestTimeout, user);
      if (!values) {
         return [];
      }
      const results: IBrowsedNode[] = [];
      for (let ii = 0; ii < values.length; ii++) {
         const value = values[ii];
         const node = m.current.nodes.get(value.nodeId);
         if (node) {
            node.value = value.value;
            setLastUpdatedNode(value.nodeId);
            results.push(node);
         }
         else {
            results.push({ id: value.nodeId, reference: { NodeId: value.nodeId }, value: value.value });
         }
      }
      return results;
   }, [user]);

   const context = {
      nodes: m.current.nodes,
      lastUpdatedNode,
      visibleNodes,
      setVisibleNodes,
      requestTimeout,
      setRequestTimeout,
      browseChildren,
      readValues
   } as IApplicationContext;

   return (
      <ApplicationContext.Provider value={context}>
         {children}
      </ApplicationContext.Provider>
   );
};

export default ApplicationProvider;
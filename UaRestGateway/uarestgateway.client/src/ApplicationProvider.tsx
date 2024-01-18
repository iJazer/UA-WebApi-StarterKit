import * as React from 'react';
import * as OpcUa from './opcua';
import { IUserContext, UserContext } from './UserProvider';

export interface ITreeNode {
   id: number,
   reference: OpcUa.ReferenceDescription;
   value?: string;
   children: ITreeNode[];
}

export interface IApplicationContext {
   nodes: Map<number, ITreeNode>
   update: (oldNodes?: ITreeNode[], newNodes?: ITreeNode[]) => void
   userContext: IUserContext
}

export const ApplicationContext = React.createContext<IApplicationContext>({
   nodes: new Map(),
   update: () => { },
   userContext: {} as IUserContext
});

interface ApplicationProviderProps {
   children?: React.ReactNode
}

export const ApplicationProvider = ({ children }: ApplicationProviderProps) => {
   const [nodes, setNodes] = React.useState<Map<number, ITreeNode>>(new Map<number, ITreeNode>());
   const userContext = React.useContext(UserContext);
   const name = userContext?.user?.name;

   React.useEffect(() => {
      console.log(`ApplicationProvider: ${name}`);
   }, [name]);

   const context = {
      nodes,
      userContext: userContext,
      update: (oldNodes?: ITreeNode[], newNodes?: ITreeNode[]) => {
         setNodes(existing => {
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
import React from 'react';

import { NodeIcon } from './NodeIcon';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';

import { ApplicationContext, ITreeNode } from '../ApplicationProvider';
import { call, HandleFactory } from '../opcua-utils';
import * as OpcUa from '../opcua';
import { Account } from '../api';

async function browseChildren(
   nodeId: string,
   serverId?: string,
   requestTimeout: number = 120000,
   controller?: AbortController,
   user?: Account
): Promise<ITreeNode[] | undefined> {
   const request: OpcUa.BrowseRequestMessage = {
      Body: {
         RequestHeader: {
            Timestamp: new Date(),
            TimeoutHint: requestTimeout
         },
         RequestedMaxReferencesPerNode: 20,
         NodesToBrowse: [
            {
               NodeId: nodeId,
               BrowseDirection: OpcUa.BrowseDirection.Forward,
               ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
               IncludeSubtypes: true,
               ResultMask: 63
            }
         ]
      }
   };
   const response = await call(`/opcua/browse${(serverId) ? `?serverId=${serverId}` : ''}`, request, controller, user);
   if (!response) {
      return undefined;
   }
   if (response.code) {
      console.warn(`Browse call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
      return undefined;
   }
   const nodes: ITreeNode[] = [];
   let continuationPoints: string[] = [];
   let result: OpcUa.BrowseResponse = response.Body;

   do {
      continuationPoints = [];
      result.Results?.forEach(x => x.References?.forEach((y) => {
         if (OpcUa.StatusCode.isGood(x.StatusCode)) {
            nodes.push({
               id: HandleFactory.increment(),
               reference: y,
               children: []
            });
            if (x.ContinuationPoint) {
               continuationPoints.push(x.ContinuationPoint);
            }
         }
      }));
      if (continuationPoints.length > 0) {
         const request: OpcUa.BrowseNextRequestMessage = {
            Body: {
               RequestHeader: {
                  Timestamp: new Date(),
                  TimeoutHint: requestTimeout
               },
               ReleaseContinuationPoints: false,
               ContinuationPoints: continuationPoints
            }
         };
         const response = await call(`/opcua/browsenext${(serverId) ? `?serverId=${serverId}` : ''}`, request, controller, user);
         if (!response) {
            return undefined;
         }
         if (response.code) {
            console.warn(`BrowseNext call failed: [${OpcUa.StatusCodes[response.code] ?? response.code}] '${response.message ?? ''}'`);
            return undefined;
         }
         result = response.Body;
      }
   } while (continuationPoints.length > 0);

   console.info(`browse(${nodeId}) = ${nodes?.length}`);
   return nodes;
}

export interface TreeNodeViewProps {
   serverId?: string
   parentId?: string
   requestTimeout?: number,
   selectionId?: number,
   onChildrenUpdated?: (oldNodes: ITreeNode[], newNodes: ITreeNode[]) => void
}

type TreeNodeViewState = {
   nodes?: ITreeNode[];
   oldNodes?: ITreeNode[];
};

type TreeNodeViewAction = {
   type: 'update';
   newNodes?: ITreeNode[];
};

function treeViewReducer(state: TreeNodeViewState, action: TreeNodeViewAction): TreeNodeViewState {
   switch (action.type) {
      case 'update':
         return { oldNodes: state.nodes, nodes: action.newNodes };
      default:
         throw new Error();
   }
}

export const TreeNodeView = ({ serverId, parentId, requestTimeout, selectionId }: TreeNodeViewProps) => {
   const [state, dispatch] = React.useReducer(treeViewReducer, {});
   const [user, setUser] = React.useState<Account | undefined>();
   const context = React.useRef(React.useContext(ApplicationContext));

   React.useEffect(() => {
      const controller = new AbortController();
      if (parentId) {
         browseChildren(parentId, serverId, requestTimeout, controller, user).then((x) => {
            if (x) dispatch({ type: 'update', newNodes: x });
         });
      }
      return () => {
         controller.abort();
      };
   }, [parentId, serverId, requestTimeout, user]);

   React.useEffect(() => {
      context?.current?.update(state.oldNodes, state.nodes);
   }, [state, context]);

   React.useEffect(() => {
      setUser(context?.current?.userContext?.user);
   }, [context?.current?.userContext]);

   return (
      <React.Fragment>
         {state.nodes?.map((node) => {
            return (
               <TreeItem
                  key={node.id}
                  nodeId={`${node.id}`}
                  label={node?.reference?.DisplayName?.Text ?? node?.reference?.BrowseName}
                  icon={<NodeIcon nodeClass={node?.reference?.NodeClass} typeDefinitionId={node?.reference?.TypeDefinition} />}
               >
                  <TreeNodeView
                     serverId={serverId}
                     parentId={node.reference.NodeId}
                     requestTimeout={requestTimeout}
                     selectionId={selectionId}
                  />
               </TreeItem>)
         })}
      </React.Fragment>);
};

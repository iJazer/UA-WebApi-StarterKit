import React from 'react';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';

import * as OpcUa from '../opcua';
import { IBrowseTreeNode, ApplicationContext } from '../ApplicationProvider';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import { NodeIcon } from './NodeIcon';
import { browseChildren } from '../opcua-utils';

export interface BrowseTreeNodeProps {
   parentId?: string
   requestTimeout?: number,
   selectionId?: string,
   onChildrenUpdated?: (oldNodes: IBrowseTreeNode[], newNodes: IBrowseTreeNode[]) => void
}

type BrowseTreeNodeState = {
   parentId?: string;
   nodes?: IBrowseTreeNode[];
   oldNodes?: IBrowseTreeNode[];
   dirty: boolean;
};

type BrowseTreeNodeAction = {
   type: 'update' | 'clear';
   parentId?: string;
   newNodes?: IBrowseTreeNode[];
};

function treeViewReducer(state: BrowseTreeNodeState, action: BrowseTreeNodeAction): BrowseTreeNodeState {
   switch (action.type) {
      case 'clear':
         return { parentId: state.parentId, oldNodes: state.oldNodes, nodes: state.nodes, dirty: false };
      case 'update':
         return { oldNodes: state.nodes, parentId: action.parentId, nodes: action.newNodes, dirty: true };
      default:
         throw new Error();
   }
}

export const BrowseTreeNode = ({ parentId, requestTimeout, selectionId }: BrowseTreeNodeProps) => {
   const [state, dispatch] = React.useReducer(treeViewReducer, { dirty: false });
   const context = React.useContext(ApplicationContext);
   const controller = React.useRef(new AbortController());

   React.useEffect(() => {
      const current = controller.current;
      return () => {
         current.abort();
      };
   }, []);

   React.useEffect(() => {
      if (parentId) {
         browseChildren(parentId, requestTimeout, controller, context?.userContext?.user).then((x) => {
            if (x) dispatch({ type: 'update', parentId: parentId, newNodes: x });
         });
      }
   }, [parentId, requestTimeout, context?.userContext?.user]);

   React.useEffect(() => {
      if (state.dirty && context?.updateNodes) {
         context?.updateNodes(state.parentId, state.oldNodes, state.nodes);
         dispatch({ type: 'clear' });
      }
   }, [state, context]);

   return (
      <React.Fragment>
         {state.nodes?.map((node) => {
            return (
               <TreeItem
                  key={node.id}
                  nodeId={`${node.reference.NodeId}`}
                  label={node?.reference?.DisplayName?.Text ?? node?.reference?.BrowseName}
                  icon={<NodeIcon nodeClass={node?.reference?.NodeClass} typeDefinitionId={node?.reference?.TypeDefinition} />}
               >
                  <BrowseTreeNode
                     parentId={node.reference.NodeId}
                     requestTimeout={requestTimeout}
                     selectionId={selectionId}
                  />
               </TreeItem>)
         })}
      </React.Fragment>);
};

interface BrowseTreeViewProps {
   rootNodeId?: string
   requestTimeout?: number
   onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

export const BrowseTreeView = ({ rootNodeId, requestTimeout, onSelectionChanged }: BrowseTreeViewProps) => {
   const [selectionId, setSelectionId] = React.useState<string | undefined>();
   const context = React.useContext(ApplicationContext);

   const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
      const treeNode: IBrowseTreeNode | undefined = context.nodes.get(nodeId);
      if (treeNode && onSelectionChanged) {
         onSelectionChanged(treeNode.reference);
      }
      setSelectionId(treeNode?.id);
   }, [onSelectionChanged, context]);

   const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
      context.setVisibleNodes(nodeIds);
   }

   return (
      <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
         <TreeView
            defaultCollapseIcon={<ExpandMoreIcon />}
            defaultExpandIcon={<ChevronRightIcon />}
            expanded={context.visibleNodes}
            onNodeSelect={(e: React.SyntheticEvent, nodeId: string) => handleNodeSelect(e, nodeId)}
            onNodeToggle={(e: React.SyntheticEvent, nodeIds: string[]) => handleToggle(e, nodeIds)}
         >
            <BrowseTreeNode
               parentId={rootNodeId}
               requestTimeout={requestTimeout}
               selectionId={selectionId}
            />
         </TreeView>
      </Paper>
   );
}

export default BrowseTreeView;
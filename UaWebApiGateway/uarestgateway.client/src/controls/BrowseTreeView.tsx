/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';

import * as OpcUa from 'opcua-webapi';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import { NodeIcon } from './NodeIcon';
import { IBrowsedNode } from '../service/IBrowsedNode';

import { BrowseContext } from '../BrowseContext';
import { HandleFactory } from '../service/HandleFactory';
import { SessionContext } from '../SessionContext';

interface BrowseTreeNodeProps {
   parentId?: string
   selectionId?: string,
   onChildrenUpdated?: (oldNodes: IBrowsedNode[], newNodes: IBrowsedNode[]) => void,
   onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

interface BrowseTreeNodeInternals {
   requestId: number
}

const BrowseTreeNode = ({ parentId, selectionId, onSelectionChanged }: BrowseTreeNodeProps) => {
   const [children, setChildren] = React.useState<IBrowsedNode[]>([]);
   const { browseChildren, processResults, responseCount } = React.useContext(BrowseContext);
   const { isConnected } = React.useContext(SessionContext);

   const m = React.useRef<BrowseTreeNodeInternals>({
      requestId: HandleFactory.increment()
   });

   React.useEffect(() => {
      if (browseChildren && parentId && selectionId === parentId) {
         browseChildren(m.current.requestId, parentId);
      }
   }, [browseChildren, parentId, selectionId, isConnected]);

   React.useEffect(() => {
      const results = processResults((result) => {
         return m.current.requestId && m.current.requestId === result.callerHandle ? true : false;
      });
      if (results) {
         results?.forEach(result => {
            setChildren(result.children ?? []);
         });
      }
   }, [processResults, responseCount, setChildren]);

   return (
      <React.Fragment>
         {children?.map((node) => {
            if (!node?.reference?.NodeId) {
               return null;
            }
            return (
               <TreeItem
                  key={node.id}
                  nodeId={`${node.reference.NodeId}`}
                  label={node?.reference?.DisplayName?.Text ?? node?.reference?.BrowseName}
                  icon={<NodeIcon nodeClass={node?.reference?.NodeClass} typeDefinitionId={node?.reference?.TypeDefinition} />}
               >
                  <BrowseTreeNode
                     parentId={node.reference.NodeId}
                     selectionId={selectionId}
                     onSelectionChanged={onSelectionChanged}
                  />
               </TreeItem>)
         })}
      </React.Fragment>);
};

interface BrowseTreeViewProps {
   rootNodeId?: string
   onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

const BrowseTreeRoot = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
   const [selectionId, setSelectionId] = React.useState<string | undefined>();
   const { visibleNodes, setVisibleNodes, nodes } = React.useContext(BrowseContext);

   const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
      const treeNode: IBrowsedNode | undefined = nodes.get(nodeId);
      if (treeNode && onSelectionChanged) {
         onSelectionChanged(treeNode.reference);
      }
      setSelectionId(treeNode?.id);
   }, [onSelectionChanged, nodes]);

   const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
      setVisibleNodes(nodeIds);
   }

   return (
      <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
         <TreeView
            defaultCollapseIcon={<ExpandMoreIcon />}
            defaultExpandIcon={<ChevronRightIcon />}
            expanded={visibleNodes}
            onNodeSelect={(e: React.SyntheticEvent, nodeId: string) => handleNodeSelect(e, nodeId)}
            onNodeToggle={(e: React.SyntheticEvent, nodeIds: string[]) => handleToggle(e, nodeIds)}
         >
            <BrowseTreeNode
               parentId={rootNodeId}
               selectionId={selectionId ?? rootNodeId}
               onSelectionChanged={onSelectionChanged}
            />
         </TreeView>
      </Paper>
   );
}


export const BrowseTreeView = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
   return (
      <BrowseTreeRoot rootNodeId={rootNodeId} onSelectionChanged={onSelectionChanged} />
   );
}

export default BrowseTreeView;
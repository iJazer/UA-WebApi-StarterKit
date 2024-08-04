/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';

import * as OpcUa from '../opcua';
import { ApplicationContext } from '../ApplicationProvider';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import { NodeIcon } from './NodeIcon';
import { IBrowsedNode } from '../service/IBrowsedNode';

export interface BrowseTreeNodeProps {
   parentId?: string
   selectionId?: string,
   onChildrenUpdated?: (oldNodes: IBrowsedNode[], newNodes: IBrowsedNode[]) => void,
   onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

export const BrowseTreeNode = ({ parentId, selectionId, onSelectionChanged }: BrowseTreeNodeProps) => {
   const [children, setChildren] = React.useState<IBrowsedNode[]>([]);
   const { browseChildren, visibleNodes, nodes } = React.useContext(ApplicationContext);

   React.useEffect(() => {
      if (browseChildren && parentId && selectionId === parentId) {
         browseChildren(parentId, 0).then((x) => {
            setChildren(x);
         });
      }
   }, [browseChildren, visibleNodes, selectionId, parentId, nodes, onSelectionChanged]);

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

export const BrowseTreeView = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
   const [selectionId, setSelectionId] = React.useState<string | undefined>();
   const { visibleNodes, setVisibleNodes, nodes } = React.useContext(ApplicationContext);

   const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
      //console.error(`SELECT node ${nodeId}`);
      const treeNode: IBrowsedNode | undefined = nodes.get(nodeId);
      if (treeNode && onSelectionChanged) {
         onSelectionChanged(treeNode.reference);
      }
      setSelectionId(treeNode?.id);
   }, [onSelectionChanged, nodes]);

   const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
      //console.error(`TOGGLE node ${nodeIds.join()}`);
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

export default BrowseTreeView;
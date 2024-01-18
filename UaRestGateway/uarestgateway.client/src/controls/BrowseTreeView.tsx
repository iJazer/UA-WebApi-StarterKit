import React from 'react';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';

import * as OpcUa from '../opcua';
import { TreeNodeView } from './TreeNodeView';
import { ITreeNode, ApplicationContext } from '../ApplicationProvider';

interface BrowseTreeViewProps {
   serverId?: string
   rootNodeId?: string
   requestTimeout?: number
   onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

export const BrowseTreeView = ({ serverId, rootNodeId, requestTimeout, onSelectionChanged }: BrowseTreeViewProps) => {
   const [selectionId, setSelectionId] = React.useState<number | undefined>();
   const context = React.useContext(ApplicationContext);

   const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
      const id = Number(nodeId);
      const treeNode: ITreeNode | undefined = context.nodes.get(id);
      if (treeNode && onSelectionChanged) {
         onSelectionChanged(treeNode.reference);
      }
      setSelectionId(treeNode?.id);
   }, [onSelectionChanged, context]);

   return (
      <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
         <TreeView
            defaultCollapseIcon={<ExpandMoreIcon />}
            defaultExpandIcon={<ChevronRightIcon />}
            onNodeSelect={(e: React.SyntheticEvent, nodeId: string) => handleNodeSelect(e, nodeId)}
         >
            <TreeNodeView
               serverId={serverId}
               parentId={rootNodeId}
               requestTimeout={requestTimeout}
               selectionId={selectionId}
            />
         </TreeView>
      </Paper>
   );
}

export default BrowseTreeView;
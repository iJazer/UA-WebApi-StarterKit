import React from 'react';

import Paper from '@mui/material/Paper/Paper';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { NodeIcon } from './NodeIcon';

import * as OpcUa from '../opcua';
import { ApplicationContext, IApplicationContext } from '../ApplicationProvider';
import { IMonitoredItem } from '../opcua-utils';

export interface Item {
   path: string[],
   name: string,
   children: Item[]
}

function buildTree(monitoredItems: IMonitoredItem[]): Item[] | undefined {
   const root: Item[] = [];
   if (monitoredItems) {
      for (let ii = 0; ii < (monitoredItems?.length ?? 0); ii++) {
         const path = monitoredItems[ii].path;
         let hit = root.find((item) => item.name === path[0]);
         if (!hit) {
            hit = { path: path.slice(0, 1), name: path[0], children: [] };
            root.push(hit);
         }
         let children = hit.children;
         for (let jj = 1; jj < (path?.length ?? 0); jj++) {
            hit = children.find((item) => item.name === path[jj]);
            if (!hit) {
               hit = { path: path.slice(0, jj + 1), name: path[jj], children: [] };
               children.push(hit);
            }
            children = hit.children;
         }
      }
   }
   return root;
}

function renderNodes(context: IApplicationContext, item: Item): React.ReactNode | undefined
{
   const itemId = item.path.join('/');
   if (!item.children?.length) {
      return (
         <TreeItem
            key={itemId}
            nodeId={itemId}
            label={item.name}
            icon={<NodeIcon nodeClass={OpcUa.NodeClass.Variable} />}
         >
         </TreeItem>
      );
   }
   else {
      return (
         <TreeItem
            key={itemId}
            nodeId={itemId}
            label={item.name}
            icon={<NodeIcon nodeClass={OpcUa.NodeClass.Object} typeDefinitionId={OpcUa.ObjectTypeIds.FolderType} />}
         >
            {item.children?.map((ii) => {
               return (ii) ? renderNodes(context, ii) : null;
            })}
         </TreeItem>
      );
   }
}

interface MonitoredItemTreeViewListProps {
   onSelectionChanged?: (selection: string) => void
}

export const MonitoredItemTreeView = ({ onSelectionChanged }: MonitoredItemTreeViewListProps) => {
   const [items, setItems] = React.useState<Item[]>([]);
   const context = React.useContext(ApplicationContext);
   const monitoredItems = context.monitoredItems;

   React.useEffect(() => {
      if (monitoredItems) {
         setItems(buildTree(Array.from(monitoredItems.values())) ?? []);
      }
   }, [monitoredItems]);

   const handleSelect = (_e: React.SyntheticEvent, nodeId: string) => {
      if (onSelectionChanged) {
         onSelectionChanged(nodeId);
      }
   };

   const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
      context.setVisibleMonitoredItems(nodeIds);
   }

   return (
      <Paper elevation={3} sx={{ minWidth: '300px', minHeight: '600px', mr: '5px', height: '100%', width: 'auto' }}>
         <TreeView
            defaultCollapseIcon={<ExpandMoreIcon />}
            defaultExpandIcon={<ChevronRightIcon />}
            expanded={context.visibleMonitoredItems}
            onNodeSelect={(e: React.SyntheticEvent, nodeId: string) => handleSelect(e, nodeId)}
            onNodeToggle={(e: React.SyntheticEvent, nodeIds: string[]) => handleToggle(e, nodeIds)}
         >
            {items?.map((ii) => {
               return (ii) ? renderNodes(context, ii) : null;
            })}
         </TreeView>
      </Paper>
   );
}

export default MonitoredItemTreeView;
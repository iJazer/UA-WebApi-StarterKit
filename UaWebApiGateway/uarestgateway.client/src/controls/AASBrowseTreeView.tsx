/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography';

//import * as OpcUa from 'opcua-webapi';
//import { ApplicationContext } from '../ApplicationProvider';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import { NodeIcon } from './NodeIcon';
import { IBrowsedNode } from '../service/IBrowsedNode';

import { SessionContext } from '../SessionProvider';

export interface AASBrowseTreeNodeProps {
    parentId?: string
    selectionId?: string,
    //onChildrenUpdated?: (oldNodes: IBrowsedNode[], newNodes: IBrowsedNode[]) => void,
    //onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

export const AASBrowseTreeNode = ({ parentId, selectionId/*, onSelectionChanged */}: AASBrowseTreeNodeProps) => {
    const [children, setChildren] = React.useState<IBrowsedNode[]>([]);
    //const { browseChildren, visibleNodes, nodes } = React.useContext(ApplicationContext);
    const { browseChildren, visibleNodes, nodes } = React.useContext(SessionContext);

    React.useEffect(() => {
        if (browseChildren && parentId && selectionId === parentId) {
            browseChildren(parentId, 0).then((x) => {
                setChildren(x);
            });
        }
    }, [browseChildren, visibleNodes, selectionId, parentId, nodes/*, onSelectionChanged*/]);

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
                        <AASBrowseTreeNode
                            parentId={node.reference.NodeId}
                            selectionId={selectionId}
                            //onSelectionChanged={onSelectionChanged}
                        />
                    </TreeItem>)
            })}
        </React.Fragment>);
};

interface AASBrowseTreeViewProps {
    rootNodeId?: string
    //onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
}

export const AASBrowseTreeView = () => {
    //const [selectionId, setSelectionId] = React.useState<string | undefined>();
    ////const { visibleNodes, setVisibleNodes, nodes } = React.useContext(ApplicationContext);
    //const { visibleNodes, setVisibleNodes, nodes } = React.useContext(SessionContext);

    //const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
    //    //console.error(`SELECT node ${nodeId}`);
    //    const treeNode: IBrowsedNode | undefined = nodes.get(nodeId);
    //    //if (treeNode && onSelectionChanged) {
    //    //    onSelectionChanged(treeNode.reference);
    //    //}
    //    setSelectionId(treeNode?.id);
    //}, [/*onSelectionChanged,*/ nodes]);

    //const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
    //    //console.error(`TOGGLE node ${nodeIds.join()}`);
    //    setVisibleNodes(nodeIds);
    //}

    return (
        <Paper elevation={3} sx={{ minWidth: '300px', mr: '5px', height: '100%', width: 'auto' }}>
            <Typography variant="h5" component="h2" gutterBottom>
                AAS
            </Typography>
            <TreeView
                defaultCollapseIcon={<ExpandMoreIcon />}
                defaultExpandIcon={<ChevronRightIcon />}
            >
                <TreeItem nodeId="1" label="Data Structures">
                    <TreeItem nodeId="2" label="Array" />
                    <TreeItem nodeId="3" label="Max Heap" />
                    <TreeItem nodeId="4" label="Stack" />
                </TreeItem>
                <TreeItem nodeId="5" label="Algorithms">
                    <TreeItem nodeId="10" label="Gready" />
                    <TreeItem nodeId="6" label="Graph">
                        <TreeItem nodeId="8" label="DFS" />
                        <TreeItem nodeId="8" label="BFS" />
                    </TreeItem>
                </TreeItem> 
            </TreeView>
        </Paper>
    );
}

export default AASBrowseTreeView;
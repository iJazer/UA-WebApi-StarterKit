/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';
import Box from '@mui/material/Box/Box';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography';

import * as OpcUa from 'opcua-webapi';
//import { ApplicationContext } from '../ApplicationProvider';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import { NodeIcon } from './NodeIcon';
import { IBrowsedNode } from '../service/IBrowsedNode';

import { BrowseContext } from '../BrowseContext';
import { HandleFactory } from '../service/HandleFactory';

import ContextMenu from '../ContextMenu';
import VariableValueList from './VariableValueList';

interface BrowseTreeNodeProps {
    parentId?: string;
    selectionId?: string;
    onChildrenUpdated?: (oldNodes: IBrowsedNode[], newNodes: IBrowsedNode[]) => void;
    onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void;
    onAddAccessViewItem: (displayName: string, nodeId: string) => void;
}

interface BrowseTreeNodeInternals {
    requestId: number;
}

const BrowseTreeNode = ({ parentId, selectionId, onSelectionChanged, onAddAccessViewItem }: BrowseTreeNodeProps) => {
    const [children, setChildren] = React.useState<IBrowsedNode[]>([]);
    const { browseChildren, lastCompletedRequest, stateChangeCount } = React.useContext(BrowseContext);
    const [contextMenu, setContextMenu] = React.useState<{ mouseX: number, mouseY: number, displayName: string } | null>(null);

    const m = React.useRef<BrowseTreeNodeInternals>({
        requestId: HandleFactory.increment()
    });

    React.useEffect(() => {
        if (browseChildren && parentId && selectionId === parentId) {
            browseChildren(m.current.requestId, parentId);
        }
    }, [browseChildren, parentId, selectionId, stateChangeCount]);

    React.useEffect(() => {
        if (lastCompletedRequest && m.current.requestId && m.current.requestId === lastCompletedRequest.callerHandle) {
            setChildren(lastCompletedRequest.children ?? []);
        }
    }, [lastCompletedRequest, setChildren]);

    const handleContextMenu = React.useCallback((event: React.MouseEvent, displayName: string) => {
        event.preventDefault();
        event.stopPropagation();
        setContextMenu((prevContextMenu) =>
            prevContextMenu === null
                ? { mouseX: event.clientX - 2, mouseY: event.clientY - 4, displayName }
                : null,
        );
    }, []);

    const handleClose = React.useCallback(() => {
        setContextMenu(null);
    }, []);

    const handleOnAddAccessView = React.useCallback(() => {
        if (contextMenu) {
            const { displayName } = contextMenu;
            const nodeId = children.find(node => (node.reference.DisplayName?.Text ?? '') === displayName)?.reference.NodeId;
            if (nodeId) {
                onAddAccessViewItem(displayName, nodeId);
            }
        }
        handleClose();
    }, [contextMenu, handleClose, children, onAddAccessViewItem]);

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
                        onContextMenu={(event) => handleContextMenu(event, node?.reference?.DisplayName?.Text ?? '')}>
                        <BrowseTreeNode
                            parentId={node.reference.NodeId}
                            selectionId={selectionId}
                            onSelectionChanged={onSelectionChanged}
                            onAddAccessViewItem={onAddAccessViewItem}
                        />
                    </TreeItem>
                );
            })}
            <ContextMenu
                anchorPosition={contextMenu}
                handleClose={handleClose}
                onAddAccessView={handleOnAddAccessView}
            />
        </React.Fragment>
    );
};

interface BrowseTreeViewProps {
    rootNodeId?: string;
    onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void;
}

const BrowseTreeRoot = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
    const [selectionId, setSelectionId] = React.useState<string | undefined>();
    const { visibleNodes, setVisibleNodes, nodes } = React.useContext(BrowseContext);
    const [accessViewItems, setAccessViewItems] = React.useState<{ displayName: string, nodeId: string }[]>([]);


    const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
        const treeNode: IBrowsedNode | undefined = nodes.get(nodeId);
        if (treeNode && onSelectionChanged) {
            onSelectionChanged(treeNode.reference);
        }
        setSelectionId(treeNode?.id);
    }, [onSelectionChanged, nodes]);

    const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
        setVisibleNodes(nodeIds);
    };

    const handleAddAccessViewItem = React.useCallback((displayName: string, nodeId: string) => {
        setAccessViewItems((prevItems) => [...prevItems, { displayName, nodeId }]);
    }, []);

    return (
        <Box display="flex" p={2} pb={4} sx={{ width: '100%', height: '33.33vh' }}>
            <Paper sx={{ mr: '5px', height: '100%', width: '40%', overflow: 'auto' }}>
                <Typography variant="h5" component="h2" gutterBottom>
                    OPC UA
                </Typography>
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
                        onAddAccessViewItem={handleAddAccessViewItem}
                    />
                </TreeView>
            </Paper>
            <VariableValueList accessViewItems={accessViewItems} />
        </Box>
    );
};

export const BrowseTreeView = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
    return (
        <BrowseTreeRoot rootNodeId={rootNodeId} onSelectionChanged={onSelectionChanged} />
    );
};

export default BrowseTreeView;

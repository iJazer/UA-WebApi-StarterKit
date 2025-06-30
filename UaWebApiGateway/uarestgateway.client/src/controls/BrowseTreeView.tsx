/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';
import Box from '@mui/material/Box/Box';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { TreeView } from '@mui/x-tree-view/TreeView/TreeView';
import Paper from '@mui/material/Paper/Paper';
import Typography from '@mui/material/Typography';

import * as OpcUa from 'opcua-webapi';
import { TreeItem } from '@mui/x-tree-view/TreeItem/TreeItem';
import { NodeIcon } from './NodeIcon';
import { IBrowsedNode } from '../service/IBrowsedNode';

import { BrowseContext } from '../BrowseContext';
import { HandleFactory } from '../service/HandleFactory';

import { SessionContext } from '../SessionContext';
import ContextMenu from '../ContextMenu';
import VariableValueList from './VariableValueList';

/**
 * 
 * Props for the BrowseTreeNode component.
 * @param parentId - The ID of the parent node.
 * @param selectionId - The ID of the selected node.
 * @param onChildrenUpdated - Callback function to be called when the children of the node are updated.
 * @param onSelectionChanged - Callback function to be called when the selection changes.
 * @param onAddAccessViewItem - Callback function to be called when an access view item is added.
 * @returns {JSX.Element} The rendered BrowseTreeNode component.
 * 
 */
interface BrowseTreeNodeProps {
    parentId?: string
    selectionId?: string,
    onChildrenUpdated?: (oldNodes: IBrowsedNode[], newNodes: IBrowsedNode[]) => void,
    onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void
    onAddAccessViewItem: (displayName: string, nodeId: string) => void;
}

/**
 * 
 */
interface BrowseTreeNodeInternals {
    requestId: number
}

/**
 * 
 * BrowseTreeNode component that represents a node in the browse tree.
 * It fetches and displays the children of the node.
 * 
 * @param {BrowseTreeNodeProps} props - The props for the component.
 * @param {string} props.parentId - The ID of the parent node.
 * @param {string} props.selectionId - The ID of the selected node.
 * @param {function} props.onChildrenUpdated - Callback function to be called when the children of the node are updated.
 * @param {function} props.onSelectionChanged - Callback function to be called when the selection changes.
 * @param {function} props.onAddAccessViewItem - Callback function to be called when an access view item is added.
 * @returns {JSX.Element} The rendered BrowseTreeNode component.
 * 
 */
const BrowseTreeNode = ({ parentId, selectionId, onSelectionChanged, onAddAccessViewItem }: BrowseTreeNodeProps) => {
    const [children, setChildren] = React.useState<IBrowsedNode[]>([]);
    const { browseChildren, processResults, responseCount } = React.useContext(BrowseContext);
    const { isConnected } = React.useContext(SessionContext);
    const [contextMenu, setContextMenu] = React.useState<{ mouseX: number, mouseY: number, displayName: string } | null>(null);

    const m = React.useRef<BrowseTreeNodeInternals>({
        requestId: HandleFactory.increment()
    });

    /**
     * Effect to fetch children nodes when parentId or selectionId changes.
     * If browseChildren is available and parentId is defined, it calls browseChildren with the current requestId and parentId.
     */
    React.useEffect(() => {
        if (browseChildren && parentId && selectionId === parentId) {
            browseChildren(m.current.requestId, parentId);
        }
    }, [browseChildren, parentId, selectionId, isConnected]);

    /**
     * Effect to update children nodes when a request completes.
     * If the last completed request matches the current requestId, it updates the children state with the new nodes.
     */
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

    /**
     * Handles the context menu opening on right-click.
     * @param event - The mouse event.
     * @param displayName - The display name of the node.
     */
    const handleContextMenu = React.useCallback((event: React.MouseEvent, displayName: string) => {
        event.preventDefault();
        event.stopPropagation();
        setContextMenu((prevContextMenu) =>
            prevContextMenu === null
                ? { mouseX: event.clientX - 2, mouseY: event.clientY - 4, displayName }
                : null,
        );
    }, []);

    /**
     * Handles the context menu closing.
     */
    const handleClose = React.useCallback(() => {
        setContextMenu(null);
    }, []);

    /**
     * Handles the addition of an access view item.
     * If contextMenu is available, it finds the nodeId based on the displayName and calls onAddAccessViewItem with the displayName and nodeId.
     */
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

/**
 * Props for the BrowseTreeView component.
 * This component is responsible for rendering the browse tree view.
 * 
 * @param rootNodeId - The ID of the root node.
 * @param onSelectionChanged - Callback function to be called when the selection changes.
 */
interface BrowseTreeViewProps {
    rootNodeId?: string;
    onSelectionChanged?: (node: OpcUa.ReferenceDescription) => void;
}

/**
 * BrowseTreeRoot component that serves as the root of the browse tree.
 * It manages the state of the visible nodes and handles selection changes.
 * 
 * @param {BrowseTreeViewProps} props - The props for the component.
 * @param {string} props.rootNodeId - The ID of the root node.
 * @param {function} props.onSelectionChanged - Callback function to be called when the selection changes.
 * @returns {JSX.Element} The rendered BrowseTreeRoot component.
 */
const BrowseTreeRoot = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
    const [selectionId, setSelectionId] = React.useState<string | undefined>();
    const { visibleNodes, setVisibleNodes, nodes } = React.useContext(BrowseContext);
    const [accessViewItems, setAccessViewItems] = React.useState<{ displayName: string, nodeId: string }[]>([]);

    /**
     * Handles the selection of a node.
     * It updates the selectionId state and calls the onSelectionChanged callback if available.
     * 
     * @param {React.SyntheticEvent} _e - The synthetic event.
     * @param {string} nodeId - The ID of the selected node.
     */
    const handleNodeSelect = React.useCallback((_e: React.SyntheticEvent, nodeId: string) => {
        const treeNode: IBrowsedNode | undefined = nodes.get(nodeId);
        if (treeNode && onSelectionChanged) {
            onSelectionChanged(treeNode.reference);
        }
        setSelectionId(treeNode?.id);
    }, [onSelectionChanged, nodes]);

    /**
     * Handles the toggle of nodes in the tree view.
     * It updates the visibleNodes state with the new node IDs.
     * 
     * @param {React.SyntheticEvent} _e - The synthetic event.
     * @param {string[]} nodeIds - The array of node IDs that are currently expanded.
     */
    const handleToggle = (_e: React.SyntheticEvent, nodeIds: string[]) => {
        setVisibleNodes(nodeIds);
    };

    /**
     * Handles the addition of an access view item.
     * It updates the accessViewItems state with the new item.
     * 
     * @param {string} displayName - The display name of the node.
     * @param {string} nodeId - The ID of the node.
     */
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

/**
 * BrowseTreeView component that serves as the main entry point for the browse tree.
 * It renders the BrowseTreeRoot component and manages the selection changes.
 * 
 * @param {BrowseTreeViewProps} props - The props for the component.
 * @param {string} props.rootNodeId - The ID of the root node.
 * @param {function} props.onSelectionChanged - Callback function to be called when the selection changes.
 * @returns {JSX.Element} The rendered BrowseTreeView component.
 */
export const BrowseTreeView = ({ rootNodeId, onSelectionChanged }: BrowseTreeViewProps) => {
    return (
        <BrowseTreeRoot rootNodeId={rootNodeId} onSelectionChanged={onSelectionChanged} />
    );
};

export default BrowseTreeView;

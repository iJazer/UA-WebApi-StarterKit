import React, { useState, useEffect } from "react";
import axios from "axios";
import { TreeView } from "@mui/x-tree-view/TreeView";
import { TreeItem } from "@mui/x-tree-view/TreeItem";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";

const DefaultServerUrl = `https://${location.host}/api/aas/tree`;

interface TreeNode {
    id: string;
    name: string;
    link?: string;
    children?: TreeNode[];
}

const AASTreeView: React.FC = () => {
    const [treeData, setTreeData] = useState<TreeNode | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        axios.get<TreeNode>(DefaultServerUrl)
            .then((response) => {
                setTreeData(response.data);
                setLoading(false);
            })
            .catch((error) => {
                console.error("Error fetching AAS data:", error);
                setLoading(false);
            });
    }, []);

    const renderTree = (nodes: TreeNode) => (
        <TreeItem
            key={nodes.id}
            nodeId={nodes.id}
            label={
                nodes.link ? (
                    <a href={nodes.link} target="_blank" rel="noopener noreferrer" style={{ textDecoration: "none", color: "inherit" }}>
                        {nodes.name}
                    </a>
                ) : (
                    nodes.name
                )
            }
        >
            {nodes.children?.map((node) => renderTree(node))}
        </TreeItem>
    );

    if (loading) return <p>Loading AAS Tree...</p>;
    if (!treeData) return <p>No data available.</p>;

    return (
        <TreeView
            aria-label="AAS tree view"
            defaultCollapseIcon={<ExpandMoreIcon />}
            defaultExpandIcon={<ChevronRightIcon />}
        >
            {renderTree(treeData)}
        </TreeView>
    );
};

export default AASTreeView;
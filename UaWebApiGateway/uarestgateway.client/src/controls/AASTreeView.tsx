import React, { useEffect, useState, useCallback } from "react";
import { TreeView } from "@mui/x-tree-view/TreeView";
import { TreeItem } from "@mui/x-tree-view/TreeItem";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import * as aas from "@aas-core-works/aas-core3.0-typescript";
import ContextMenu from "../ContextMenu";

const API_BASE = `https://${location.host}/api/v3.0`;

interface TreeNode {
    id: string;
    name: string;
    type: string;
    children?: TreeNode[];
    original: aas.types.Class | null;
    parentAASId?: string;
    parentSubmodelId?: string;
    path?: string;
    pollIntervalId?: number;
    value?: any;
}

const AASTreeView: React.FC = () => {
    const [treeData, setTreeData] = useState<TreeNode | null>(null);
    const [selected, setSelected] = useState<aas.types.Class | null>(null);
    const [contextMenu, setContextMenu] = useState<{
        mouseX: number;
        mouseY: number;
        node: TreeNode;
    } | null>(null);
    const [accessViewItems, setAccessViewItems] = useState<TreeNode[]>([]);
    const [accessViewContextMenu, setAccessViewContextMenu] = useState<{
        mouseX: number;
        mouseY: number;
        index: number;
    } | null>(null);

    useEffect(() => {
        loadTree();

        return () => {
            accessViewItems.forEach(item => {
                if (item.pollIntervalId) {
                    clearInterval(item.pollIntervalId);
                }
            });
        };
    }, []);

    const loadTree = async () => {
        const res = await fetch(`${API_BASE}/shells`);
        const shellJson = await res.json();
        const shell = aas.jsonization.assetAdministrationShellFromJsonable(shellJson).mustValue();

        const children: TreeNode[] = [];
        for (const ref of shell.submodels ?? []) {
            const id = ref.keys[0].value;
            const smRes = await fetch(`${API_BASE}/shells/${encodeId(shell.id)}/submodels/${encodeId(id)}`);
            const smJson = await smRes.json();
            const sm = aas.jsonization.submodelFromJsonable(smJson).mustValue();
            children.push(await submodelToTree(sm, shell.id));
        }

        setTreeData({
            id: shell.id,
            name: `AAS: ${shell.idShort}`,
            type: "AssetAdministrationShell",
            original: shell,
            children,
        });
    };

    const submodelToTree = async (submodel: aas.types.Submodel, aasId: string): Promise<TreeNode> => {
        const children: TreeNode[] = [];

        for (const el of submodel.submodelElements ?? []) {
            children.push(await elementToTree(el, aasId, submodel.id, el.idShort ?? "", ""));
        }

        return {
            id: submodel.id,
            name: `Submodel: ${submodel.idShort}`,
            type: "Submodel",
            original: submodel,
            parentAASId: aasId,
            parentSubmodelId: submodel.id,
            children,
        };
    };

    const elementToTree = async (
        element: aas.types.ISubmodelElement,
        aasId: string,
        submodelId: string,
        idShort: string,
        parentPath: string
    ): Promise<TreeNode> => {
        const label = `${getSubmodelElementAbbreviation(element.constructor.name)}: ${element.idShort}`;
        const currentPath = parentPath ? `${parentPath}.${idShort}` : idShort;
        let children: TreeNode[] = [];

        if (element instanceof aas.types.SubmodelElementCollection && element.value) {
            for (const el of element.value) {
                children.push(await elementToTree(el, aasId, submodelId, el.idShort ?? "", currentPath));
            }
        }

        return {
            id: crypto.randomUUID(),
            name: label,
            type: element.constructor.name,
            original: element,
            parentAASId: aasId,
            parentSubmodelId: submodelId,
            path: currentPath,
            children,
        };
    };

    const handleContextMenu = (event: React.MouseEvent, node: TreeNode) => {
        event.preventDefault();
        setContextMenu(
            contextMenu === null
                ? {
                    mouseX: event.clientX + 2,
                    mouseY: event.clientY + 2,
                    node,
                }
                : null
        );
    };

    const handleCloseContextMenu = () => {
        setContextMenu(null);
    };

    const fetchValue = async (node: TreeNode) => {
        if (!node.parentAASId || !node.parentSubmodelId || !node.path) return null;

        try {
            const res = await fetch(`${API_BASE}/shells/${encodeId(node.parentAASId)}/submodels/${encodeId(node.parentSubmodelId)}/submodel-elements/${node.path}`);
            const json = await res.json();
            return json.value ?? json;
        } catch (e) {
            console.error("Polling error:", e);
            return null;
        }
    };

    const handleOnAddAccessView = useCallback(() => {
        if (contextMenu?.node) {
            const node = contextMenu.node;
            console.log("Adding to access view:", node);

            const poll = async () => {
                const value = await fetchValue(node);
                setAccessViewItems(prev => prev.map(i => i.id === node.id ? { ...i, value } : i));
            };

            poll();
            const intervalId = window.setInterval(poll, 3000);

            setAccessViewItems((prev) => [...prev, { ...node, pollIntervalId: intervalId }]);
        }
        handleCloseContextMenu();
    }, [contextMenu]);

    const handleAccessViewContextMenu = (
        event: React.MouseEvent,
        index: number
    ) => {
        event.preventDefault();
        setAccessViewContextMenu({
            mouseX: event.clientX + 2,
            mouseY: event.clientY + 2,
            index,
        });
    };

    const handleRemoveAccessViewItem = (index: number) => {
        setAccessViewItems((prev) => {
            const item = prev[index];
            if (item.pollIntervalId) {
                clearInterval(item.pollIntervalId);
            }
            return prev.filter((_, i) => i !== index);
        });
        setAccessViewContextMenu(null);
    };

    const renderValue = (val: any): string => {
        if (val == null) return "";
        if (typeof val === "string" || typeof val === "number" || typeof val === "boolean") return val.toString();

        if (Array.isArray(val) && val.every(v => v.language && v.text)) {
            return val.map(v => `[${v.language}]: ${v.text}`).join(", ");
        }

        try {
            return JSON.stringify(val);
        } catch {
            return "[Unsupported Value]";
        }
    };

    const renderTree = (node: TreeNode): React.ReactNode => (
        <TreeItem
            key={node.id}
            nodeId={node.id}
            label={node.name}
            onClick={() => setSelected(node.original)}
            onContextMenu={(e) => {
                e.preventDefault();
                e.stopPropagation();
                console.log("Right-clicked on:", node.name, node.type);
                handleContextMenu(e, node)
            }}
        >
            {node.children?.map(renderTree)}
        </TreeItem>
    );

    const renderDetails = () => {
        if (!selected) return <div>Select a node to see details</div>;

        const entries = Object.entries(selected);
        return (
            <table style={{ width: "100%", borderCollapse: "collapse" }}>
                <thead>
                    <tr>
                        <th style={thStyle}>Property</th>
                        <th style={thStyle}>Value</th>
                    </tr>
                </thead>
                <tbody>
                    {entries.map(([key, value]) => (
                        <tr key={key}>
                            <td style={tdStyle}>{key}</td>
                            <td style={tdStyle}>{JSON.stringify(value)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        );
    };

    return (
        <div style={{ display: "flex", gap: "1rem" }}>
            <div style={{ flex: 1 }}>
                <TreeView defaultCollapseIcon={<ExpandMoreIcon />} defaultExpandIcon={<ChevronRightIcon />}>
                    {treeData && renderTree(treeData)}
                </TreeView>
            </div>
            <div style={{ flex: 1 }}>{renderDetails()}</div>
            <div style={{ flex: 1 }}>
                <table style={{ width: "100%", borderCollapse: "collapse" }}>
                    <thead>
                        <tr>
                            <th style={thStyle}>Name (idShort)</th>
                            <th style={thStyle}>Value</th>
                        </tr>
                    </thead>
                    <tbody>
                        {accessViewItems.map((item, idx) => {
                            const original = item.original as aas.types.Class;
                            const idShort = (original as any)?.idShort ?? item.name;
                            const value = item.value ?? (original as any)?.value ?? null;
                            return (
                                <tr
                                    key={idx}
                                    onContextMenu={(e) => handleAccessViewContextMenu(e, idx)}
                                    style={{ cursor: "context-menu" }}
                                >
                                    <td style={tdStyle}>{idShort}</td>
                                    <td style={tdStyle}>{renderValue(value)}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>

            <ContextMenu
                anchorPosition={
                    contextMenu ? { mouseX: contextMenu.mouseX, mouseY: contextMenu.mouseY } : null
                }
                handleClose={handleCloseContextMenu}
                onAddAccessView={handleOnAddAccessView}
            />

            {accessViewContextMenu && (
                <ul
                    style={{
                        position: "fixed",
                        top: accessViewContextMenu.mouseY,
                        left: accessViewContextMenu.mouseX,
                        backgroundColor: "white",
                        border: "1px solid #ccc",
                        boxShadow: "2px 2px 6px rgba(0,0,0,0.2)",
                        listStyle: "none",
                        margin: 0,
                        padding: "4px 0",
                        zIndex: 1000,
                    }}
                    onMouseLeave={() => setAccessViewContextMenu(null)}
                >
                    <li
                        style={{ padding: "4px 12px", cursor: "pointer" }}
                        onClick={() => handleRemoveAccessViewItem(accessViewContextMenu.index)}
                    >
                        Remove from Access View
                    </li>
                </ul>
            )}
        </div>
    );
};

function encodeId(id: string): string {
    return btoa(id).replace(/\+/g, "-").replace(/\//g, "_").replace(/=+$/, "");
}

const thStyle: React.CSSProperties = {
    textAlign: "left",
    background: "#f0f0f0",
    padding: "8px",
    borderBottom: "1px solid #ccc",
};

const tdStyle: React.CSSProperties = {
    padding: "8px",
    borderBottom: "1px solid #eee",
};

function getSubmodelElementAbbreviation(name: string): string {
    switch (name) {
        case "Property":
            return "Prop";
        case "MultiLanguageProperty":
            return "MLP";
        case "Range":
            return "Range";
        case "ReferenceElement":
            return "RefEle";
        case "RelationshipElement":
            return "RelEle";
        case "AnnotatedRelationshipElement":
            return "ARelEle";
        case "File":
            return "File";
        case "Blob":
            return "Blob";
        case "SubmodelElementCollection":
            return "SMC";
        case "SubmodelElementList":
            return "SML";
        case "Entity":
            return "Ent";
        case "BasicEventElement":
            return "Evt";
        case "Capability":
            return "Cap";
        default:
            return "unnamed";
    }
}

export default AASTreeView;

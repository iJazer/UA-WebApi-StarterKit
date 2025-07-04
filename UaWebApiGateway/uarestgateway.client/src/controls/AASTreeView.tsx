﻿import React, { useEffect, useState, useCallback, useContext, useRef } from "react";
import { TreeView } from "@mui/x-tree-view/TreeView";
import { TreeItem } from "@mui/x-tree-view/TreeItem";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import * as aas from "@aas-core-works/aas-core3.0-typescript";
import ContextMenu from "../ContextMenu";
import { SessionContext } from "../SessionContext";
import { sendAASRequest } from "../utils/SendAASRequest";

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
    const [contextMenu, setContextMenu] = useState<{ mouseX: number; mouseY: number; node: TreeNode } | null>(null);
    const [accessViewItems, setAccessViewItems] = useState<TreeNode[]>([]);
    const [accessViewContextMenu, setAccessViewContextMenu] = useState<{ mouseX: number; mouseY: number; index: number } | null>(null);

    const session = useContext(SessionContext);
    const sessionRef = useRef(session);

    useEffect(() => {
        sessionRef.current = session;
    }, [session]);

    useEffect(() => {
        loadTree();
        return () => {
            accessViewItems.forEach(item => {
                if (item.pollIntervalId) clearInterval(item.pollIntervalId);
            });
        };
    }, []);

    useEffect(() => {
        if (sessionRef.current.addPushUpdateListener) {
            sessionRef.current.addPushUpdateListener((response) => {
                const updatedPath = response?.Body?.Path;
                const value = response?.Body?.Result?.value ?? response?.Body?.Result;

                if (!updatedPath) return;

                setAccessViewItems(prev =>
                    prev.map(item =>
                        item.path === updatedPath ? { ...item, value } : item
                    )
                );
            });
        }
    }, []);

    const loadTree = async () => {
        const shellJson = await sendAASRequest(sessionRef.current, "GET", "/shells");
        const shell = aas.jsonization.assetAdministrationShellFromJsonable(shellJson).mustValue();

        const children: TreeNode[] = [];
        for (const ref of shell.submodels ?? []) {
            const id = ref.keys[0].value;
            const smJson = await sendAASRequest(sessionRef.current, "GET", `/shells/${encodeId(shell.id)}/submodels/${encodeId(id)}`);
            const sm = aas.jsonization.submodelFromJsonable(smJson).mustValue();
            children.push(await submodelToTree(sm, shell.id));
        }

        setTreeData({ id: shell.id, name: `AAS: ${shell.idShort}`, type: "AssetAdministrationShell", original: shell, children });
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

    const elementToTree = async (element: aas.types.ISubmodelElement, aasId: string, submodelId: string, idShort: string, parentPath: string): Promise<TreeNode> => {
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

    const fetchValue = async (node: TreeNode) => {
        if (!node.parentAASId || !node.parentSubmodelId || !node.path) return null;
        try {
            const result = await sendAASRequest(sessionRef.current, "GET", `/shells/${encodeId(node.parentAASId)}/submodels/${encodeId(node.parentSubmodelId)}/submodel-elements/${node.path}`);
            return result?.value ?? result;
        } catch (e) {
            console.error("Polling error:", e);
            return null;
        }
    };

    const registeredViaWebSocket = useRef<Set<string>>(new Set());

    const handleOnAddAccessView = useCallback(() => {
        if (!contextMenu?.node) return;

        const node = contextMenu.node;
        const session = sessionRef.current;
        const path = node.path!;
        const url = `/shells/${encodeId(node.parentAASId!)}/submodels/${encodeId(node.parentSubmodelId!)}/submodel-elements/${path}`;

        const fetchAndUpdate = async () => {
            const value = await fetchValue(node);
            setAccessViewItems(prev =>
                prev.map(i => i.id === node.id ? { ...i, value } : i)
            );
        };

        const updateItem = (patch: Partial<TreeNode>) => {
            setAccessViewItems(prev =>
                prev.map(i => i.id === node.id ? { ...i, ...patch } : i)
            );
        };

        // If the item already exists in access view, cancel its poll
        const existing = accessViewItems.find(i => i.id === node.id);
        if (existing?.pollIntervalId) {
            clearInterval(existing.pollIntervalId);
            updateItem({ pollIntervalId: undefined });
        }

        // Fetch once for value display
        fetchAndUpdate();

        if (session.isConnected) {
            // WebSocket mode
            if (!registeredViaWebSocket.current.has(path)) {
                registeredViaWebSocket.current.add(path);
                sendAASRequest(session, "GET", url).catch(err => {
                    console.error("WebSocket GET failed:", err);
                    registeredViaWebSocket.current.delete(path);
                });
            }
            if (!existing) {
                setAccessViewItems(prev => [...prev, { ...node }]);
            }
        } else {
            // Fallback to polling
            const intervalId = window.setInterval(fetchAndUpdate, 3000);
            if (!existing) {
                setAccessViewItems(prev => [...prev, { ...node, pollIntervalId: intervalId }]);
            } else {
                updateItem({ pollIntervalId: intervalId });
            }
        }

        handleCloseContextMenu();
    }, [contextMenu, accessViewItems]);

    const handleContextMenu = (event: React.MouseEvent, node: TreeNode) => {
        event.preventDefault();
        setContextMenu({ mouseX: event.clientX + 2, mouseY: event.clientY + 2, node });
    };

    const handleCloseContextMenu = () => setContextMenu(null);

    const handleAccessViewContextMenu = (event: React.MouseEvent, index: number) => {
        event.preventDefault();
        setAccessViewContextMenu({ mouseX: event.clientX + 2, mouseY: event.clientY + 2, index });
    };

    const handleRemoveAccessViewItem = (index: number) => {
        setAccessViewItems(prev => {
            const item = prev[index];
            if (item.pollIntervalId) clearInterval(item.pollIntervalId);
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

    return (
        <div style={{ display: "flex", height: "80vh", width: "100%" }}>
            <div style={{ width: "33%", overflow: "auto", borderRight: "1px solid #ccc" }}>
                <TreeView defaultCollapseIcon={<ExpandMoreIcon />} defaultExpandIcon={<ChevronRightIcon />}>
                    {treeData && renderTree(treeData)}
                </TreeView>
            </div>
            <div style={{ width: "33%", overflow: "auto", borderRight: "1px solid #ccc", padding: "0 8px" }}>
                {renderDetails()}
            </div>
            <div style={{ width: "34%", overflow: "auto", padding: "0 8px" }}>
                <table style={{ width: "100%", tableLayout: "fixed" }}>
                    <thead><tr><th style={thStyle}>Name (idShort)</th><th style={thStyle}>Value</th></tr></thead>
                    <tbody>
                        {accessViewItems.map((item, idx) => {
                            const original = item.original as aas.types.Class;
                            const idShort = (original as any)?.idShort ?? item.name;
                            const value = item.value ?? (original as any)?.value ?? null;
                            return (
                                <tr key={idx} onContextMenu={(e) => handleAccessViewContextMenu(e, idx)} style={{ cursor: "context-menu" }}>
                                    <td style={tdStyle}>{idShort}</td>
                                    <td style={{ ...tdStyle, whiteSpace: "nowrap" }}>{renderValue(value)}</td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>

            <ContextMenu anchorPosition={contextMenu ? { mouseX: contextMenu.mouseX, mouseY: contextMenu.mouseY } : null} handleClose={handleCloseContextMenu} onAddAccessView={handleOnAddAccessView} />

            {accessViewContextMenu && (
                <ul style={{
                    position: "fixed",
                    top: accessViewContextMenu.mouseY,
                    left: accessViewContextMenu.mouseX,
                    backgroundColor: "white",
                    border: "1px solid #ccc",
                    boxShadow: "2px 2px 6px rgba(0,0,0,0.2)",
                    listStyle: "none",
                    margin: 0,
                    padding: "4px 0",
                    zIndex: 1000
                }} onMouseLeave={() => setAccessViewContextMenu(null)}>
                    <li style={{ padding: "4px 12px", cursor: "pointer" }} onClick={() => handleRemoveAccessViewItem(accessViewContextMenu.index)}>
                        Remove from Access View
                    </li>
                </ul>
            )}
        </div>
    );

    function renderTree(node: TreeNode): React.ReactNode {
        return (
            <TreeItem key={node.id} nodeId={node.id} label={node.name} onClick={() => setSelected(node.original)} onContextMenu={(e) => { e.preventDefault(); e.stopPropagation(); handleContextMenu(e, node); }}>
                {node.children?.map(renderTree)}
            </TreeItem>
        );
    }

    function renderDetails() {
        if (!selected) return <div>Select a node to see details</div>;
        const entries = Object.entries(selected);
        return (
            <table style={{ width: "100%", tableLayout: "fixed" }}>
                <thead><tr><th style={thStyle}>Property</th><th style={thStyle}>Value</th></tr></thead>
                <tbody>
                    {entries.map(([key, value]) => (
                        <tr key={key}><td style={tdStyle}>{key}</td><td style={tdStyle}>{JSON.stringify(value)}</td></tr>
                    ))}
                </tbody>
            </table>
        );
    }
};

const thStyle: React.CSSProperties = {
    textAlign: "left",
    background: "#f0f0f0",
    padding: "8px",
    borderBottom: "1px solid #ccc",
    whiteSpace: "nowrap",
    overflow: "hidden",
    textOverflow: "ellipsis",
};

const tdStyle: React.CSSProperties = {
    padding: "8px",
    borderBottom: "1px solid #eee",
    whiteSpace: "nowrap",
    overflow: "auto",
};

function encodeId(id: string): string {
    return btoa(id).replace(/\+/g, "-").replace(/\//g, "_").replace(/=+$/, "");
}

function getSubmodelElementAbbreviation(name: string): string {
    switch (name) {
        case "Property": return "Prop";
        case "MultiLanguageProperty": return "MLP";
        case "Range": return "Range";
        case "ReferenceElement": return "RefEle";
        case "RelationshipElement": return "RelEle";
        case "AnnotatedRelationshipElement": return "ARelEle";
        case "File": return "File";
        case "Blob": return "Blob";
        case "SubmodelElementCollection": return "SMC";
        case "SubmodelElementList": return "SML";
        case "Entity": return "Ent";
        case "BasicEventElement": return "Evt";
        case "Capability": return "Cap";
        default: return "unnamed";
    }
}

export default AASTreeView;

import React, { useEffect, useState } from "react";
import { TreeView } from "@mui/x-tree-view/TreeView";
import { TreeItem } from "@mui/x-tree-view/TreeItem";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";

import * as aas from "@aas-core-works/aas-core3.0-typescript";

const API_BASE = `https://${location.host}/api/v3.0`;

interface TreeNode {
    id: string;
    name: string;
    type: string;
    children?: TreeNode[];
    original: aas.types.Class | null;
}

const AASTreeView: React.FC = () => {
    const [treeData, setTreeData] = useState<TreeNode | null>(null);
    const [selected, setSelected] = useState<aas.types.Class | null>(null);

    useEffect(() => {
        loadTree();
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
            //const sm = aas.jsonization.deserializeSubmodel(smJson);
            const sm = aas.jsonization.submodelFromJsonable(smJson).mustValue();
            children.push(await submodelToTree(sm));
        }

        setTreeData({
            id: shell.id,
            name: `AAS: ${shell.idShort}`,
            type: "AssetAdministrationShell",
            original: shell,
            children,
        });
    };

    const submodelToTree = async (submodel: aas.types.Submodel): Promise<TreeNode> => {
        const children: TreeNode[] = [];

        for (const el of submodel.submodelElements ?? []) {
            children.push(await elementToTree(el));
        }

        return {
            id: submodel.id,
            name: `Submodel: ${submodel.idShort}`,
            type: "Submodel",
            original: submodel,
            children,
        };
    };

    const elementToTree = async (element: aas.types.ISubmodelElement): Promise<TreeNode> => {
        const label = `${element.constructor.name}: ${element.idShort}`;
        let children: TreeNode[] = [];

        if (element instanceof aas.types.SubmodelElementCollection && element.value) {
            for (const el of element.value) {
                children.push(await elementToTree(el));
            }
        }

        return {
            id: element.idShort ?? crypto.randomUUID(),
            name: label,
            type: element.constructor.name,
            original: element,
            children,
        };
    };

    const renderTree = (node: TreeNode): React.ReactNode => (
        <TreeItem
            key={node.id}
            nodeId={node.id}
            label={node.name}
            onClick={() => setSelected(node.original)}
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
                <TreeView
                    defaultCollapseIcon={<ExpandMoreIcon />}
                    defaultExpandIcon={<ChevronRightIcon />}
                >
                    {treeData && renderTree(treeData)}
                </TreeView>
            </div>
            <div style={{ flex: 1 }}>{renderDetails()}</div>
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

export default AASTreeView;
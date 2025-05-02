import React, { useEffect, useState } from "react";
import axios from "axios";
import { TreeView } from "@mui/x-tree-view/TreeView";
import { TreeItem } from "@mui/x-tree-view/TreeItem";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";

const DefaultServerUrl = `https://${location.host}/api/v3.0`;

// Interfaces
interface SubmodelRef {
    keys: { value: string }[];
}

interface AssetAdministrationShell {
    id: string;
    idShort: string;
    submodels: SubmodelRef[];
}

interface Submodel {
    id: string;
    idShort: string;
    submodelElements: SubmodelElement[];
}

interface SubmodelElement {
    idShort: string;
    modelType: string;
    value?: SubmodelElement[]; // For collections
}

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
        async function loadTree() {
            try {
                const aasRes = await axios.get<AssetAdministrationShell>(DefaultServerUrl +"/shells");
                const aas = aasRes.data;

                const submodels: Submodel[] = [];

                for (const ref of aas.submodels || []) {
                    const submodelId = ref.keys?.[0]?.value;
                    if (!submodelId) continue;

                    var smEndpoint = DefaultServerUrl + `/shells/${base64UrlEncode(aas.id)}/submodels/${base64UrlEncode(submodelId)}`;
                    const submodelRes = await axios.get<Submodel>(smEndpoint);

                    submodels.push(submodelRes.data);
                }

                const tree = convertAasToTree(aas, submodels);
                setTreeData(tree);
            } catch (err) {
                console.error("Failed to load AAS or submodels", err);
            } finally {
                setLoading(false);
            }
        }

        loadTree();
    }, []);

    const renderTree = (node: TreeNode) => (
        <TreeItem key={node.id} nodeId={node.id} label={node.name}>
            {node.children?.map(renderTree)}
        </TreeItem>
    );

    if (loading) return <p>Loading...</p>;
    if (!treeData) return <p>No tree data.</p>;

    return (
        <TreeView defaultCollapseIcon={<ExpandMoreIcon />} defaultExpandIcon={<ChevronRightIcon />}>
            {renderTree(treeData)}
        </TreeView>
    );
};

function base64UrlEncode(str: string): string {
    return btoa(str)
        .replace(/\+/g, "-")
        .replace(/\//g, "_")
        .replace(/=+$/, "");
}

// Converter Functions
function convertAasToTree(aas: AssetAdministrationShell, submodels: Submodel[]): TreeNode {
    const root: TreeNode = {
        id: aas.id,
        name: `AAS: ${aas.idShort}`,
        children: [],
    };

    for (const sm of submodels) {
        root.children!.push(convertSubmodelToTree(sm));
    }

    return root;
}

function convertSubmodelToTree(submodel: Submodel): TreeNode {
    return {
        id: submodel.id,
        name: `Submodel: ${submodel.idShort}`,
        children: (submodel.submodelElements || []).map(convertElementToTree),
    };
}

function convertElementToTree(element: SubmodelElement): TreeNode {
    const type = element.modelType;

    const node: TreeNode = {
        id: element.idShort,
        name: `${type}: ${element.idShort}`,
    };

    // Only SubmodelElementCollection should render children
    if (type === "SubmodelElementCollection" && Array.isArray(element.value)) {
        node.children = element.value.map(convertElementToTree);
    }

    // All other types: no children, no value shown
    return node;
}

export default AASTreeView;

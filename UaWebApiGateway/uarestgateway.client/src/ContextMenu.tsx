import React from 'react';
import { Menu, MenuItem } from '@mui/material';

interface ContextMenuProps {
    anchorPosition: { mouseX: number, mouseY: number } | null;
    handleClose: () => void;
    onAddAccessView: () => void;
}

const ContextMenu: React.FC<ContextMenuProps> = ({ anchorPosition, handleClose, onAddAccessView }) => {
    return (
        <Menu
            anchorReference="anchorPosition"
            anchorPosition={anchorPosition ? { top: anchorPosition.mouseY, left: anchorPosition.mouseX } : undefined}
            open={Boolean(anchorPosition)}
            onClose={handleClose}
        >
            <MenuItem onClick={onAddAccessView}>Add to Access View</MenuItem>
        </Menu>
    );
};

export default ContextMenu;

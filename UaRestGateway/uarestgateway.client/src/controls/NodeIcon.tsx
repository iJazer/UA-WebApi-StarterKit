
import WidgetsIcon from '@mui/icons-material/Widgets';
import LabelIcon from '@mui/icons-material/Label';
import PlayCircleIcon from '@mui/icons-material/PlayCircle';
import FolderIcon from '@mui/icons-material/Folder';
import DeveloperBoardIcon from '@mui/icons-material/DeveloperBoard';
import SystemUpdateAltIcon from '@mui/icons-material/SystemUpdateAlt';
import BookmarksIcon from '@mui/icons-material/Bookmarks';

import * as OpcUa from '../opcua';

interface NodeIconProps {
   nodeClass?: number
   typeDefinitionId?: string
}

export const NodeIcon = ({ nodeClass, typeDefinitionId }: NodeIconProps) => {
   switch (nodeClass) {
      case OpcUa.NodeClass.View:
         return <WidgetsIcon sx={{ color: 'GoldenRod' }} />;
      case OpcUa.NodeClass.Object:
         return (typeDefinitionId === OpcUa.ObjectTypeIds.FolderType) 
            ? <FolderIcon sx={{ color: 'Gold' }} />
            : (typeDefinitionId === OpcUa.ObjectTypeIds.FileType) 
            ? <SystemUpdateAltIcon sx={{ color: 'blue' }} /> 
            : <WidgetsIcon sx={{ color: 'blue' }} />;
      case OpcUa.NodeClass.Variable:
         return (typeDefinitionId === OpcUa.VariableTypeIds.PropertyType)
            ? <LabelIcon sx={{ color: 'green' }} />
            : <BookmarksIcon sx={{ color: 'navy' }} />;
      case OpcUa.NodeClass.Method:
         return <PlayCircleIcon sx={{ color: 'red' }} />;
      case OpcUa.NodeClass.ObjectType:
      case OpcUa.NodeClass.VariableType:
      case OpcUa.NodeClass.DataType:
      case OpcUa.NodeClass.ReferenceType:
         return <DeveloperBoardIcon sx={{ color: 'DarkSlateGray' }} />;
      default:
         return <FolderIcon sx={{ color: 'grey' }} />;
   }
}

import { Box, SxProps, Theme, Typography } from '@mui/material';
import { format } from 'date-fns'
import * as OpcUa from '../opcua';

interface DataValueDisplayProps {
   value?: OpcUa.DataValue
   sx?: SxProps<Theme>
}

function toText(value?: OpcUa.Variant): string {
   if (!value) {
      return '';
   }
   switch (value.Type) {
      case OpcUa.BuiltInType.Boolean: {
         return value.Body ? 'True' : 'False';
      }
      case OpcUa.BuiltInType.DateTime: {
         const utcDate = new Date(value.Body as string);
         const offsetInMinutes = utcDate.getTimezoneOffset();
         const localTimeInMilliseconds = utcDate.getTime() - (offsetInMinutes * 60000);
         const localDate = new Date(localTimeInMilliseconds);
         return format(localDate, 'yyyy-MM-dd HH:mm:ss');
      }
      case OpcUa.BuiltInType.StatusCode: {
         return OpcUa.StatusCodes[OpcUa.StatusCode.codeBits(value.Body as number)];
      }
      case OpcUa.BuiltInType.LocalizedText: {
         const text = value.Body as OpcUa.LocalizedText;
         return text?.Text ?? '';
      }
      case OpcUa.BuiltInType.ByteString: {
         const text = value.Body as string;
         if (text) {
            const buffer = Buffer.from(value.Body as string, 'base64');
            return Array.from(new Uint8Array(buffer))
               .map(byte => byte.toString(16).padStart(2, '0'))
               .join('');
         }
         return '';
      }
      case OpcUa.BuiltInType.SByte:
      case OpcUa.BuiltInType.Byte:
      case OpcUa.BuiltInType.Int16:
      case OpcUa.BuiltInType.UInt16:
      case OpcUa.BuiltInType.Int32:
      case OpcUa.BuiltInType.UInt32:
      case OpcUa.BuiltInType.Int64:
      case OpcUa.BuiltInType.UInt64:
      case OpcUa.BuiltInType.Float:
      case OpcUa.BuiltInType.Double:
      case OpcUa.BuiltInType.String:
      case OpcUa.BuiltInType.Guid:
      case OpcUa.BuiltInType.NodeId:
      case OpcUa.BuiltInType.ExpandedNodeId:
      case OpcUa.BuiltInType.QualifiedName:
      {
         return `${value.Body}`;
      }
      default:
      {
         return JSON.stringify(value.Body);
      }
   }
}

export const DataValueDisplay = ({ value, sx }: DataValueDisplayProps) => {
   if (!value) {
      return null;
   }
   if (OpcUa.StatusCode.isBad(value.StatusCode)) {
      return (
         <Typography sx={sx}>
            {OpcUa.StatusCodes[OpcUa.StatusCode.codeBits(value.StatusCode)]}
         </Typography>
      );
   }
   if (Array.isArray(value.Value?.Body)) {
      const array = value.Value.Body;
      return (
         <Box ml={6} sx={{ flexGrow: 0, display: 'flex' }}>
            <Typography sx={sx}>
               {toText({ Type: value.Value.Type, Body: array.at(0) }) + '...'}
            </Typography>
         </Box>
      );
   }
   return (
      <Typography sx={sx}>
         {toText(value.Value)}
      </Typography>
   );
}

export default DataValueDisplay;
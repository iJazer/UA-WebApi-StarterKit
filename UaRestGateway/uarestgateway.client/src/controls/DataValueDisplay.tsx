import React from 'react';

import { Box, Stack, SxProps, Table, TableBody, TableCell, TableRow, Theme, Typography, styled } from '@mui/material';
import { format } from 'date-fns'
import * as OpcUa from '../opcua';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';

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

function toBuiltInType(value?: unknown): OpcUa.BuiltInType {
   if (value === undefined) {
      return OpcUa.BuiltInType.Null;
   }
   if (typeof value === 'number') {
      return OpcUa.BuiltInType.Double;
   }
   if (typeof value === 'boolean') {
      return OpcUa.BuiltInType.Boolean;
   }
   if (typeof value === 'string') {
      return OpcUa.BuiltInType.String;
   }
   if (Array.isArray(value)) {
      return toBuiltInType(value.at(0));
   }
   return OpcUa.BuiltInType.ExtensionObject;
}

function toVariant(value?: unknown): OpcUa.Variant {
   return { Type: toBuiltInType(value), Body: value } as OpcUa.Variant;
}

const CustomTable = styled(Table)(({ theme }) => ({
   border: '2px solid ' + theme.palette.primary.main,
   marginBottom: '4px'
}));

const CustomTableRow = styled(TableRow)(({ theme }) => ({
   '&:nth-of-type(even)': {
      background: theme.palette.background.default,
   },
   '&:nth-of-type(odd)': {
      background: theme.palette.background.paper,
   },
   '& th, td': {
      paddingLeft: '4px',
      paddingRight: '4px',
      paddingTop: '1px',
      paddingBottom: '1px',
   }
}));

export const DataValueDisplay = ({ value, sx }: DataValueDisplayProps) => {
   const [expanded, setExpanded] = React.useState<boolean>(false);

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
         <Box sx={{ flexGrow: 0, display: 'flex' }}>
            <Stack>
               {
                  array.map((item, index) => {
                     if (index && !expanded) {
                        return null;
                     }
                     return (
                        <Box key={index} sx={{ display: 'flex' }}>
                           <Box sx={{ minWidth: 30 }}>
                              {
                                 (!index) ? (expanded)
                                    ? <ExpandLessIcon onClick={() => setExpanded(false)} />
                                    : <ExpandMoreIcon onClick={() => setExpanded(true)} />
                                    : null
                              }
                           </Box>
                           <DataValueDisplay value={{ Value: { Type: value.Value?.Type, Body: item }}} />
                        </Box>
                     );
                  })
               }
            </Stack>
         </Box>
      );
   }
   if (value.Value?.Type === OpcUa.BuiltInType.ExtensionObject) {
      const target = value.Value.Body?.Body ?? value.Value.Body ?? {};
      const keys = Object.keys(target);
      return (
         <Box sx={{ flexGrow: 0, display: 'flex' }}>
            {
               (expanded)
                  ? <ExpandLessIcon onClick={() => setExpanded(false)} />
                  : <ExpandMoreIcon onClick={() => setExpanded(true)} />
            }
            {
               <CustomTable>
                  <TableBody>
                     {
                        keys.map((ii, index) => {
                           if (!expanded && index) {
                              return;
                           }
                           return (
                              <CustomTableRow key={ii}>
                                 <TableCell>
                                    <Typography sx={{ ...sx, fontWeight: 'bolder' }}>
                                       {ii}
                                    </Typography>
                                 </TableCell>
                                 <TableCell>
                                    <DataValueDisplay value={{ Value: toVariant(target[ii]) }} />
                                 </TableCell>
                              </CustomTableRow>
                           );
                        })
                     }
                  </TableBody>
               </CustomTable>
            }
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
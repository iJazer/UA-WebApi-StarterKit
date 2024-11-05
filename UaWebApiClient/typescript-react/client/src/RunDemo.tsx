/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';
import * as OpcUa from 'opcua-webapi';
import * as Measurements from 'measurements';
import WebClient from './WebClient';

function variantToArgumentList(input: OpcUa.DataValue | undefined): OpcUa.Argument[] {
   const output: OpcUa.Argument[] = [];
   if (input) {
      if (input.UaType === OpcUa.BuiltInType.ExtensionObject) {
         if (Array.isArray(input.Value)) {
            input.Value.map(x => {
               const eo = OpcUa.ExtensionObjectFromJSON(x)
               if (eo.UaTypeId === OpcUa.DataTypeIds.Argument) {
                  output.push(OpcUa.ArgumentFromJSON(x));
               }
            })
         }
      }
   }
   return output;
}

const run = async (
   configuration: OpcUa.Configuration,
   reportProgress: (message: string, level?: number) => void
) => {
   const api = new WebClient(configuration);

   reportProgress("==== Browsing ObjectsFolder", 1);
   let references = await api.browseChildren(OpcUa.ObjectIds.ObjectsFolder);
   references?.map(ii => {
      reportProgress(`${ii?.DisplayName?.Text} [${Object.keys(OpcUa.NodeClass).find(key => (OpcUa.NodeClass as any)[key] === ii.NodeClass)}]`);
   });

   const data = references?.find(x => (x?.BrowseName?.endsWith("Measurements") ? x : undefined));

   reportProgress("==== Browsing Data", 1);
   references = await api.browseChildren(data?.NodeId ?? '');
   references?.map(ii => {
      reportProgress(`${ii?.DisplayName?.Text} [${Object.keys(OpcUa.NodeClass).find(key => (OpcUa.NodeClass as any)[key] === ii.NodeClass)}]`);
   });

   reportProgress("==== Read Data", 1);
   let variableIds = references?.filter(x => x.NodeClass === OpcUa.NodeClass.Variable).map(x => x.NodeId ?? '') ?? [];
   let values = await api.readValues(variableIds);

   let valuesToWrite: OpcUa.WriteValue[] = [];
   const variables: OpcUa.ReferenceDescription[] = [];
   values?.map((value, index) => {
      const node = references?.find(x => x.NodeId === variableIds[index]);
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(value.Value)}`);
      if (value?.UaType === OpcUa.BuiltInType.Double) {
         if (node) variables.push(node);
         valuesToWrite.push(OpcUa.WriteValueFromJSON({
            NodeId: node?.NodeId,
            AttributeId: OpcUa.Attributes.Value,
            Value: OpcUa.DataValueFromJSON({
               UaType: Number(OpcUa.BuiltInType.Double),
               Value: Number(value?.Value ?? 0.0) + 1.0
            })
         }));
      }
   });
   variableIds = variables.map(x => x.NodeId ?? '');

   reportProgress("==== Write Data", 1);
   const results = await api.writeValues(valuesToWrite ?? []);
   results?.map((value, index) => {
      const node = references?.[index];
      reportProgress(`${node?.DisplayName?.Text} [${OpcUa.StatusCodes[value?.Code ?? 0]}]`);
   });

   reportProgress("==== Read Back Data", 1);
   values = await api.readValues(variableIds);
   values?.map((value, index) => {
      const node = variables?.[index];
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(value.Value)}`);
   });

   reportProgress("==== Browsing Method Arguments", 1);
   const method = references?.find(x => x.NodeClass === OpcUa.NodeClass.Method);
   const properties = await api.browseChildren(method?.NodeId ?? '');
   const inputArguments = properties?.find(x => x.BrowseName === OpcUa.BrowseNames.InputArguments);
   const outputArguments = properties?.find(x => x.BrowseName === OpcUa.BrowseNames.OutputArguments);

   reportProgress("==== Reading Method Arguments", 1);
   values = await api.readValues([inputArguments?.NodeId ?? '', outputArguments?.NodeId ?? '']);

   reportProgress(`${method?.DisplayName?.Text}(`);
   let inputArgumentDefinitions = null;
   if (inputArguments) {
      inputArgumentDefinitions = variantToArgumentList(values?.[0]);
      inputArgumentDefinitions.map(x => {
         reportProgress(`   [in]  ${Object.keys(OpcUa.DataTypeIds).find(key => (OpcUa.DataTypeIds as any)[key] === x.DataType)} ${x.Name}`);
      });
   }
   let outputArgumentDefinitions = null;
   if (outputArguments) {
      outputArgumentDefinitions = variantToArgumentList(values?.[1]);
      outputArgumentDefinitions.map(x => {
         reportProgress(`   [out] ${Object.keys(OpcUa.DataTypeIds).find(key => (OpcUa.DataTypeIds as any)[key] === x.DataType)} ${x.Name}`);
      });
   }
   reportProgress(`);`);

   const inputs: OpcUa.Variant[] = [
      { Body: 40.0, UaType: Number(OpcUa.BuiltInType.Double) },
      { Body: 80.0, UaType: Number(OpcUa.BuiltInType.Double) }
   ];

   reportProgress("==== Call Method", 1);

   inputs.map((value, index) => {
      const args = inputArgumentDefinitions?.[index];
      reportProgress(`${args?.Name} = ${JSON.stringify(value.Body)}`);
   });

   const outputs = await api.call(data?.NodeId ?? '', method?.NodeId ?? '', inputs);

   outputs?.map((value, index) => {
      const args = outputArgumentDefinitions?.[index];
      reportProgress(`${args?.Name} = ${JSON.stringify(value.Body)}`);
   });

   reportProgress("==== Read Back Data", 1);
   values = await api.readValues(variableIds);
   values?.map((value, index) => {
      const node = variables?.[index];
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(value.Value)}`);
   });

   reportProgress('==== Read Complex Data', 1);
   const complexVariables = references?.filter(x => x.BrowseName?.includes(Measurements.BrowseNames.Orientation)) ?? [];
   values = await api.readValues(complexVariables.map(x => x.NodeId ?? ''));
   valuesToWrite = [];

   for (let ii = 0; ii < complexVariables.length; ii++) {
      let orientation: Measurements.OrientationDataType = { X: 1, Y: 1, Rotation: 90 };

      if (values?.[ii].Value) {
         orientation = Measurements.OrientationDataTypeFromJSON(values?.[ii].Value);
         reportProgress(`   ProfileName = ${orientation.ProfileName}`);
         reportProgress(`   X = ${orientation.X}`);
         reportProgress(`   Y = ${orientation.Y}`);
         reportProgress(`   Rotation = ${orientation.Rotation}`);
      }

      orientation.X = !orientation.X ? 1 : orientation.X + 1.0;
      orientation.Y = !orientation.Y ? 1 : orientation.Y + 1.0;
      orientation.Rotation = !orientation.Rotation ? 1 : orientation.Rotation + 1.0;

      valuesToWrite.push({
         NodeId: complexVariables[ii].NodeId,
         AttributeId: OpcUa.Attributes.Value,
         Value: {
               UaType: OpcUa.BuiltInType.ExtensionObject,
               Value:  {
                  UaTypeId: Measurements.DataTypeIds.OrientationDataType,
                  ...orientation
              }
            }
         }
      );
   }

   reportProgress('==== Write Complex Data', 1);
   const writeResults = await api.writeValues(valuesToWrite);

   for (let ii = 0; ii < complexVariables.length; ii++) {
      reportProgress(`${complexVariables[ii].DisplayName?.Text}: ${OpcUa.StatusCodes[writeResults?.[ii]?.Code ?? 0]}`);
   }

   reportProgress('==== Read Back Complex Data', 1);

   // Read the values again to verify the write operation
   values = await api.readValues(complexVariables.map(x => x.NodeId ?? ''));

   for (let ii = 0; ii < complexVariables.length; ii++) {
      const node = complexVariables[ii];
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(values?.[ii].Value)}`);
   }
}

export interface RunDemoProps {
   configuration: OpcUa.Configuration
   isRunning: boolean
}

export const RunDemo = ({ configuration, isRunning }: RunDemoProps) => {
   const [inProgress, setInProgress] = React.useState<boolean>(false);
   const [logs, setLogs] = React.useState<React.ReactNode[]>([]);

   const handleUpdate = (message: string, level?: number): void => {
      setLogs((prevLogs) => [...prevLogs, <span style={{ color: (level && level > 0) ? 'blue' : 'black' }}>{message}</span>]);
   };

   React.useEffect(() => {
      setInProgress(isRunning);
      if (isRunning) {
         setLogs([]);
      }
   }, [isRunning]);

   React.useEffect(() => {
      if (inProgress) {
         const performOperation = async () => {
            await run(configuration, handleUpdate);
            setLogs((prevLogs) => [...prevLogs, <span style={{ color: 'blue' }}>Operation complete!</span>]);
            setInProgress(false);
         };
         performOperation();
      }
   }, [inProgress, configuration]);

   return (
      <div style={{ padding: '20px', maxWidth: '500px', margin: '0 auto', fontFamily: 'monospace', textAlign: 'left' }}>
         {logs.length > 0 ? (
            logs.map((log, index) => <p style={{ padding: '1px', margin: '1px' }} key={index}>{log}</p>)
         ) : (
               <p style={{ padding: '1px', margin: '1px' }}><span style={{ color: 'red' }}>No logs yet...</span></p>
         )}
      </div>
   );
};

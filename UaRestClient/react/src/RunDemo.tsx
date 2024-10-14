/* eslint-disable @typescript-eslint/no-explicit-any */
import React from 'react';
import * as OpcUa from 'opcua-webapi';
import WebClient from './WebClient';

function variantToArgumentList(input: OpcUa.Variant | undefined): OpcUa.Argument[] {
   const output: OpcUa.Argument[] = [];
   if (input) {
      if (input.Type === OpcUa.BuiltInType.ExtensionObject) {
         if (Array.isArray(input.Body)) {
            input.Body.map(x => {
               const eo = OpcUa.ExtensionObjectFromJSON(x)
               if (eo.TypeId === OpcUa.DataTypeIds.Argument) {
                  output.push(OpcUa.ArgumentFromJSON(eo.Body));
               }
            })
         }
      }
   }
   return output;
}

const run = async (
   configuration: OpcUa.Configuration,
   reportProgress: (message: string) => void
) => {
   const api = new WebClient(configuration);

   reportProgress("==== Browsing ObjectsFolder");
   let references = await api.browseChildren(OpcUa.ObjectIds.ObjectsFolder);
   references?.map(ii => {
      reportProgress(`${ii?.DisplayName?.Text} [${Object.keys(OpcUa.NodeClass).find(key => (OpcUa.NodeClass as any)[key] === ii.NodeClass)}]`);
   });

   const data = references?.find(x => (x?.BrowseName?.endsWith("Data") ? x : undefined));

   reportProgress("==== Browsing Data");
   references = await api.browseChildren(data?.NodeId ?? '');
   references?.map(ii => {
      reportProgress(`${ii?.DisplayName?.Text} [${Object.keys(OpcUa.NodeClass).find(key => (OpcUa.NodeClass as any)[key] === ii.NodeClass)}]`);
   });

   reportProgress("==== Read Data");
   const variableIds = references?.filter(x => x.NodeClass === OpcUa.NodeClass.Variable).map(x => x.NodeId ?? '') ?? [];
   let values = await api.readValues(variableIds);

   const valuesToWrite: OpcUa.WriteValue[] = [];
   values?.map((value, index) => {
      const node = references?.[index];
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(value.Value?.Body)}`);
      valuesToWrite.push(OpcUa.WriteValueFromJSON({
         NodeId: node?.NodeId,
         AttributeId: OpcUa.Attributes.Value,
         Value: OpcUa.DataValueFromJSON({
            Value: {
               Type: Number(OpcUa.BuiltInType.Double),
               Body: Number(value?.Value?.Body ?? 0.0) + 1.0
            }
         })
      }));
   });

   reportProgress("==== Write Data");
   const results = await api.writeValues(valuesToWrite ?? []);
   results?.map((value, index) => {
      const node = references?.[index];
      reportProgress(`${node?.DisplayName?.Text} [${OpcUa.StatusCodes[value]}]`);
   });

   reportProgress("==== Read Back Data");
   values = await api.readValues(variableIds);
   values?.map((value, index) => {
      const node = references?.[index];
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(value.Value?.Body)}`);
   });

   reportProgress("==== Browsing Method Arguments");
   const method = references?.find(x => x.NodeClass === OpcUa.NodeClass.Method);
   references = await api.browseChildren(method?.NodeId ?? '');
   const inputArguments = references?.find(x => x.BrowseName === OpcUa.BrowseNames.InputArguments);
   const outputArguments = references?.find(x => x.BrowseName === OpcUa.BrowseNames.OutputArguments);

   reportProgress("==== Reading Method Arguments");
   values = await api.readValues([inputArguments?.NodeId ?? '', outputArguments?.NodeId ?? '']);

   reportProgress(`${method?.DisplayName?.Text}(`);
   let inputArgumentDefinitions = null;
   if (inputArguments) {
      inputArgumentDefinitions = variantToArgumentList(values?.[0].Value);
      inputArgumentDefinitions.map(x => {
         reportProgress(`   [in]  ${Object.keys(OpcUa.DataTypeIds).find(key => (OpcUa.DataTypeIds as any)[key] === x.DataType)} ${x.Name}`);
      });
   }
   let outputArgumentDefinitions = null;
   if (outputArguments) {
      outputArgumentDefinitions = variantToArgumentList(values?.[1].Value);
      outputArgumentDefinitions.map(x => {
         reportProgress(`   [out] ${Object.keys(OpcUa.DataTypeIds).find(key => (OpcUa.DataTypeIds as any)[key] === x.DataType)} ${x.Name}`);
      });
   }
   reportProgress(`);`);

   const inputs: OpcUa.Variant[] = [
      { Body: 40.0, Type: Number(OpcUa.BuiltInType.Double) },
      { Body: 80.0, Type: Number(OpcUa.BuiltInType.Double) }
   ];

   reportProgress("==== Call Method");

   inputs.map((value, index) => {
      const args = inputArgumentDefinitions?.[index];
      reportProgress(`${args?.Name} = ${JSON.stringify(value.Body)}`);
   });

   const outputs = await api.call(data?.NodeId ?? '', method?.NodeId ?? '', inputs);

   outputs?.map((value, index) => {
      const args = outputArgumentDefinitions?.[index];
      reportProgress(`${args?.Name} = ${JSON.stringify(value.Body)}`);
   });

   reportProgress("==== Read Back Data");
   values = await api.readValues(variableIds);
   values?.map((value, index) => {
      const node = references?.[index];
      reportProgress(`${node?.DisplayName?.Text} = ${JSON.stringify(value.Value?.Body)}`);
   });
}

export interface RunDemoProps {
   configuration: OpcUa.Configuration
   isRunning: boolean
}

export const RunDemo = ({ configuration, isRunning }: RunDemoProps) => {
   const [inProgress, setInProgress] = React.useState<boolean>(false);
   const [logs, setLogs] = React.useState<string[]>([]);

   const handleUpdate = (message: string): void => {
      setLogs((prevLogs) => [...prevLogs, message]);
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
            setLogs((prevLogs) => [...prevLogs, 'Operation complete!']);
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
               <p style={{ padding: '1px', margin: '1px' }}>No logs yet...</p>
         )}
      </div>
   );
};

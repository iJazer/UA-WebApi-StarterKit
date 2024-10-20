'use strict';
const https = require('https');
const OpcUa = require('opcua-webapi');
const WebClient = require('./WebClient.js');
const Measurements = require('../Measurements');

const testUserName = 'user1';
const testUserPassword = 'password1';
const allowLocalServerDebugging = false;
const defaultUrl = (allowLocalServerDebugging) ? "https://localhost:44429/opcua" : "https://opcua-rest-dashboard.azurewebsites.net/opcua";
  
const configuration = new OpcUa.Configuration({
    basePath: defaultUrl,

    fetchApi: (url, init) => {
        init.agent = (allowLocalServerDebugging) ? new https.Agent({ rejectUnauthorized: false }) : undefined;
        const authorization = {
            'Authorization': `Basic ${btoa(`${testUserName}:${testUserPassword}`)}`
        };
        init.headers = { ...init.headers, ...authorization };
        return fetch(url, init);
    }
});

function variantToArgumentList(input)
{
    const output = [];
    if (input)
    {
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

const Run = async () => {
    const api = new WebClient(configuration);

    console.log("==== Browsing ObjectsFolder");
    let references = await api.browseChildren(OpcUa.ObjectIds.ObjectsFolder);
    references.map(ii => {
        console.log(`${ii.DisplayName.Text} [${Object.keys(OpcUa.NodeClass).find(key => OpcUa.NodeClass[key] === ii.NodeClass)}]`);
    });

    const data = references.find(x => (x.BrowseName.endsWith("Measurements") ? x: undefined));

    console.log("==== Browsing Data");
    references = await api.browseChildren(data.NodeId);
    references.map(ii => {
        console.log(`${ii.DisplayName.Text} [${Object.keys(OpcUa.NodeClass).find(key => OpcUa.NodeClass[key] === ii.NodeClass)}]`);
    });

    console.log("==== Read Data");
    const variableIds = references.filter(x => x.NodeClass === OpcUa.NodeClass.Variable).map(x => x.NodeId);
    let values = await api.readValues(variableIds);

    let valuesToWrite = [];
    const variables = [];
    values.map((value, index) => {
        const node = references.find(x => x.NodeId === variableIds[index])
        variables.push(node);
        console.log(`${node.DisplayName.Text} = ${JSON.stringify(value.Value?.Body)}`);
        if (OpcUa.BuiltInType.Double === value.Value?.Type)
        {
            valuesToWrite.push(OpcUa.WriteValueFromJSON({
                NodeId: node.NodeId,
                AttributeId: OpcUa.Attributes.Value,
                Value: OpcUa.DataValueFromJSON({
                    Value: {
                        Type: Number(OpcUa.BuiltInType.Double),
                        Body: Number(value.Value.Body) + 1.0
                    }
                })
            }));
        }
    });

    console.log("==== Write Data");
    let results = await api.writeValues(valuesToWrite);
    results.map((value, index) => {
        const node = references.find(x => x.NodeId === valuesToWrite[index].NodeId)
        console.log(`${node.DisplayName.Text} [${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === value)}]`);
    });

    console.log("==== Read Back Data");
    values = await api.readValues(variableIds);
    values.map((value, index) => {
        const node = references.find(x => x.NodeId === variableIds[index])
        console.log(`${node.DisplayName.Text} = ${JSON.stringify(value.Value?.Body)}`);
    });

    console.log("==== Browsing Method Arguments");
    const method = references.find(x => x.NodeClass === OpcUa.NodeClass.Method);
    references = await api.browseChildren(method.NodeId);
    const inputArguments = references.find(x => x.BrowseName === OpcUa.BrowseNames.InputArguments);
    const outputArguments = references.find(x =>x.BrowseName === OpcUa.BrowseNames.OutputArguments);

    console.log("==== Reading Method Arguments");
    values = await api.readValues([inputArguments?.NodeId, outputArguments?.NodeId]);

    console.log(`${method.DisplayName.Text}(`);
    let inputArgumentDefinitions = null;
    if (inputArguments) {
        inputArgumentDefinitions = variantToArgumentList(values[0].Value);
        inputArgumentDefinitions.map(x => {
            console.log(`   [in]  ${Object.keys(OpcUa.DataTypeIds).find(key => OpcUa.DataTypeIds[key] === x.DataType)} ${x.Name}`);
        });
    }
    let outputArgumentDefinitions = null;
    if (outputArguments) {
        outputArgumentDefinitions = variantToArgumentList(values[1].Value);
        outputArgumentDefinitions.map(x => {
            console.log(`   [out] ${Object.keys(OpcUa.DataTypeIds).find(key => OpcUa.DataTypeIds[key] === x.DataType)} ${x.Name}`);
        });
    }
    console.log(`);`);

    const inputs = [
        { Body: 40.0, Type: Number(OpcUa.BuiltInType.Double) },
        { Body: 80.0, Type: Number(OpcUa.BuiltInType.Double) }
    ];

    console.log("==== Call Method");

    inputs.map((value, index) => {
        const args = inputArgumentDefinitions[index];
        console.log(`${args.Name} = ${JSON.stringify(value.Body)}`);
    });

    const outputs = await api.call(data.NodeId, method.NodeId, inputs);

    outputs.map((value, index) => {
        const args = outputArgumentDefinitions[index];
        console.log(`${args.Name} = ${JSON.stringify(value.Body)}`);
    });

    console.log("==== Read Back Data");
    values = await api.readValues(variableIds);
    values.map((value, index) => {
        const node = variables[index];
        console.log(`${node.DisplayName.Text} = ${JSON.stringify(value.Value?.Body)}`);
    });

    console.log('==== Read Complex Data');

    const complexVariables = variables.filter(x => x.BrowseName.includes(Measurements.BrowseNames.Orientation));
    values = await api.readValues(complexVariables.map(x => x.NodeId));
    valuesToWrite = [];

    for (let ii = 0; ii < complexVariables.length; ii++) {
        let orientation = {};  

        if (values[ii].Value?.Body) {
            let json = values[ii].Value.Body.Body;
            orientation = Measurements.OrientationDataTypeFromJSON(json);  

            console.log(`   ProfileName = ${orientation.ProfileName}`);
            console.log(`   X = ${orientation.X}`);
            console.log(`   Y = ${orientation.Y}`);
            console.log(`   Rotation = ${orientation.Rotation}`);
        }

        orientation.X += 1.0;
        orientation.Y += 1.0;
        orientation.Rotation += 1.0;

        valuesToWrite.push({
            NodeId: complexVariables[ii].NodeId,
            AttributeId: OpcUa.Attributes.Value,  
            Value: {
                Value: {
                    Type: OpcUa.BuiltInType.ExtensionObject,  
                    Body: OpcUa.ExtensionObjectFromJSON({
                        TypeId: Measurements.DataTypeIds.OrientationDataType,
                        Body: orientation
                    })
                }
            }
        });
    }

    console.log('==== Write Complex Data');

    // Write the values back to the server
    let writeResults = await api.writeValues(valuesToWrite);

    // Log the results of the write operation
    for (let ii = 0; ii < complexVariables.length; ii++) {
        console.log(`${complexVariables[ii].DisplayName?.Text}: ${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === writeResults[ii])}`);
    }

    console.log('==== Read Back Complex Data');

    // Read the values again to verify the write operation
    values = await api.readValues(complexVariables.map(x => x.NodeId));

    for (let ii = 0; ii < complexVariables.length; ii++) {
        const node = complexVariables[ii];
        console.log(`${node.DisplayName.Text} = ${JSON.stringify(values[ii].Value?.Body)}`);
    }
};
  
Run().then(() => console.log("==== Done!!"));
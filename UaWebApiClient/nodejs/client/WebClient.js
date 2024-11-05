'use strict';
const OpcUa = require('opcua-webapi');

class WebClient {
    constructor(configuration) {
        this.configuration = configuration;
        this.requestHandle = 1;
        this.api = new OpcUa.DefaultApi(configuration);
    }

    async browseChildren(nodeId)
    {
        var request = new OpcUa.BrowseRequestFromJSON({
            RequestHeader: {
                RequestHandle: this.requestHandle++,
                TimeoutHint: 100000,
                Timestamp: new Date().toISOString()            
            },
            NodesToBrowse: [
                {
                    NodeId: nodeId,
                    BrowseDirection: OpcUa.BrowseDirection.Forward,
                    ReferenceTypeId: OpcUa.ReferenceTypeIds.HierarchicalReferences,
                    IncludeSubtypes: true,
                    NodeClassMask: Number(OpcUa.NodeClass.Object | OpcUa.NodeClass.Variable | OpcUa.NodeClass.Method),
                    ResultMask: 63
                }
            ]
        }); 

        const response = await this.api.browse({ browseRequest: request });

        if (response?.ResponseHeader?.ServiceResult?.Code)
        {
            throw new Error(`Browse failed with status code ${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === response.ResponseHeader.ServiceResult.Code)}}.`);
        }

        return response.Results[0].References;
    }

    async readValues(nodeIds)
    {
        var request = new OpcUa.ReadRequestFromJSON({
            RequestHeader: {
                RequestHandle: this.requestHandle++,
                TimeoutHint: 100000,
                Timestamp: new Date().toISOString()            
            },
            NodesToRead: nodeIds.map(x => OpcUa.ReadValueIdFromJSON({ NodeId: x, AttributeId: OpcUa.Attributes.Value }))
        }); 

        const response = await this.api.read({ readRequest: request });

        if (response?.ResponseHeader?.ServiceResult?.Code)
        {
            throw new Error(`Read failed with status code ${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === response.ResponseHeader.ServiceResult.Code)}.`);
        }

        return response.Results;
    }

    async writeValues(nodesToWrite)
    {
        var request = new OpcUa.WriteRequestFromJSON({
            RequestHeader: {
                RequestHandle: this.requestHandle++,
                TimeoutHint: 100000,
                Timestamp: new Date().toISOString()            
            },
            NodesToWrite: nodesToWrite
        }); 

        const response = await this.api.write({ writeRequest: request });

        if (response?.ResponseHeader?.ServiceResult?.Code)
        {
            throw new Error(`Write failed with status code ${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === response.ResponseHeader.ServiceResult.Code)}.`);
        }

        return response.Results;
    }

    async call(objectId, methodId, inputArguments)
    {
        var request = new OpcUa.CallRequestFromJSON({
            RequestHeader: {
                RequestHandle: this.requestHandle++,
                TimeoutHint: 100000,
                Timestamp: new Date().toISOString()            
            },
            MethodsToCall: [
                {
                    ObjectId: objectId,
                    MethodId: methodId,
                    InputArguments: inputArguments
                }
            ]
        }); 

        const response = await this.api.call({ callRequest: request });

        if (response?.ResponseHeader?.ServiceResul?.Code)
        {
            throw new Error(`Call failed with status code ${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === response.ResponseHeader.ServiceResult.Code)}.`);
        }

        if (response.Results[0].StatusCode?.Code)
        {
            throw new Error(`Method return failed status ${Object.keys(OpcUa.StatusCodes).find(key => OpcUa.StatusCodes[key] === response.Results[0].StatusCode.Code)}.`);
        }

        return response.Results[0].OutputArguments;
    }
}

module.exports = WebClient;
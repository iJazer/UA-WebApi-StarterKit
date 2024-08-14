
import json
from typing import List

from generated.openapi_client import ApiClient, Configuration
from generated.openapi_client.api import DefaultApi
from generated.openapi_client.models import CallMethodRequest, CallRequestMessage, CallRequest, Variant, WriteRequestMessage, WriteRequest, ReadValueId, WriteValue, RequestHeader, BrowseRequestMessage, BrowseRequest, BrowseDescription, BrowseDirection, BrowseResult, ReadRequestMessage, ReadRequest

from opcua_attributes import *
from opcua_statuscodes import *
from opcua_constants import *

from datetime import datetime, timezone

from openapi_client.models.data_value import DataValue

class OpcUaClient:
    def __init__(self, configuration: Configuration):
        api = ApiClient(configuration)
        api.default_headers['Authorization'] = configuration.get_basic_auth_token()
        self.client = DefaultApi(api)

    def dispose(self):
        if self.client:
            self.client.close()
            self.client = None

    def deserialize(self, value, value_type):
        if value["Type"] != "ExtensionObject":
            return None

        if isinstance(value["Body"], list):
            array = [json.loads(json.dumps(item)) for item in value["Body"]]
            items = [self.deserialize(eo, value_type) for eo in array]
            return items

        return None

    def deserialize_eo(self, eo, value_type):
        if eo is None:
            return None

        return json.loads(eo["Body"])

    def browse_children(self, node_id) -> List[BrowseResult]:

        message = BrowseRequestMessage(
            service_id = int(DataTypeIds.BrowseRequest.value[2:]),
            body = BrowseRequest(
                RequestHeader=RequestHeader(
                    TimeoutHint=120000,
                    Timestamp=datetime.now(timezone.utc)
                ),
                NodesToBrowse=[
                    BrowseDescription(
                        NodeId=node_id,
                        BrowseDirection=BrowseDirection.NUMBER_0, # Forward
                        ReferenceTypeId=ReferenceTypeIds.HierarchicalReferences.value,
                        IncludeSubtypes=True,
                        NodeClassMask=0,
                        ResultMask=63
                    )
                ]
            )
        );

        response = self.client.browse(message);     

        if  StatusCode.is_bad(response.body.response_header.service_result):
            raise Exception(f"Browse failed with service level status code {get_StatusCodes_name(response.body.response_header.service_result)}")

        if  StatusCode.is_bad(response.body.results[0].status_code):
            raise Exception(f"Browse failed with operation level status code {get_StatusCodes_name(response.body.results[0].status_code)}")
        
        return response.body.results[0].references

    def read_values(self, variable_ids : List[str]) -> List[DataValue]:

        message = ReadRequestMessage(
            service_id = int(DataTypeIds.ReadRequest.value[2:]),
            body = ReadRequest(
                RequestHeader=RequestHeader(
                    TimeoutHint=120000,
                    Timestamp=datetime.now(timezone.utc)
                )
            )
        )

        message.body.nodes_to_read = []

        if variable_ids is not None:
            for x in variable_ids:
                message.body.nodes_to_read.append(ReadValueId(
                    NodeId=x,
                    AttributeId=Attributes.Value.value
                ))

        response = self.client.read(message);     

        if  StatusCode.is_bad(response.body.response_header.service_result):
            raise Exception(f"Read failed with service level status code {get_StatusCodes_name(response.body.response_header.service_result)}")
        
        return response.body.results
    
    def write_values(self, values_to_write : List[WriteValue]) -> List[int]:

        message = WriteRequestMessage(
            service_id = int(DataTypeIds.WriteRequest.value[2:]),
            body = WriteRequest(
                RequestHeader=RequestHeader(
                    TimeoutHint=120000,
                    Timestamp=datetime.now(timezone.utc)
                ),
                NodesToWrite=values_to_write
            )
        )

        response = self.client.write(message);     

        if  StatusCode.is_bad(response.body.response_header.service_result):
            raise Exception(f"Write failed with service level status code {get_StatusCodes_name(response.body.response_header.service_result)}")
        
        return response.body.results
    
    def call(self, object_id, method_id, inputs : List[Variant]) -> List[Variant]:

        message = CallRequestMessage(
            service_id = int(DataTypeIds.CallRequest.value[2:]),
            body = CallRequest(
                RequestHeader=RequestHeader(
                    TimeoutHint=120000,
                   # Timestamp=datetime.now(timezone.utc)
                ),
                MethodsToCall=[CallMethodRequest(
                    ObjectId=object_id,
                    MethodId=method_id,
                    InputArguments=inputs
                )]
            )
        )

        json = CallRequestMessage.to_json(message)
        response = self.client.call(message);     

        if  StatusCode.is_bad(response.body.response_header.service_result):
            raise Exception(f"Call failed with service level status code {get_StatusCodes_name(response.body.response_header.service_result)}")
        
        if  StatusCode.is_bad(response.body.results[0].status_code):
            raise Exception(f"Call failed with operation level status code {get_StatusCodes_name(response.body.results[0].status_code)}")
        
        return response.body.results[0].output_arguments
    
    # async def call(self, object_id, method_id, inputs):
    #     message = {
    #         "body": {
    #             "RequestHeader": {
    #                 "TimeoutHint": 100000,
    #                 "Timestamp": datetime.utcnow().isoformat() + 'Z'
    #             },
    #             "MethodsToCall": [
    #                 {
    #                     "ObjectId": object_id,
    #                     "MethodId": method_id,
    #                     "InputArguments": inputs
    #                 }
    #             ]
    #         }
    #     }

    #     response = await self.post_async("call", message)

    #     if response["Body"]["ResponseHeader"]["ServiceResult"] != 0:
    #         raise Exception(f"Call failed with status code {response['Body']['ResponseHeader']['ServiceResult']}")

    #     if response["Body"]["Results"][0]["StatusCode"] != 0:
    #         raise Exception(f"Method return failed with status {response['Body']['Results'][0]['StatusCode']}")

    #     return [json.loads(x) for x in response["Body"]["Results"][0]["OutputArguments"]]

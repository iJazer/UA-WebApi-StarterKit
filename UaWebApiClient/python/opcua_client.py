
import json
from typing import List
from datetime import datetime, timezone
from opcua_webapi import *

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

        request = BrowseRequest(
            RequestHeader=RequestHeader(
                TimeoutHint=120000,
                Timestamp=datetime.now(timezone.utc)
            ),
            NodesToBrowse=[
                BrowseDescription(
                    NodeId=node_id,
                    BrowseDirection=BrowseDirection.Forward,
                    ReferenceTypeId=ReferenceTypeIds.HierarchicalReferences.value,
                    IncludeSubtypes=True,
                    NodeClassMask=0,
                    ResultMask=63
                )
            ]
        );

        response = self.client.browse(request);     

        if  StatusUtils.is_bad(response.response_header.service_result):
            raise Exception(f"Browse failed with service level status code {StatusUtils.to_text(response.response_header.service_result)}")

        if  StatusUtils.is_bad(response.results[0].status_code):
            raise Exception(f"Browse failed with operation level status code {StatusUtils.to_text(response.results[0].status_code)}")
        
        return response.results[0].references

    def read_values(self, variable_ids : List[str]) -> List[DataValue]:

        request = ReadRequest(
            RequestHeader=RequestHeader(
                TimeoutHint=120000,
                Timestamp=datetime.now(timezone.utc)
            )
        )

        request.nodes_to_read = []

        if variable_ids is not None:
            for x in variable_ids:
                request.nodes_to_read.append(ReadValueId(
                    NodeId=x,
                    AttributeId=Attributes.Value.value
                ))

        response = self.client.read(request);     

        if  StatusUtils.is_bad(response.response_header.service_result):
            raise Exception(f"Read failed with service level status code {StatusUtils.to_text(response.response_header.service_result)}")
        
        return response.results
    
    def write_values(self, values_to_write : List[WriteValue]) -> List[StatusCode]:

        request = WriteRequest(
            RequestHeader=RequestHeader(
                TimeoutHint=120000,
                Timestamp=datetime.now(timezone.utc)
            ),
            NodesToWrite=values_to_write
        )

        response = self.client.write(request);     

        if  StatusUtils.is_bad(response.response_header.service_result):
            raise Exception(f"Write failed with service level status code {StatusUtils.to_text(response.response_header.service_result)}")
        
        return response.results
    
    def call(self, object_id, method_id, inputs : List[Variant]) -> List[Variant]:

        request = CallRequest(
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

        response = self.client.call(request);     

        if  StatusUtils.is_bad(response.response_header.service_result):
            raise Exception(f"Call failed with service level status code {StatusUtils.to_text(response.response_header.service_result)}")
        
        if  StatusUtils.is_bad(response.results[0].status_code):
            raise Exception(f"Call failed with operation level status code {StatusUtils.to_text(response.results[0].status_code)}")
        
        return response.results[0].output_arguments
    
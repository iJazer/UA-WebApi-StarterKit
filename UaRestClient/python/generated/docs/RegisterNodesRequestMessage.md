# RegisterNodesRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**RegisterNodesRequest**](RegisterNodesRequest.md) |  | 

## Example

```python
from openapi_client.models.register_nodes_request_message import RegisterNodesRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of RegisterNodesRequestMessage from a JSON string
register_nodes_request_message_instance = RegisterNodesRequestMessage.from_json(json)
# print the JSON string representation of the object
print RegisterNodesRequestMessage.to_json()

# convert the object into a dict
register_nodes_request_message_dict = register_nodes_request_message_instance.to_dict()
# create an instance of RegisterNodesRequestMessage from a dict
register_nodes_request_message_form_dict = register_nodes_request_message.from_dict(register_nodes_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



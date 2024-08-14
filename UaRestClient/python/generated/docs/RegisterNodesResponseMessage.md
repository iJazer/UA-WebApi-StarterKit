# RegisterNodesResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**RegisterNodesResponse**](RegisterNodesResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.register_nodes_response_message import RegisterNodesResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of RegisterNodesResponseMessage from a JSON string
register_nodes_response_message_instance = RegisterNodesResponseMessage.from_json(json)
# print the JSON string representation of the object
print RegisterNodesResponseMessage.to_json()

# convert the object into a dict
register_nodes_response_message_dict = register_nodes_response_message_instance.to_dict()
# create an instance of RegisterNodesResponseMessage from a dict
register_nodes_response_message_form_dict = register_nodes_response_message.from_dict(register_nodes_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



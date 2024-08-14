# UnregisterNodesResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**UnregisterNodesResponse**](UnregisterNodesResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.unregister_nodes_response_message import UnregisterNodesResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of UnregisterNodesResponseMessage from a JSON string
unregister_nodes_response_message_instance = UnregisterNodesResponseMessage.from_json(json)
# print the JSON string representation of the object
print UnregisterNodesResponseMessage.to_json()

# convert the object into a dict
unregister_nodes_response_message_dict = unregister_nodes_response_message_instance.to_dict()
# create an instance of UnregisterNodesResponseMessage from a dict
unregister_nodes_response_message_form_dict = unregister_nodes_response_message.from_dict(unregister_nodes_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



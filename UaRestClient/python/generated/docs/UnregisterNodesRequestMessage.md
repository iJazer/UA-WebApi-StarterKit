# UnregisterNodesRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**UnregisterNodesRequest**](UnregisterNodesRequest.md) |  | 

## Example

```python
from openapi_client.models.unregister_nodes_request_message import UnregisterNodesRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of UnregisterNodesRequestMessage from a JSON string
unregister_nodes_request_message_instance = UnregisterNodesRequestMessage.from_json(json)
# print the JSON string representation of the object
print UnregisterNodesRequestMessage.to_json()

# convert the object into a dict
unregister_nodes_request_message_dict = unregister_nodes_request_message_instance.to_dict()
# create an instance of UnregisterNodesRequestMessage from a dict
unregister_nodes_request_message_form_dict = unregister_nodes_request_message.from_dict(unregister_nodes_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



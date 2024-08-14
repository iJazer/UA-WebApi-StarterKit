# TranslateBrowsePathsToNodeIdsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**TranslateBrowsePathsToNodeIdsRequest**](TranslateBrowsePathsToNodeIdsRequest.md) |  | 

## Example

```python
from openapi_client.models.translate_browse_paths_to_node_ids_request_message import TranslateBrowsePathsToNodeIdsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of TranslateBrowsePathsToNodeIdsRequestMessage from a JSON string
translate_browse_paths_to_node_ids_request_message_instance = TranslateBrowsePathsToNodeIdsRequestMessage.from_json(json)
# print the JSON string representation of the object
print TranslateBrowsePathsToNodeIdsRequestMessage.to_json()

# convert the object into a dict
translate_browse_paths_to_node_ids_request_message_dict = translate_browse_paths_to_node_ids_request_message_instance.to_dict()
# create an instance of TranslateBrowsePathsToNodeIdsRequestMessage from a dict
translate_browse_paths_to_node_ids_request_message_form_dict = translate_browse_paths_to_node_ids_request_message.from_dict(translate_browse_paths_to_node_ids_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



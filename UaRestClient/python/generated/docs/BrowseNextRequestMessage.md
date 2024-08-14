# BrowseNextRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**BrowseNextRequest**](BrowseNextRequest.md) |  | 

## Example

```python
from openapi_client.models.browse_next_request_message import BrowseNextRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of BrowseNextRequestMessage from a JSON string
browse_next_request_message_instance = BrowseNextRequestMessage.from_json(json)
# print the JSON string representation of the object
print BrowseNextRequestMessage.to_json()

# convert the object into a dict
browse_next_request_message_dict = browse_next_request_message_instance.to_dict()
# create an instance of BrowseNextRequestMessage from a dict
browse_next_request_message_form_dict = browse_next_request_message.from_dict(browse_next_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



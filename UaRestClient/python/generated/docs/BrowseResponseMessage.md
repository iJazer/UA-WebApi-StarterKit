# BrowseResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**BrowseResponse**](BrowseResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.browse_response_message import BrowseResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of BrowseResponseMessage from a JSON string
browse_response_message_instance = BrowseResponseMessage.from_json(json)
# print the JSON string representation of the object
print BrowseResponseMessage.to_json()

# convert the object into a dict
browse_response_message_dict = browse_response_message_instance.to_dict()
# create an instance of BrowseResponseMessage from a dict
browse_response_message_form_dict = browse_response_message.from_dict(browse_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



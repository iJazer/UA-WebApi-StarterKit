# BrowseNextResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**BrowseNextResponse**](BrowseNextResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.browse_next_response_message import BrowseNextResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of BrowseNextResponseMessage from a JSON string
browse_next_response_message_instance = BrowseNextResponseMessage.from_json(json)
# print the JSON string representation of the object
print BrowseNextResponseMessage.to_json()

# convert the object into a dict
browse_next_response_message_dict = browse_next_response_message_instance.to_dict()
# create an instance of BrowseNextResponseMessage from a dict
browse_next_response_message_form_dict = browse_next_response_message.from_dict(browse_next_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



# CancelResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CancelResponse**](CancelResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.cancel_response_message import CancelResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CancelResponseMessage from a JSON string
cancel_response_message_instance = CancelResponseMessage.from_json(json)
# print the JSON string representation of the object
print CancelResponseMessage.to_json()

# convert the object into a dict
cancel_response_message_dict = cancel_response_message_instance.to_dict()
# create an instance of CancelResponseMessage from a dict
cancel_response_message_form_dict = cancel_response_message.from_dict(cancel_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



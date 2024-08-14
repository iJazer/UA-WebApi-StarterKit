# CancelRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CancelRequest**](CancelRequest.md) |  | 

## Example

```python
from openapi_client.models.cancel_request_message import CancelRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CancelRequestMessage from a JSON string
cancel_request_message_instance = CancelRequestMessage.from_json(json)
# print the JSON string representation of the object
print CancelRequestMessage.to_json()

# convert the object into a dict
cancel_request_message_dict = cancel_request_message_instance.to_dict()
# create an instance of CancelRequestMessage from a dict
cancel_request_message_form_dict = cancel_request_message.from_dict(cancel_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



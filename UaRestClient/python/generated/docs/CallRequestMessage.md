# CallRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CallRequest**](CallRequest.md) |  | 

## Example

```python
from openapi_client.models.call_request_message import CallRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CallRequestMessage from a JSON string
call_request_message_instance = CallRequestMessage.from_json(json)
# print the JSON string representation of the object
print CallRequestMessage.to_json()

# convert the object into a dict
call_request_message_dict = call_request_message_instance.to_dict()
# create an instance of CallRequestMessage from a dict
call_request_message_form_dict = call_request_message.from_dict(call_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



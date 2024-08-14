# CloseSessionRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CloseSessionRequest**](CloseSessionRequest.md) |  | 

## Example

```python
from openapi_client.models.close_session_request_message import CloseSessionRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CloseSessionRequestMessage from a JSON string
close_session_request_message_instance = CloseSessionRequestMessage.from_json(json)
# print the JSON string representation of the object
print CloseSessionRequestMessage.to_json()

# convert the object into a dict
close_session_request_message_dict = close_session_request_message_instance.to_dict()
# create an instance of CloseSessionRequestMessage from a dict
close_session_request_message_form_dict = close_session_request_message.from_dict(close_session_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



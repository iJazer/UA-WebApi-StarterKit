# CallResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CallResponse**](CallResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.call_response_message import CallResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CallResponseMessage from a JSON string
call_response_message_instance = CallResponseMessage.from_json(json)
# print the JSON string representation of the object
print CallResponseMessage.to_json()

# convert the object into a dict
call_response_message_dict = call_response_message_instance.to_dict()
# create an instance of CallResponseMessage from a dict
call_response_message_form_dict = call_response_message.from_dict(call_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



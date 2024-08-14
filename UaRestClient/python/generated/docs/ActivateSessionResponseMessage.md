# ActivateSessionResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**ActivateSessionResponse**](ActivateSessionResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.activate_session_response_message import ActivateSessionResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of ActivateSessionResponseMessage from a JSON string
activate_session_response_message_instance = ActivateSessionResponseMessage.from_json(json)
# print the JSON string representation of the object
print ActivateSessionResponseMessage.to_json()

# convert the object into a dict
activate_session_response_message_dict = activate_session_response_message_instance.to_dict()
# create an instance of ActivateSessionResponseMessage from a dict
activate_session_response_message_form_dict = activate_session_response_message.from_dict(activate_session_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



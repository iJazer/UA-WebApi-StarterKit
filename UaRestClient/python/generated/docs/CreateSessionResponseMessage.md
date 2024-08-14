# CreateSessionResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CreateSessionResponse**](CreateSessionResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.create_session_response_message import CreateSessionResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CreateSessionResponseMessage from a JSON string
create_session_response_message_instance = CreateSessionResponseMessage.from_json(json)
# print the JSON string representation of the object
print CreateSessionResponseMessage.to_json()

# convert the object into a dict
create_session_response_message_dict = create_session_response_message_instance.to_dict()
# create an instance of CreateSessionResponseMessage from a dict
create_session_response_message_form_dict = create_session_response_message.from_dict(create_session_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



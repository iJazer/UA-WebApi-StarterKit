# SetTriggeringResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**SetTriggeringResponse**](SetTriggeringResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.set_triggering_response_message import SetTriggeringResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of SetTriggeringResponseMessage from a JSON string
set_triggering_response_message_instance = SetTriggeringResponseMessage.from_json(json)
# print the JSON string representation of the object
print SetTriggeringResponseMessage.to_json()

# convert the object into a dict
set_triggering_response_message_dict = set_triggering_response_message_instance.to_dict()
# create an instance of SetTriggeringResponseMessage from a dict
set_triggering_response_message_form_dict = set_triggering_response_message.from_dict(set_triggering_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



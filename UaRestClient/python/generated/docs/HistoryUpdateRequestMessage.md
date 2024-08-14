# HistoryUpdateRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**HistoryUpdateRequest**](HistoryUpdateRequest.md) |  | 

## Example

```python
from openapi_client.models.history_update_request_message import HistoryUpdateRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of HistoryUpdateRequestMessage from a JSON string
history_update_request_message_instance = HistoryUpdateRequestMessage.from_json(json)
# print the JSON string representation of the object
print HistoryUpdateRequestMessage.to_json()

# convert the object into a dict
history_update_request_message_dict = history_update_request_message_instance.to_dict()
# create an instance of HistoryUpdateRequestMessage from a dict
history_update_request_message_form_dict = history_update_request_message.from_dict(history_update_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



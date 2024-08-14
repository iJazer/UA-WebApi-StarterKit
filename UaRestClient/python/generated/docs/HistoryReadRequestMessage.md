# HistoryReadRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**HistoryReadRequest**](HistoryReadRequest.md) |  | 

## Example

```python
from openapi_client.models.history_read_request_message import HistoryReadRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of HistoryReadRequestMessage from a JSON string
history_read_request_message_instance = HistoryReadRequestMessage.from_json(json)
# print the JSON string representation of the object
print HistoryReadRequestMessage.to_json()

# convert the object into a dict
history_read_request_message_dict = history_read_request_message_instance.to_dict()
# create an instance of HistoryReadRequestMessage from a dict
history_read_request_message_form_dict = history_read_request_message.from_dict(history_read_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



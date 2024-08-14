# HistoryReadResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**HistoryReadResponse**](HistoryReadResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.history_read_response_message import HistoryReadResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of HistoryReadResponseMessage from a JSON string
history_read_response_message_instance = HistoryReadResponseMessage.from_json(json)
# print the JSON string representation of the object
print HistoryReadResponseMessage.to_json()

# convert the object into a dict
history_read_response_message_dict = history_read_response_message_instance.to_dict()
# create an instance of HistoryReadResponseMessage from a dict
history_read_response_message_form_dict = history_read_response_message.from_dict(history_read_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



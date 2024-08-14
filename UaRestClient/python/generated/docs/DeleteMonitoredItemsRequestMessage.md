# DeleteMonitoredItemsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**DeleteMonitoredItemsRequest**](DeleteMonitoredItemsRequest.md) |  | 

## Example

```python
from openapi_client.models.delete_monitored_items_request_message import DeleteMonitoredItemsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of DeleteMonitoredItemsRequestMessage from a JSON string
delete_monitored_items_request_message_instance = DeleteMonitoredItemsRequestMessage.from_json(json)
# print the JSON string representation of the object
print DeleteMonitoredItemsRequestMessage.to_json()

# convert the object into a dict
delete_monitored_items_request_message_dict = delete_monitored_items_request_message_instance.to_dict()
# create an instance of DeleteMonitoredItemsRequestMessage from a dict
delete_monitored_items_request_message_form_dict = delete_monitored_items_request_message.from_dict(delete_monitored_items_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



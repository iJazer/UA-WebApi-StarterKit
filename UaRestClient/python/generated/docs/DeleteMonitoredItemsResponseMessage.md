# DeleteMonitoredItemsResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**DeleteMonitoredItemsResponse**](DeleteMonitoredItemsResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.delete_monitored_items_response_message import DeleteMonitoredItemsResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of DeleteMonitoredItemsResponseMessage from a JSON string
delete_monitored_items_response_message_instance = DeleteMonitoredItemsResponseMessage.from_json(json)
# print the JSON string representation of the object
print DeleteMonitoredItemsResponseMessage.to_json()

# convert the object into a dict
delete_monitored_items_response_message_dict = delete_monitored_items_response_message_instance.to_dict()
# create an instance of DeleteMonitoredItemsResponseMessage from a dict
delete_monitored_items_response_message_form_dict = delete_monitored_items_response_message.from_dict(delete_monitored_items_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



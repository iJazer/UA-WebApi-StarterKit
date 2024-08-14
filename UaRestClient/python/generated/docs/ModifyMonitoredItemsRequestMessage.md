# ModifyMonitoredItemsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**ModifyMonitoredItemsRequest**](ModifyMonitoredItemsRequest.md) |  | 

## Example

```python
from openapi_client.models.modify_monitored_items_request_message import ModifyMonitoredItemsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of ModifyMonitoredItemsRequestMessage from a JSON string
modify_monitored_items_request_message_instance = ModifyMonitoredItemsRequestMessage.from_json(json)
# print the JSON string representation of the object
print ModifyMonitoredItemsRequestMessage.to_json()

# convert the object into a dict
modify_monitored_items_request_message_dict = modify_monitored_items_request_message_instance.to_dict()
# create an instance of ModifyMonitoredItemsRequestMessage from a dict
modify_monitored_items_request_message_form_dict = modify_monitored_items_request_message.from_dict(modify_monitored_items_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



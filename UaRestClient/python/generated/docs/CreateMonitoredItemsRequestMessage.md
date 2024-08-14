# CreateMonitoredItemsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CreateMonitoredItemsRequest**](CreateMonitoredItemsRequest.md) |  | 

## Example

```python
from openapi_client.models.create_monitored_items_request_message import CreateMonitoredItemsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CreateMonitoredItemsRequestMessage from a JSON string
create_monitored_items_request_message_instance = CreateMonitoredItemsRequestMessage.from_json(json)
# print the JSON string representation of the object
print CreateMonitoredItemsRequestMessage.to_json()

# convert the object into a dict
create_monitored_items_request_message_dict = create_monitored_items_request_message_instance.to_dict()
# create an instance of CreateMonitoredItemsRequestMessage from a dict
create_monitored_items_request_message_form_dict = create_monitored_items_request_message.from_dict(create_monitored_items_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



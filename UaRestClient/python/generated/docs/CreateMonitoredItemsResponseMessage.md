# CreateMonitoredItemsResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CreateMonitoredItemsResponse**](CreateMonitoredItemsResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.create_monitored_items_response_message import CreateMonitoredItemsResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CreateMonitoredItemsResponseMessage from a JSON string
create_monitored_items_response_message_instance = CreateMonitoredItemsResponseMessage.from_json(json)
# print the JSON string representation of the object
print CreateMonitoredItemsResponseMessage.to_json()

# convert the object into a dict
create_monitored_items_response_message_dict = create_monitored_items_response_message_instance.to_dict()
# create an instance of CreateMonitoredItemsResponseMessage from a dict
create_monitored_items_response_message_form_dict = create_monitored_items_response_message.from_dict(create_monitored_items_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



# SetMonitoringModeRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**SetMonitoringModeRequest**](SetMonitoringModeRequest.md) |  | 

## Example

```python
from openapi_client.models.set_monitoring_mode_request_message import SetMonitoringModeRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of SetMonitoringModeRequestMessage from a JSON string
set_monitoring_mode_request_message_instance = SetMonitoringModeRequestMessage.from_json(json)
# print the JSON string representation of the object
print SetMonitoringModeRequestMessage.to_json()

# convert the object into a dict
set_monitoring_mode_request_message_dict = set_monitoring_mode_request_message_instance.to_dict()
# create an instance of SetMonitoringModeRequestMessage from a dict
set_monitoring_mode_request_message_form_dict = set_monitoring_mode_request_message.from_dict(set_monitoring_mode_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



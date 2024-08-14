# SetMonitoringModeResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**SetMonitoringModeResponse**](SetMonitoringModeResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.set_monitoring_mode_response_message import SetMonitoringModeResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of SetMonitoringModeResponseMessage from a JSON string
set_monitoring_mode_response_message_instance = SetMonitoringModeResponseMessage.from_json(json)
# print the JSON string representation of the object
print SetMonitoringModeResponseMessage.to_json()

# convert the object into a dict
set_monitoring_mode_response_message_dict = set_monitoring_mode_response_message_instance.to_dict()
# create an instance of SetMonitoringModeResponseMessage from a dict
set_monitoring_mode_response_message_form_dict = set_monitoring_mode_response_message.from_dict(set_monitoring_mode_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



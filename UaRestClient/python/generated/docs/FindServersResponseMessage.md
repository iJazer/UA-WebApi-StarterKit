# FindServersResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**FindServersResponse**](FindServersResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.find_servers_response_message import FindServersResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of FindServersResponseMessage from a JSON string
find_servers_response_message_instance = FindServersResponseMessage.from_json(json)
# print the JSON string representation of the object
print FindServersResponseMessage.to_json()

# convert the object into a dict
find_servers_response_message_dict = find_servers_response_message_instance.to_dict()
# create an instance of FindServersResponseMessage from a dict
find_servers_response_message_form_dict = find_servers_response_message.from_dict(find_servers_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



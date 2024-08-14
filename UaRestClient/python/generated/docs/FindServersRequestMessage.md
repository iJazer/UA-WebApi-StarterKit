# FindServersRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**FindServersRequest**](FindServersRequest.md) |  | 

## Example

```python
from openapi_client.models.find_servers_request_message import FindServersRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of FindServersRequestMessage from a JSON string
find_servers_request_message_instance = FindServersRequestMessage.from_json(json)
# print the JSON string representation of the object
print FindServersRequestMessage.to_json()

# convert the object into a dict
find_servers_request_message_dict = find_servers_request_message_instance.to_dict()
# create an instance of FindServersRequestMessage from a dict
find_servers_request_message_form_dict = find_servers_request_message.from_dict(find_servers_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



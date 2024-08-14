# GetEndpointsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**GetEndpointsRequest**](GetEndpointsRequest.md) |  | 

## Example

```python
from openapi_client.models.get_endpoints_request_message import GetEndpointsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of GetEndpointsRequestMessage from a JSON string
get_endpoints_request_message_instance = GetEndpointsRequestMessage.from_json(json)
# print the JSON string representation of the object
print GetEndpointsRequestMessage.to_json()

# convert the object into a dict
get_endpoints_request_message_dict = get_endpoints_request_message_instance.to_dict()
# create an instance of GetEndpointsRequestMessage from a dict
get_endpoints_request_message_form_dict = get_endpoints_request_message.from_dict(get_endpoints_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



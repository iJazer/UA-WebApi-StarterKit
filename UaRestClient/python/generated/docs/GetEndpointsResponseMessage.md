# GetEndpointsResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**GetEndpointsResponse**](GetEndpointsResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.get_endpoints_response_message import GetEndpointsResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of GetEndpointsResponseMessage from a JSON string
get_endpoints_response_message_instance = GetEndpointsResponseMessage.from_json(json)
# print the JSON string representation of the object
print GetEndpointsResponseMessage.to_json()

# convert the object into a dict
get_endpoints_response_message_dict = get_endpoints_response_message_instance.to_dict()
# create an instance of GetEndpointsResponseMessage from a dict
get_endpoints_response_message_form_dict = get_endpoints_response_message.from_dict(get_endpoints_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



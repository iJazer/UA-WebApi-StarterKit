# ReadRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**ReadRequest**](ReadRequest.md) |  | 

## Example

```python
from openapi_client.models.read_request_message import ReadRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of ReadRequestMessage from a JSON string
read_request_message_instance = ReadRequestMessage.from_json(json)
# print the JSON string representation of the object
print ReadRequestMessage.to_json()

# convert the object into a dict
read_request_message_dict = read_request_message_instance.to_dict()
# create an instance of ReadRequestMessage from a dict
read_request_message_form_dict = read_request_message.from_dict(read_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



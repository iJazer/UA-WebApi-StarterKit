# ReadResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**ReadResponse**](ReadResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.read_response_message import ReadResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of ReadResponseMessage from a JSON string
read_response_message_instance = ReadResponseMessage.from_json(json)
# print the JSON string representation of the object
print ReadResponseMessage.to_json()

# convert the object into a dict
read_response_message_dict = read_response_message_instance.to_dict()
# create an instance of ReadResponseMessage from a dict
read_response_message_form_dict = read_response_message.from_dict(read_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



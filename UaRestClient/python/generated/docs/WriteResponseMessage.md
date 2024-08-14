# WriteResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**WriteResponse**](WriteResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.write_response_message import WriteResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of WriteResponseMessage from a JSON string
write_response_message_instance = WriteResponseMessage.from_json(json)
# print the JSON string representation of the object
print WriteResponseMessage.to_json()

# convert the object into a dict
write_response_message_dict = write_response_message_instance.to_dict()
# create an instance of WriteResponseMessage from a dict
write_response_message_form_dict = write_response_message.from_dict(write_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



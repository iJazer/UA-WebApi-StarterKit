# WriteRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**WriteRequest**](WriteRequest.md) |  | 

## Example

```python
from openapi_client.models.write_request_message import WriteRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of WriteRequestMessage from a JSON string
write_request_message_instance = WriteRequestMessage.from_json(json)
# print the JSON string representation of the object
print WriteRequestMessage.to_json()

# convert the object into a dict
write_request_message_dict = write_request_message_instance.to_dict()
# create an instance of WriteRequestMessage from a dict
write_request_message_form_dict = write_request_message.from_dict(write_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



# RepublishRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**RepublishRequest**](RepublishRequest.md) |  | 

## Example

```python
from openapi_client.models.republish_request_message import RepublishRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of RepublishRequestMessage from a JSON string
republish_request_message_instance = RepublishRequestMessage.from_json(json)
# print the JSON string representation of the object
print RepublishRequestMessage.to_json()

# convert the object into a dict
republish_request_message_dict = republish_request_message_instance.to_dict()
# create an instance of RepublishRequestMessage from a dict
republish_request_message_form_dict = republish_request_message.from_dict(republish_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



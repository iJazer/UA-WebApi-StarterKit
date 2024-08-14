# RepublishResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**RepublishResponse**](RepublishResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.republish_response_message import RepublishResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of RepublishResponseMessage from a JSON string
republish_response_message_instance = RepublishResponseMessage.from_json(json)
# print the JSON string representation of the object
print RepublishResponseMessage.to_json()

# convert the object into a dict
republish_response_message_dict = republish_response_message_instance.to_dict()
# create an instance of RepublishResponseMessage from a dict
republish_response_message_form_dict = republish_response_message.from_dict(republish_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



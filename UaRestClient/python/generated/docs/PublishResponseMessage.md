# PublishResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**PublishResponse**](PublishResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.publish_response_message import PublishResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of PublishResponseMessage from a JSON string
publish_response_message_instance = PublishResponseMessage.from_json(json)
# print the JSON string representation of the object
print PublishResponseMessage.to_json()

# convert the object into a dict
publish_response_message_dict = publish_response_message_instance.to_dict()
# create an instance of PublishResponseMessage from a dict
publish_response_message_form_dict = publish_response_message.from_dict(publish_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



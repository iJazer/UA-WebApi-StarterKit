# DeleteSubscriptionsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**DeleteSubscriptionsRequest**](DeleteSubscriptionsRequest.md) |  | 

## Example

```python
from openapi_client.models.delete_subscriptions_request_message import DeleteSubscriptionsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of DeleteSubscriptionsRequestMessage from a JSON string
delete_subscriptions_request_message_instance = DeleteSubscriptionsRequestMessage.from_json(json)
# print the JSON string representation of the object
print DeleteSubscriptionsRequestMessage.to_json()

# convert the object into a dict
delete_subscriptions_request_message_dict = delete_subscriptions_request_message_instance.to_dict()
# create an instance of DeleteSubscriptionsRequestMessage from a dict
delete_subscriptions_request_message_form_dict = delete_subscriptions_request_message.from_dict(delete_subscriptions_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



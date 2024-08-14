# ModifySubscriptionResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**ModifySubscriptionResponse**](ModifySubscriptionResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.modify_subscription_response_message import ModifySubscriptionResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of ModifySubscriptionResponseMessage from a JSON string
modify_subscription_response_message_instance = ModifySubscriptionResponseMessage.from_json(json)
# print the JSON string representation of the object
print ModifySubscriptionResponseMessage.to_json()

# convert the object into a dict
modify_subscription_response_message_dict = modify_subscription_response_message_instance.to_dict()
# create an instance of ModifySubscriptionResponseMessage from a dict
modify_subscription_response_message_form_dict = modify_subscription_response_message.from_dict(modify_subscription_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



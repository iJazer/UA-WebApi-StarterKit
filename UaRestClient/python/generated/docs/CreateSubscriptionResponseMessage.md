# CreateSubscriptionResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CreateSubscriptionResponse**](CreateSubscriptionResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.create_subscription_response_message import CreateSubscriptionResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CreateSubscriptionResponseMessage from a JSON string
create_subscription_response_message_instance = CreateSubscriptionResponseMessage.from_json(json)
# print the JSON string representation of the object
print CreateSubscriptionResponseMessage.to_json()

# convert the object into a dict
create_subscription_response_message_dict = create_subscription_response_message_instance.to_dict()
# create an instance of CreateSubscriptionResponseMessage from a dict
create_subscription_response_message_form_dict = create_subscription_response_message.from_dict(create_subscription_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



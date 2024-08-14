# CreateSubscriptionRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**CreateSubscriptionRequest**](CreateSubscriptionRequest.md) |  | 

## Example

```python
from openapi_client.models.create_subscription_request_message import CreateSubscriptionRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of CreateSubscriptionRequestMessage from a JSON string
create_subscription_request_message_instance = CreateSubscriptionRequestMessage.from_json(json)
# print the JSON string representation of the object
print CreateSubscriptionRequestMessage.to_json()

# convert the object into a dict
create_subscription_request_message_dict = create_subscription_request_message_instance.to_dict()
# create an instance of CreateSubscriptionRequestMessage from a dict
create_subscription_request_message_form_dict = create_subscription_request_message.from_dict(create_subscription_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



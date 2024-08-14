# TransferSubscriptionsResponseMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**TransferSubscriptionsResponse**](TransferSubscriptionsResponse.md) |  | [optional] 

## Example

```python
from openapi_client.models.transfer_subscriptions_response_message import TransferSubscriptionsResponseMessage

# TODO update the JSON string below
json = "{}"
# create an instance of TransferSubscriptionsResponseMessage from a JSON string
transfer_subscriptions_response_message_instance = TransferSubscriptionsResponseMessage.from_json(json)
# print the JSON string representation of the object
print TransferSubscriptionsResponseMessage.to_json()

# convert the object into a dict
transfer_subscriptions_response_message_dict = transfer_subscriptions_response_message_instance.to_dict()
# create an instance of TransferSubscriptionsResponseMessage from a dict
transfer_subscriptions_response_message_form_dict = transfer_subscriptions_response_message.from_dict(transfer_subscriptions_response_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



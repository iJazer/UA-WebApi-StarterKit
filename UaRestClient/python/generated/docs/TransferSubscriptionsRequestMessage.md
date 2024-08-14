# TransferSubscriptionsRequestMessage


## Properties
Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**namespace_uris** | **List[str]** |  | [optional] 
**server_uris** | **List[str]** |  | [optional] 
**locale_ids** | **List[str]** |  | [optional] 
**service_id** | **int** |  | [optional] 
**body** | [**TransferSubscriptionsRequest**](TransferSubscriptionsRequest.md) |  | 

## Example

```python
from openapi_client.models.transfer_subscriptions_request_message import TransferSubscriptionsRequestMessage

# TODO update the JSON string below
json = "{}"
# create an instance of TransferSubscriptionsRequestMessage from a JSON string
transfer_subscriptions_request_message_instance = TransferSubscriptionsRequestMessage.from_json(json)
# print the JSON string representation of the object
print TransferSubscriptionsRequestMessage.to_json()

# convert the object into a dict
transfer_subscriptions_request_message_dict = transfer_subscriptions_request_message_instance.to_dict()
# create an instance of TransferSubscriptionsRequestMessage from a dict
transfer_subscriptions_request_message_form_dict = transfer_subscriptions_request_message.from_dict(transfer_subscriptions_request_message_dict)
```
[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)



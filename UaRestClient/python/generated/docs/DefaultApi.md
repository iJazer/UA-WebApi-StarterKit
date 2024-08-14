# openapi_client.DefaultApi

All URIs are relative to *http://localhost:4840*

Method | HTTP request | Description
------------- | ------------- | -------------
[**activate_session**](DefaultApi.md#activate_session) | **POST** /activatesession | 
[**browse**](DefaultApi.md#browse) | **POST** /browse | 
[**browse_next**](DefaultApi.md#browse_next) | **POST** /browsenext | 
[**call**](DefaultApi.md#call) | **POST** /call | 
[**cancel**](DefaultApi.md#cancel) | **POST** /cancel | 
[**close_session**](DefaultApi.md#close_session) | **POST** /closesession | 
[**create_monitored_items**](DefaultApi.md#create_monitored_items) | **POST** /createmonitoreditems | 
[**create_session**](DefaultApi.md#create_session) | **POST** /createsession | 
[**create_subscription**](DefaultApi.md#create_subscription) | **POST** /createsubscription | 
[**delete_monitored_items**](DefaultApi.md#delete_monitored_items) | **POST** /deletemonitoreditems | 
[**delete_subscriptions**](DefaultApi.md#delete_subscriptions) | **POST** /deletesubscriptions | 
[**find_servers**](DefaultApi.md#find_servers) | **POST** /findservers | 
[**get_endpoints**](DefaultApi.md#get_endpoints) | **POST** /getendpoints | 
[**history_read**](DefaultApi.md#history_read) | **POST** /historyread | 
[**history_update**](DefaultApi.md#history_update) | **POST** /historyupdate | 
[**modify_monitored_items**](DefaultApi.md#modify_monitored_items) | **POST** /modifymonitoreditems | 
[**modify_subscription**](DefaultApi.md#modify_subscription) | **POST** /modifysubscription | 
[**publish**](DefaultApi.md#publish) | **POST** /publish | 
[**read**](DefaultApi.md#read) | **POST** /read | 
[**register_nodes**](DefaultApi.md#register_nodes) | **POST** /registernodes | 
[**republish**](DefaultApi.md#republish) | **POST** /republish | 
[**set_monitoring_mode**](DefaultApi.md#set_monitoring_mode) | **POST** /setmonitoringmode | 
[**set_publishing_mode**](DefaultApi.md#set_publishing_mode) | **POST** /setpublishingmode | 
[**set_triggering**](DefaultApi.md#set_triggering) | **POST** /settriggering | 
[**transfer_subscriptions**](DefaultApi.md#transfer_subscriptions) | **POST** /transfersubscriptions | 
[**translate_browse_paths_to_node_ids**](DefaultApi.md#translate_browse_paths_to_node_ids) | **POST** /translate | 
[**unregister_nodes**](DefaultApi.md#unregister_nodes) | **POST** /unregisternodes | 
[**write**](DefaultApi.md#write) | **POST** /write | 


# **activate_session**
> ActivateSessionResponseMessage activate_session(activate_session_request_message=activate_session_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.activate_session_request_message import ActivateSessionRequestMessage
from openapi_client.models.activate_session_response_message import ActivateSessionResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    activate_session_request_message = openapi_client.ActivateSessionRequestMessage() # ActivateSessionRequestMessage | ActivateSessionRequestMessage (optional)

    try:
        api_response = api_instance.activate_session(activate_session_request_message=activate_session_request_message)
        print("The response of DefaultApi->activate_session:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->activate_session: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **activate_session_request_message** | [**ActivateSessionRequestMessage**](ActivateSessionRequestMessage.md)| ActivateSessionRequestMessage | [optional] 

### Return type

[**ActivateSessionResponseMessage**](ActivateSessionResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | ActivateSessionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **browse**
> BrowseResponseMessage browse(browse_request_message=browse_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.browse_request_message import BrowseRequestMessage
from openapi_client.models.browse_response_message import BrowseResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    browse_request_message = openapi_client.BrowseRequestMessage() # BrowseRequestMessage | BrowseRequestMessage (optional)

    try:
        api_response = api_instance.browse(browse_request_message=browse_request_message)
        print("The response of DefaultApi->browse:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->browse: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **browse_request_message** | [**BrowseRequestMessage**](BrowseRequestMessage.md)| BrowseRequestMessage | [optional] 

### Return type

[**BrowseResponseMessage**](BrowseResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | BrowseResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **browse_next**
> BrowseNextResponseMessage browse_next(browse_next_request_message=browse_next_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.browse_next_request_message import BrowseNextRequestMessage
from openapi_client.models.browse_next_response_message import BrowseNextResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    browse_next_request_message = openapi_client.BrowseNextRequestMessage() # BrowseNextRequestMessage | BrowseNextRequestMessage (optional)

    try:
        api_response = api_instance.browse_next(browse_next_request_message=browse_next_request_message)
        print("The response of DefaultApi->browse_next:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->browse_next: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **browse_next_request_message** | [**BrowseNextRequestMessage**](BrowseNextRequestMessage.md)| BrowseNextRequestMessage | [optional] 

### Return type

[**BrowseNextResponseMessage**](BrowseNextResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | BrowseNextResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **call**
> CallResponseMessage call(call_request_message=call_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.call_request_message import CallRequestMessage
from openapi_client.models.call_response_message import CallResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    call_request_message = openapi_client.CallRequestMessage() # CallRequestMessage | CallRequestMessage (optional)

    try:
        api_response = api_instance.call(call_request_message=call_request_message)
        print("The response of DefaultApi->call:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->call: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **call_request_message** | [**CallRequestMessage**](CallRequestMessage.md)| CallRequestMessage | [optional] 

### Return type

[**CallResponseMessage**](CallResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | CallResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **cancel**
> CancelResponseMessage cancel(cancel_request_message=cancel_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.cancel_request_message import CancelRequestMessage
from openapi_client.models.cancel_response_message import CancelResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    cancel_request_message = openapi_client.CancelRequestMessage() # CancelRequestMessage | CancelRequestMessage (optional)

    try:
        api_response = api_instance.cancel(cancel_request_message=cancel_request_message)
        print("The response of DefaultApi->cancel:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->cancel: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **cancel_request_message** | [**CancelRequestMessage**](CancelRequestMessage.md)| CancelRequestMessage | [optional] 

### Return type

[**CancelResponseMessage**](CancelResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | CancelResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **close_session**
> CloseSessionResponseMessage close_session(close_session_request_message=close_session_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.close_session_request_message import CloseSessionRequestMessage
from openapi_client.models.close_session_response_message import CloseSessionResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    close_session_request_message = openapi_client.CloseSessionRequestMessage() # CloseSessionRequestMessage | CloseSessionRequestMessage (optional)

    try:
        api_response = api_instance.close_session(close_session_request_message=close_session_request_message)
        print("The response of DefaultApi->close_session:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->close_session: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **close_session_request_message** | [**CloseSessionRequestMessage**](CloseSessionRequestMessage.md)| CloseSessionRequestMessage | [optional] 

### Return type

[**CloseSessionResponseMessage**](CloseSessionResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | CloseSessionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **create_monitored_items**
> CreateMonitoredItemsResponseMessage create_monitored_items(create_monitored_items_request_message=create_monitored_items_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.create_monitored_items_request_message import CreateMonitoredItemsRequestMessage
from openapi_client.models.create_monitored_items_response_message import CreateMonitoredItemsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    create_monitored_items_request_message = openapi_client.CreateMonitoredItemsRequestMessage() # CreateMonitoredItemsRequestMessage | CreateMonitoredItemsRequestMessage (optional)

    try:
        api_response = api_instance.create_monitored_items(create_monitored_items_request_message=create_monitored_items_request_message)
        print("The response of DefaultApi->create_monitored_items:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->create_monitored_items: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **create_monitored_items_request_message** | [**CreateMonitoredItemsRequestMessage**](CreateMonitoredItemsRequestMessage.md)| CreateMonitoredItemsRequestMessage | [optional] 

### Return type

[**CreateMonitoredItemsResponseMessage**](CreateMonitoredItemsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | CreateMonitoredItemsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **create_session**
> CreateSessionResponseMessage create_session(create_session_request_message=create_session_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.create_session_request_message import CreateSessionRequestMessage
from openapi_client.models.create_session_response_message import CreateSessionResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    create_session_request_message = openapi_client.CreateSessionRequestMessage() # CreateSessionRequestMessage | CreateSessionRequestMessage (optional)

    try:
        api_response = api_instance.create_session(create_session_request_message=create_session_request_message)
        print("The response of DefaultApi->create_session:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->create_session: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **create_session_request_message** | [**CreateSessionRequestMessage**](CreateSessionRequestMessage.md)| CreateSessionRequestMessage | [optional] 

### Return type

[**CreateSessionResponseMessage**](CreateSessionResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | CreateSessionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **create_subscription**
> CreateSubscriptionResponseMessage create_subscription(create_subscription_request_message=create_subscription_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.create_subscription_request_message import CreateSubscriptionRequestMessage
from openapi_client.models.create_subscription_response_message import CreateSubscriptionResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    create_subscription_request_message = openapi_client.CreateSubscriptionRequestMessage() # CreateSubscriptionRequestMessage | CreateSubscriptionRequestMessage (optional)

    try:
        api_response = api_instance.create_subscription(create_subscription_request_message=create_subscription_request_message)
        print("The response of DefaultApi->create_subscription:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->create_subscription: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **create_subscription_request_message** | [**CreateSubscriptionRequestMessage**](CreateSubscriptionRequestMessage.md)| CreateSubscriptionRequestMessage | [optional] 

### Return type

[**CreateSubscriptionResponseMessage**](CreateSubscriptionResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | CreateSubscriptionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **delete_monitored_items**
> DeleteMonitoredItemsResponseMessage delete_monitored_items(delete_monitored_items_request_message=delete_monitored_items_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.delete_monitored_items_request_message import DeleteMonitoredItemsRequestMessage
from openapi_client.models.delete_monitored_items_response_message import DeleteMonitoredItemsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    delete_monitored_items_request_message = openapi_client.DeleteMonitoredItemsRequestMessage() # DeleteMonitoredItemsRequestMessage | DeleteMonitoredItemsRequestMessage (optional)

    try:
        api_response = api_instance.delete_monitored_items(delete_monitored_items_request_message=delete_monitored_items_request_message)
        print("The response of DefaultApi->delete_monitored_items:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->delete_monitored_items: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **delete_monitored_items_request_message** | [**DeleteMonitoredItemsRequestMessage**](DeleteMonitoredItemsRequestMessage.md)| DeleteMonitoredItemsRequestMessage | [optional] 

### Return type

[**DeleteMonitoredItemsResponseMessage**](DeleteMonitoredItemsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | DeleteMonitoredItemsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **delete_subscriptions**
> DeleteSubscriptionsResponseMessage delete_subscriptions(delete_subscriptions_request_message=delete_subscriptions_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.delete_subscriptions_request_message import DeleteSubscriptionsRequestMessage
from openapi_client.models.delete_subscriptions_response_message import DeleteSubscriptionsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    delete_subscriptions_request_message = openapi_client.DeleteSubscriptionsRequestMessage() # DeleteSubscriptionsRequestMessage | DeleteSubscriptionsRequestMessage (optional)

    try:
        api_response = api_instance.delete_subscriptions(delete_subscriptions_request_message=delete_subscriptions_request_message)
        print("The response of DefaultApi->delete_subscriptions:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->delete_subscriptions: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **delete_subscriptions_request_message** | [**DeleteSubscriptionsRequestMessage**](DeleteSubscriptionsRequestMessage.md)| DeleteSubscriptionsRequestMessage | [optional] 

### Return type

[**DeleteSubscriptionsResponseMessage**](DeleteSubscriptionsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | DeleteSubscriptionsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **find_servers**
> FindServersResponseMessage find_servers(find_servers_request_message=find_servers_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.find_servers_request_message import FindServersRequestMessage
from openapi_client.models.find_servers_response_message import FindServersResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    find_servers_request_message = openapi_client.FindServersRequestMessage() # FindServersRequestMessage | FindServersRequestMessage (optional)

    try:
        api_response = api_instance.find_servers(find_servers_request_message=find_servers_request_message)
        print("The response of DefaultApi->find_servers:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->find_servers: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **find_servers_request_message** | [**FindServersRequestMessage**](FindServersRequestMessage.md)| FindServersRequestMessage | [optional] 

### Return type

[**FindServersResponseMessage**](FindServersResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | FindServersResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get_endpoints**
> GetEndpointsResponseMessage get_endpoints(get_endpoints_request_message=get_endpoints_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.get_endpoints_request_message import GetEndpointsRequestMessage
from openapi_client.models.get_endpoints_response_message import GetEndpointsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    get_endpoints_request_message = openapi_client.GetEndpointsRequestMessage() # GetEndpointsRequestMessage | GetEndpointsRequestMessage (optional)

    try:
        api_response = api_instance.get_endpoints(get_endpoints_request_message=get_endpoints_request_message)
        print("The response of DefaultApi->get_endpoints:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->get_endpoints: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **get_endpoints_request_message** | [**GetEndpointsRequestMessage**](GetEndpointsRequestMessage.md)| GetEndpointsRequestMessage | [optional] 

### Return type

[**GetEndpointsResponseMessage**](GetEndpointsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | GetEndpointsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **history_read**
> HistoryReadResponseMessage history_read(history_read_request_message=history_read_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.history_read_request_message import HistoryReadRequestMessage
from openapi_client.models.history_read_response_message import HistoryReadResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    history_read_request_message = openapi_client.HistoryReadRequestMessage() # HistoryReadRequestMessage | HistoryReadRequestMessage (optional)

    try:
        api_response = api_instance.history_read(history_read_request_message=history_read_request_message)
        print("The response of DefaultApi->history_read:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->history_read: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **history_read_request_message** | [**HistoryReadRequestMessage**](HistoryReadRequestMessage.md)| HistoryReadRequestMessage | [optional] 

### Return type

[**HistoryReadResponseMessage**](HistoryReadResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | HistoryReadResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **history_update**
> HistoryUpdateResponseMessage history_update(history_update_request_message=history_update_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.history_update_request_message import HistoryUpdateRequestMessage
from openapi_client.models.history_update_response_message import HistoryUpdateResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    history_update_request_message = openapi_client.HistoryUpdateRequestMessage() # HistoryUpdateRequestMessage | HistoryUpdateRequestMessage (optional)

    try:
        api_response = api_instance.history_update(history_update_request_message=history_update_request_message)
        print("The response of DefaultApi->history_update:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->history_update: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **history_update_request_message** | [**HistoryUpdateRequestMessage**](HistoryUpdateRequestMessage.md)| HistoryUpdateRequestMessage | [optional] 

### Return type

[**HistoryUpdateResponseMessage**](HistoryUpdateResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | HistoryUpdateResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **modify_monitored_items**
> ModifyMonitoredItemsResponseMessage modify_monitored_items(modify_monitored_items_request_message=modify_monitored_items_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.modify_monitored_items_request_message import ModifyMonitoredItemsRequestMessage
from openapi_client.models.modify_monitored_items_response_message import ModifyMonitoredItemsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    modify_monitored_items_request_message = openapi_client.ModifyMonitoredItemsRequestMessage() # ModifyMonitoredItemsRequestMessage | ModifyMonitoredItemsRequestMessage (optional)

    try:
        api_response = api_instance.modify_monitored_items(modify_monitored_items_request_message=modify_monitored_items_request_message)
        print("The response of DefaultApi->modify_monitored_items:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->modify_monitored_items: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **modify_monitored_items_request_message** | [**ModifyMonitoredItemsRequestMessage**](ModifyMonitoredItemsRequestMessage.md)| ModifyMonitoredItemsRequestMessage | [optional] 

### Return type

[**ModifyMonitoredItemsResponseMessage**](ModifyMonitoredItemsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | ModifyMonitoredItemsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **modify_subscription**
> ModifySubscriptionResponseMessage modify_subscription(modify_subscription_request_message=modify_subscription_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.modify_subscription_request_message import ModifySubscriptionRequestMessage
from openapi_client.models.modify_subscription_response_message import ModifySubscriptionResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    modify_subscription_request_message = openapi_client.ModifySubscriptionRequestMessage() # ModifySubscriptionRequestMessage | ModifySubscriptionRequestMessage (optional)

    try:
        api_response = api_instance.modify_subscription(modify_subscription_request_message=modify_subscription_request_message)
        print("The response of DefaultApi->modify_subscription:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->modify_subscription: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **modify_subscription_request_message** | [**ModifySubscriptionRequestMessage**](ModifySubscriptionRequestMessage.md)| ModifySubscriptionRequestMessage | [optional] 

### Return type

[**ModifySubscriptionResponseMessage**](ModifySubscriptionResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | ModifySubscriptionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **publish**
> PublishResponseMessage publish(publish_request_message=publish_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.publish_request_message import PublishRequestMessage
from openapi_client.models.publish_response_message import PublishResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    publish_request_message = openapi_client.PublishRequestMessage() # PublishRequestMessage | PublishRequestMessage (optional)

    try:
        api_response = api_instance.publish(publish_request_message=publish_request_message)
        print("The response of DefaultApi->publish:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->publish: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **publish_request_message** | [**PublishRequestMessage**](PublishRequestMessage.md)| PublishRequestMessage | [optional] 

### Return type

[**PublishResponseMessage**](PublishResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | PublishResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **read**
> ReadResponseMessage read(read_request_message=read_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.read_request_message import ReadRequestMessage
from openapi_client.models.read_response_message import ReadResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    read_request_message = openapi_client.ReadRequestMessage() # ReadRequestMessage | ReadRequestMessage (optional)

    try:
        api_response = api_instance.read(read_request_message=read_request_message)
        print("The response of DefaultApi->read:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->read: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **read_request_message** | [**ReadRequestMessage**](ReadRequestMessage.md)| ReadRequestMessage | [optional] 

### Return type

[**ReadResponseMessage**](ReadResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | ReadResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **register_nodes**
> RegisterNodesResponseMessage register_nodes(register_nodes_request_message=register_nodes_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.register_nodes_request_message import RegisterNodesRequestMessage
from openapi_client.models.register_nodes_response_message import RegisterNodesResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    register_nodes_request_message = openapi_client.RegisterNodesRequestMessage() # RegisterNodesRequestMessage | RegisterNodesRequestMessage (optional)

    try:
        api_response = api_instance.register_nodes(register_nodes_request_message=register_nodes_request_message)
        print("The response of DefaultApi->register_nodes:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->register_nodes: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **register_nodes_request_message** | [**RegisterNodesRequestMessage**](RegisterNodesRequestMessage.md)| RegisterNodesRequestMessage | [optional] 

### Return type

[**RegisterNodesResponseMessage**](RegisterNodesResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | RegisterNodesResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **republish**
> RepublishResponseMessage republish(republish_request_message=republish_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.republish_request_message import RepublishRequestMessage
from openapi_client.models.republish_response_message import RepublishResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    republish_request_message = openapi_client.RepublishRequestMessage() # RepublishRequestMessage | RepublishRequestMessage (optional)

    try:
        api_response = api_instance.republish(republish_request_message=republish_request_message)
        print("The response of DefaultApi->republish:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->republish: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **republish_request_message** | [**RepublishRequestMessage**](RepublishRequestMessage.md)| RepublishRequestMessage | [optional] 

### Return type

[**RepublishResponseMessage**](RepublishResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | RepublishResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **set_monitoring_mode**
> SetMonitoringModeResponseMessage set_monitoring_mode(set_monitoring_mode_request_message=set_monitoring_mode_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.set_monitoring_mode_request_message import SetMonitoringModeRequestMessage
from openapi_client.models.set_monitoring_mode_response_message import SetMonitoringModeResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    set_monitoring_mode_request_message = openapi_client.SetMonitoringModeRequestMessage() # SetMonitoringModeRequestMessage | SetMonitoringModeRequestMessage (optional)

    try:
        api_response = api_instance.set_monitoring_mode(set_monitoring_mode_request_message=set_monitoring_mode_request_message)
        print("The response of DefaultApi->set_monitoring_mode:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->set_monitoring_mode: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **set_monitoring_mode_request_message** | [**SetMonitoringModeRequestMessage**](SetMonitoringModeRequestMessage.md)| SetMonitoringModeRequestMessage | [optional] 

### Return type

[**SetMonitoringModeResponseMessage**](SetMonitoringModeResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | SetMonitoringModeResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **set_publishing_mode**
> SetPublishingModeResponseMessage set_publishing_mode(set_publishing_mode_request_message=set_publishing_mode_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.set_publishing_mode_request_message import SetPublishingModeRequestMessage
from openapi_client.models.set_publishing_mode_response_message import SetPublishingModeResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    set_publishing_mode_request_message = openapi_client.SetPublishingModeRequestMessage() # SetPublishingModeRequestMessage | SetPublishingModeRequestMessage (optional)

    try:
        api_response = api_instance.set_publishing_mode(set_publishing_mode_request_message=set_publishing_mode_request_message)
        print("The response of DefaultApi->set_publishing_mode:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->set_publishing_mode: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **set_publishing_mode_request_message** | [**SetPublishingModeRequestMessage**](SetPublishingModeRequestMessage.md)| SetPublishingModeRequestMessage | [optional] 

### Return type

[**SetPublishingModeResponseMessage**](SetPublishingModeResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | SetPublishingModeResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **set_triggering**
> SetTriggeringResponseMessage set_triggering(set_triggering_request_message=set_triggering_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.set_triggering_request_message import SetTriggeringRequestMessage
from openapi_client.models.set_triggering_response_message import SetTriggeringResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    set_triggering_request_message = openapi_client.SetTriggeringRequestMessage() # SetTriggeringRequestMessage | SetTriggeringRequestMessage (optional)

    try:
        api_response = api_instance.set_triggering(set_triggering_request_message=set_triggering_request_message)
        print("The response of DefaultApi->set_triggering:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->set_triggering: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **set_triggering_request_message** | [**SetTriggeringRequestMessage**](SetTriggeringRequestMessage.md)| SetTriggeringRequestMessage | [optional] 

### Return type

[**SetTriggeringResponseMessage**](SetTriggeringResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | SetTriggeringResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **transfer_subscriptions**
> TransferSubscriptionsResponseMessage transfer_subscriptions(transfer_subscriptions_request_message=transfer_subscriptions_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.transfer_subscriptions_request_message import TransferSubscriptionsRequestMessage
from openapi_client.models.transfer_subscriptions_response_message import TransferSubscriptionsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    transfer_subscriptions_request_message = openapi_client.TransferSubscriptionsRequestMessage() # TransferSubscriptionsRequestMessage | TransferSubscriptionsRequestMessage (optional)

    try:
        api_response = api_instance.transfer_subscriptions(transfer_subscriptions_request_message=transfer_subscriptions_request_message)
        print("The response of DefaultApi->transfer_subscriptions:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->transfer_subscriptions: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **transfer_subscriptions_request_message** | [**TransferSubscriptionsRequestMessage**](TransferSubscriptionsRequestMessage.md)| TransferSubscriptionsRequestMessage | [optional] 

### Return type

[**TransferSubscriptionsResponseMessage**](TransferSubscriptionsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | TransferSubscriptionsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **translate_browse_paths_to_node_ids**
> TranslateBrowsePathsToNodeIdsResponseMessage translate_browse_paths_to_node_ids(translate_browse_paths_to_node_ids_request_message=translate_browse_paths_to_node_ids_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.translate_browse_paths_to_node_ids_request_message import TranslateBrowsePathsToNodeIdsRequestMessage
from openapi_client.models.translate_browse_paths_to_node_ids_response_message import TranslateBrowsePathsToNodeIdsResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    translate_browse_paths_to_node_ids_request_message = openapi_client.TranslateBrowsePathsToNodeIdsRequestMessage() # TranslateBrowsePathsToNodeIdsRequestMessage | TranslateBrowsePathsToNodeIdsRequestMessage (optional)

    try:
        api_response = api_instance.translate_browse_paths_to_node_ids(translate_browse_paths_to_node_ids_request_message=translate_browse_paths_to_node_ids_request_message)
        print("The response of DefaultApi->translate_browse_paths_to_node_ids:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->translate_browse_paths_to_node_ids: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **translate_browse_paths_to_node_ids_request_message** | [**TranslateBrowsePathsToNodeIdsRequestMessage**](TranslateBrowsePathsToNodeIdsRequestMessage.md)| TranslateBrowsePathsToNodeIdsRequestMessage | [optional] 

### Return type

[**TranslateBrowsePathsToNodeIdsResponseMessage**](TranslateBrowsePathsToNodeIdsResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | TranslateBrowsePathsToNodeIdsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **unregister_nodes**
> UnregisterNodesResponseMessage unregister_nodes(unregister_nodes_request_message=unregister_nodes_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.unregister_nodes_request_message import UnregisterNodesRequestMessage
from openapi_client.models.unregister_nodes_response_message import UnregisterNodesResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    unregister_nodes_request_message = openapi_client.UnregisterNodesRequestMessage() # UnregisterNodesRequestMessage | UnregisterNodesRequestMessage (optional)

    try:
        api_response = api_instance.unregister_nodes(unregister_nodes_request_message=unregister_nodes_request_message)
        print("The response of DefaultApi->unregister_nodes:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->unregister_nodes: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **unregister_nodes_request_message** | [**UnregisterNodesRequestMessage**](UnregisterNodesRequestMessage.md)| UnregisterNodesRequestMessage | [optional] 

### Return type

[**UnregisterNodesResponseMessage**](UnregisterNodesResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | UnregisterNodesResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **write**
> WriteResponseMessage write(write_request_message=write_request_message)



### Example

```python
import time
import os
import openapi_client
from openapi_client.models.write_request_message import WriteRequestMessage
from openapi_client.models.write_response_message import WriteResponseMessage
from openapi_client.rest import ApiException
from pprint import pprint

# Defining the host is optional and defaults to http://localhost:4840
# See configuration.py for a list of all supported configuration parameters.
configuration = openapi_client.Configuration(
    host = "http://localhost:4840"
)


# Enter a context with an instance of the API client
with openapi_client.ApiClient(configuration) as api_client:
    # Create an instance of the API class
    api_instance = openapi_client.DefaultApi(api_client)
    write_request_message = openapi_client.WriteRequestMessage() # WriteRequestMessage | WriteRequestMessage (optional)

    try:
        api_response = api_instance.write(write_request_message=write_request_message)
        print("The response of DefaultApi->write:\n")
        pprint(api_response)
    except Exception as e:
        print("Exception when calling DefaultApi->write: %s\n" % e)
```



### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **write_request_message** | [**WriteRequestMessage**](WriteRequestMessage.md)| WriteRequestMessage | [optional] 

### Return type

[**WriteResponseMessage**](WriteResponseMessage.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
**200** | WriteResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


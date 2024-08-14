# Org.OpenAPITools.Api.DefaultApi

All URIs are relative to *http://localhost:4840*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ActivateSession**](DefaultApi.md#activatesession) | **POST** /activatesession |  |
| [**Browse**](DefaultApi.md#browse) | **POST** /browse |  |
| [**BrowseNext**](DefaultApi.md#browsenext) | **POST** /browsenext |  |
| [**Call**](DefaultApi.md#call) | **POST** /call |  |
| [**Cancel**](DefaultApi.md#cancel) | **POST** /cancel |  |
| [**CloseSession**](DefaultApi.md#closesession) | **POST** /closesession |  |
| [**CreateMonitoredItems**](DefaultApi.md#createmonitoreditems) | **POST** /createmonitoreditems |  |
| [**CreateSession**](DefaultApi.md#createsession) | **POST** /createsession |  |
| [**CreateSubscription**](DefaultApi.md#createsubscription) | **POST** /createsubscription |  |
| [**DeleteMonitoredItems**](DefaultApi.md#deletemonitoreditems) | **POST** /deletemonitoreditems |  |
| [**DeleteSubscriptions**](DefaultApi.md#deletesubscriptions) | **POST** /deletesubscriptions |  |
| [**FindServers**](DefaultApi.md#findservers) | **POST** /findservers |  |
| [**GetEndpoints**](DefaultApi.md#getendpoints) | **POST** /getendpoints |  |
| [**HistoryRead**](DefaultApi.md#historyread) | **POST** /historyread |  |
| [**HistoryUpdate**](DefaultApi.md#historyupdate) | **POST** /historyupdate |  |
| [**ModifyMonitoredItems**](DefaultApi.md#modifymonitoreditems) | **POST** /modifymonitoreditems |  |
| [**ModifySubscription**](DefaultApi.md#modifysubscription) | **POST** /modifysubscription |  |
| [**Publish**](DefaultApi.md#publish) | **POST** /publish |  |
| [**Read**](DefaultApi.md#read) | **POST** /read |  |
| [**RegisterNodes**](DefaultApi.md#registernodes) | **POST** /registernodes |  |
| [**Republish**](DefaultApi.md#republish) | **POST** /republish |  |
| [**SetMonitoringMode**](DefaultApi.md#setmonitoringmode) | **POST** /setmonitoringmode |  |
| [**SetPublishingMode**](DefaultApi.md#setpublishingmode) | **POST** /setpublishingmode |  |
| [**SetTriggering**](DefaultApi.md#settriggering) | **POST** /settriggering |  |
| [**TransferSubscriptions**](DefaultApi.md#transfersubscriptions) | **POST** /transfersubscriptions |  |
| [**TranslateBrowsePathsToNodeIds**](DefaultApi.md#translatebrowsepathstonodeids) | **POST** /translate |  |
| [**UnregisterNodes**](DefaultApi.md#unregisternodes) | **POST** /unregisternodes |  |
| [**Write**](DefaultApi.md#write) | **POST** /write |  |

<a id="activatesession"></a>
# **ActivateSession**
> ActivateSessionResponseMessage ActivateSession (ActivateSessionRequestMessage? activateSessionRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class ActivateSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var activateSessionRequestMessage = new ActivateSessionRequestMessage?(); // ActivateSessionRequestMessage? | ActivateSessionRequestMessage (optional) 

            try
            {
                ActivateSessionResponseMessage result = apiInstance.ActivateSession(activateSessionRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ActivateSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ActivateSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ActivateSessionResponseMessage> response = apiInstance.ActivateSessionWithHttpInfo(activateSessionRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ActivateSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **activateSessionRequestMessage** | [**ActivateSessionRequestMessage?**](ActivateSessionRequestMessage?.md) | ActivateSessionRequestMessage | [optional]  |

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
| **200** | ActivateSessionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="browse"></a>
# **Browse**
> BrowseResponseMessage Browse (BrowseRequestMessage? browseRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class BrowseExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var browseRequestMessage = new BrowseRequestMessage?(); // BrowseRequestMessage? | BrowseRequestMessage (optional) 

            try
            {
                BrowseResponseMessage result = apiInstance.Browse(browseRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Browse: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the BrowseWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<BrowseResponseMessage> response = apiInstance.BrowseWithHttpInfo(browseRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.BrowseWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **browseRequestMessage** | [**BrowseRequestMessage?**](BrowseRequestMessage?.md) | BrowseRequestMessage | [optional]  |

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
| **200** | BrowseResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="browsenext"></a>
# **BrowseNext**
> BrowseNextResponseMessage BrowseNext (BrowseNextRequestMessage? browseNextRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class BrowseNextExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var browseNextRequestMessage = new BrowseNextRequestMessage?(); // BrowseNextRequestMessage? | BrowseNextRequestMessage (optional) 

            try
            {
                BrowseNextResponseMessage result = apiInstance.BrowseNext(browseNextRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.BrowseNext: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the BrowseNextWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<BrowseNextResponseMessage> response = apiInstance.BrowseNextWithHttpInfo(browseNextRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.BrowseNextWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **browseNextRequestMessage** | [**BrowseNextRequestMessage?**](BrowseNextRequestMessage?.md) | BrowseNextRequestMessage | [optional]  |

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
| **200** | BrowseNextResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="call"></a>
# **Call**
> CallResponseMessage Call (CallRequestMessage? callRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CallExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var callRequestMessage = new CallRequestMessage?(); // CallRequestMessage? | CallRequestMessage (optional) 

            try
            {
                CallResponseMessage result = apiInstance.Call(callRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Call: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CallWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CallResponseMessage> response = apiInstance.CallWithHttpInfo(callRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CallWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **callRequestMessage** | [**CallRequestMessage?**](CallRequestMessage?.md) | CallRequestMessage | [optional]  |

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
| **200** | CallResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="cancel"></a>
# **Cancel**
> CancelResponseMessage Cancel (CancelRequestMessage? cancelRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CancelExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var cancelRequestMessage = new CancelRequestMessage?(); // CancelRequestMessage? | CancelRequestMessage (optional) 

            try
            {
                CancelResponseMessage result = apiInstance.Cancel(cancelRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Cancel: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CancelWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CancelResponseMessage> response = apiInstance.CancelWithHttpInfo(cancelRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CancelWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **cancelRequestMessage** | [**CancelRequestMessage?**](CancelRequestMessage?.md) | CancelRequestMessage | [optional]  |

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
| **200** | CancelResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="closesession"></a>
# **CloseSession**
> CloseSessionResponseMessage CloseSession (CloseSessionRequestMessage? closeSessionRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CloseSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var closeSessionRequestMessage = new CloseSessionRequestMessage?(); // CloseSessionRequestMessage? | CloseSessionRequestMessage (optional) 

            try
            {
                CloseSessionResponseMessage result = apiInstance.CloseSession(closeSessionRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CloseSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CloseSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CloseSessionResponseMessage> response = apiInstance.CloseSessionWithHttpInfo(closeSessionRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CloseSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **closeSessionRequestMessage** | [**CloseSessionRequestMessage?**](CloseSessionRequestMessage?.md) | CloseSessionRequestMessage | [optional]  |

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
| **200** | CloseSessionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createmonitoreditems"></a>
# **CreateMonitoredItems**
> CreateMonitoredItemsResponseMessage CreateMonitoredItems (CreateMonitoredItemsRequestMessage? createMonitoredItemsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CreateMonitoredItemsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var createMonitoredItemsRequestMessage = new CreateMonitoredItemsRequestMessage?(); // CreateMonitoredItemsRequestMessage? | CreateMonitoredItemsRequestMessage (optional) 

            try
            {
                CreateMonitoredItemsResponseMessage result = apiInstance.CreateMonitoredItems(createMonitoredItemsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CreateMonitoredItems: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateMonitoredItemsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CreateMonitoredItemsResponseMessage> response = apiInstance.CreateMonitoredItemsWithHttpInfo(createMonitoredItemsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CreateMonitoredItemsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createMonitoredItemsRequestMessage** | [**CreateMonitoredItemsRequestMessage?**](CreateMonitoredItemsRequestMessage?.md) | CreateMonitoredItemsRequestMessage | [optional]  |

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
| **200** | CreateMonitoredItemsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createsession"></a>
# **CreateSession**
> CreateSessionResponseMessage CreateSession (CreateSessionRequestMessage? createSessionRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CreateSessionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var createSessionRequestMessage = new CreateSessionRequestMessage?(); // CreateSessionRequestMessage? | CreateSessionRequestMessage (optional) 

            try
            {
                CreateSessionResponseMessage result = apiInstance.CreateSession(createSessionRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CreateSession: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateSessionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CreateSessionResponseMessage> response = apiInstance.CreateSessionWithHttpInfo(createSessionRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CreateSessionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createSessionRequestMessage** | [**CreateSessionRequestMessage?**](CreateSessionRequestMessage?.md) | CreateSessionRequestMessage | [optional]  |

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
| **200** | CreateSessionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createsubscription"></a>
# **CreateSubscription**
> CreateSubscriptionResponseMessage CreateSubscription (CreateSubscriptionRequestMessage? createSubscriptionRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CreateSubscriptionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var createSubscriptionRequestMessage = new CreateSubscriptionRequestMessage?(); // CreateSubscriptionRequestMessage? | CreateSubscriptionRequestMessage (optional) 

            try
            {
                CreateSubscriptionResponseMessage result = apiInstance.CreateSubscription(createSubscriptionRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.CreateSubscription: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateSubscriptionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<CreateSubscriptionResponseMessage> response = apiInstance.CreateSubscriptionWithHttpInfo(createSubscriptionRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.CreateSubscriptionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createSubscriptionRequestMessage** | [**CreateSubscriptionRequestMessage?**](CreateSubscriptionRequestMessage?.md) | CreateSubscriptionRequestMessage | [optional]  |

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
| **200** | CreateSubscriptionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemonitoreditems"></a>
# **DeleteMonitoredItems**
> DeleteMonitoredItemsResponseMessage DeleteMonitoredItems (DeleteMonitoredItemsRequestMessage? deleteMonitoredItemsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class DeleteMonitoredItemsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var deleteMonitoredItemsRequestMessage = new DeleteMonitoredItemsRequestMessage?(); // DeleteMonitoredItemsRequestMessage? | DeleteMonitoredItemsRequestMessage (optional) 

            try
            {
                DeleteMonitoredItemsResponseMessage result = apiInstance.DeleteMonitoredItems(deleteMonitoredItemsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.DeleteMonitoredItems: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteMonitoredItemsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<DeleteMonitoredItemsResponseMessage> response = apiInstance.DeleteMonitoredItemsWithHttpInfo(deleteMonitoredItemsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.DeleteMonitoredItemsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **deleteMonitoredItemsRequestMessage** | [**DeleteMonitoredItemsRequestMessage?**](DeleteMonitoredItemsRequestMessage?.md) | DeleteMonitoredItemsRequestMessage | [optional]  |

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
| **200** | DeleteMonitoredItemsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletesubscriptions"></a>
# **DeleteSubscriptions**
> DeleteSubscriptionsResponseMessage DeleteSubscriptions (DeleteSubscriptionsRequestMessage? deleteSubscriptionsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class DeleteSubscriptionsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var deleteSubscriptionsRequestMessage = new DeleteSubscriptionsRequestMessage?(); // DeleteSubscriptionsRequestMessage? | DeleteSubscriptionsRequestMessage (optional) 

            try
            {
                DeleteSubscriptionsResponseMessage result = apiInstance.DeleteSubscriptions(deleteSubscriptionsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.DeleteSubscriptions: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteSubscriptionsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<DeleteSubscriptionsResponseMessage> response = apiInstance.DeleteSubscriptionsWithHttpInfo(deleteSubscriptionsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.DeleteSubscriptionsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **deleteSubscriptionsRequestMessage** | [**DeleteSubscriptionsRequestMessage?**](DeleteSubscriptionsRequestMessage?.md) | DeleteSubscriptionsRequestMessage | [optional]  |

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
| **200** | DeleteSubscriptionsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="findservers"></a>
# **FindServers**
> FindServersResponseMessage FindServers (FindServersRequestMessage? findServersRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class FindServersExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var findServersRequestMessage = new FindServersRequestMessage?(); // FindServersRequestMessage? | FindServersRequestMessage (optional) 

            try
            {
                FindServersResponseMessage result = apiInstance.FindServers(findServersRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.FindServers: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the FindServersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<FindServersResponseMessage> response = apiInstance.FindServersWithHttpInfo(findServersRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.FindServersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **findServersRequestMessage** | [**FindServersRequestMessage?**](FindServersRequestMessage?.md) | FindServersRequestMessage | [optional]  |

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
| **200** | FindServersResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getendpoints"></a>
# **GetEndpoints**
> GetEndpointsResponseMessage GetEndpoints (GetEndpointsRequestMessage? getEndpointsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class GetEndpointsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var getEndpointsRequestMessage = new GetEndpointsRequestMessage?(); // GetEndpointsRequestMessage? | GetEndpointsRequestMessage (optional) 

            try
            {
                GetEndpointsResponseMessage result = apiInstance.GetEndpoints(getEndpointsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.GetEndpoints: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEndpointsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<GetEndpointsResponseMessage> response = apiInstance.GetEndpointsWithHttpInfo(getEndpointsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.GetEndpointsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **getEndpointsRequestMessage** | [**GetEndpointsRequestMessage?**](GetEndpointsRequestMessage?.md) | GetEndpointsRequestMessage | [optional]  |

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
| **200** | GetEndpointsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="historyread"></a>
# **HistoryRead**
> HistoryReadResponseMessage HistoryRead (HistoryReadRequestMessage? historyReadRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class HistoryReadExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var historyReadRequestMessage = new HistoryReadRequestMessage?(); // HistoryReadRequestMessage? | HistoryReadRequestMessage (optional) 

            try
            {
                HistoryReadResponseMessage result = apiInstance.HistoryRead(historyReadRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.HistoryRead: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the HistoryReadWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<HistoryReadResponseMessage> response = apiInstance.HistoryReadWithHttpInfo(historyReadRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.HistoryReadWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **historyReadRequestMessage** | [**HistoryReadRequestMessage?**](HistoryReadRequestMessage?.md) | HistoryReadRequestMessage | [optional]  |

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
| **200** | HistoryReadResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="historyupdate"></a>
# **HistoryUpdate**
> HistoryUpdateResponseMessage HistoryUpdate (HistoryUpdateRequestMessage? historyUpdateRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class HistoryUpdateExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var historyUpdateRequestMessage = new HistoryUpdateRequestMessage?(); // HistoryUpdateRequestMessage? | HistoryUpdateRequestMessage (optional) 

            try
            {
                HistoryUpdateResponseMessage result = apiInstance.HistoryUpdate(historyUpdateRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.HistoryUpdate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the HistoryUpdateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<HistoryUpdateResponseMessage> response = apiInstance.HistoryUpdateWithHttpInfo(historyUpdateRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.HistoryUpdateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **historyUpdateRequestMessage** | [**HistoryUpdateRequestMessage?**](HistoryUpdateRequestMessage?.md) | HistoryUpdateRequestMessage | [optional]  |

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
| **200** | HistoryUpdateResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="modifymonitoreditems"></a>
# **ModifyMonitoredItems**
> ModifyMonitoredItemsResponseMessage ModifyMonitoredItems (ModifyMonitoredItemsRequestMessage? modifyMonitoredItemsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class ModifyMonitoredItemsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var modifyMonitoredItemsRequestMessage = new ModifyMonitoredItemsRequestMessage?(); // ModifyMonitoredItemsRequestMessage? | ModifyMonitoredItemsRequestMessage (optional) 

            try
            {
                ModifyMonitoredItemsResponseMessage result = apiInstance.ModifyMonitoredItems(modifyMonitoredItemsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ModifyMonitoredItems: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModifyMonitoredItemsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ModifyMonitoredItemsResponseMessage> response = apiInstance.ModifyMonitoredItemsWithHttpInfo(modifyMonitoredItemsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ModifyMonitoredItemsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **modifyMonitoredItemsRequestMessage** | [**ModifyMonitoredItemsRequestMessage?**](ModifyMonitoredItemsRequestMessage?.md) | ModifyMonitoredItemsRequestMessage | [optional]  |

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
| **200** | ModifyMonitoredItemsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="modifysubscription"></a>
# **ModifySubscription**
> ModifySubscriptionResponseMessage ModifySubscription (ModifySubscriptionRequestMessage? modifySubscriptionRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class ModifySubscriptionExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var modifySubscriptionRequestMessage = new ModifySubscriptionRequestMessage?(); // ModifySubscriptionRequestMessage? | ModifySubscriptionRequestMessage (optional) 

            try
            {
                ModifySubscriptionResponseMessage result = apiInstance.ModifySubscription(modifySubscriptionRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.ModifySubscription: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ModifySubscriptionWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ModifySubscriptionResponseMessage> response = apiInstance.ModifySubscriptionWithHttpInfo(modifySubscriptionRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ModifySubscriptionWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **modifySubscriptionRequestMessage** | [**ModifySubscriptionRequestMessage?**](ModifySubscriptionRequestMessage?.md) | ModifySubscriptionRequestMessage | [optional]  |

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
| **200** | ModifySubscriptionResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="publish"></a>
# **Publish**
> PublishResponseMessage Publish (PublishRequestMessage? publishRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class PublishExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var publishRequestMessage = new PublishRequestMessage?(); // PublishRequestMessage? | PublishRequestMessage (optional) 

            try
            {
                PublishResponseMessage result = apiInstance.Publish(publishRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Publish: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PublishWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<PublishResponseMessage> response = apiInstance.PublishWithHttpInfo(publishRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.PublishWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **publishRequestMessage** | [**PublishRequestMessage?**](PublishRequestMessage?.md) | PublishRequestMessage | [optional]  |

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
| **200** | PublishResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="read"></a>
# **Read**
> ReadResponseMessage Read (ReadRequestMessage? readRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class ReadExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var readRequestMessage = new ReadRequestMessage?(); // ReadRequestMessage? | ReadRequestMessage (optional) 

            try
            {
                ReadResponseMessage result = apiInstance.Read(readRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Read: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ReadWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<ReadResponseMessage> response = apiInstance.ReadWithHttpInfo(readRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.ReadWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **readRequestMessage** | [**ReadRequestMessage?**](ReadRequestMessage?.md) | ReadRequestMessage | [optional]  |

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
| **200** | ReadResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="registernodes"></a>
# **RegisterNodes**
> RegisterNodesResponseMessage RegisterNodes (RegisterNodesRequestMessage? registerNodesRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class RegisterNodesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var registerNodesRequestMessage = new RegisterNodesRequestMessage?(); // RegisterNodesRequestMessage? | RegisterNodesRequestMessage (optional) 

            try
            {
                RegisterNodesResponseMessage result = apiInstance.RegisterNodes(registerNodesRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.RegisterNodes: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RegisterNodesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<RegisterNodesResponseMessage> response = apiInstance.RegisterNodesWithHttpInfo(registerNodesRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.RegisterNodesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **registerNodesRequestMessage** | [**RegisterNodesRequestMessage?**](RegisterNodesRequestMessage?.md) | RegisterNodesRequestMessage | [optional]  |

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
| **200** | RegisterNodesResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="republish"></a>
# **Republish**
> RepublishResponseMessage Republish (RepublishRequestMessage? republishRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class RepublishExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var republishRequestMessage = new RepublishRequestMessage?(); // RepublishRequestMessage? | RepublishRequestMessage (optional) 

            try
            {
                RepublishResponseMessage result = apiInstance.Republish(republishRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Republish: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RepublishWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<RepublishResponseMessage> response = apiInstance.RepublishWithHttpInfo(republishRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.RepublishWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **republishRequestMessage** | [**RepublishRequestMessage?**](RepublishRequestMessage?.md) | RepublishRequestMessage | [optional]  |

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
| **200** | RepublishResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="setmonitoringmode"></a>
# **SetMonitoringMode**
> SetMonitoringModeResponseMessage SetMonitoringMode (SetMonitoringModeRequestMessage? setMonitoringModeRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class SetMonitoringModeExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var setMonitoringModeRequestMessage = new SetMonitoringModeRequestMessage?(); // SetMonitoringModeRequestMessage? | SetMonitoringModeRequestMessage (optional) 

            try
            {
                SetMonitoringModeResponseMessage result = apiInstance.SetMonitoringMode(setMonitoringModeRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.SetMonitoringMode: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SetMonitoringModeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<SetMonitoringModeResponseMessage> response = apiInstance.SetMonitoringModeWithHttpInfo(setMonitoringModeRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.SetMonitoringModeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **setMonitoringModeRequestMessage** | [**SetMonitoringModeRequestMessage?**](SetMonitoringModeRequestMessage?.md) | SetMonitoringModeRequestMessage | [optional]  |

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
| **200** | SetMonitoringModeResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="setpublishingmode"></a>
# **SetPublishingMode**
> SetPublishingModeResponseMessage SetPublishingMode (SetPublishingModeRequestMessage? setPublishingModeRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class SetPublishingModeExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var setPublishingModeRequestMessage = new SetPublishingModeRequestMessage?(); // SetPublishingModeRequestMessage? | SetPublishingModeRequestMessage (optional) 

            try
            {
                SetPublishingModeResponseMessage result = apiInstance.SetPublishingMode(setPublishingModeRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.SetPublishingMode: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SetPublishingModeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<SetPublishingModeResponseMessage> response = apiInstance.SetPublishingModeWithHttpInfo(setPublishingModeRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.SetPublishingModeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **setPublishingModeRequestMessage** | [**SetPublishingModeRequestMessage?**](SetPublishingModeRequestMessage?.md) | SetPublishingModeRequestMessage | [optional]  |

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
| **200** | SetPublishingModeResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="settriggering"></a>
# **SetTriggering**
> SetTriggeringResponseMessage SetTriggering (SetTriggeringRequestMessage? setTriggeringRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class SetTriggeringExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var setTriggeringRequestMessage = new SetTriggeringRequestMessage?(); // SetTriggeringRequestMessage? | SetTriggeringRequestMessage (optional) 

            try
            {
                SetTriggeringResponseMessage result = apiInstance.SetTriggering(setTriggeringRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.SetTriggering: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SetTriggeringWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<SetTriggeringResponseMessage> response = apiInstance.SetTriggeringWithHttpInfo(setTriggeringRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.SetTriggeringWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **setTriggeringRequestMessage** | [**SetTriggeringRequestMessage?**](SetTriggeringRequestMessage?.md) | SetTriggeringRequestMessage | [optional]  |

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
| **200** | SetTriggeringResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="transfersubscriptions"></a>
# **TransferSubscriptions**
> TransferSubscriptionsResponseMessage TransferSubscriptions (TransferSubscriptionsRequestMessage? transferSubscriptionsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class TransferSubscriptionsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var transferSubscriptionsRequestMessage = new TransferSubscriptionsRequestMessage?(); // TransferSubscriptionsRequestMessage? | TransferSubscriptionsRequestMessage (optional) 

            try
            {
                TransferSubscriptionsResponseMessage result = apiInstance.TransferSubscriptions(transferSubscriptionsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.TransferSubscriptions: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the TransferSubscriptionsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<TransferSubscriptionsResponseMessage> response = apiInstance.TransferSubscriptionsWithHttpInfo(transferSubscriptionsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.TransferSubscriptionsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **transferSubscriptionsRequestMessage** | [**TransferSubscriptionsRequestMessage?**](TransferSubscriptionsRequestMessage?.md) | TransferSubscriptionsRequestMessage | [optional]  |

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
| **200** | TransferSubscriptionsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="translatebrowsepathstonodeids"></a>
# **TranslateBrowsePathsToNodeIds**
> TranslateBrowsePathsToNodeIdsResponseMessage TranslateBrowsePathsToNodeIds (TranslateBrowsePathsToNodeIdsRequestMessage? translateBrowsePathsToNodeIdsRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class TranslateBrowsePathsToNodeIdsExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var translateBrowsePathsToNodeIdsRequestMessage = new TranslateBrowsePathsToNodeIdsRequestMessage?(); // TranslateBrowsePathsToNodeIdsRequestMessage? | TranslateBrowsePathsToNodeIdsRequestMessage (optional) 

            try
            {
                TranslateBrowsePathsToNodeIdsResponseMessage result = apiInstance.TranslateBrowsePathsToNodeIds(translateBrowsePathsToNodeIdsRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.TranslateBrowsePathsToNodeIds: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the TranslateBrowsePathsToNodeIdsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<TranslateBrowsePathsToNodeIdsResponseMessage> response = apiInstance.TranslateBrowsePathsToNodeIdsWithHttpInfo(translateBrowsePathsToNodeIdsRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.TranslateBrowsePathsToNodeIdsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **translateBrowsePathsToNodeIdsRequestMessage** | [**TranslateBrowsePathsToNodeIdsRequestMessage?**](TranslateBrowsePathsToNodeIdsRequestMessage?.md) | TranslateBrowsePathsToNodeIdsRequestMessage | [optional]  |

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
| **200** | TranslateBrowsePathsToNodeIdsResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="unregisternodes"></a>
# **UnregisterNodes**
> UnregisterNodesResponseMessage UnregisterNodes (UnregisterNodesRequestMessage? unregisterNodesRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class UnregisterNodesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var unregisterNodesRequestMessage = new UnregisterNodesRequestMessage?(); // UnregisterNodesRequestMessage? | UnregisterNodesRequestMessage (optional) 

            try
            {
                UnregisterNodesResponseMessage result = apiInstance.UnregisterNodes(unregisterNodesRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.UnregisterNodes: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UnregisterNodesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<UnregisterNodesResponseMessage> response = apiInstance.UnregisterNodesWithHttpInfo(unregisterNodesRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.UnregisterNodesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **unregisterNodesRequestMessage** | [**UnregisterNodesRequestMessage?**](UnregisterNodesRequestMessage?.md) | UnregisterNodesRequestMessage | [optional]  |

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
| **200** | UnregisterNodesResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="write"></a>
# **Write**
> WriteResponseMessage Write (WriteRequestMessage? writeRequestMessage = null)



### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class WriteExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://localhost:4840";
            var apiInstance = new DefaultApi(config);
            var writeRequestMessage = new WriteRequestMessage?(); // WriteRequestMessage? | WriteRequestMessage (optional) 

            try
            {
                WriteResponseMessage result = apiInstance.Write(writeRequestMessage);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling DefaultApi.Write: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the WriteWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    ApiResponse<WriteResponseMessage> response = apiInstance.WriteWithHttpInfo(writeRequestMessage);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling DefaultApi.WriteWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **writeRequestMessage** | [**WriteRequestMessage?**](WriteRequestMessage?.md) | WriteRequestMessage | [optional]  |

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
| **200** | WriteResponseMessage |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)


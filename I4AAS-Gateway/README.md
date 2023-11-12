### OPC UA REST API Demonstration

This demonstration shows how to use the OPC UA REST API to access OPC UA data from a web browser.

The sample server address space includes an AAS ProductCarbonFootprint and some simple variables and methods.

The 'Swagger' page allows developers to experiment with the OPC UA REST API directly.

Interesting requests are:

#### Browse
```
{
  "Body": {
    "RequestHeader": {
      "Timestamp": "2023-11-11T13:29:44.551Z",
      "RequestHandle": 1234,
      "TimeoutHint": 60000
    },
    "NodesToBrowse": [
      {
        "NodeId": "nsu=tag:opcua-is-great.net,2023:testing;s=Tests",
        "BrowseDirection": 0,
        "IncludeSubtypes": true,
        "NodeClassMask": 255,
        "ResultMask": 63
      }
    ]
  }
}
```

#### Read
```
{
  "Body": {
    "RequestHeader": {
      "Timestamp": "2023-11-11T13:22:50.400Z",
      "RequestHandle": 1234,
      "TimeoutHint": 60000,
    },
    "MaxAge": 0,
    "TimestampsToReturn": 0,
    "NodesToRead": [
      {
        "NodeId": "nsu=tag:opcua-is-great.net,2023:testing;s=Tests_Temperature",
        "AttributeId": 13
      }
    ]
  }
}
```

#### Write
```
{
  "Body": {
    "RequestHeader": {
      "Timestamp": "2023-11-12T12:36:22.221Z",
      "RequestHandle": 1234,
      "TimeoutHint": 60000
    },
    "NodesToWrite": [
      {
        "NodeId": "tag:opcua-is-great.net,2023:testing;s=Tests_Temperature",
        "AttributeId": 13,
        "Value": {
          "Value": {
            "Type": 11,
            "Body": 123.45
          }
        }
      }
    ]
  }
}
```

#### Call
```
{
  "Body": {
    "RequestHeader": {
      "Timestamp": "2023-11-12T12:36:22.221Z",
      "RequestHandle": 1234,
      "TimeoutHint": 60000
    },
    "MethodsToCall": [
      {
        "ObjectId": "nsu=tag:opcua-is-great.net,2023:testing;s=Tests",
        "MethodId": "nsu=tag:opcua-is-great.net,2023:testing;s=Tests_Reset",
        "InputArguments": [
          {
            "Type": 11,
            "Body": 98.765
          }
        ]
      }
    ]
  }
}
```
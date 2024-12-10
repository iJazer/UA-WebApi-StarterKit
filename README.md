# UA-WebApi-StarterKit
A set of sample applications used to demonstrate the OPC UA WebApi based on [OpenAPI](https://swagger.io/specification/) definitions. 

The OPC UA WebApi describes the OPC UA services described in [Part 4](https://reference.opcfoundation.org/Core/Part4/v105/docs/). The primary documentation for the OPC UA WebApi can be found in the linked specification. 

The follow projects are part of the repository:
| Project | Description | 
|---|---| 
| [UaWebApiGateway](./UaWebApiGateway/) | A React-TypeScript OPC UA Client to connect to a C# OPC UA Server using the OPC UA WebApi. It supports HTTPS and WebSockets based communication. |
| [UaWebApiClient](./UaWebApiClient/) | A number of simple OPC UA Clients written use different development environments that uses the OPC UA WebApi. |
| [OpenApiGenerator](./OpenApiGenerator/) | The [OpenApiGenerator](https://openapi-generator.tech/) is a tool written in Java that uses an OpenAPI definition to generate code in different programming languages. |

## UaWebApiGateway

The UaWebApiGateway is available on the [web](https://webapi.opcfoundation.org/). 

The Project [UaWebApiGateway](./UaRestGateway/UaRestGateway.sln) is a React/TypeScript client that runs in a web browser and a OPC UA Server that supports the basic [OPC UA Web API](https://opcua-rest-gateway.azurewebsites.net/swagger). 
The client application uses OAuth2 to authenticate users with the [OPC Foundation website](https://opcfoundation.org/login). The JSON Web Token (JWT) returned by the OPC Foundation website passed in the HTTP Authorization header.  

The UaRestGateway server has two parts: the REST handler and an OPC UA Server. The REST handler validates the JWT passed in the HTTP Authorization header and routes the request to the OPC UA Server. 

The OPC UA Server supports an IssuedUserIdentityToken that supports JWTs as described in [Part 6](https://reference.opcfoundation.org/Core/Part6/v105/docs/6.5.2). The JWT is passed to the OPC UA Server where is validated again (a technically unnecessary step but done to provide sample code on how to do it). The OPC UA Server component then uses the JWT to fetch membership information from the OPC Foundation website. If the JWT was issued to an OPC Foundation member then they are granted the "Engineer" well known role.

The Nodes that appear under the Objects Folder change depending on thec current. If logged in with an account that is a OPC Foundation member the "Reset" method is visible.

Note that the OPC Foundation login requires a secret assigned by the OPC Foundation website. This secret is not part of the code base for security reasons. Getting the code to work will require that each user provide credentials to their OAuth2 server. This can be done fairly easily by creating a Word Press instance and installing the [OAuth2 plugin](https://wp-oauth.com/documentation/).

The OPC UA Server exposes a simple [information model](./UaWebApiGateway/UaRestGateway.Server/Service/Measurements/Measurements.NodeSet2.xml) which includes a ObjectType with a few AnalogItemType Variables and a Method. It also includes a Variable with a Structure value to demonstrate the use of DataTypes defined in a custom information model.

## UaWebApiClient

The following sample clients are available:

| Client | Description | 
|---|---| 
| [DotNet](./UaWebApiClient/csharp/) | A client written in C# that uses the [DotNet stubs](https://github.com/OPCFoundation/opcua-webapi-dotnet). |
| [NodeJS](./UaWebApiClient/nodejs) | A client written in JavaScript that uses the [TypeScript stubs](https://github.com/OPCFoundation/opcua-webapi-typescript). |
| [React](./UaWebApiClient/python) | A client written in TypeScript and runs in a Web Browser that uses the [TypeScript stubs](https://github.com/OPCFoundation/opcua-webapi-typescript). |
| [Python](./UaWebApiClient/python) | A client written in JavaScript that uses the [Python stubs](https://github.com/OPCFoundation/opcua-webapi-python). |

The stubs are pre-built libraries that define classes and constants needed to implemenet the OPC UA Web API in a specific environment. The libraries are generated with the [OpenApiGenerator](https://openapi-generator.tech/docs/installation) and the [OPC UA OpenApi schema](https://github.com/OPCFoundation/UA-Nodeset/blob/latest/OpenApi/opc.ua.openapi.allservices.json). These pre-built libraries also include definitions for BrowseNames and NodeIds defined by the specification.

The sample clients have knowledge of the OPC UA Server's information model and use the [NodeSet](./UaWebApiClient/NodeSets/README.md) to build classes that can be used to programmatically. The [Opc.Ua.ModelCompiler](https://github.com/OPCFoundation/UA-ModelCompiler) is used convert any OPC UA information model defined in a NodeSet into an OpenAPI schema file. This file can then be used to generate the classes that are used by the sample clients to handle the DataTypes defined by the information model.





# UA-WebApi-StarterKit
A set of sample applications used to demonstrate the OPC UA WebApi based on [OpenAPI](https://swagger.io/specification/) definitions. 

The OPC UA WebApi describes the OPC UA services described in [Part 4](https://reference.opcfoundation.org/Core/Part4/v105/docs/). The primary documentation for the OPC UA WebApi can be found in the linked specification. 

The follow projects are part of the repository:
| Project | Description | 
|---|---| 
| [UaWebApiGateway](./UaWebApiGateway/) | A React-TypeScript OPC UA Client to connect to a C# OPC UA Server using the OPC UA WebApi. It supports HTTPS and WebSockets based communication |
| [UaWebApiClient](./UaWebApiClient/) | A number of simple OPC UA Clients written use different development environments that uses the OPC UA WebApi. |
| [OpenApiGenerator](./OpenApiGenerator/) | The [OpenApiGenerator](https://openapi-generator.tech/) is a tool written in Java that uses an OpenAPI definition to generate code in different programming languages. | 

The UaRestGateway is available on the [web](https://opcua-rest-gateway.azurewebsites.net/). 

The Project [UaWebApiGateway](./UaRestGateway/UaRestGateway.sln) is a React/TypeScript client that runs in a web browser and a OPC UA Server that supports the basic [OPC UA Web API](https://opcua-rest-gateway.azurewebsites.net/swagger). 
The client application uses OAuth2 to authenticate users with the [OPC Foundation website](https://opcfoundation.org/login). The JSON Web Token (JWT) returned by the OPC Foundation website passed in the HTTP Authorization header.  

The UaRestGateway server has two parts: the REST handler and an OPC UA Server. The REST handler validates the JWT passed in the HTTP Authorization header and routes the request (using a UA TCP connection) to the OPC UA Server. 

The OPC UA Server supports an IssuedUserIdentityToken that supports JWTs as described in [Part 6](https://reference.opcfoundation.org/Core/Part6/v105/docs/6.5.2). The JWT is passed to the OPC UA Server where is validated again (a technically unnecessary step but done to provide sample code on how to do it). The OPC UA Server component then uses the JWT to fetch membership information from the OPC Foundation website. If the JWT was issued to an OPC Foundation member then they are granted the "Engineer" well known role.

The Nodes that appear under the Objects Folder change depending on thec current. If logged in with an account that is a OPC Foundation member the "OnlyMembersCanSee" variable is visible.

Note that the OPC Foundation login requires a secret assigned by the OPC Foundation website. This secret is not part of the code base for security reasons. Getting the code to work will require that each user provide credentials to their OAuth2 server. This can be done fairly easily by creating a Word Press instance and installing the [OAuth2 plugin](https://wp-oauth.com/documentation/).

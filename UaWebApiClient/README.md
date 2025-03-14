## OPC UA WebApi Client Starter Kits

### Introduction

The contents of this folder have sample OPC UA WebApi clients using different programming languages. The clients are built using the [OpenAPI Generator](https://openapi-generator.tech/docs/installation) to generate OPC UA specific types that are used to construct and parse the requests and responses. The [OPC UA OpenAPI Schema](../UaRestGateway/UaRestGateway.Server/wwwroot/data/opc.ua.openapi.allservices.json) that is published with the OPC UA specification is the input to the OpenAPI Generator.

The samples in different programming languages can be found here:

1. [C#](./csharp)
2. [Python](./python)
3. [Node.js](./nodejs)
4. [TypeScript/React](./typescript-react)

Each of the above folders contains a README file that provides instructions on how to build the OPC UA specific types. 

Developing an OPC UA client also requires from constants for well known node ids, status codes, et .al. The [UA Model Compiler](https://github.com/OPCFoundation/UA-ModelCompiler) generates these constants for different programming languages. The generated constants can be found in the [here](https://github.com/OPCF-Members/UA-NodeSet/tree/latest/OpenApi).

### Sample Funcationality

The samples demonstrate the following functionality:

	1. Connect to an OPC UA server using Basic authentication;
	2. Browse the server address space;
	3. Read the value of a variable nodes;
	4. Write a value to a variable nodes;
	5. Browse the arguments of a method node;
	6. Call a method.
	7. Read a server defined structure value;
	8. Write a server defined structure value;




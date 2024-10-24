## Generating Classes from NodeSets

Information models may define DataTypes that client applications need to consume. Classes representing DataTypes in different development environments may be generated using the [OpenAPI Generator](https://openapi-generator.tech/).

The OpenAPI file needed as an input to the [OpenAPI Generator](https://openapi-generator.tech/) is produced by the [UA Model Compiler](https://github.com/OPCFoundation/UA-ModelCompiler). The [script](./create_openapi_schema.ps1) invokes the UA Model Compiler and outputs the result to this [directory](../Model/Measurements).

The output also produces the constants defined by the NodeSet.


@echo off
setlocal

REM This generates C# classes for the DataTypes in the model used by the sample.
REM It uses the OpenAPI Generator: https://openapi-generator.tech/docs/installation/
REM It also copies the NodeIds and BrowseName constant declarations that were generated with the openapi schema.
REM These constants are also published with the NodeSet for new companion specifications: https://github.com/OPCFoundation/UA-Nodeset

set ROOT=%~dp0
SET JAVA="java.exe" 
set MODEL_NAME=Measurements
set MODEL_URI=urn:opcfoundation.org:2024-10:starterkit:measurements
SET OUTPUT="%ROOT%%MODEL_NAME%"

%JAVA% -jar "%ROOT%..\..\OpenApiGenerator\openapi-generator-cli.jar" generate -g csharp -i "%ROOT%..\Model\%MODEL_NAME%\%MODEL_NAME%.openapi.json" -o "%ROOT%%MODEL_NAME%" -p targetFramework=netstandard2.0,packageName=%MODEL_NAME%,packageVersion=1.0.0,optionalProjectFile=%MODEL_NAME%,generateModelDocumentation=false

copy "%ROOT%..\Model\%MODEL_NAME%\CSharp\*" "%ROOT%%MODEL_NAME%\src\%MODEL_NAME%"


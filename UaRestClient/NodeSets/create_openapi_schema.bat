@echo off
setlocal

REM This script creates files containing the BrowseNames and NodeIds from a NodeSet in different languages.
REM It also creates a OpenAPI schema file that can be used to generate language specific classes for any DataTypes.
REM Note that OpenAPI file is published with the NodeSet for new releases of companion specifications: https://github.com/OPCFoundation/UA-Nodeset
REM The Opc.Ua.ModelCompiler can be fetched and built from here: https://github.com/OPCFoundation/UA-ModelCompiler
REM A docker image is available for user that do not have the .NET build environment.

set MC="D:\Work\OPC\UA-ModelCompiler\build\bin\Release\net8.0\Opc.Ua.ModelCompiler.exe"
set NODESET_DIR="."
set MODEL_NAME=Measurements
set MODEL_URI=urn:opcfoundation.org:2024-10:starterkit:measurements

echo Building Files for NodeSet with URI=%MODEL_URI%
%MC% compile-nodesets -input %NODESET_DIR% -o2 ..\Model\%MODEL_NAME% -uri %MODEL_URI% -prefix %MODEL_NAME%
echo ..\Model\%MODEL_NAME%\%MODEL_NAME%_openapi.json created.
 




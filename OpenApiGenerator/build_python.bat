ECHO OFF
SETLOCAL

set ROOT=%~dp0
SET JAVA="java.exe" 
SET NODESETS="%ROOT%\..\..\nodesets\v105"
SET OUTPUT="%ROOT%\..\UaRestClient\opcua-webapi-python"

IF EXIST %OUTPUT%\generated RMDIR /Q /S %OUTPUT%\generated

%JAVA% -jar openapi-generator-cli.jar generate -g python -i %NODESETS%\Schema\opc.ua.openapi.allservices.json -o %OUTPUT% -p packageName=opcua_webapi,projectName=opcua-webapi,packageVersion=1.504.0,packageUrl=https://github.com/opcfoundation-org/opcua-webapi-python/

FOR %%f IN (%NODESETS%\OpenApi\Python\*.py) DO (
    COPY /Y "%%~dpnxf" "%OUTPUT%\opcua_webapi"
)
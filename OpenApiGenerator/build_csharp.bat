ECHO ON
SETLOCAL

set ROOT=%~dp0
SET JAVA="java.exe" 
SET NODESETS="%ROOT%\..\..\nodesets\v105"
SET OUTPUT="%ROOT%\..\UaRestClient\opcua-webapi-net"

IF EXIST %OUTPUT%\generated RMDIR /Q /S %OUTPUT%

%JAVA% -jar openapi-generator-cli.jar generate -g csharp -i %NODESETS%\Schema\opc.ua.openapi.allservices.json -o %OUTPUT% -p packageName=Opc.Ua.WebApi,packageVersion=1.504.0,optionalProjectFile=Opc.Ua.WebApi

FOR %%f IN (%NODESETS%\OpenApi\CSharp\*.cs) DO (
    COPY /Y "%%~dpnxf" "%OUTPUT%\src\Opc.Ua.WebApi"
)
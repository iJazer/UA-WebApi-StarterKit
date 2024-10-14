ECHO OFF
SETLOCAL

set ROOT=%~dp0
SET JAVA="java.exe" 
SET NODESETS="%ROOT%\..\..\nodesets\v105"
SET OUTPUT=..\UaRestClient\opcua-webapi-typescript

REM IF EXIST %OUTPUT% RMDIR /Q /S %OUTPUT%

%JAVA% -jar openapi-generator-cli.jar generate -g typescript-fetch -i %NODESETS%\Schema\opc.ua.openapi.allservices.json -o %OUTPUT% -p enumPropertyNaming=PascalCase,modelPropertyNaming=PascalCase,npmName=opcua-webapi,npmRepository=https://github.com/OPCFoundation/opcua-webapi,supportsES6=true

FOR %%f IN (%NODESETS%\OpenApi\TypeScript\*.ts) DO (
   COPY /Y "%%~dpnxf" "%OUTPUT%"\src
)
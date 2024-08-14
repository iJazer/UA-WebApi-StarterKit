ECHO OFF
SETLOCAL

set ROOT=%~dp0
SET JAVA="C:\Program Files\Java\jdk-21\bin\java.exe" 
SET NODESETS=%ROOT%..\..\nodesets\v105
SET OUTPUT=%ROOT%..\UaRestGateway\uarestgateway.client\src\opcua

IF EXIST "%OUTPUT%\models" RMDIR /Q /S "%OUTPUT%\models"
IF EXIST "%OUTPUT%\apis" RMDIR /Q /S "%OUTPUT%\apis"

%JAVA% -jar openapi-generator-cli.jar generate -g typescript-fetch -i "%NODESETS%\OpenApi\opc.ua.openapi.allservices.json" -p enumPropertyNaming=PascalCase,modelPropertyNaming=PascalCase -o "%OUTPUT%"

REM Fix the generated index.ts 
IF EXIST "%OUTPUT%\apis" RMDIR /Q /S "%OUTPUT%\apis"
DEL "%OUTPUT%\index.ts"

ECHO /* tslint:disable */ >> "%OUTPUT%\index.ts"
ECHO /* eslint-disable */ >> "%OUTPUT%\index.ts"
ECHO export * from './runtime'; >> "%OUTPUT%\index.ts"
ECHO export * from './models/index'; >> "%OUTPUT%\index.ts"
ECHO export * from './opcua_attributes.ts' >> "%OUTPUT%\index.ts"
ECHO export * from './opcua_constants.ts' >> "%OUTPUT%\index.ts"
ECHO export * from './opcua_statuscodes.ts' >> "%OUTPUT%\index.ts"

COPY /Y "%NODESETS%\OpenApi\opc.ua.openapi.allservices.json" "%ROOT%..\UaRestGateway\UaRestGateway.Server\wwwroot\data\opc.ua.openapi.allservices.json"

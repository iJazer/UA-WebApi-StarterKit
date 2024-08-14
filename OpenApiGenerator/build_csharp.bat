ECHO OFF
SETLOCAL

set ROOT=%~dp0
SET JAVA="C:\Program Files\Java\jdk-21\bin\java.exe" 
SET NODESETS="%ROOT%\..\..\nodesets\v105"
SET OUTPUT="%ROOT%\..\UaRestClient\csharp"

IF EXIST %OUTPUT%\generated RMDIR /Q /S %OUTPUT%\generated

%JAVA% -jar openapi-generator-cli.jar generate -g csharp -i %NODESETS%\Schema\opc.ua.openapi.allservices.json -o %OUTPUT%\generated 

FOR %%f IN (%NODESETS%\OpenApi\CSharp\*.cs) DO (
    COPY /Y "%%~dpnxf" "%OUTPUT%"
)
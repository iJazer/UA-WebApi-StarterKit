@echo on
setlocal

REM This generates Python classes for the DataTypes in the model used by the sample.
REM It uses the OpenAPI Generator: https://openapi-generator.tech/docs/installation/
REM It also copies the NodeIds and BrowseName constant declarations that were generated with the openapi schema.
REM These constants are also published with the NodeSet for new companion specifications: https://github.com/OPCFoundation/UA-Nodeset
REM The script creates a package which is not needed for the sample. The needed files are copied and the package deleted.

set ROOT=%~dp0
SET JAVA="java.exe" 
set MODEL_NAME=Measurements
set MODEL_URI=urn:opcfoundation.org:2024-10:starterkit:measurements
SET OUTPUT="%ROOT%%MODEL_NAME%"

%JAVA% -jar "%ROOT%..\..\OpenApiGenerator\openapi-generator-cli.jar" generate -g python -i "%ROOT%..\Model\%MODEL_NAME%\%MODEL_NAME%.openapi.json" -o "%ROOT%%MODEL_NAME%" -p packageName=%MODEL_NAME%,projectName=%MODEL_NAME%,packageVersion=1.0.0 --global-property models

copy "%ROOT%..\Model\%MODEL_NAME%\Python\*" "%ROOT%"
copy "%ROOT%%MODEL_NAME%\%MODEL_NAME%\models\*.py" "%ROOT%"

del "%ROOT%response.py"
rmdir /S /Q "%ROOT%%MODEL_NAME%"
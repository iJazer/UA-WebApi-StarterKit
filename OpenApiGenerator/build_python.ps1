$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$NodeSets = "$Root\..\..\nodesets\v105"
$Java = "java.exe"
$ModelName = "OpcUa"
$ModelUri = "http://opcfoundation.org/UA/"
$ProjectName = "opcua-webapi"
# $Output = "D:\Work\OPC\UA-IIoT-StarterKit\python\opcua-webapi"
$Output = "..\UaWebApiClient\opcua-webapi\python"

& $Java -jar ".\openapi-generator-cli.jar" generate -g python `
    -i "$NodeSets\Schema\opc.ua.openapi.allservices.json" `
    -o $Output `
    -p packageName=opcua_webapi,projectName=$ProjectName,packageVersion=1.504.1,packageUrl=https://github.com/opcfoundation-org/opcua-webapi-python/

Get-ChildItem -Path "$NodeSets\OpenApi\Constants\Python" -Filter "*.py" | ForEach-Object {
    Copy-Item  -Path "$NodeSets\OpenApi\Constants\Python\$_" -Destination "$Output\opcua_webapi"
}
		
$Imports = @"

# import OPC UA constants
from opcua_webapi.opcua_constants import BrowseNames
from opcua_webapi.opcua_constants import BuiltInType
from opcua_webapi.opcua_constants import ObjectIds
from opcua_webapi.opcua_constants import VariableIds
from opcua_webapi.opcua_constants import MethodIds
from opcua_webapi.opcua_constants import ObjectTypeIds
from opcua_webapi.opcua_constants import VariableTypeIds
from opcua_webapi.opcua_constants import DataTypeIds
from opcua_webapi.opcua_constants import ReferenceTypeIds
from opcua_webapi.opcua_attributes import Attributes
from opcua_webapi.opcua_statuscodes import StatusCodes
from opcua_webapi.opcua_statuscodes import StatusUtils
"@

Add-Content -Path "$Output\opcua_webapi\__init__.py" -Value $Imports

cd $Output 
& git checkout -- setup.py
& git checkout -- pyproject.toml

$content = Get-Content opcua_webapi\models\message_security_mode.py
$content = $content -replace "None = 1", "NoSignOrEncrypt = 1"
$content | Set-Content opcua_webapi\models\message_security_mode.py

& pip uninstall -y opcua-webapi
& python setup.py bdist_wheel
& pip install dist/opcua_webapi-1.504.1-py3-none-any.whl

cd $Root 


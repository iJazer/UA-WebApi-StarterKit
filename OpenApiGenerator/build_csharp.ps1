$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$NodeSets = "$Root\..\..\nodesets\v105"
$Java = "java.exe"
$ModelName = "OpcUa"
$ModelUri = "http://opcfoundation.org/UA/"
$ProjectName = "opcua-webapi"
$Output = "D:\Work\OPC\UA-IIoT-StarterKit\dotnet\opcua-webapi"

& $Java -jar ".\openapi-generator-cli.jar" generate -g csharp `
    -i "$NodeSets\Schema\opc.ua.openapi.allservices.json" `
    -o $Output `
    -p packageName=Opc.Ua.WebApi,packageVersion=1.504.0,optionalProjectFile=Opc.Ua.WebApi,targetFramework=netstandard2.0

Get-ChildItem -Path "$NodeSets\OpenApi\Constants\CSharp" -Filter "*.cs" | ForEach-Object {
    Copy-Item  -Path "$NodeSets\OpenApi\Constants\CSharp\$_" -Destination "$Output\src\Opc.Ua.WebApi"
}

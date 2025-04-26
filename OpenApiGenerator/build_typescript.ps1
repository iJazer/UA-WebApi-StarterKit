$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$NodeSets = "$Root\..\..\nodesets\v105"
$Java = "java.exe"
$ModelName = "OpcUa"
$ModelUri = "http://opcfoundation.org/UA/"
$ProjectName = "opcua-webapi"
$Output = "..\UaWebApiClient\opcua-webapi\typescript"

& $Java -jar ".\openapi-generator-cli.jar" generate -g typescript-fetch `
    -i "$NodeSets\Schema\opc.ua.openapi.allservices.json" `
    -o $Output `
    -p enumPropertyNaming=PascalCase,modelPropertyNaming=PascalCase,npmName=$ProjectName,npmRepository=https://github.com/OPCFoundation/opcua-webapi-typescript,supportsES6=true,npmVersion=1.504.1

Get-ChildItem -Path "$NodeSets\OpenApi\Constants\TypeScript" -Filter "*.ts" | ForEach-Object {
    $export = "export * from './" + $_.BaseName + "';"
    Copy-Item  -Path "$NodeSets\OpenApi\Constants\TypeScript\$_" -Destination "$Output\src"
    Add-Content -Path "$Output\src\index.ts" -Value $export
}

cd $Output 
& git checkout -- package.json
& npm update
& npm run build

cd $Root

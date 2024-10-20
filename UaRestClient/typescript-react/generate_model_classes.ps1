# This generates a TypeScript package for the DataTypes in the model used by the sample.
# It uses the OpenAPI Generator: https://openapi-generator.tech/docs/installation/
# It also copies the NodeIds and BrowseName constant declarations that were generated with the OpenAPI schema.
# These constants are also published with the NodeSet for new companion specifications: https://github.com/OPCFoundation/UA-Nodeset
# The script creates a package which is not needed for the sample. The needed files are copied and the package deleted.

$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$Java = "java.exe"
$ModelName = "Measurements"
$ModelUri = "urn:opcfoundation.org:2024-10:starterkit:measurements"
$ProjectName = $ModelName.ToLower()
$Output = Join-Path $Root $ProjectName

& $Java -jar "$Root\..\..\OpenApiGenerator\openapi-generator-cli.jar" generate -g typescript-fetch `
    -i "D:\Work\OPC\UA-REST-StarterKit\UaRestClient\Model\Measurements\measurements.openapi.json" `
    -o $Output `
    -p enumPropertyNaming=PascalCase,modelPropertyNaming=PascalCase,npmName=$ProjectName,supportsES6=true

$ConstantsFile = $ProjectName + "_constants.ts"
Copy-Item -Path "$Root\..\Model\$ModelName\TypeScript\$ConstantsFile" -Destination "$ProjectName\src\$ConstantsFile"
$ImportStatement = "export * from './$ProjectName" + "_constants'"
Add-Content -Path "$ProjectName\src\index.ts" -Value $ImportStatement

cd $ProjectName
Write-Host Update Package
& npm update

Write-Host Publish Package
& npm link

cd "$Root\client"

Write-Host Link Package
& npm link ..\$ProjectName



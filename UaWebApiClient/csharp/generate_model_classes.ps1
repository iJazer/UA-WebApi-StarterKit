# This generates C# classes for the DataTypes in the model used by the sample.
# It uses the OpenAPI Generator: https://openapi-generator.tech/docs/installation/
# It also copies the NodeIds and BrowseName constant declarations that were generated with the openapi schema.
# These constants are also published with the NodeSet for new companion specifications: https://github.com/OPCFoundation/UA-Nodeset

$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$Java = "java.exe"
$ModelName = "Measurements"
$ModelUri = "urn:opcfoundation.org:2024-10:starterkit:measurements"
$ProjectName = $ModelName.ToLower()
$Output = Join-Path $Root $ModelName

& $Java -jar "$Root\..\..\OpenApiGenerator\openapi-generator-cli.jar" generate -g csharp `
    -i "$Root\..\Model\$ModelName\$ProjectName.openapi.json" `
    -o $Output `
    -p targetFramework=netstandard2.0,packageName=$ModelName,packageVersion=1.0.0,optionalProjectFile=$ModelName,generateModelDocumentation=false

$ConstantsFile = $ProjectName + "_constants.cs"
Copy-Item -Path "$Root\..\Model\$ModelName\Constants\CSharp\$ConstantsFile" -Destination "$Root\$ModelName\src\$ModelName\Model\$ConstantsFile"

& cd $ModelName
& dotnet tool install dotnet-outdated-tool --global
& dotnet outdated --upgrade
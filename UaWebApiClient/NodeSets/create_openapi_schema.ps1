# This script creates files containing the BrowseNames and NodeIds from a NodeSet in different languages.
# It also creates a OpenAPI schema file that can be used to generate language specific classes for any DataTypes.
# Note that OpenAPI file is published with the NodeSet for new releases of companion specifications: https://github.com/OPCFoundation/UA-Nodeset
# The Opc.Ua.ModelCompiler can be fetched and built from here: https://github.com/OPCFoundation/UA-ModelCompiler
# A docker image is available for user that do not have the .NET build environment.

$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$Compiler="$Root\..\..\..\UA-ModelCompiler\build\bin\Release\net8.0\Opc.Ua.ModelCompiler.exe"
$Java = "java.exe"
$ModelName = "Measurements"
$ModelUri = "urn:opcfoundation.org:2024-10:starterkit:measurements"
$ProjectName = $ModelName.ToLower()
$Output = Join-Path $Root $ModelName

Write-Host Building Files for NodeSet with URI=$ModelUri
& $Compiler compile-nodesets -input . -o2 ..\Model\$ModelName -uri $ModelUri -prefix $ModelName
Write-Host "..\Model\$ModelName\$ProjectName`_openapi.json created."
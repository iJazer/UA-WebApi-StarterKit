$Root = Split-Path -Parent $MyInvocation.MyCommand.Definition
$Java = "java.exe"
$ModelName = "Measurements"
$ModelUri = "urn:opcfoundation.org:2024-10:starterkit:measurements"
$ProjectName = $ModelName.ToLower()
$Output = Join-Path $Root $ProjectName

& $Java -jar "$Root\..\..\OpenApiGenerator\openapi-generator-cli.jar" generate -g python `
    -i "$Root\..\Model\Measurements\measurements.openapi.json" `
    -o $Output `
    -p packageName=$ModelName,projectName=$ProjectName,packageVersion=1.0.0 --global-property models

$ConstantsFile = $ProjectName + "_constants.py"
Copy-Item -Path "$Root\..\Model\$ModelName\Constants\Python\$ConstantsFile" -Destination "$ConstantsFile"
Copy-Item "$Root\$ProjectName\$ModelName\models\*.py" -Destination "$Root" -Force

Remove-Item -Path "$Root\Response.py"
Remove-Item -Path "$Root\$ProjectName" -Recurse

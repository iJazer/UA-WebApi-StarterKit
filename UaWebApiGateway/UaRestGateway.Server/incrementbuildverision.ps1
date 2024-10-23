
$filepath = "..\uarestgateway.client\"
$counter = Get-Content $filepath/"counter.txt"
$counter = [convert]::ToInt32($counter)
$counter++
$counter | Out-file -FilePath $filepath/"counter.txt"
$counter = "{0:0000}" -f $counter
$now = Get-Date -Format "yyyy-MM-dd";
$now = "export const BuildVersion : string = '[" + $now  + "-" + $counter + "]';"
Write-Host $now
$now | Out-file -Encoding ASCII -FilePath $filepath/"src/version.ts"

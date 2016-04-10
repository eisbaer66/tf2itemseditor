param(
[string]$GitHubToken,
[string]$ReleaseName)

$ErrorActionPreference = "Stop"

$zipLocation = "$Env:SYSTEM_DEFAULTWORKINGDIRECTORY/tf2itemseditor/zip/TF2ItemsEditor.zip"

$authheader = "Basic " + ([Convert]::ToBase64String([System.Text.encoding]::ASCII.GetBytes($GitHubToken)))
$header = @{Authorization=$authheader}


$body = "{""tag_name"": ""$ReleaseName"",""target_commitish"": ""eisbaer"",""name"": ""$ReleaseName"",""body"": """",""draft"": true,""prerelease"": true}"

$response = Invoke-RestMethod -Method Post -Uri https://api.github.com/repos/eisbaer66/tf2itemseditor/releases -Body $body -Headers $header;
$uploadUrl = $response.upload_url
$indexEndOfUploadUrl = $uploadUrl.IndexOf("{")
$uploadUrl = $uploadUrl.Substring(0, $indexEndOfUploadUrl)
$uploadUrl = $uploadUrl + "?name=TF2ItemsEditor.zip"
Invoke-RestMethod -Method Post -Uri $uploadUrl -InFile $zipLocation -Headers $header -ContentType "application/zip";
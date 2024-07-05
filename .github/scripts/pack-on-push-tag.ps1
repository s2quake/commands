#!/usr/local/bin/pwsh
param (
    [Parameter(Mandatory = $true)]
    [string]$OutputPath,
    [Parameter(Mandatory = $true)]
    [string]$tagName
)

if (!$env:PRIVATE_KEY_PATH) {
    throw "Environment variable 'PRIVATE_KEY_PATH' is not set."
}

if (!$env:NUGET_API_KEY) {
    throw "Environment variable 'NUGET_API_KEY' is not set."
}

Remove-Item -Path $OutputPath -Force -Recurse -ErrorAction SilentlyContinue

.github/scripts/pack.ps1 `
    -OutputPath $OutputPath `
    -KeyPath $env:PRIVATE_KEY_PATH `
    -CommitSHA $tagName

$nupkgs = Get-ChildItem -Path $OutputPath -Filter "*.nupkg" | ForEach-Object { $_.FullName }
$nupkgs | ForEach-Object {
    dotnet nuget push `
        $_ `
        --api-key $env:NUGET_API_KEY `
        --source https://api.nuget.org/v3/index.json
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to push $_ to NuGet."
    }
}

gh release create --generate-notes --latest --title "Release $tagName" $tagName $nupkgs

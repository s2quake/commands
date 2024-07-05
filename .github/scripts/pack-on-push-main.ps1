#!/usr/local/bin/pwsh
param (
    [Parameter(Mandatory = $true)]
    [string]$OutputPath,
    [Parameter(Mandatory = $true)]
    [ValidateScript({ $_ })]
    [string]$CommitSHA
)

if (!$env:PRIVATE_KEY_PATH) {
    throw "Environment variable 'PRIVATE_KEY_PATH' is not set."
}

if (!$env:NUGET_API_KEY) {
    throw "Environment variable 'NUGET_API_KEY' is not set."
}

$commitMessage = "$(git log $CommitSHA -1 --pretty=%B)"
$pattern = "(?<=Merge pull request #)(\d+)"
if (!($commitMessage -match $pattern)) {
    throw "Commit message does not contain a pull request number."
}

$pullRequestNumber = $Matches[1]

Remove-Item -Path $OutputPath -Force -Recurse -ErrorAction SilentlyContinue

.github/scripts/pack.ps1 `
    -OutputPath $OutputPath `
    -PullRequestNumber $pullRequestNumber `
    -KeyPath $env:PRIVATE_KEY_PATH `
    -CommitSHA $CommitSHA

$labelsValue = gh pr view $pullRequestNumber --json labels | ConvertFrom-Json -AsHashtable
$labels = $labelsValue | Select-Object -ExpandProperty labels | ForEach-Object {
    $_.name
}
$skipNugetUpload = $labels -contains "skip nuget upload"
if ($skipNugetUpload) {
    Write-Host "Skipping NuGet upload"
}

$nupkgs = Get-ChildItem -Path $OutputPath -Filter "*.nupkg" | ForEach-Object { $_.FullName }
$nupkgs = $skipNugetUpload ? @() : $nupkgs
$nupkgs | ForEach-Object {
    dotnet nuget push `
        $_ `
        --api-key $env:NUGET_API_KEY `
        --source https://api.nuget.org/v3/index.json
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to push $_ to NuGet."
    }
}

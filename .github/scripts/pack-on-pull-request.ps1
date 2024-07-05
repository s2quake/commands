#!/usr/local/bin/pwsh
param (
    [Parameter(Mandatory = $true)]
    [string]$OutputPath,
    [Parameter(Mandatory = $true)]
    [int]$PullRequestNumber
)

if (!$env:PRIVATE_KEY_PATH) {
    throw "Environment variable 'PRIVATE_KEY_PATH' is not set."
}

.github/scripts/pack.ps1 `
    -OutputPath $OutputPath `
    -PullRequestNumber $PullRequestNumber `
    -KeyPath $env:PRIVATE_KEY_PATH

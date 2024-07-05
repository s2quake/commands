#!/usr/local/bin/pwsh
param (
    [ValidateScript({ ($_ -eq "") -or !(Test-Path $_ -PathType Leaf) })]
    [string]$OutputPath = "",
    [ValidateScript({ $_ -ge 0 })]
    [int]$PullRequestNumber = 0,
    [ValidateScript({ ($_ -eq "") -or (Test-Path $_) })]
    [string]$KeyPath = "",
    [string]$CommitSHA = ""
)

$namespaces = @{
    ns = "http://schemas.microsoft.com/developer/msbuild/2003"
}
$propsPath = "Directory.Build.props"
$fileVersionPath = "/ns:Project/ns:PropertyGroup/ns:FileVersion"
$result = Select-Xml -Path $propsPath -Namespace $namespaces -XPath $fileVersionPath
if ($null -eq $result) {
    Write-Host "File version not found"
    exit 1
}
$fileVersion = $result.Node.InnerText

$packageProjectUrlPath = "/ns:Project/ns:PropertyGroup/ns:PackageProjectUrl"
$result = Select-Xml -Path $propsPath -Namespace $namespaces -XPath $packageProjectUrlPath
if ($null -eq $result) {
    Write-Host "Package project URL not found"
    exit 1
}
$packageProjectUrl = $result.Node.InnerText

$KeyPath = $KeyPath ? $(Resolve-Path -Path $KeyPath) : ""
$OutputPath = $OutputPath ? [System.IO.Path]::GetFullPath($OutputPath) : ""
$keyPathExits = Test-Path -Path $KeyPath

$options = @(
    $OutputPath ? "-o '$OutputPath'" : ""
    "-p:FileVersion='$fileVersion'"
    $PullRequestNumber ? "--version-suffix pr.$PullRequestNumber" : ""
    $keyPathExits ? "-p:TreatWarningsAsErrors=true" : ""
    $keyPathExits ? "-p:AssemblyOriginatorKeyFile='$KeyPath'" : ""
    $CommitSHA ? "-p:PackageProjectUrl='$packageProjectUrl/tree/$CommitSHA'" : ""
) | Where-Object { $_ }

Invoke-Expression -Command "dotnet pack $($options -join " ")"

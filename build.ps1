# Released under the MIT License.

# Copyright (c) 2024 Jeesu Choi

# Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
# documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
# rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
# persons to whom the Software is furnished to do so, subject to the following conditions:

# The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
# Software.

# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
# WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
# COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
# OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

param(
    [string]$OutputPath = "",
    [string]$Framework = "net8.0",
    [string]$KeyPath = "",
    [string]$LogPath = ""
)

$solutionPath = Join-Path $PSScriptRoot "commands.sln" -Resolve
$buildFile = Join-Path $PSScriptRoot ".build" "build.ps1"
if (!$buildFile -or !(Test-Path -Path $buildFile -ErrorAction SilentlyContinue)) {
    
    Write-Error "To build, you need the .build submodule.`nPlease execute the command below and try again.`ngit submodule update --init .build"
    exit 1
}
$buildFile = Resolve-Path -Path $buildFile
& $buildFile $solutionPath -Publish -KeyPath $KeyPath -Sign -OutputPath $OutputPath -Framework $Framework -LogPath $LogPath

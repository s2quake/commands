// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System.ComponentModel;
using System.ComponentModel.Composition;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[Category("Git-like")]
sealed class CleanCommand : CommandBase
{
    [CommandPropertySwitch('d')]
    public bool IsDirectory { get; set; }

    [CommandPropertySwitch('f')]
    public bool IsUntrackedFiles { get; set; }

    [CommandPropertySwitch('n')]
    public bool IsDryRun { get; set; }

    [CommandPropertySwitch('x')]
    public bool IsIgnoreFiles { get; set; }

    [CommandProperty('e')]
    public string Pattern { get; set; } = string.Empty;

    protected override void OnExecute()
    {
        Out.WriteLine($"{nameof(IsDirectory)}: {IsDirectory}");
        Out.WriteLine($"{nameof(IsUntrackedFiles)}: {IsUntrackedFiles}");
        Out.WriteLine($"{nameof(IsDryRun)}: {IsDryRun}");
        Out.WriteLine($"{nameof(IsIgnoreFiles)}: {IsIgnoreFiles}");
    }
}

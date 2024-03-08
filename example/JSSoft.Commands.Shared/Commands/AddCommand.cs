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

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[CommandStaticProperty(typeof(GlobalSettings))]
[Category("Git-like")]
sealed class AddCommand : CommandBase
{
    [CommandPropertyRequired]
    public string Path { get; set; } = string.Empty;

    [CommandPropertySwitch('n', useName: true)]
    public bool DryRun { get; set; }

    [CommandPropertySwitch('v', useName: true)]
    public bool Verbose { get; set; }

    [CommandPropertySwitch('f', useName: true)]
    public bool Force { get; set; }

    [CommandPropertySwitch('i', useName: true)]
    public bool Interactive { get; set; }

    [CommandPropertySwitch('P', useName: true)]
    public bool Patch { get; set; }

    protected override void OnExecute()
    {
        throw new NotImplementedException();
    }
}

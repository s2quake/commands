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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using JSSoft.Commands.AppUI.Controls;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[method: ImportingConstructor]
sealed class StashCommand(TerminalControl terminalControl) : CommandMethodBase
{
    public const string DefaultKey = "default";

    private readonly TerminalControl _terminalControl = terminalControl;
    private readonly Dictionary<string, object> _dataByName = [];

    [CommandMethod]
    public void List()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(Environment.NewLine, _dataByName.Keys);
        Out.WriteLine(sb.ToString());
    }

    [CommandMethod]
    public void Push(string name = DefaultKey)
    {
        // var data = _terminalControl.Save();
        // _dataByName[name] = data;
    }

    [CommandMethod]
    public void Pop(string name = DefaultKey)
    {
        // if (_dataByName.ContainsKey(name) is true)
        // {
        //     var data = _dataByName[name];
        //     _dataByName.Remove(name);
        //     _terminalControl.Load(data);
        // }
    }
}

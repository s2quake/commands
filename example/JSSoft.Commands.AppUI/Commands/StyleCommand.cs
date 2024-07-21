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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using JSSoft.Commands.AppUI.Controls;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[Category(nameof(CategoryAttribute.Appearance))]
sealed class StyleCommand : CommandMethodBase
{
    private readonly TerminalControl _terminalControl;
    private readonly IReadOnlyDictionary<string, TerminalStyle> _styleByName;
    private readonly string[] _styleNames;

    [ImportingConstructor]
    public StyleCommand(TerminalStyles terminalStyles, TerminalControl terminalControl)
    {
        _terminalControl = terminalControl;
        _styleByName = GetStyles(terminalStyles);
        _styleNames = _styleByName.Keys.ToArray();
    }

    [CommandMethod]
    public void List()
    {
        var sb = new TerminalStringBuilder();
        foreach (var item in _styleByName)
        {
            var isCurrent = _terminalControl.TerminalStyle == item.Value;
            var prefix = isCurrent ? "*" : " ";
            sb.IsBold = isCurrent;
            sb.AppendLine($"{prefix} {item.Key}");
            sb.ResetOptions();
        }
        Out.Write(sb.ToString());
    }

    [CommandMethod]
    [CommandMethodCompletion(nameof(CompletionApply))]
    public void Set(string styleName)
    {
        _terminalControl.TerminalStyle = _styleByName[styleName];
    }

    [CommandMethod]
    [CommandMethodCompletion(nameof(CompletionApply))]
    public void Unset()
    {
        if (_terminalControl.TerminalStyle is null)
            throw new InvalidOperationException("No style is set in the Terminal.");
        _terminalControl.TerminalStyle = null;
    }

    private string[] CompletionApply(CommandMemberDescriptor memberDescriptor, string find)
    {
        if (memberDescriptor.MemberName == "styleName")
        {
            return _styleNames;
        }
        return [];
    }

    private static IReadOnlyDictionary<string, TerminalStyle> GetStyles(TerminalStyles terminalStyles)
    {
        var styleByName = new Dictionary<string, TerminalStyle>(terminalStyles.Count);
        foreach (var item in terminalStyles.Keys)
        {
            if (terminalStyles[item] is TerminalStyle terminalStyle)
            {
                styleByName.Add($"{item}", terminalStyle);
            }
        }
        return styleByName;
    }
}

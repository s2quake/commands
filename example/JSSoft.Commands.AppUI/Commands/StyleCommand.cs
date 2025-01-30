// <copyright file="StyleCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using JSSoft.Commands.AppUI.Controls;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[Category(nameof(CategoryAttribute.Appearance))]
internal sealed class StyleCommand : CommandMethodBase
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
        {
            throw new InvalidOperationException("No style is set in the Terminal.");
        }

        _terminalControl.TerminalStyle = null;
    }

    private static IReadOnlyDictionary<string, TerminalStyle> GetStyles(
        TerminalStyles terminalStyles)
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

    private string[] CompletionApply(CommandMemberDescriptor memberDescriptor)
    {
        if (memberDescriptor.MemberName == "styleName")
        {
            return _styleNames;
        }

        return [];
    }
}

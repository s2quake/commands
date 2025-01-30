// <copyright file="PrintCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
internal sealed class PrintCommand() : CommandBase
{
    [CommandPropertyRequired(DefaultValue = "")]
    public string Text { get; set; } = string.Empty;

    protected override void OnExecute()
    {
        var s = Regex.Unescape(Text);
        Out.WriteLine(s);
    }
}

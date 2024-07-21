// <copyright file="EchoCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.Composition;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[CommandSummary("Write arguments to the standard output")]
[Description("The echo utility writes any specified operands, separated by single blank (‘ ’) " +
             "characters and followed by a newline (‘\\n’) character, to the standard output.")]
internal sealed class EchoCommand : CommandBase
{
    [CommandPropertyRequired(DefaultValue = "")]
    public string Text { get; set; } = string.Empty;

    protected override void OnExecute() => Out.WriteLine(Text);
}

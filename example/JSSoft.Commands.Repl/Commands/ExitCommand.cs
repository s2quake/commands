// <copyright file="ExitCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.Composition;
using JSSoft.Commands.Applications;

namespace JSSoft.Commands.Repl.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[method: ImportingConstructor]
internal sealed class ExitCommand(IApplication application) : CommandBase("exit")
{
    private readonly IApplication _application = application;

    [CommandPropertyRequired(DefaultValue = 0)]
    public int ExitCode { get; set; }

    protected override void OnExecute()
    {
        _application.Cancel();
    }
}

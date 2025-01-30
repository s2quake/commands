// <copyright file="ExitCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.Composition;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
internal sealed class ExitCommand() : CommandBase("exit")
{
    public override bool IsEnabled
        => ApplicationLifetime is IClassicDesktopStyleApplicationLifetime;

    private IApplicationLifetime? ApplicationLifetime => Application.Current?.ApplicationLifetime;

    protected override void OnExecute()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}

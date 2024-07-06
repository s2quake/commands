// <copyright file="AddCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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

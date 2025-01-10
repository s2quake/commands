// <copyright file="TestCommandContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandContextTests;

public class TestCommandContext(CommandSettings settings, ICommand[] commands)
    : CommandContextBase(commands, settings)
{
    public TestCommandContext(ICommand[] commands)
        : this(CommandSettings.Default, commands)
    {
    }
}

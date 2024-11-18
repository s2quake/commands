// <copyright file="ICommandContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

public interface ICommandContext
{
    TextWriter Out { get; }

    TextWriter Error { get; }

    string ExecutionName { get; }

    ICommand Node { get; }

    string Name { get; }

    string Copyright { get; }

    string Version { get; }

    CommandSettings Settings { get; }

    ICommand HelpCommand { get; }

    ICommand VersionCommand { get; }
}

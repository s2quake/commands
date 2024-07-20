// <copyright file="IApplication.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Security;
using System.Threading.Tasks;

namespace JSSoft.Commands.Applications;

public interface IApplication
{
#if JSSOFT_COMMANDS_REPL
    event EventHandler DirectoryChanged;

    string CurrentDirectory { get; set; }

    void Cancel();

    Task StartAsync();

    string? ReadString(string prompt, string command);

    SecureString? ReadSecureString(string prompt);
#endif // JSSOFT_COMMANDS_REPL
}

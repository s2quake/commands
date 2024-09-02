// <copyright file="SystemTerminal.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Applications;
using JSSoft.Commands.Extensions;
using JSSoft.Terminals;

namespace JSSoft.Commands.Repl;

[Export]
internal sealed class SystemTerminal : SystemTerminalBase
{
    private static readonly string Suffix = "$ ";
    private static readonly string SuffixC
        = TerminalStringBuilder.GetString(Suffix, TerminalColorType.BrightGreen);

    private static readonly string SeparatorC
        = TerminalStringBuilder.GetString($"{Path.DirectorySeparatorChar}", TerminalColorType.Red);

    private readonly IApplication _application;
    private readonly CommandContext _commandContext;

    [ImportingConstructor]
    public SystemTerminal(IApplication application, CommandContext commandContext)
    {
        _application = application;
        _application.DirectoryChanged += Application_DirectoryChanged;
        _commandContext = commandContext;
        _commandContext.Owner = application;
        UpdatePrompt();
    }

    protected override string FormatPrompt(string prompt)
    {
        if (prompt.EndsWith(Suffix) is true)
        {
            var text = prompt.Substring(0, prompt.Length - Suffix.Length);
            var textC = Regex.Replace(text, $"\\{Path.DirectorySeparatorChar}", SeparatorC);
            return textC + SuffixC;
        }

        return prompt;
    }

    protected override string[] GetCompletion(string[] items, string find)
    {
        return _commandContext.GetCompletion(items, find);
    }

    protected override Task OnExecuteAsync(string command, CancellationToken cancellationToken)
    {
        return _commandContext.ExecuteAsync(command, cancellationToken);
    }

    protected override void OnInitialize(TextWriter @out, TextWriter error)
    {
        _commandContext.Out = @out;
        _commandContext.Error = error;
    }

    private void Application_DirectoryChanged(object? sender, EventArgs e)
    {
        UpdatePrompt();
    }

    private void UpdatePrompt()
    {
        Prompt = $"{_application.CurrentDirectory}{Suffix}";
    }
}

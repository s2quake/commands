// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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
sealed class SystemTerminal : SystemTerminalBase
{
    private static readonly string postfix = "$ ";
    private static readonly string postfixC = TerminalStringBuilder.GetString(postfix, TerminalColorType.BrightGreen);
    private static readonly string separatorC = TerminalStringBuilder.GetString($"{Path.DirectorySeparatorChar}", TerminalColorType.Red);
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
        if (prompt.EndsWith(postfix) == true)
        {
            var text = prompt.Substring(0, prompt.Length - postfix.Length);
            var textC = Regex.Replace(text, $"\\{Path.DirectorySeparatorChar}", separatorC);
            return textC + postfixC;
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
        Prompt = $"{_application.CurrentDirectory}{postfix}";
    }
}

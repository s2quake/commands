// <copyright file="TestCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable MEN002 // Line is too long
#if JSSOFT_COMMANDS_REPL

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Terminals;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[CommandSummary("Test Command")]
[method: ImportingConstructor]
[Category("Hidden")]
internal sealed class TestCommand(IApplication application) : CommandMethodBase(["t"]), IDisposable
{
    private readonly IApplication _application = application;
    private Task? _task;
    private CancellationTokenSource? _cancellationTokenSource;

    public bool CanStart => true;

    [CommandProperty]
    public string P3 { get; set; } = string.Empty;

    [CommandPropertyExplicitRequired]
    public string P4 { get; set; } = string.Empty;

    [CommandPropertyRequired]
    public string P5 { get; set; } = string.Empty;

    [CommandPropertySwitch("reverse", 'r')]
    public bool IsReverse { get; set; }

    [CommandPropertySwitch('p')]
    public bool IsPrompt { get; set; }

    [CommandMethod]
    [CommandMethodProperty(nameof(IsPrompt))]
    [CommandSummary("Start async task")]
    [CommandMethodStaticProperty(typeof(FilterProperties))]
    [CommandMethodValidation(typeof(TestCommandExtension), nameof(TestCommandExtension.CanStart))]
    [CommandMethodCompletion(typeof(TestCommandExtension), nameof(TestCommandExtension.CompleteStart))]
    public void Start(string p1 = "")
    {
        _cancellationTokenSource = new();
        _task = TestAsync();
    }

    [CommandMethod]
    public async Task AsyncAsync(CancellationToken cancellationToken)
    {
        await Out.WriteLineAsync("type control+c to cancel");
        while (cancellationToken.IsCancellationRequested is false)
        {
            await Task.Delay(100, cancellationToken);
        }
    }

    [CommandMethod]
    [CommandSummary("Stop async task")]
    [CommandExample("werwer")]
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource = null;
        }

        await _task!;
        _task = null;
    }

    [CommandMethod]
    public void Login()
    {
        if (_application.ReadSecureString("password: ") is { } secureString)
        {
            Out.WriteLine($"password length is '{secureString.Length}'");
        }
    }

    [CommandMethod]
    public void PushMany(params string[] items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }

    [CommandMethod("items", Aliases = ["ls"])]
    [CommandMethodProperty(nameof(IsReverse))]
    public void ShowItem(string path = "123")
    {
        Console.WriteLine(path);
        var items = new string[] { "a", "b", "c" };
        if (IsReverse is false)
        {
            var i = 0;
            foreach (var item in items)
            {
                Console.WriteLine($"{i++,2}: {item}");
            }
        }
        else
        {
            var i = items.Length - 1;
            foreach (var item in items.Reverse())
            {
                Console.WriteLine($"{i--,2}: {item}");
            }
        }
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(P4), nameof(P3), nameof(P5))]
    [CommandSummary("Order method")]
    public void Order([CommandParameterCompletion(nameof(GetNamesAsync))] string p1, string p2 = "123")
    {
        Out.WriteLine($"{p1}, {p2}");
    }

    [CommandMethod("cmp")]
    public async Task CompareAsync(string p1, string p2, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
    }

    [CommandMethod]
    public async Task SleepAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(10000, cancellationToken);
    }

    [CommandMethod]
    public async Task CancelAsync(int timeout = 1000, CancellationToken cancellationToken = default)
    {
        using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken);
        cancellationTokenSource.CancelAfter(timeout);
        await Task.Delay(timeout, cancellationTokenSource.Token);
    }

    public string[] CompleteShowItem(CommandMemberDescriptor memberDescriptor, string find)
    {
        return [];
    }

    public void Dispose()
    {
        if (_cancellationTokenSource is not null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    protected override bool IsMethodEnabled(CommandMethodDescriptor methodDescriptor)
    {
        if (methodDescriptor.MethodName == nameof(Start))
        {
            return _task is null;
        }
        else if (methodDescriptor.MethodName == nameof(StopAsync))
        {
            return _task is not null && _cancellationTokenSource!.IsCancellationRequested is false;
        }

        throw new NotSupportedException();
    }

    private async Task TestAsync()
    {
        while (!_cancellationTokenSource!.IsCancellationRequested)
        {
            if (IsPrompt is true)
            {
                _application.CurrentDirectory = $"{DateTime.UtcNow}";
            }
            else
            {
                Console.Write(DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow");
                await Out.WriteAsync(DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow");
                await Out.WriteAsync(TerminalStringBuilder.GetString("01234567890123456789012345678901234567890123456789012345678901234567890123456789", TerminalColorType.Red));
                await Out.WriteAsync(DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow");
                var v = DateTime.UtcNow.Millisecond % 4;
                switch (v)
                {
                    case 0:
                        await Out.WriteLineAsync(DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + "12093810938012");
                        break;
                    case 1:
                        await Out.WriteAsync(DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow" + Environment.NewLine + DateTime.UtcNow + Environment.NewLine + "wow");
                        break;
                    case 2:
                        await Out.WriteAsync("01234567890123456789012345678901234567890123456789012345678901234567890123456789");
                        break;
                    case 3:
                        await Out.WriteLineAsync($"{DateTime.UtcNow}");
                        break;
                }
            }

            Thread.Sleep(1000);
        }
    }

    private async Task<string[]> GetNamesAsync()
    {
        await Task.Delay(1);
        return ["a", "b", "c"];
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Pre-execution for command")]
    private async Task<string[]> CompleteCompareAsync(
        CommandMemberDescriptor memberDescriptor, string find, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return ["a", "b", "c"];
    }
}
#endif // JSSOFT_COMMANDS_REPL

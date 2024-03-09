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

#if JSSOFT_COMMANDS_REPL

using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Terminals;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[CommandSummary("Test Command")]
[method: ImportingConstructor]
sealed class TestCommand(IApplication application) : CommandMethodBase(new string[] { "t" })
{
    private readonly IApplication _application = application;
    private Task? _task;
    private CancellationTokenSource? _cancellationTokenSource;

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

    public bool CanStart => true;

    [CommandMethod]
    public async Task AsyncAsync(CancellationToken cancellationToken)
    {
        Out.WriteLine("type control+c to cancel");
        while (cancellationToken.IsCancellationRequested == false)
        {
            await Task.Delay(100, cancellationToken);
        }
    }


    [CommandMethod]
    [CommandSummary("Stop async task")]
    [CommandExample("werwer")]
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource?.Cancel();
        await _task!;
        _cancellationTokenSource = null;
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
        if (IsReverse == false)
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

    [CommandProperty]
    public string P3 { get; set; } = string.Empty;

    [CommandPropertyExplicitRequired]
    public string P4 { get; set; } = string.Empty;

    [CommandPropertyRequired]
    public string P5 { get; set; } = string.Empty;

    public string[] CompleteShowItem(CommandMemberDescriptor memberDescriptor, string find)
    {
        return [];
    }

    [CommandPropertySwitch("reverse", 'r')]
    public bool IsReverse { get; set; }

    [CommandPropertySwitch('p')]
    public bool IsPrompt { get; set; }

    private async Task TestAsync()
    {
        while (!_cancellationTokenSource!.IsCancellationRequested)
        {
            if (IsPrompt == true)
            {
                _application.CurrentDirectory = $"{DateTime.Now}";
            }
            else
            {
                Console.Write(DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow");
                await Out.WriteAsync(DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow");
                await Out.WriteAsync(TerminalStringBuilder.GetString("01234567890123456789012345678901234567890123456789012345678901234567890123456789", TerminalColorType.Red));
                await Out.WriteAsync(DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow");
                var v = DateTime.Now.Millisecond % 4;
                // v = 2;
                switch (v)
                {
                    case 0:
                        await Out.WriteLineAsync(DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + "12093810938012");
                        break;
                    case 1:
                        await Out.WriteAsync(DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow" + Environment.NewLine + DateTime.Now + Environment.NewLine + "wow");
                        break;
                    case 2:
                        await Out.WriteAsync("01234567890123456789012345678901234567890123456789012345678901234567890123456789");
                        break;
                    case 3:
                        await Out.WriteLineAsync($"{DateTime.Now}");
                        break;
                }
            }
            Thread.Sleep(1000);
        }
    }

    protected override bool IsMethodEnabled(CommandMethodDescriptor methodDescriptor)
    {
        if (methodDescriptor.MethodName == nameof(Start))
        {
            return _task == null;
        }
        else if (methodDescriptor.MethodName == nameof(StopAsync))
        {
            return _task != null && _cancellationTokenSource!.IsCancellationRequested == false;
        }
        throw new NotImplementedException();
    }

    private async Task<string[]> GetNamesAsync()
    {
        await Task.Delay(1);
        return ["a", "b", "c"];
    }

    private async Task<string[]> CompleteCompareAsync(CommandMemberDescriptor memberDescriptor, string find, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return ["a", "b", "c"];
    }
}
#endif // JSSOFT_COMMANDS_REPL

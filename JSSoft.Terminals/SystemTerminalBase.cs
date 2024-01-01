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

using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Terminals;

public abstract class SystemTerminalBase
{
    private readonly SystemTerminalHost _terminal;
    private string _prompt = string.Empty;

    protected SystemTerminalBase()
    {
        _terminal = new InternalSystemTerminalHost(this);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var consoleOut = Console.Out;
        var consoleError = Console.Error;

        var @out = new TerminalTextWriter(_terminal, Console.OutputEncoding);
        var error = new TerminalTextWriter(_terminal, Console.OutputEncoding);

        Console.SetOut(@out);
        Console.SetError(error);
        OnInitialize(@out, error);

        while (cancellationToken.IsCancellationRequested == false)
        {
            var isEnabled = _terminal.IsEnabled;
            var prompt = Prompt;
            if (isEnabled == true && _terminal.ReadStringInternal(prompt, cancellationToken) is { } text)
            {
                await ExecuteAsync(error, text);
            }
#pragma warning disable CA2016
            await Task.Delay(1);
#pragma warning restore
        }

        Console.SetOut(consoleOut);
        Console.SetError(consoleError);
        Console.Write("\u001b[?25h");
    }

    public string? ReadString(string prompt, string command) => _terminal.ReadString(prompt, command);

    public SecureString? ReadSecureString(string prompt) => _terminal.ReadSecureString(prompt);

    public string Prompt
    {
        get => _prompt;
        set
        {
            _prompt = value;
            if (_terminal.IsReading == true)
                _terminal.Prompt = value;
        }
    }

    public bool DetailErrorMessage { get; set; }

    protected virtual bool OnCanExecute(string text) => true;

    protected virtual void OnExecuted(Exception? exception)
    {
    }

    protected virtual string FormatPrompt(string prompt) => prompt;

    protected virtual string FormatCommand(string command) => command;

    protected virtual string[] GetCompletion(string[] items, string find) => [];

    protected abstract void OnInitialize(TextWriter @out, TextWriter error);

    protected abstract Task OnExecuteAsync(string command, CancellationToken cancellationToken);

    private async Task ExecuteAsync(TextWriter error, string text)
    {
        var consoleControlC = Console.IsInputRedirected != true && Console.TreatControlCAsInput;
        var cancellationTokenSource = new CancellationTokenSource();
        try
        {
            if (Console.IsInputRedirected == false)
                Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += ConsoleCancelEventHandler;
            if (OnCanExecute(text) == false)
                return;
            var task = OnExecuteAsync(text, cancellationTokenSource.Token);
            while (task.IsCompleted == false)
            {
                _terminal.Update();
                await Task.Delay(1);
            }
            if (task.Exception != null)
                throw task.Exception;
            OnExecuted(exception: null);
        }
        catch (Exception e)
        {
            OnExecuteWithException(error, e);
        }
        finally
        {
            if (Console.IsInputRedirected == false)
                Console.TreatControlCAsInput = consoleControlC;
            Console.CancelKeyPress -= ConsoleCancelEventHandler;
            cancellationTokenSource = null;
        }

        void ConsoleCancelEventHandler(object? sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
        }
    }

    private void OnExecuteWithException(TextWriter error, Exception e)
    {
        if (e is AggregateException e2)
        {
            foreach (var item in e2.InnerExceptions)
            {
                WriteException(error, item);
            }
            OnExecuted(e2);
        }
        else
        {
            WriteException(error, e);
            OnExecuted(e);
        }
    }

    private void WriteException(TextWriter error, Exception e)
    {
        var exception = e is TargetInvocationException ? e.InnerException ?? e : e;
        var errorMesssage = DetailErrorMessage == true ? $"{exception}" : exception.Message;
        var formattedMessage = TerminalStringBuilder.GetString(errorMesssage, TerminalColorType.Red);
        error.WriteLine(formattedMessage);
    }

    #region InternalSystemTerminalHost

    sealed class InternalSystemTerminalHost(SystemTerminalBase terminalBase)
        : SystemTerminalHost
    {
        protected override string FormatPrompt(string prompt) => terminalBase.FormatPrompt(prompt);

        protected override string FormatCommand(string command) => terminalBase.FormatCommand(command);

        protected override string[] GetCompletion(string[] items, string find)
        {
            return terminalBase.GetCompletion(items, find);
        }
    }

    #endregion

    #region TerminalTextWriter

    sealed class TerminalTextWriter(SystemTerminalHost terminalHost, Encoding encoding)
        : TextWriter
    {
        public override Encoding Encoding => encoding;

        public override void Write(char value) => WriteToStream(value.ToString());

        public override void Write(string? value) => WriteToStream(value);

        public override void WriteLine(string? value) => WriteToStream(value + Environment.NewLine);

        public override Task WriteAsync(char value) => WriteToStreamAsync(value.ToString());

        public override Task WriteAsync(string? value) => WriteToStreamAsync(value);

        public override Task WriteLineAsync(string? value) => WriteToStreamAsync(value + Environment.NewLine);

        private void WriteToStream(string? text) => terminalHost.EnqueueString(text ?? string.Empty);

        private Task WriteToStreamAsync(string? text) => terminalHost.EnqueueStringAsync(text ?? string.Empty);
    }

    #endregion
}

// <copyright file="SystemTerminalBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Terminals;

public abstract class SystemTerminalBase : IDisposable
{
    private static SystemTerminalBase? _instance;
    private readonly SystemTerminalHost _terminal;
    private readonly TextWriter _oldOut;
    private readonly TextWriter _oldError;
    private readonly TextWriter _out;
    private readonly TextWriter _error;
    private string _prompt = string.Empty;
    private bool _isDisposed;

    protected SystemTerminalBase()
    {
        if (_instance is not null)
        {
            throw new InvalidOperationException("The instance can only be created once.");
        }

        _instance = this;
        _terminal = new InternalSystemTerminalHost(this);
        _oldOut = Console.Out;
        _oldError = Console.Error;

        _out = new TerminalTextWriter(_terminal, Console.OutputEncoding);
        _error = new TerminalTextWriter(_terminal, Console.OutputEncoding);

        Console.SetOut(_out);
        Console.SetError(_error);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        OnInitialize(_out, _error);

        while (cancellationToken.IsCancellationRequested != true)
        {
            var isEnabled = _terminal.IsEnabled;
            var prompt = Prompt;
            if (isEnabled == true && _terminal.ReadStringInternal(prompt, cancellationToken) is { } text)
            {
                await ExecuteAsync(_error, text);
            }
#pragma warning disable CA2016
            await Task.Delay(1);
#pragma warning restore
        }
    }

    public string? ReadString(string prompt, string command) => _terminal.ReadString(prompt, command);

    public SecureString? ReadSecureString(string prompt) => _terminal.ReadSecureString(prompt);

    public void Dispose()
    {
        if (_isDisposed == true)
        {
            throw new ObjectDisposedException($"{this}");
        }

        OnDispose();

        Console.SetOut(_oldOut);
        Console.SetError(_oldError);
        Console.Write("\u001b[?25h");
        _instance = null;
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    public string Prompt
    {
        get => _prompt;
        set
        {
            _prompt = value;
            if (_terminal.IsReading == true)
            {
                _terminal.Prompt = value;
            }
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

    protected virtual void OnDispose()
    {
    }

    protected abstract void OnInitialize(TextWriter @out, TextWriter error);

    protected abstract Task OnExecuteAsync(string command, CancellationToken cancellationToken);

    private async Task ExecuteAsync(TextWriter error, string text)
    {
        var consoleControlC = Console.IsInputRedirected != true && Console.TreatControlCAsInput;
        var cancellationTokenSource = new CancellationTokenSource();
        try
        {
            if (Console.IsInputRedirected != true)
            {
                Console.TreatControlCAsInput = false;
            }

            Console.CancelKeyPress += ConsoleCancelEventHandler;
            if (OnCanExecute(text) != true)
            {
                return;
            }

            var task = OnExecuteAsync(text, cancellationTokenSource.Token);
            while (task.IsCompleted != true)
            {
                _terminal.Update();
                await Task.Delay(1);
            }

            if (task.IsCanceled == true)
            {
                throw new TaskCanceledException(task);
            }

            if (task.Exception is not null)
            {
                throw task.Exception;
            }

            OnExecuted(exception: null);
        }
        catch (Exception e)
        {
            OnExecuteWithException(error, e);
        }
        finally
        {
            if (Console.IsInputRedirected != true)
            {
                Console.TreatControlCAsInput = consoleControlC;
            }

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
        var errorMessage = DetailErrorMessage == true ? $"{exception}" : exception.Message;
        var formattedMessage = TerminalStringBuilder.GetString(errorMessage, TerminalColorType.Red);
        error.WriteLine(formattedMessage);
    }

    private sealed class InternalSystemTerminalHost(SystemTerminalBase terminalBase)
        : SystemTerminalHost
    {
        protected override string FormatPrompt(string prompt) => terminalBase.FormatPrompt(prompt);

        protected override string FormatCommand(string command) => terminalBase.FormatCommand(command);

        protected override string[] GetCompletion(string[] items, string find)
        {
            return terminalBase.GetCompletion(items, find);
        }
    }

    private sealed class TerminalTextWriter(SystemTerminalHost terminalHost, Encoding encoding)
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
}

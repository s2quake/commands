using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace JSSoft.Commands.AppUI.Controls;

sealed class TerminalControlTextWriter(TerminalControl terminalControl) : TextWriter
{
    private readonly TerminalControl _terminalControl = terminalControl;

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Append($"{value}"));
    }

    public override void Write(string? value)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.Append($"{value}"));
    }

    public override void WriteLine(string? value)
    {
        Dispatcher.UIThread.Invoke(() => _terminalControl.AppendLine($"{value}"));
    }

    public override async Task WriteAsync(char value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => _terminalControl.Append($"{value}"));
    }

    public override async Task WriteAsync(string? value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => _terminalControl.Append($"{value}"));
    }

    public override async Task WriteLineAsync(string? value)
    {
        await Dispatcher.UIThread.InvokeAsync(() => _terminalControl.AppendLine($"{value}"));
    }
}
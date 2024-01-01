using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.AppUI.Controls;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[method: ImportingConstructor]
sealed class SystemCommand(TerminalControl terminalControl) : CommandAsyncBase
{
    private readonly TerminalControl _terminalControl = terminalControl;

    protected override async Task OnExecuteAsync(CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        var process = new SystemProcess("brew")
        {
            Out = _terminalControl.Out,
            Error = _terminalControl.Error,
            In = _terminalControl.In,
            ArgumentList =
            {
                "list"
            },
        };
        await process.StartAsync(cancellationToken);
    }
}

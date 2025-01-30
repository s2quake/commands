// <copyright file="ProgressCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
internal sealed class ProgressCommand : CommandAsyncBase
{
    protected override async Task OnExecuteAsync(CancellationToken cancellationToken)
    {
        var progress = this.Progress;
        for (var i = 0; i < 4; i++)
        {
            var text = $"Progress{i,2}";
            for (var j = 0; j < 100; j++)
            {
                await Task.Delay(10, cancellationToken);
                progress.Report(j / 100.0, text);
            }

            progress.Complete(text);
        }
    }
}

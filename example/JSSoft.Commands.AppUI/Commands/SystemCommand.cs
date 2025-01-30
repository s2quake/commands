// <copyright file="SystemCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
internal sealed class SystemCommand : CommandAsyncBase
{
    protected override async Task OnExecuteAsync(CancellationToken cancellationToken)
    {
        var process = new SystemProcess("brew")
        {
            ArgumentList =
            {
                "list",
            },
        };
        await process.StartAsync(cancellationToken);
    }
}

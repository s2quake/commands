// <copyright file="Application.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using JSSoft.Commands.Applications;

namespace JSSoft.Commands.Repl;

sealed class Application : IApplication
{
    private readonly CompositionContainer _container;

    public Application()
    {
        _container = new CompositionContainer(new AssemblyCatalog(typeof(Application).Assembly));
        _container.ComposeExportedValue<IApplication>(this);
        _container.ComposeExportedValue(this);
    }

    public void Execute(string[] args)
    {
        var commandContext = _container.GetExportedValue<CommandContext>();
        commandContext.Execute(args);
    }
}

// <copyright file="StashCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
internal sealed class StashCommand : CommandMethodBase
{
    public const string DefaultKey = "default";

    private readonly Dictionary<string, object> _dataByName = [];

    [CommandMethod]
    public void List()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(Environment.NewLine, _dataByName.Keys);
        Out.WriteLine(sb.ToString());
    }

    [CommandMethod]
    public void Push(string name = DefaultKey)
    {
        Out.WriteLine($"push {name}");
    }

    [CommandMethod]
    public void Pop(string name = DefaultKey)
    {
        Out.WriteLine($"pop {name}");
    }
}

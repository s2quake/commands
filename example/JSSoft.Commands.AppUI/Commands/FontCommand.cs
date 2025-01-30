// <copyright file="FontCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Avalonia.Media;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[Category(nameof(CategoryAttribute.Appearance))]
internal sealed class FontCommand : CommandBase
{
    private readonly string[] _fontNames;

    public FontCommand()
    {
        _fontNames = FontManager.Current.SystemFonts.Select(item => item.Name).ToArray();
    }

    protected override void OnExecute()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(Environment.NewLine, _fontNames);
        Out.Write(sb.ToString());
    }
}

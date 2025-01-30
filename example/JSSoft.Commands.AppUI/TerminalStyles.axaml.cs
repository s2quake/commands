// <copyright file="TerminalStyles.axaml.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JSSoft.Commands.AppUI;

/// <summary>
/// This class contains the styles for the terminal.
/// </summary>
internal partial class TerminalStyles : ResourceDictionary
{
    public TerminalStyles()
    {
        AvaloniaXamlLoader.Load(this);
        App.Current.RegisterService(this);
    }
}

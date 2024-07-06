// <copyright file="ITerminalScroll.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Terminals;

public interface ITerminalScroll : INotifyPropertyChanged
{
    int Minimum { get; set; }

    int Maximum { get; set; }

    int SmallChange { get; set; }

    int LargeChange { get; set; }

    int ViewportSize { get; set; }

    bool IsVisible { get; set; }

    int Value { get; set; }
}

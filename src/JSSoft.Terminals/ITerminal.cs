// <copyright file="ITerminal.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.IO;
using JSSoft.Terminals.Input;

namespace JSSoft.Terminals;

public interface ITerminal : INotifyPropertyChanged
{
    string Title { get; set; }

    bool IsReadOnly { get; set; }

    TerminalDisplayInfo DisplayInfo { get; set; }

    ITerminalStyle ActualStyle { get; }

    ITerminalStyle? Style { get; set; }

    ITerminalScroll Scroll { get; }

    ITerminalModes Modes { get; }

    IInputHandler? InputHandler { get; set; }

    bool IsFocused { get; set; }

    int MaximumBufferHeight { get; set; }

    TerminalSize BufferSize { get; }

    TerminalSize Size { get; set; }

    IReadOnlyList<ITerminalRow> View { get; }

    TerminalCoord CursorCoordinate { get; }

    TerminalCoord OriginCoordinate { get; set; }

    TerminalCoord ViewCoordinate { get; set; }

    ITerminalSelectionCollection Selections { get; }

    TerminalSelection Selecting { get; set; }

    TextWriter Out { get; }

    TextReader In { get; }

    TerminalPoint ViewToWorld(TerminalPoint position);

    TerminalCoord ViewToWorld(TerminalCoord viewCoord);

    TerminalCoord PositionToCoordinate(TerminalPoint position);

    TerminalCharacterInfo? GetInfo(TerminalCoord coord);

    bool BringIntoView(int y);

    string Copy();

    void Paste(string text);

    [EditorBrowsable(EditorBrowsableState.Never)]
    void Update(params ITerminalRow[] rows);

    void WriteInput(string text);

    event EventHandler<TerminalUpdateEventArgs>? Updated;

    event EventHandler<TerminalModeChangedEventArgs>? ModeChanged;
}

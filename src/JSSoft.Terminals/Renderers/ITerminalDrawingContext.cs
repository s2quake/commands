// <copyright file="ITerminalDrawingContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

public interface ITerminalDrawingContext
{
    void FillRectangle(TerminalColor color, TerminalRect rect);

    void DrawRectangle(TerminalColor color, int thickness, TerminalRect rect);

    void DrawText(TerminalGlyphRun glyphRun);

    IDisposable PushTransform(TerminalPoint position);
}

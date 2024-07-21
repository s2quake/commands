// <copyright file="TerminalRectRun.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

public class TerminalRectRun(TerminalRectRunInfo info) : ITerminalRenderer
{
    public void Render(ITerminalDrawingContext drawingContext)
    {
        drawingContext.FillRectangle(Color, Rect);
    }

    public TerminalColor Color { get; } = info.Color;

    public TerminalRect Rect { get; } = info.Rect;
}

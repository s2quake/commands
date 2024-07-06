// <copyright file="ITerminalDrawingContextExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers.Extensions;

public static class ITerminalDrawingContextExtensions
{
    public static IDisposable PushTransform(this ITerminalDrawingContext @this, int x, int y)
    {
        return @this.PushTransform(new TerminalPoint(x, y));
    }
}

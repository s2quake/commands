// <copyright file="TerminalRenderRange.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

internal sealed class TerminalRenderRange(int begin, int end, IDisposable transformScope) : IDisposable
{
    private readonly IDisposable _transformScope = transformScope;

    public bool Contains(int value) => value >= Begin && value < End;

    public int Begin { get; } = begin;

    public int End { get; } = end;

    public void Dispose()
    {
        _transformScope.Dispose();
    }
}

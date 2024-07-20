// <copyright file="TerminalRendererBase.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Renderers;

public abstract class TerminalRendererBase : TerminalPropertyChangedBase, ITerminalRenderer, IDisposable
{
    private bool _isDisposed;

    protected abstract void OnRender(ITerminalDrawingContext drawingContext);

    protected virtual void OnDispose()
    {
    }

    void ITerminalRenderer.Render(ITerminalDrawingContext drawingContext) => OnRender(drawingContext);

    void IDisposable.Dispose()
    {
        if (_isDisposed == true)
        {
            throw new ObjectDisposedException($"{this}");
        }

        OnDispose();
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }
}

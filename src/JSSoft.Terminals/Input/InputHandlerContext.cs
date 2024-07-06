// <copyright file="InputHandlerContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Input;

public abstract class InputHandlerContext(ITerminal terminal) : IDisposable
{
    private bool _isDragging;

    public ITerminal Terminal { get; } = terminal;

    internal void Select() => OnSelect();

    internal void Deselect() => OnDeselect();

    internal void BeginDrag(IPointerEventData pointerEventData) => OnBeginDrag(pointerEventData);

    internal void Drag(IPointerEventData pointerEventData) => OnDrag(pointerEventData);

    internal void EndDrag(IPointerEventData pointerEventData) => OnEndDrag(pointerEventData);

    internal void PointerDown(IPointerEventData pointerEventData) => OnPointerDown(pointerEventData);

    internal void PointerMove(IPointerEventData pointerEventData)
    {
        OnPointerMove(pointerEventData);
        if (pointerEventData.IsMouseLeftButton == true)
        {
            if (_isDragging == false)
            {
                OnBeginDrag(pointerEventData);
                _isDragging = true;
            }
            if (_isDragging == true)
            {
                OnDrag(pointerEventData);
            }
        }
    }

    internal void PointerUp(IPointerEventData pointerEventData)
    {
        if (_isDragging == true)
        {
            OnEndDrag(pointerEventData);
        }
        OnPointerUp(pointerEventData);
        _isDragging = false;
    }

    internal void PointerEnter(IPointerEventData pointerEventData) => OnPointerEnter(pointerEventData);

    internal void PointerExit(IPointerEventData pointerEventData) => OnPointerExit(pointerEventData);

    internal void Dispose() => OnDispose();

    protected virtual void OnSelect()
    {
    }

    protected virtual void OnDeselect()
    {
    }

    protected virtual void OnBeginDrag(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnDrag(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnEndDrag(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnPointerDown(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnPointerMove(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnPointerUp(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnPointerEnter(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnPointerExit(IPointerEventData pointerEventData)
    {
    }

    protected virtual void OnDispose()
    {
    }

    #region IDisposable

    void IDisposable.Dispose()
    {
        Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}

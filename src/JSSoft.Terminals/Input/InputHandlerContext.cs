// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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

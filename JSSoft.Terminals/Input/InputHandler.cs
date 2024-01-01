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

public abstract class InputHandler<T> : IInputHandler where T : InputHandlerContext
{
    private readonly Dictionary<ITerminal, T> ContextByTerminal = [];

    protected virtual void OnSelect(InputHandlerContext context) => context.Select();

    protected virtual void OnDeselect(InputHandlerContext context) => context.Deselect();

    protected virtual void OnPointerDown(InputHandlerContext context, IPointerEventData eventData) => context.PointerDown(eventData);

    protected virtual void OnPointerMove(InputHandlerContext context, IPointerEventData eventData) => context.PointerMove(eventData);

    protected virtual void OnPointerUp(InputHandlerContext context, IPointerEventData eventData) => context.PointerUp(eventData);

    protected virtual void OnPointerEnter(InputHandlerContext context, IPointerEventData eventData) => context.PointerEnter(eventData);

    protected virtual void OnPointerExit(InputHandlerContext context, IPointerEventData eventData) => context.PointerExit(eventData);

    protected abstract T CreateContext(ITerminal terminal);

    #region IInputHandler

    void IInputHandler.Select(ITerminal terminal) => OnSelect(ContextByTerminal[terminal]);

    void IInputHandler.Deselect(ITerminal terminal) => OnDeselect(ContextByTerminal[terminal]);

    void IInputHandler.PointerDown(ITerminal terminal, IPointerEventData eventData) => OnPointerDown(ContextByTerminal[terminal], eventData);

    void IInputHandler.PointerMove(ITerminal terminal, IPointerEventData eventData) => OnPointerMove(ContextByTerminal[terminal], eventData);

    void IInputHandler.PointerUp(ITerminal terminal, IPointerEventData eventData) => OnPointerUp(ContextByTerminal[terminal], eventData);

    void IInputHandler.PointerEnter(ITerminal terminal, IPointerEventData eventData) => OnPointerEnter(ContextByTerminal[terminal], eventData);

    void IInputHandler.PointerExit(ITerminal terminal, IPointerEventData eventData) => OnPointerExit(ContextByTerminal[terminal], eventData);

    void IInputHandler.Attach(ITerminal terminal) => ContextByTerminal.Add(terminal, CreateContext(terminal));

    void IInputHandler.Detach(ITerminal terminal)
    {
        ContextByTerminal[terminal].Dispose();
        ContextByTerminal.Remove(terminal);
    }

    #endregion
}

// <copyright file="InputHandler.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Input;

public abstract class InputHandler<T> : IInputHandler
    where T : InputHandlerContext

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
}

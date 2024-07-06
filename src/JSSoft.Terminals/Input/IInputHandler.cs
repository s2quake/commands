// <copyright file="IInputHandler.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Input;

public interface IInputHandler
{
    void Select(ITerminal terminal);

    void Deselect(ITerminal terminal);

    void PointerDown(ITerminal terminal, IPointerEventData eventData);

    void PointerMove(ITerminal terminal, IPointerEventData eventData);

    void PointerUp(ITerminal terminal, IPointerEventData eventData);

    void PointerEnter(ITerminal terminal, IPointerEventData eventData);

    void PointerExit(ITerminal terminal, IPointerEventData eventData);

    void Attach(ITerminal terminal);

    void Detach(ITerminal terminal);
}

static class IInputHandlerExtensions
{
    public static void SelectIf(this IInputHandler @this, bool condition, ITerminal terminal)
    {
        if (condition == true)
        {
            @this.Select(terminal);
        }
    }

    public static void DeselectIf(this IInputHandler @this, bool condition, ITerminal terminal)
    {
        if (condition == true)
        {
            @this.Deselect(terminal);
        }
    }
}

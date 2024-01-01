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

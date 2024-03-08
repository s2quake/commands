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

using Avalonia;
using Avalonia.Input;
using JSSoft.Terminals;
using JSSoft.Terminals.Input;

namespace JSSoft.Commands.AppUI.Controls;

sealed class TerminalPointerEventData : IPointerEventData
{
    public TerminalPointerEventData(PointerEventArgs e, Visual visual)
    {
        Position = TerminalMarshal.Convert(e.GetPosition(visual));
        Timestamp = e.Timestamp / 1000.0;
        MouseButton = Convert(e.GetCurrentPoint(visual).Properties);
    }

    public TerminalPointerEventData(PointerPressedEventArgs e, Visual visual)
    {
        Position = TerminalMarshal.Convert(e.GetPosition(visual));
        Timestamp = e.Timestamp / 1000.0;
        PointerType = e.Pointer.Type;
        MouseButton = Convert(e.GetCurrentPoint(visual).Properties);
    }

    public TerminalPointerEventData(PointerReleasedEventArgs e, Visual visual)
    {
        Position = TerminalMarshal.Convert(e.GetPosition(visual));
        Timestamp = e.Timestamp / 1000.0;
        PointerType = e.Pointer.Type;
        MouseButton = e.InitialPressMouseButton;
    }

    public bool IsPointerMouse => PointerType == PointerType.Mouse;

    public bool IsPointerTouch => PointerType == PointerType.Touch;

    public bool IsPointerPen => PointerType == PointerType.Pen;

    public bool IsMouseLeftButton => MouseButton == MouseButton.Left;

    public bool IsMouseMiddleButton => MouseButton == MouseButton.Middle;

    public bool IsMouseRightButton => MouseButton == MouseButton.Right;

    public PointerType PointerType { get; }

    public MouseButton MouseButton { get; }

    public TerminalPoint Position { get; }

    public double Timestamp { get; }

    private static MouseButton Convert(PointerPointProperties properties)
    {
        if (properties.IsLeftButtonPressed == true)
        {
            return MouseButton.Left;
        }
        return MouseButton.None;
    }
}

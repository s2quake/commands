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

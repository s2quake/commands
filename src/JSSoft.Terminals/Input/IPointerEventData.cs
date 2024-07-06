// <copyright file="IPointerEventData.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Input;

public interface IPointerEventData
{
    bool IsPointerMouse { get; }

    bool IsPointerTouch { get; }

    bool IsPointerPen { get; }

    bool IsMouseLeftButton { get; }

    bool IsMouseMiddleButton { get; }

    bool IsMouseRightButton { get; }

    TerminalPoint Position { get; }

    double Timestamp { get; }
}

// <copyright file="TerminalKeyBindings.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals;

public static class TerminalKeyBindings
{
    public static TerminalKeyBindingCollection GetDefaultBindings()
    {
        if (TerminalEnvironment.IsMacOS() is true)
        {
            return MacOS;
        }

        return Common;
    }

    public static readonly TerminalKeyBindingCollection Common =
    [
#if UNITY_ANDROID
        new TerminalKeyBinding(TerminalKey.UpArrow, (t) => t.PrevHistory()),
        new TerminalKeyBinding(TerminalKey.DownArrow, (t) => t.NextHistory()),
        new TerminalKeyBinding(TerminalKey.LeftArrow, (t) => t.MoveLeft()),
        new TerminalKeyBinding(TerminalKey.RightArrow, (t) => t.MoveRight()),
        new TerminalKeyBinding(TerminalKey.Backspace, (t) => t.Backspace()),
#else
        new TerminalKeyBinding(TerminalKey.UpArrow, (t) => t.KeyUp()),
        new TerminalKeyBinding(TerminalKey.DownArrow, (t) => t.KeyDown()),
        new TerminalKeyBinding(TerminalKey.LeftArrow, (t) => t.KeyLeft()),
        new TerminalKeyBinding(TerminalKey.RightArrow, (t) => t.KeyRight()),
        new TerminalKeyBinding(TerminalKey.Backspace, (t) => t.Backspace()),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.C, (t) => t.Cancel()),
#endif
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.LeftArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.RightArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.UpArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.DownArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.LeftArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.RightArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.UpArrow, (t) => true),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.DownArrow, (t) => true),

        new TerminalKeyBinding(TerminalKey.Delete, (t) => t.Delete()),
    ];

    public static readonly TerminalKeyBindingCollection MacOS = new(Common)
    {
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.E, (t) => t.MoveToLast()),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.A, (t) => t.MoveToFirst()),

        new TerminalKeyBinding(TerminalKey.PageUp, (g) => g.Scroll.PageUp()),
        new TerminalKeyBinding(TerminalKey.PageDown, (g) => g.Scroll.PageDown()),
    };

    public static readonly TerminalKeyBindingCollection Windows = new(Common)
    {
        new TerminalKeyBinding(TerminalKey.Home, (t) => t.MoveToFirst()),
        new TerminalKeyBinding(TerminalKey.End, (t) => t.MoveToLast()),

        new TerminalKeyBinding(TerminalKey.PageUp, (g) => g.Scroll.PageUp()),
        new TerminalKeyBinding(TerminalKey.PageDown, (g) => g.Scroll.PageDown()),
        new TerminalKeyBinding(TerminalModifiers.Alt | TerminalModifiers.Control, TerminalKey.PageUp, (g) => g.Scroll.LineUp()),
        new TerminalKeyBinding(TerminalModifiers.Alt | TerminalModifiers.Control, TerminalKey.PageDown, (g) => g.Scroll.LineDown()),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.Home, (g) => g.Scroll.ScrollToTop()),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.End, (g) => g.Scroll.ScrollToBottom()),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.A, (g) => g.Selections.SelectAll()),
    };

    public static readonly TerminalKeyBindingCollection Linux = new(Common)
    {
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.E, t => t.MoveToLast()),
        new TerminalKeyBinding(TerminalModifiers.Control, TerminalKey.A, t => t.MoveToFirst()),
        new TerminalKeyBinding(TerminalKey.Home, (t) => t.MoveToFirst()),
        new TerminalKeyBinding(TerminalKey.End, (t) => t.MoveToLast()),

        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.PageUp, (g) => g.Scroll.PageUp()),
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.PageDown, (g) => g.Scroll.PageDown()),
        new TerminalKeyBinding(TerminalModifiers.Control | TerminalModifiers.Shift, TerminalKey.DownArrow, (g) => g.Scroll.LineDown()),
        new TerminalKeyBinding(TerminalModifiers.Control | TerminalModifiers.Shift, TerminalKey.UpArrow, (g) => g.Scroll.LineUp()),
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.Home, (g) => g.Scroll.ScrollToTop()),
        new TerminalKeyBinding(TerminalModifiers.Shift, TerminalKey.End, (g) => g.Scroll.ScrollToBottom()),
    };
}

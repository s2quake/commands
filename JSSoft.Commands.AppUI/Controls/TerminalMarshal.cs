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

using System.Collections.Generic;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

static class TerminalMarshal
{
    private static readonly Dictionary<uint, IBrush> _brushByColorCode = [];

    public static Rect Convert(TerminalRect rect)
    {
        return new Rect(rect.X, rect.Top, rect.Width, rect.Height);
    }

    public static Color Convert(TerminalColor color)
    {
        return new Color(color.A, color.R, color.G, color.B);
    }

    public static TerminalColor Convert(Color color)
    {
        return TerminalColor.FromArgb(color.A, color.R, color.G, color.B);
    }

    public static IBrush ToBrush(TerminalColor color)
    {
        var colorCode = color.ToUInt32();
        if (_brushByColorCode.ContainsKey(colorCode) == false)
        {
            _brushByColorCode.Add(colorCode, new ImmutableSolidColorBrush(colorCode));
        }
        return _brushByColorCode[colorCode];
    }

    public static TerminalPoint Convert(Point point)
    {
        return new TerminalPoint((int)point.X, (int)point.Y);
    }

    public static TerminalModifiers Convert(KeyModifiers modifiers)
    {
        TerminalModifiers terminalModifiers = 0;
        if (modifiers.HasFlag(KeyModifiers.Alt) == true)
        {
            terminalModifiers |= TerminalModifiers.Alt;
        }
        if (modifiers.HasFlag(KeyModifiers.Control) == true)
        {
            terminalModifiers |= TerminalModifiers.Control;
        }
        if (modifiers.HasFlag(KeyModifiers.Shift) == true)
        {
            terminalModifiers |= TerminalModifiers.Shift;
        }
        if (modifiers.HasFlag(KeyModifiers.Meta) == true)
        {
            terminalModifiers |= (TerminalModifiers)KeyModifiers.Meta;
        }
        return terminalModifiers;
    }

    public static TerminalKey Convert(Key key) => key switch
    {
        Key.None => (TerminalKey)0,
        // Key.Cancel => ,
        Key.Back => TerminalKey.Backspace,
        Key.Tab => TerminalKey.Tab,
        // Key.LineFeed => ,
        Key.Clear => TerminalKey.Clear,
        Key.Enter => TerminalKey.Enter,
        Key.Pause => TerminalKey.Pause,
        // Key.CapsLock => ,
        // Key.Capital => ,
        // Key.HangulMode => ,
        // Key.KanaMode => ,
        // Key.JunjaMode => ,
        // Key.FinalMode => ,
        // Key.KanjiMode => ,
        // Key.HanjaMode => ,
        Key.Escape => TerminalKey.Escape,
        // Key.ImeConvert => ,
        // Key.ImeNonConvert => ,
        // Key.ImeAccept => ,
        // Key.ImeModeChange => ,
        Key.Space => TerminalKey.Spacebar,
        Key.PageUp => TerminalKey.PageUp,
        // Key.Prior => ,
        Key.PageDown => TerminalKey.PageDown,
        // Key.Next => ,
        Key.End => TerminalKey.End,
        Key.Home => TerminalKey.Home,
        Key.Left => TerminalKey.LeftArrow,
        Key.Up => TerminalKey.UpArrow,
        Key.Right => TerminalKey.RightArrow,
        Key.Down => TerminalKey.DownArrow,
        Key.Select => TerminalKey.Select,
        Key.Print => TerminalKey.Print,
        Key.Execute => TerminalKey.Execute,
        // Key.Snapshot => ,
        Key.PrintScreen => TerminalKey.PrintScreen,
        Key.Insert => TerminalKey.Insert,
        Key.Delete => TerminalKey.Delete,
        Key.Help => TerminalKey.Help,
        Key.D0 => TerminalKey.D0,
        Key.D1 => TerminalKey.D1,
        Key.D2 => TerminalKey.D2,
        Key.D3 => TerminalKey.D3,
        Key.D4 => TerminalKey.D4,
        Key.D5 => TerminalKey.D5,
        Key.D6 => TerminalKey.D6,
        Key.D7 => TerminalKey.D7,
        Key.D8 => TerminalKey.D8,
        Key.D9 => TerminalKey.D9,
        Key.A => TerminalKey.A,
        Key.B => TerminalKey.B,
        Key.C => TerminalKey.C,
        Key.D => TerminalKey.D,
        Key.E => TerminalKey.E,
        Key.F => TerminalKey.F,
        Key.G => TerminalKey.G,
        Key.H => TerminalKey.H,
        Key.I => TerminalKey.I,
        Key.J => TerminalKey.J,
        Key.K => TerminalKey.K,
        Key.L => TerminalKey.L,
        Key.M => TerminalKey.M,
        Key.N => TerminalKey.N,
        Key.O => TerminalKey.O,
        Key.P => TerminalKey.P,
        Key.Q => TerminalKey.Q,
        Key.R => TerminalKey.R,
        Key.S => TerminalKey.S,
        Key.T => TerminalKey.T,
        Key.U => TerminalKey.U,
        Key.V => TerminalKey.V,
        Key.W => TerminalKey.W,
        Key.X => TerminalKey.X,
        Key.Y => TerminalKey.Y,
        Key.Z => TerminalKey.Z,
        Key.LWin => TerminalKey.LeftWindows,
        Key.RWin => TerminalKey.RightWindows,
        Key.Apps => TerminalKey.Applications,
        Key.Sleep => TerminalKey.Sleep,
        Key.NumPad0 => TerminalKey.NumPad0,
        Key.NumPad1 => TerminalKey.NumPad1,
        Key.NumPad2 => TerminalKey.NumPad2,
        Key.NumPad3 => TerminalKey.NumPad3,
        Key.NumPad4 => TerminalKey.NumPad4,
        Key.NumPad5 => TerminalKey.NumPad5,
        Key.NumPad6 => TerminalKey.NumPad6,
        Key.NumPad7 => TerminalKey.NumPad7,
        Key.NumPad8 => TerminalKey.NumPad8,
        Key.NumPad9 => TerminalKey.NumPad9,
        Key.Multiply => TerminalKey.Multiply,
        Key.Add => TerminalKey.Add,
        Key.Separator => TerminalKey.Separator,
        Key.Subtract => TerminalKey.Subtract,
        Key.Decimal => TerminalKey.Decimal,
        Key.Divide => TerminalKey.Divide,
        Key.F1 => TerminalKey.F1,
        Key.F2 => TerminalKey.F2,
        Key.F3 => TerminalKey.F3,
        Key.F4 => TerminalKey.F4,
        Key.F5 => TerminalKey.F5,
        Key.F6 => TerminalKey.F6,
        Key.F7 => TerminalKey.F7,
        Key.F8 => TerminalKey.F8,
        Key.F9 => TerminalKey.F9,
        Key.F10 => TerminalKey.F10,
        Key.F11 => TerminalKey.F11,
        Key.F12 => TerminalKey.F12,
        Key.F13 => TerminalKey.F13,
        Key.F14 => TerminalKey.F14,
        Key.F15 => TerminalKey.F15,
        Key.F16 => TerminalKey.F16,
        Key.F17 => TerminalKey.F17,
        Key.F18 => TerminalKey.F18,
        Key.F19 => TerminalKey.F19,
        Key.F20 => TerminalKey.F20,
        Key.F21 => TerminalKey.F21,
        Key.F22 => TerminalKey.F22,
        Key.F23 => TerminalKey.F23,
        Key.F24 => TerminalKey.F24,
        // Key.NumLock => ,
        // Key.Scroll => ,
        // Key.LeftShift =>,
        // Key.RightShift => ,
        // Key.LeftCtrl => ,
        // Key.RightCtrl => ,
        // Key.LeftAlt => ,
        // Key.RightAlt => ,
        Key.BrowserBack => TerminalKey.BrowserBack,
        Key.BrowserForward => TerminalKey.BrowserForward,
        Key.BrowserRefresh => TerminalKey.BrowserRefresh,
        Key.BrowserStop => TerminalKey.BrowserStop,
        Key.BrowserSearch => TerminalKey.BrowserSearch,
        Key.BrowserFavorites => TerminalKey.BrowserFavorites,
        Key.BrowserHome => TerminalKey.BrowserHome,
        Key.VolumeMute => TerminalKey.VolumeMute,
        Key.VolumeDown => TerminalKey.VolumeDown,
        Key.VolumeUp => TerminalKey.VolumeUp,
        Key.MediaNextTrack => TerminalKey.MediaNext,
        Key.MediaPreviousTrack => TerminalKey.MediaPrevious,
        Key.MediaStop => TerminalKey.MediaStop,
        Key.MediaPlayPause => TerminalKey.MediaStop,
        Key.LaunchMail => TerminalKey.LaunchMail,
        Key.SelectMedia => TerminalKey.LaunchMediaSelect,
        Key.LaunchApplication1 => TerminalKey.LaunchApp1,
        Key.LaunchApplication2 => TerminalKey.LaunchApp2,
        // Key.OemSemicolon => ,
        Key.Oem1 => TerminalKey.Oem1,
        Key.OemPlus => TerminalKey.OemPlus,
        Key.OemComma => TerminalKey.OemComma,
        Key.OemMinus => TerminalKey.OemMinus,
        Key.OemPeriod => TerminalKey.OemPeriod,
        // Key.OemQuestion => ,
        Key.Oem2 => TerminalKey.Oem2,
        // Key.OemTilde => ,
        Key.Oem3 => TerminalKey.Oem3,
        // Key.AbntC1 => ,
        // Key.AbntC2 => ,
        // Key.OemOpenBrackets => ,
        Key.Oem4 => TerminalKey.Oem4,
        // Key.OemPipe => ,
        Key.Oem5 => TerminalKey.Oem5,
        // Key.OemCloseBrackets => ,
        Key.Oem6 => TerminalKey.Oem6,
        // Key.OemQuotes => ,
        Key.Oem7 => TerminalKey.Oem7,
        Key.Oem8 => TerminalKey.Oem8,
        // Key.OemBackslash => ,
        Key.Oem102 => TerminalKey.Oem102,
        // Key.ImeProcessed => ,
        // Key.System => ,
        // Key.OemAttn => ,
        // Key.DbeAlphanumeric => ,
        // Key.OemFinish => ,
        // Key.DbeKatakana => ,
        // Key.DbeHiragana => ,
        // Key.OemCopy => ,
        // Key.DbeSbcsChar => ,
        // Key.OemAuto => ,
        // Key.DbeDbcsChar => ,
        // Key.OemEnlw => ,
        // Key.OemBackTab => ,
        // Key.DbeRoman => ,
        // Key.DbeNoRoman => ,
        Key.Attn => TerminalKey.Attention,
        Key.CrSel => TerminalKey.CrSel,
        // Key.DbeEnterWordRegisterMode => ,
        Key.ExSel => TerminalKey.ExSel,
        // Key.DbeEnterImeConfigureMode => ,
        Key.EraseEof => TerminalKey.EraseEndOfFile,
        // Key.DbeFlushString => ,
        Key.Play => TerminalKey.Play,
        // Key.DbeCodeInput => ,
        // Key.DbeNoCodeInput => ,
        Key.Zoom => TerminalKey.Zoom,
        Key.NoName => TerminalKey.NoName,
        // Key.DbeDetermineString => ,
        // Key.DbeEnterDialogConversionMode => ,
        Key.Pa1 => TerminalKey.Pa1,
        Key.OemClear => TerminalKey.OemClear,
        // Key.DeadCharProcessed => ,
        // Key.FnLeftArrow => ,
        // Key.FnRightArrow => ,
        // Key.FnUpArrow => ,
        // Key.FnDownArrow => ,
        _ => (TerminalKey)0,
    };
}

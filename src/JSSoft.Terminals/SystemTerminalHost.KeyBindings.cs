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

using System.Text.RegularExpressions;
using KeyBinding = JSSoft.Terminals.SystemTerminalKeyBinding;

namespace JSSoft.Terminals;

partial class SystemTerminalHost
{
    private TerminalKeyBindingCollection? _keyBindings;

    public virtual TerminalKeyBindingCollection KeyBindings => _keyBindings ??= GetDefaultKeyBindings();

    public static TerminalKeyBindingCollection CommonKeyBindings { get; } =
    [
        new KeyBinding(TerminalKey.Delete, (t) => t.Delete()),
        new KeyBinding(TerminalKey.Home, (t) => t.MoveToFirst()),
        new KeyBinding(TerminalKey.End, (t) => t.MoveToLast()),
        new KeyBinding(TerminalKey.UpArrow, (t) => t.PrevHistory(), (t) => !t.IsPassword),
        new KeyBinding(TerminalKey.DownArrow, (t) => t.NextHistory(), (t) => !t.IsPassword),
        new KeyBinding(TerminalKey.LeftArrow, (t) => t.Left()),
        new KeyBinding(TerminalKey.RightArrow, (t) => t.Right()),
    ];

    public static TerminalKeyBindingCollection WindowsKeyBindings { get; } = new(CommonKeyBindings)
    {
        new KeyBinding(TerminalKey.Enter, InputEnter),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.C, (t) => t.CancelInput()),
        new KeyBinding(TerminalKey.Escape, (t) => t.Command = string.Empty, (t) => !t.IsPassword),
        new KeyBinding(TerminalKey.Backspace, (t) => t.Backspace()),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.H, (t) => t.Backspace()),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.Home, DeleteToFirst, (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.End, DeleteToLast, (t) => !t.IsPassword),
        new KeyBinding(TerminalKey.Tab, (t) => t.NextCompletion(), (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Shift, TerminalKey.Tab, (t) => t.PrevCompletion(), (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.LeftArrow, (t) => PrevWord(t), (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.RightArrow, (t) => NextWord(t), (t) => !t.IsPassword),
    };

    public static TerminalKeyBindingCollection LinuxKeyBindings { get; } = new(CommonKeyBindings)
    {
        new KeyBinding(TerminalKey.Enter, InputEnter),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.C, (t) => t.CancelInput()),
        new KeyBinding(TerminalKey.Escape, (t) => {}),
        new KeyBinding(TerminalKey.Backspace, (t) => t.Backspace()),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.U, DeleteToFirst, (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.K, DeleteToLast, (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.E, (t) => t.MoveToLast()),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.A, (t) => t.MoveToFirst()),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.W, DeletePrevWord, (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Control, TerminalKey.L, (t) => t.Clear(), (t) => !t.IsPassword),
        new KeyBinding(TerminalKey.Tab, (t) => t.NextCompletion(), (t) => !t.IsPassword),
        new KeyBinding(TerminalModifiers.Shift, TerminalKey.Tab, (t) => t.PrevCompletion(), (t) => !t.IsPassword),
    };

    private static TerminalKeyBindingCollection GetDefaultKeyBindings()
    {
        return TerminalEnvironment.IsWindows() == true ? WindowsKeyBindings : LinuxKeyBindings;
    }

    private static int PrevWord(SystemTerminalHost terminalHost)
    {
        if (terminalHost.CursorIndex > 0)
        {
            var index = terminalHost.CursorIndex - 1;
            var command = terminalHost.Command;
            var pattern = @"^\w|(?=\b)\w|$";
            var matches = Regex.Matches(command, pattern).Cast<Match>();
            var match = matches.Where(item => item.Index <= index).Last();
            terminalHost.CursorIndex = match.Index;
        }
        return terminalHost.CursorIndex;
    }

    private static int NextWord(SystemTerminalHost terminalHost)
    {
        var command = terminalHost.Command;
        if (terminalHost.CursorIndex < command.Length)
        {
            var index = terminalHost.CursorIndex;
            var pattern = @"\w(?<=\b)|$";
            var matches = Regex.Matches(command, pattern).Cast<Match>();
            var match = matches.Where(item => item.Index > index).First();
            terminalHost.CursorIndex = Math.Min(command.Length, match.Index + 1);
        }
        return terminalHost.CursorIndex;
    }

    private static void DeleteToLast(SystemTerminalHost terminalHost)
    {
        var index = terminalHost.CursorIndex;
        var command = terminalHost.Command;
        terminalHost.Command = command.Substring(0, index);
    }

    private static void DeleteToFirst(SystemTerminalHost terminalHost)
    {
        var index = terminalHost.CursorIndex;
        var command = terminalHost.Command;
        terminalHost.Command = command.Remove(0, index);
        terminalHost.CursorIndex = 0;
    }

    private static void DeletePrevWord(SystemTerminalHost terminalHost)
    {
        var index2 = terminalHost.CursorIndex;
        var command = terminalHost.Command;
        var index1 = PrevWord(terminalHost);
        var length = index2 - index1;
        terminalHost.Command = command.Remove(index1, length);
    }

    private static void InputEnter(SystemTerminalHost terminalHost)
    {
        if (CommandUtility.TrySplit(terminalHost.Command, out var _) == true)
            terminalHost.EndInput();
        else
            terminalHost.InsertNewLine();
    }
}

// <copyright file="SystemTerminalHost.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals;

public partial class SystemTerminalHost
{
    private const string EscClearScreen = "\u001b[2J";
    private const string EscCursorHome = "\u001b[H";
    private const string EscEraseDown = "\u001b[J";
    private const string EscCursorInvisible = "\u001b[?25l";
    private const string EscCursorVisible = "\u001b[?25h";
    private const string PasswordPattern = "[~`! @#$%^&*()_\\-+={[}\\]|\\\\:;\"'<,>.?/0-9a-zA-Z]";

    private static readonly byte[] charWidths;

    private readonly List<string> _histories = [];
    private readonly Queue<string> _stringQueue = new();
    private readonly ManualResetEvent _eventSet = new(false);
    private readonly StringBuilder _outputText = new();

    private TerminalCoord _pt1 = new(0, Console.IsOutputRedirected is true ? 0 : Console.CursorTop);
    private TerminalCoord _pt2;
    private TerminalCoord _pt3;
    private TerminalCoord _pt4;
    private TerminalCoord _ot1;
    private int _width = Console.IsOutputRedirected is true ? int.MaxValue : Console.BufferWidth;
    private int _height = Console.IsOutputRedirected is true ? int.MaxValue : Console.BufferHeight;
    private int _historyIndex;
    private int _cursorIndex;
    private SystemTerminalPrompt _prompt = SystemTerminalPrompt.Empty;
    private SystemTerminalCommand _command = SystemTerminalCommand.Empty;
    private string _promptText = string.Empty;
    private string _inputText = string.Empty;
    private string _completion = string.Empty;
    private SecureString? _secureString;

    private TerminalFlags _flags;

    private Func<string, bool> _validator = (item) => true;

    static SystemTerminalHost()
    {
        //System.Diagnostics.Debugger.Launch();
        var platformName = GetPlatformName(Environment.OSVersion.Platform);
        var name = $"JSSoft.Terminals.{platformName}.dat";
        using var stream = typeof(SystemTerminalHost).Assembly.GetManifestResourceStream(name)!;
        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        charWidths = buffer;

        static string GetPlatformName(PlatformID platformID)
        {
            return platformID switch
            {
                PlatformID.Unix => $"{PlatformID.Unix}",
                PlatformID.Win32NT => $"{PlatformID.Win32NT}",
                _ => $"{PlatformID.Win32NT}",
            };
        }

        SystemTerminalNative.Initialize();
    }

    public static string NextCompletion(string[] completions, string text)
    {
        if (completions.Contains(text) is true)
        {
            for (var i = 0; i < completions.Length; i++)
            {
                var r = string.Compare(text, completions[i], true);
                if (r is 0)
                {
                    if (i + 1 < completions.Length)
                    {
                        return completions[i + 1];
                    }
                    else
                    {
                        return completions.First();
                    }
                }
            }
        }
        else
        {
            for (var i = 0; i < completions.Length; i++)
            {
                var r = string.Compare(text, completions[i], true);
                if (r < 0)
                {
                    return completions[i];
                }
            }
        }

        return text;
    }

    public static string PrevCompletion(string[] completions, string text)
    {
        if (completions.Contains(text) is true)
        {
            for (var i = completions.Length - 1; i >= 0; i--)
            {
                var r = string.Compare(text, completions[i], true);
                if (r is 0)
                {
                    if (i - 1 >= 0)
                    {
                        return completions[i - 1];
                    }
                    else
                    {
                        return completions.Last();
                    }
                }
            }
        }
        else
        {
            for (var i = completions.Length - 1; i >= 0; i--)
            {
                var r = string.Compare(text, completions[i], true);
                if (r < 0)
                {
                    return completions[i];
                }
            }
        }

        return text;
    }

    public static int GetLength(string text)
    {
        var length = 0;
        foreach (var item in text)
        {
            length += charWidths[(int)item];
        }

        return length;
    }

    public long? ReadLong(string prompt)
    {
        var result = ReadNumber(prompt, null, i => long.TryParse(i, out long v));
        if (result is long value)
        {
            return value;
        }

        return null;
    }

    public long? ReadLong(string prompt, long defaultValue)
    {
        var result = ReadNumber(prompt, defaultValue, i => long.TryParse(i, out long v));
        if (result is long value)
        {
            return value;
        }

        return null;
    }

    public double? ReadDouble(string prompt)
    {
        var result = ReadNumber(prompt, null, i => double.TryParse(i, out double v));
        if (result is double value)
        {
            return value;
        }

        return null;
    }

    public double? ReadDouble(string prompt, double defaultValue)
    {
        var result = ReadNumber(prompt, defaultValue, i => double.TryParse(i, out double v));
        if (result is double value)
        {
            return value;
        }

        return null;
    }

    public string? ReadString(string prompt)
    {
        return ReadString(prompt, string.Empty);
    }

    public string? ReadString(string prompt, string command)
    {
        using var initializer = new Initializer(this)
        {
            Prompt = prompt,
            Command = command,
        };
        return initializer.ReadLineImpl(i => true) as string;
    }

    public SecureString? ReadSecureString(string prompt)
    {
        using var initializer = new Initializer(this)
        {
            Prompt = prompt,
            Command = _command,
            Flags = TerminalFlags.IsPassword,
        };
        return initializer.ReadLineImpl(i => true) as SecureString;
    }

    public ConsoleKey ReadKey(string prompt, params ConsoleKey[] filters)
    {
        using var initializer = new Initializer(this)
        {
            Prompt = prompt,
        };
        return initializer.ReadKeyImpl(filters);
    }

    public void InsertNewLine()
    {
        MoveToLast();
        InsertText(Environment.NewLine);
    }

    public void EndInput()
    {
        _flags |= TerminalFlags.IsInputEnded;
    }

    public void CancelInput()
    {
        _flags |= TerminalFlags.IsInputCancelled;
    }

    public void NextHistory()
    {
        lock (LockedObject)
        {
            if (IsPassword is true)
            {
                throw new InvalidOperationException();
            }

            if (_historyIndex + 1 < _histories.Count)
            {
                SetHistoryIndex(_historyIndex + 1);
            }
        }
    }

    public void PrevHistory()
    {
        lock (LockedObject)
        {
            if (IsPassword is true)
            {
                throw new InvalidOperationException();
            }

            if (_historyIndex > 0)
            {
                SetHistoryIndex(_historyIndex - 1);
            }
            else if (_histories.Count == 1)
            {
                SetHistoryIndex(0);
            }
        }
    }

    public IReadOnlyList<string> Histories => _histories;

    public int HistoryIndex
    {
        get => _historyIndex;
        set
        {
            if (value < 0 || value >= _histories.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (_historyIndex == value)
            {
                return;
            }

            lock (LockedObject)
            {
                SetHistoryIndex(value);
            }
        }
    }

    public void Clear()
    {
        lock (LockedObject)
        {
            using var stream = Console.OpenStandardOutput();
            using var writer = new StreamWriter(stream, Console.OutputEncoding);
            var offset = new TerminalCoord(0, _pt1.Y);
            var bufferWidth = _width;
            var pre = _command.Slice(0, _cursorIndex);
            var promptTextF = _prompt.FormattedText + _command.FormattedText;
            var st1 = pre.Next(_pt2, bufferWidth) - offset;
            _pt1 -= offset;
            _pt2 -= offset;
            _pt3 -= offset;
            _ot1 = TerminalCoord.Empty;
            writer.Write($"{EscClearScreen}{EscCursorHome}{promptTextF}{st1.CursorString}");
        }
    }

    public void Delete()
    {
        lock (LockedObject)
        {
            if (_cursorIndex < _command.Length)
            {
                DeleteImpl();
            }
        }
    }

    public void MoveToFirst()
    {
        lock (LockedObject)
        {
            SetCursorIndex(0);
        }
    }

    public void MoveToLast()
    {
        lock (LockedObject)
        {
            SetCursorIndex(_command.Length);
        }
    }

    public void Left()
    {
        lock (LockedObject)
        {
            if (_cursorIndex > 0)
            {
                SetCursorIndex(_cursorIndex - 1);
            }
        }
    }

    public void Right()
    {
        lock (LockedObject)
        {
            if (_cursorIndex < _command.Length)
            {
                SetCursorIndex(_cursorIndex + 1);
            }
        }
    }

    public void Backspace()
    {
        lock (LockedObject)
        {
            if (_cursorIndex > 0)
            {
                BackspaceImpl();
            }
        }
    }

    public void NextCompletion()
    {
        lock (LockedObject)
        {
            if (IsPassword is true)
            {
                throw new InvalidOperationException();
            }

            CompletionImpl(NextCompletion);
        }
    }

    public void PrevCompletion()
    {
        lock (LockedObject)
        {
            if (IsPassword is true)
            {
                throw new InvalidOperationException();
            }

            CompletionImpl(PrevCompletion);
        }
    }

    public void EnqueueString(string text)
    {
        lock (ExternalObject)
        {
            _stringQueue.Enqueue(text);
        }

        RenderStringQueue();
    }

    public async Task EnqueueStringAsync(string text)
    {
        lock (ExternalObject)
        {
            _eventSet.Reset();
            _stringQueue.Enqueue(text);
        }

        await Task.Run(_eventSet.WaitOne);
    }

    public int CursorIndex
    {
        get => _cursorIndex;
        set
        {
            if (value < 0 || value > _command.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (_cursorIndex == value)
            {
                return;
            }

            lock (LockedObject)
            {
                SetCursorIndex(value);
            }
        }
    }

    public string Command
    {
        get => _command.Text;
        set
        {
            if (IsPassword is true)
            {
                throw new InvalidOperationException();
            }

            if (_command == value)
            {
                return;
            }

            lock (LockedObject)
            {
                SetCommand(value);
            }
        }
    }

    public string Prompt
    {
        get => _prompt;
        set
        {
            if (_prompt == value)
            {
                return;
            }

            lock (LockedObject)
            {
                SetPrompt(value);
            }
        }
    }

    public bool IsReading => _flags.HasFlag(TerminalFlags.IsReading);

    public bool IsPassword => _flags.HasFlag(TerminalFlags.IsPassword);

    public bool IsEnabled { get; set; } = true;

    public static char PasswordCharacter { get; set; } = '*';

    protected virtual string FormatPrompt(string prompt) => prompt;

    protected virtual string FormatCommand(string command) => command;

    protected virtual string[] GetCompletion(string[] items, string find)
    {
        if (find == string.Empty)
        {
            return items;
        }

        var query = from item in items
                    where item.StartsWith(find)
                    orderby item
                    select item;
        return [.. query];
    }

    protected void UpdateLayout(int bufferWidth, int bufferHeight)
    {
        var prompt = _prompt;
        var command = _command;
        var offsetY = _pt4.Y - _pt1.Y;
        var lt3 = _pt3;
        var pre = command.Slice(0, _cursorIndex);
        var cursor = new TerminalCoord(Console.IsOutputRedirected is true ? 0 : Console.CursorLeft, Console.IsOutputRedirected is true ? 0 : Console.CursorTop);
        var pt1 = new TerminalCoord(0, cursor.Y - offsetY);
        var nt1 = PrevPosition(prompt + pre, bufferWidth, cursor);
        if (nt1.X is 0)
        {
            var pt2 = NextPosition(prompt, bufferWidth, nt1);
            var pt3 = command.Next(pt2, bufferWidth);
            var pt4 = pre.Next(pt2, bufferWidth);
            _width = bufferWidth;
            _height = bufferHeight;
            _pt1 = nt1;
            _pt2 = pt2;
            _pt3 = pt3;
            _pt4 = pt4;
        }
        else
        {
            var pt2 = prompt.Next(pt1, bufferWidth);
            var pt3 = command.Next(pt2, bufferWidth);
            var pt4 = pre.Next(pt2, bufferWidth);
            var offset1 = pt3.Y >= bufferHeight ? new TerminalCoord(0, pt3.Y - lt3.Y) : TerminalCoord.Empty;
            var st1 = pt1;
            var st2 = pt3;
            var st3 = pt4 - offset1;

            _width = bufferWidth;
            _height = bufferHeight;
            _pt1 = pt1 - offset1;
            _pt2 = pt2 - offset1;
            _pt3 = pt3 - offset1;
            _pt4 = st3;

            RenderString(st1, st2, st3, bufferHeight, items: [prompt, command]);
        }
    }

    private void InsertText(string text)
    {
        lock (LockedObject)
        {
            var displayText = IsPassword is true ? ConvertToPassword(text) : text;
            var oldCursorIndex = _cursorIndex;
            var bufferWidth = _width;
            var bufferHeight = _height;
            var newCursorIndex = oldCursorIndex + displayText.Length;
            var extra = _command.Slice(oldCursorIndex);
            var command = _command.Insert(oldCursorIndex, displayText);
            var prompt = _prompt;
            var pt1 = _pt1;
            var pt2 = _pt2;
            var lt3 = _pt3;

            var pre = command.Slice(0, command.Length - extra.Length);
            var pt3 = command.Next(pt2, bufferWidth);
            var pt4 = pre.Next(pt2, bufferWidth);
            var offset = pt3.Y >= bufferHeight ? new TerminalCoord(0, pt3.Y - lt3.Y) : TerminalCoord.Empty;
            var st1 = pt2;
            var st2 = pt3;
            var st3 = pt4 - offset;

            _cursorIndex = newCursorIndex;
            _command = command;
            _promptText = prompt.FormattedText + command.FormattedText;
            _inputText = pre.Text;
            _completion = string.Empty;
            _pt1 = pt1 - offset;
            _pt2 = pt2 - offset;
            _pt3 = pt3 - offset;
            _pt4 = st3;
            _secureString?.InsertAt(oldCursorIndex, text);

            RenderString(st1, st2, st3, bufferHeight, items: [command]);
        }
    }

    private static void RenderString(TerminalCoord pt1, TerminalCoord pt2, TerminalCoord ct1, int bufferHeight, object[] items)
    {
        var texts = items.Select(item => $"{item:terminal}");
        var capacity = texts.Sum(item => item.Length) + 30;
        var sb = new StringBuilder(capacity);
        var last = texts.Any() is true ? texts.Last() : string.Empty;
        sb.Append(pt1.CursorString);
        sb.Append(EscEraseDown);
        foreach (var item in texts)
        {
            sb.Append(item);
        }

        if (NeedLineBreak() is true)
        {
            sb.Append(Environment.NewLine);
        }

        sb.Append(EscEraseDown);
        sb.Append(ct1.CursorString);
        RenderString(sb.ToString());

        bool NeedLineBreak()
        {
            if (TerminalEnvironment.IsWindows() is true && Console.BufferHeight > Console.WindowHeight && pt2.Y <= bufferHeight)
            {
                return false;
            }

            if (pt2.Y <= pt1.Y)
            {
                return false;
            }

            if (pt2.X is not 0)
            {
                return false;
            }

            if (last.EndsWith(Environment.NewLine) is true)
            {
                return false;
            }

            return true;
        }
    }

    private static void RenderString(string text)
    {
        using var stream = Console.OpenStandardOutput();
        using var writer = new StreamWriter(stream, Console.OutputEncoding) { AutoFlush = true };
        writer.Write(text);
    }

    private static void SetCursorPosition(TerminalCoord pt)
    {
        using var stream = Console.OpenStandardOutput();
        using var writer = new StreamWriter(stream, Console.OutputEncoding);
        writer.Write(pt.CursorString);
    }

    private static TerminalCoord PrevPosition(string text, int bufferWidth, TerminalCoord pt)
    {
        var x = pt.X;
        var y = pt.Y;
        for (var i = text.Length - 1; i >= 0; i--)
        {
            var ch = text[i];
            if (ch == '\r')
            {
                x = bufferWidth;
                continue;
            }
            else if (ch == '\n')
            {
                x = bufferWidth;
                y--;
                continue;
            }

            var w = charWidths[(int)ch];
            if (x - w < 0)
            {
                x = bufferWidth - w;
                y--;
            }
            else
            {
                x -= w;
            }
        }

        return new TerminalCoord(x, y);
    }

    internal static TerminalCoord NextPosition(string text, int bufferWidth, TerminalCoord pt)
    {
        var x = pt.X;
        var y = pt.Y;
        for (var i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            if (ch == '\r')
            {
                x = 0;
                continue;
            }
            else if (ch == '\n')
            {
                x = 0;
                y++;
                continue;
            }

            var w = charWidths[(int)ch];
            if (x + w >= bufferWidth)
            {
                x = x + w - bufferWidth;
                y++;
            }
            else
            {
                x += w;
            }
        }

        return new TerminalCoord(x, y);
    }

    private static string StripOff(string text)
    {
        return Regex.Replace(text, @"\e\[(\d+;)*(\d+)?[ABCDHJKfmsu]", string.Empty);
    }

    private void BackspaceImpl()
    {
        var bufferWidth = _width;
        var bufferHeight = _height;
        var prompt = _prompt;
        var extra = _command.Slice(_cursorIndex);
        var command = _command.Remove(_cursorIndex - 1, 1);
        var cursorIndex = _cursorIndex - 1;
        var endPosition = _command.Length;
        var pt2 = _pt2;

        var pre = command.Slice(0, command.Length - extra.Length);
        var pt3 = pre.Next(pt2, bufferWidth);
        var pt4 = extra.Next(pt3, bufferWidth);

        _command = command;
        _promptText = prompt.FormattedText + command.FormattedText;
        _cursorIndex = cursorIndex;
        _inputText = pre;
        _pt3 = pt4;
        _pt4 = pt3;
        _secureString?.RemoveAt(cursorIndex);

        RenderString(pt2, pt4, pt3, bufferHeight, items: [command]);
    }

    private void DeleteImpl()
    {
        var bufferWidth = _width;
        var bufferHeight = _height;
        var prompt = _prompt;
        var extra = _command.Slice(_cursorIndex + 1);
        var command = _command.Remove(_cursorIndex, 1);
        var endPosition = _command.Length;
        var pt2 = _pt2;

        var pre = command.Slice(0, command.Length - extra.Length);
        var pt3 = pre.Next(pt2, bufferWidth);
        var pt4 = extra.Next(pt3, bufferWidth);

        _command = command;
        _promptText = prompt.FormattedText + command.FormattedText;
        _inputText = pre;
        _pt3 = pt4;
        _pt4 = pt3;
        _secureString?.RemoveAt(_cursorIndex);

        RenderString(pt2, pt4, pt3, bufferHeight, items: [command]);
    }

    private void CompletionImpl(Func<string[], string, string> func)
    {
#if NETFRAMEWORK
        var matchCompletions = CommandUtility.MatchCompletion(_inputText);
        var matches = new List<Match>(matchCompletions.Count);
        foreach (Match item in matchCompletions)
        {
            matches.Add(item);
        }
#else
        var matches = new List<Match>(CommandUtility.MatchCompletion(_inputText));
#endif // NETFRAMEWORK
        var find = string.Empty;
        var prefix = false;
        var postfix = false;
        var leftText = _inputText;
        if (matches.Count > 0)
        {
            var match = matches.Last();
            var matchText = match.Value;
            if (matchText.Length > 0 && matchText.First() == '\"')
            {
                prefix = true;
                matchText = matchText.Substring(1);
            }

            if (matchText.Length > 1 && matchText.Last() == '\"')
            {
                postfix = true;
                matchText = matchText.Remove(matchText.Length - 1);
            }

            if (matchText == matchText.Trim())
            {
                find = matchText;
                matches.RemoveAt(matches.Count - 1);
                leftText = _inputText.Remove(match.Index);
            }
        }

        var argList = new List<string>();
        for (var i = 0; i < matches.Count; i++)
        {
            var matchText = matches[i].Value.Trim();
            if (matchText != string.Empty)
            {
                argList.Add(matchText);
            }
        }

        var completions = GetCompletion([.. argList], find);
        if (completions is not null && completions.Length is not 0)
        {
            var completion = func(completions, _completion);
            var inputText = _inputText;
            var command = leftText + completion;
            if (prefix is true || postfix is true)
            {
                command = leftText + "\"" + completion + "\"";
            }

            SetCommand(command);
            _completion = completion;
            _inputText = inputText;
        }
    }

    private void SetPrompt(string value)
    {
        var bufferWidth = _width;
        var bufferHeight = _height;
        var command = _command;
        var prompt = new SystemTerminalPrompt(value, FormatPrompt);
        var pt1 = _pt1;
        var lt3 = _pt3;
        var pre = command.Slice(0, _cursorIndex);

        var pt2 = prompt.Next(pt1, bufferWidth);
        var pt3 = command.Next(pt2, bufferWidth);
        var pt4 = pre.Next(pt2, bufferWidth);
        var offset = pt3.Y >= bufferHeight ? new TerminalCoord(0, pt3.Y - lt3.Y) : TerminalCoord.Empty;
        var st1 = pt1;
        var st2 = pt3 - offset;
        var st3 = pt4 - offset;

        _prompt = prompt;
        _promptText = prompt.FormattedText + command.FormattedText;
        _pt1 = pt1 - offset;
        _pt2 = pt2 - offset;
        _pt3 = pt3 - offset;
        _pt4 = st3;

        if (IsReading is true)
        {
            RenderString(st1, st2, st3, bufferHeight, items: [prompt, command]);
        }
    }

    private void SetCommand(string value)
    {
        var bufferHeight = _height;
        var prompt = _prompt;
        var pt1 = _pt1;
        var pt2 = _pt2;
        var lt3 = _pt3;

        var pt3 = pt2;
        var offset = pt3.Y >= bufferHeight ? new TerminalCoord(0, pt3.Y - lt3.Y) : TerminalCoord.Empty;
        var st2 = pt3 - offset;
        var st3 = pt3 - offset;

        _command = SystemTerminalCommand.Empty;
        _promptText = prompt.FormattedText;
        _cursorIndex = _command.Length;
        _inputText = value;
        _completion = string.Empty;
        _pt1 = pt1 - offset;
        _pt2 = pt2 - offset;
        _pt3 = pt3 - offset;
        _pt4 = st3;

        FlushKeyChars(ref value);
    }

    private void SetCursorIndex(int cursorIndex)
    {
        var bufferWidth = _width;
        var pre = _command.Slice(0, cursorIndex);
        var pt4 = pre.Next(_pt2, bufferWidth);

        _pt4 = pt4;
        _cursorIndex = cursorIndex;
        _inputText = pre;
        _completion = string.Empty;

        SetCursorPosition(pt4);
    }

    private void SetHistoryIndex(int index)
    {
        SetCommand(_histories[index]);
        _historyIndex = index;
    }

    private object? ReadNumber(string prompt, object? value, Func<string, bool> validation)
    {
        using var initializer = new Initializer(this)
        {
            Prompt = prompt,
            Command = $"{value}",
        };
        return initializer.ReadLineImpl(validation);
    }

    private object? ReadLineImpl(CancellationToken cancellation)
    {
        while (IsEnabled is true && cancellation.IsCancellationRequested is false)
        {
            var text = string.Empty;
            Update();
            if (Console.IsInputRedirected is false)
            {
                while (Console.KeyAvailable is true)
                {
                    var keyInfo = Console.ReadKey(true);
                    var ch = keyInfo.KeyChar;
                    var modifiers = (TerminalModifiers)keyInfo.Modifiers;
                    var key = (TerminalKey)keyInfo.Key;
                    if (text != string.Empty)
                    {
                        FlushKeyChars(ref text);
                    }

                    if (KeyBindings.Process(this, modifiers, key) is false &&
                        PreviewKeyChar(keyInfo.KeyChar) is true &&
                        PreviewCommand(text + keyInfo.KeyChar) is true)
                    {
                        text += keyInfo.KeyChar;
                    }

                    if (IsInputEnded is true)
                    {
                        return OnInputEnd();
                    }
                    else if (IsInputCancelled is true)
                    {
                        return OnInputCancel();
                    }
                }

                if (text != string.Empty)
                {
                    FlushKeyChars(ref text);
                }

                Thread.Sleep(1);
            }
            else
            {
                if (Console.In.Peek() != -1)
                {
                    return Console.ReadLine();
                }
            }
        }

        return null;
    }

    private void FlushKeyChars(ref string keyChars)
    {
        InsertText(keyChars);
        keyChars = string.Empty;
    }

    private void RenderStringQueue()
    {
        var text = string.Empty;
        lock (ExternalObject)
        {
            if (_stringQueue.Count is not 0)
            {
                var sb = new StringBuilder();
                while (_stringQueue.Count is not 0)
                {
                    var item = _stringQueue.Dequeue();
                    sb.Append(item);
                }

                text = sb.ToString();
            }
        }

        if (text != string.Empty)
        {
            RenderOutput(text);
        }

        _eventSet.Set();
    }

    private ConsoleKey ReadKeyImpl(params ConsoleKey[] filters)
    {
        while (true)
        {
            var key = Console.ReadKey(true);

            if ((int)key.Modifiers is not 0)
            {
                continue;
            }

            if (filters.Length is 0 || filters.Any(item => item == key.Key) is true)
            {
                InsertText(key.Key.ToString());
                return key.Key;
            }
        }
    }

    private bool PreviewKeyChar(char ch)
    {
        if (ch != '\0')
        {
            if (IsPassword is true)
            {
                return Regex.IsMatch($"{ch}", PasswordPattern);
            }

            return true;
        }

        return false;
    }

    private bool PreviewCommand(string command) => _validator.Invoke(command);

    private void Initialize(string prompt, TerminalFlags flags, Func<string, bool> validator)
    {
        var bufferWidth = Console.IsOutputRedirected is true ? int.MaxValue : Console.BufferWidth;
        var bufferHeight = Console.IsOutputRedirected is true ? int.MaxValue : Console.BufferHeight;
        var pt1 = new TerminalCoord(0, Console.IsOutputRedirected is true ? _pt1.Y : Console.CursorTop);
        lock (LockedObject)
        {
            var isPassword = flags.HasFlag(TerminalFlags.IsPassword);
            var promptS = new SystemTerminalPrompt(prompt, FormatPrompt);
            var pt2 = promptS.Next(pt1, bufferWidth);
            var pt3 = pt2;
            var offset = pt3.Y >= bufferHeight ? new TerminalCoord(0, pt3.Y - pt1.Y) : TerminalCoord.Empty;
            var st1 = pt1;
            var st2 = pt3 - offset;
            var st3 = pt3 - offset;

            _width = bufferWidth;
            _height = bufferHeight;
            _prompt = promptS;
            _command = new SystemTerminalCommand(string.Empty, FormatCommand);
            _promptText = promptS.FormattedText;
            _cursorIndex = 0;
            _inputText = _command;
            _completion = string.Empty;
            _pt1 = pt1 - offset;
            _pt2 = pt2 - offset;
            _pt3 = pt3 - offset;
            _pt4 = pt3 - offset;
            _ot1 = TerminalCoord.Empty;
            _flags = flags | TerminalFlags.IsReading;
            _validator = validator;
            _secureString = isPassword is true ? new SecureString() : null;
            RenderString(st1, st2, st3, bufferHeight, items: [EscCursorVisible, promptS]);
        }
    }

    private void Release()
    {
        lock (LockedObject)
        {
            using (var stream = Console.OpenStandardOutput())
            using (var writer = new StreamWriter(stream, Console.OutputEncoding))
            {
                writer.WriteLine(EscCursorInvisible);
            }

            _outputText.AppendLine(_promptText);
            _pt1 = new TerminalCoord(0, Console.IsOutputRedirected is true ? 0 : Console.CursorTop);
            _pt2 = _pt1;
            _pt3 = _pt1;
            _pt4 = _pt1;
            _prompt = SystemTerminalPrompt.Empty;
            _command = SystemTerminalCommand.Empty;
            _promptText = string.Empty;
            _cursorIndex = 0;
            _inputText = string.Empty;
            _completion = string.Empty;
            _flags = TerminalFlags.None;
            _secureString = null;
        }
    }

    private object? OnInputEnd()
    {
        if (CanRecord is true)
        {
            RecordCommand(_command.Text);
        }

        if (IsPassword is true)
        {
            return _secureString;
        }

        var items = CommandUtility.Split(_command.Text);
        return CommandUtility.Join(items);
    }

    private object? OnInputCancel()
    {
        return null;
    }

    private void RecordCommand(string command)
    {
        if (_histories.Contains(command) is false)
        {
            _histories.Add(command);
            _historyIndex = _histories.Count;
        }
        else
        {
            _historyIndex = _histories.LastIndexOf(command) + 1;
        }
    }

    private void RenderOutput(string textF)
    {
        var bufferWidth = _width;
        var bufferHeight = _height;
        var promptText = _promptText;
        var prompt = _prompt;
        var command = _command;
        var pre = _command.Slice(0, _cursorIndex);
        var pt8 = _pt1 + _ot1;

        var text = StripOff(textF);
        var ct1 = NextPosition(text, bufferWidth, pt8);
        var text1F = NeedLineBreak(ct1, bufferHeight, text) is true ? textF + Environment.NewLine : textF;
        var text1 = StripOff(text1F);
        var promptS = text1F + promptText;
        var pt9 = NextPosition(text1, bufferWidth, pt8);
        var pt1 = pt9.X is 0 ? new TerminalCoord(pt9.X, pt9.Y) : new TerminalCoord(0, pt9.Y + 1);
        var pt2 = prompt.Next(pt1, bufferWidth);
        var pt3 = command.Next(pt2, bufferWidth);
        var pt4 = pre.Next(pt2, bufferWidth);

        var offset = pt3.Y >= bufferHeight ? new TerminalCoord(0, pt3.Y + 1 - bufferHeight) : TerminalCoord.Empty;
        var st1 = new TerminalCoord(pt8.X, pt8.Y);
        var st2 = pt3 - offset;
        var st3 = pt4 - offset;

        _pt1 = pt1 - offset;
        _pt2 = pt2 - offset;
        _pt3 = pt3 - offset;
        _pt4 = pt4 - offset;
        _ot1 = new TerminalCoord(ct1.X, ct1.X is not 0 ? -1 : 0);
        _outputText.Append(text);

        RenderString(st1, st2, st3, bufferHeight, items: [promptS]);

        static bool NeedLineBreak(TerminalCoord pt, int height, string text)
        {
            if (text == "\r")
            {
                return false;
            }

            if (text == "\n")
            {
                return false;
            }

            if (text.EndsWith(Environment.NewLine) is true)
            {
                return false;
            }

            if (pt == TerminalCoord.Empty)
            {
                return false;
            }

            if (TerminalEnvironment.IsWindows() is true && Console.BufferHeight > Console.WindowHeight && pt.Y < height)
            {
                return false;
            }

            return true;
        }
    }

    private static string ConvertToPassword(string text)
        => string.Empty.PadRight(text.Length, PasswordCharacter);

    private bool IsRecordable => _flags.HasFlag(TerminalFlags.IsRecordable);

    private bool CanRecord => IsRecordable is true && IsPassword is false && _command != string.Empty;

    private bool IsInputCancelled => _flags.HasFlag(TerminalFlags.IsInputCancelled);

    private bool IsInputEnded => _flags.HasFlag(TerminalFlags.IsInputEnded);

    internal string? ReadStringInternal(string prompt, CancellationToken cancellation)
    {
        using var initializer = new Initializer(this)
        {
            Prompt = prompt,
            Flags = TerminalFlags.IsRecordable,
        };
        return initializer.ReadLineImpl(i => true, cancellation) as string;
    }

    internal void Update()
    {
        if (Console.IsOutputRedirected is false && IsLayoutChanged() is true)
        {
            UpdateLayout(Console.BufferWidth, Console.BufferHeight);
        }

        RenderStringQueue();

        bool IsLayoutChanged()
        {
            if (_width != Console.BufferWidth || _height != Console.BufferHeight)
            {
                return true;
            }

            return false;
        }
    }

    internal static object LockedObject { get; } = new object();

    internal static object ExternalObject { get; } = new object();

    private sealed class Initializer : IDisposable
    {
        private readonly SystemTerminalHost _terminalHost;
        private readonly bool _isControlC = Console.IsInputRedirected is false && Console.TreatControlCAsInput;

        public Initializer(SystemTerminalHost terminalHost)
        {
            _terminalHost = terminalHost;
            if (Console.IsInputRedirected is false)
            {
                _isControlC = Console.TreatControlCAsInput;
                Console.TreatControlCAsInput = true;
            }
        }

        public string Prompt { get; set; } = string.Empty;

        public string Command { get; set; } = string.Empty;

        public TerminalFlags Flags { get; set; }

        public Func<string, bool> Validator { get; set; } = (s) => true;

        public object? ReadLineImpl(Func<string, bool> validation)
        {
            return ReadLineImpl(validation, CancellationToken.None);
        }

        public object? ReadLineImpl(Func<string, bool> validation, CancellationToken cancellation)
        {
            _terminalHost.Initialize(Prompt, Flags, Validator);
            _terminalHost.SetCommand(Command);
            return _terminalHost.ReadLineImpl(cancellation);
        }

        public ConsoleKey ReadKeyImpl(params ConsoleKey[] filters)
        {
            _terminalHost.Initialize(Prompt, Flags, Validator);
            _terminalHost.SetCommand(Command);
            return _terminalHost.ReadKeyImpl(filters);
        }

        public void Dispose()
        {
            _terminalHost.Release();
            if (Console.IsInputRedirected is false)
            {
                Console.TreatControlCAsInput = _isControlC;
            }
        }
    }

    [Flags]
    private enum TerminalFlags
    {
        None = 0,

        IsPassword = 1,

        IsReading = 2,

        IsRecordable = 4,

        IsInputCancelled = 8,

        IsInputEnded = 16,
    }
}

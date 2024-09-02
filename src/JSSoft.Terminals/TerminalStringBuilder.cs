// <copyright file="TerminalStringBuilder.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;

namespace JSSoft.Terminals;

public sealed class TerminalStringBuilder
{
    private static readonly Dictionary<TerminalColorType, int> ForegroundValues = new()
    {
        { TerminalColorType.Black, 30 },
        { TerminalColorType.Red, 31 },
        { TerminalColorType.Green, 32 },
        { TerminalColorType.Yellow, 33 },
        { TerminalColorType.Blue, 34 },
        { TerminalColorType.Magenta, 35 },
        { TerminalColorType.Cyan, 36 },
        { TerminalColorType.White, 37 },
        { TerminalColorType.BrightBlack, 90 },
        { TerminalColorType.BrightRed, 91 },
        { TerminalColorType.BrightGreen, 92 },
        { TerminalColorType.BrightYellow, 93 },
        { TerminalColorType.BrightBlue, 94 },
        { TerminalColorType.BrightMagenta, 95 },
        { TerminalColorType.BrightCyan, 96 },
        { TerminalColorType.BrightWhite, 97 },
    };

    private static readonly Dictionary<TerminalColorType, int> BackgroundValues = new()
    {
        { TerminalColorType.Black, 40 },
        { TerminalColorType.Red, 41 },
        { TerminalColorType.Green, 42 },
        { TerminalColorType.Yellow, 43 },
        { TerminalColorType.Blue, 44 },
        { TerminalColorType.Magenta, 45 },
        { TerminalColorType.Cyan, 46 },
        { TerminalColorType.White, 47 },
        { TerminalColorType.BrightBlack, 100 },
        { TerminalColorType.BrightRed, 101 },
        { TerminalColorType.BrightGreen, 102 },
        { TerminalColorType.BrightYellow, 103 },
        { TerminalColorType.BrightBlue, 104 },
        { TerminalColorType.BrightMagenta, 105 },
        { TerminalColorType.BrightCyan, 106 },
        { TerminalColorType.BrightWhite, 107 },
    };

    private readonly StringBuilder _sb;
    private TerminalColorType? _foreground;
    private TerminalColorType? _background;
    private bool _isBold;
    private bool _isUnderline;
    private bool _isNegative;
    private string _p1 = string.Empty;
    private string _p2 = string.Empty;

    public TerminalStringBuilder() => _sb = new();

    public TerminalStringBuilder(int capacity) => _sb = new(capacity);

    public static string GetString(string text, TerminalColorType? foreground) => GetString(text, foreground, background: null);

    public static string GetString(string text, TerminalColorType? foreground, TerminalColorType? background)
    {
        if (foreground is not null && background is not null)
        {
            return $"\x1b[0;{ForegroundValues[foreground.Value]};{BackgroundValues[background.Value]}m{text}\x1b[0m";
        }
        else if (foreground is not null)
        {
            return $"\x1b[0;{ForegroundValues[foreground.Value]}m{text}\x1b[0m";
        }
        else if (background is not null)
        {
            return $"\x1b[0;{BackgroundValues[background.Value]}m{text}\x1b[0m";
        }
        else
        {
            return text;
        }
    }

    public override string ToString() => _sb.ToString();

    public void Clear()
    {
        _sb.Clear();
        _p1 = string.Empty;
        _p2 = string.Empty;
        _foreground = null;
        _background = null;
        _isBold = false;
        _isNegative = false;
        _isUnderline = false;
    }

    public void Append(string text)
    {
        if (_p1 != _p2)
        {
            _sb.Append($"{_p1}{text}");
            _p2 = _p1;
        }
        else
        {
            _sb.Append(text);
        }
    }

    public void AppendLine() => AppendLine(string.Empty);

    public void AppendLine(string text)
    {
        if (_p1 != _p2)
        {
            _sb.AppendLine($"{_p1}{text}");
            _p2 = _p1;
        }
        else
        {
            _sb.AppendLine(text);
        }
    }

    public void AppendEnd()
    {
        _sb.Append("\x1b[0m");
        _p2 = string.Empty;
    }

    public void ResetOptions()
    {
        _foreground = null;
        _background = null;
        _isBold = false;
        _isNegative = false;
        _isUnderline = false;
        UpdateEscapeCode();
    }

    public TerminalColorType? Foreground
    {
        get => _foreground;
        set
        {
            _foreground = value;
            UpdateEscapeCode();
        }
    }

    public TerminalColorType? Background
    {
        get => _background;
        set
        {
            _background = value;
            UpdateEscapeCode();
        }
    }

    public bool IsBold
    {
        get => _isBold;
        set
        {
            _isBold = value;
            UpdateEscapeCode();
        }
    }

    public bool IsUnderline
    {
        get => _isUnderline;
        set
        {
            _isUnderline = value;
            UpdateEscapeCode();
        }
    }

    public bool IsNegative
    {
        get => _isNegative;
        set
        {
            _isNegative = value;
            UpdateEscapeCode();
        }
    }

    private void UpdateEscapeCode()
    {
        var itemList = new List<string>(3);
        if (_isBold is true)
        {
            itemList.Add($"{1}");
        }

        if (_isUnderline is true)
        {
            itemList.Add($"{4}");
        }

        if (_isNegative is true)
        {
            itemList.Add($"{7}");
        }

        if (_foreground is not null)
        {
            itemList.Add($"{ForegroundValues[_foreground.Value]}");
        }

        if (_background is not null)
        {
            itemList.Add($"{BackgroundValues[_background.Value]}");
        }

        if (itemList.Count != 0)
        {
            _p1 = $"\x1b[{string.Join(";", itemList)}m";
        }
        else if (_p2 != string.Empty)
        {
            _p1 = $"\x1b[0m";
        }
        else
        {
            _p1 = string.Empty;
        }
    }
}

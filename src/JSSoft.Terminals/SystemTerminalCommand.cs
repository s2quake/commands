// <copyright file="SystemTerminalCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

internal readonly struct SystemTerminalCommand(string text, Func<string, string> formatter)
    : IFormattable
{
    private const string MultilinePrompt = "> ";

    private readonly Func<string, string> _formatter = formatter;

    public override readonly string ToString() => Text;

    public readonly SystemTerminalCommand Slice(int start, int length)
    {
        var item = Text.Substring(start, length);
        var formatter = _formatter;
        return new SystemTerminalCommand(item, formatter);
    }

    public readonly SystemTerminalCommand Slice(int startIndex)
    {
        var item = Text.Substring(startIndex);
        var formatter = _formatter;
        return new SystemTerminalCommand(item, formatter);
    }

    public readonly SystemTerminalCommand Insert(int startIndex, string value)
    {
        var item = Text.Insert(startIndex, value);
        var formatter = _formatter;
        return new SystemTerminalCommand(item, formatter);
    }

    public readonly SystemTerminalCommand Remove(int startIndex, int count)
    {
        var item = Text.Remove(startIndex, count);
        var formatter = _formatter;
        return new SystemTerminalCommand(item, formatter);
    }

    public readonly TerminalCoord Next(TerminalCoord pt, int bufferWidth)
    {
        var text = Text.Replace(Environment.NewLine, $"{Environment.NewLine}{MultilinePrompt}");
        return SystemTerminalHost.NextPosition(text, bufferWidth, pt);
    }

    public string Text { get; } = text;

    public string FormattedText { get; } = formatter.Invoke(text) ?? text;

    public readonly int Length => Text.Length;

    public static implicit operator string(SystemTerminalCommand s)
    {
        return s.Text.Replace(Environment.NewLine, $"{Environment.NewLine}{MultilinePrompt}");
    }

    public static SystemTerminalCommand Empty { get; } = new SystemTerminalCommand(string.Empty, (s) => s);

    readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        if (format == "terminal")
        {
            return FormattedText.Replace(Environment.NewLine, $"{Environment.NewLine}{MultilinePrompt}");
        }

        return base.ToString() ?? string.Empty;
    }
}

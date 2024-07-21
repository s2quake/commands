// <copyright file="SystemTerminalPrompt.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

internal readonly struct SystemTerminalPrompt(string text, Func<string, string> formatter)
    : IFormattable
{
    public override readonly string ToString() => Text;

    public readonly TerminalCoord Next(TerminalCoord pt, int bufferWidth) => SystemTerminalHost.NextPosition(Text, bufferWidth, pt);

    public string Text { get; } = text;

    public string FormattedText { get; } = formatter.Invoke(text);

    public static implicit operator string(SystemTerminalPrompt s) => s.Text;

    public static SystemTerminalPrompt Empty { get; } = new SystemTerminalPrompt(string.Empty, (s) => s);

    readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        if (format == "terminal")
        {
            return FormattedText;
        }

        return base.ToString() ?? string.Empty;
    }
}

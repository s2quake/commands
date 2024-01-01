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

namespace JSSoft.Terminals;

readonly struct SystemTerminalPrompt(string text, Func<string, string> formatter)
    : IFormattable
{
    public override readonly string ToString() => Text;

    public readonly TerminalCoord Next(TerminalCoord pt, int bufferWidth) => SystemTerminalHost.NextPosition(Text, bufferWidth, pt);

    public string Text { get; } = text;

    public string FormattedText { get; } = formatter.Invoke(text);

    public static implicit operator string(SystemTerminalPrompt s) => s.Text;

    public static SystemTerminalPrompt Empty { get; } = new SystemTerminalPrompt(string.Empty, (s) => s);

    #region IFormattable

    readonly string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        if (format == "terminal")
            return FormattedText;
        return base.ToString() ?? string.Empty;
    }

    #endregion
}

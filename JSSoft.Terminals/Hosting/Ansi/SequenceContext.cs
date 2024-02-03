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

using System.Text;

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class SequenceContext(string option, AsciiCodeContext asciiCodeContext)
{
    private readonly AsciiCodeContext _asciiCodeContext = asciiCodeContext;

    public string Option { get; } = option;

    public int?[] Options { get; } = option.Split(';').Select(Parse).ToArray();

    public ITerminalFont Font => _asciiCodeContext.Font;

    public TerminalIndex Index
    {
        get => _asciiCodeContext.Index;
        set => _asciiCodeContext.Index = value;
    }

    public TerminalDisplayInfo DisplayInfo
    {
        get => _asciiCodeContext.DisplayInfo;
        set => _asciiCodeContext.DisplayInfo = value;
    }

    public TerminalCoord OriginCoordinate
    {
        get => _asciiCodeContext.OriginCoordinate;
        set => _asciiCodeContext.OriginCoordinate = value;
    }

    public TerminalCoord ViewCoordinate
    {
        get => _asciiCodeContext.ViewCoordinate;
        set => _asciiCodeContext.ViewCoordinate = value;
    }

    public TerminalIndex BeginIndex
    {
        get => _asciiCodeContext.BeginIndex;
        set => _asciiCodeContext.BeginIndex = value;
    }

    public TerminalRect View => _asciiCodeContext.View;

    public int? GetOptionValue(int index)
    {
        return index < Options.Length ? Options[index] : null;
    }

    private static int? Parse(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (var item in s)
        {
            if (char.IsDigit(item) == true)
            {
                sb.Append(item);
            }
        }
        return int.TryParse(sb.ToString(), out var i) == true ? i : null;
    }
}
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

sealed class SequenceContext(string parameter, AsciiCodeContext asciiCodeContext)
{
    private readonly AsciiCodeContext _asciiCodeContext = asciiCodeContext;

    public string Title
    {
        get => _asciiCodeContext.Title;
        set => _asciiCodeContext.Title = value;
    }

    public TerminalLineCollection Lines => _asciiCodeContext.Lines;

    public string Parameter { get; } = parameter;

    public string[] Parameters { get; } = parameter == string.Empty ? [] : parameter.Split(';').ToArray();

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

    public ITerminalModes Modes => _asciiCodeContext.Modes;

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

    public static int Parse(string s)
    {
        var sb = new StringBuilder(s.Length);
        foreach (var item in s)
        {
            if (char.IsDigit(item) == true)
            {
                sb.Append(item);
            }
        }
        return int.TryParse(sb.ToString(), out var i) == true ? i : 0;
    }

    public TerminalCoord GetCoordinate(TerminalLineCollection lines, TerminalIndex index)
        => _asciiCodeContext.GetCoordinate(lines, index);

    public int GetParameterAsInteger(int index)
        => Parse(Parameters[index]);

    public int GetParameterAsInteger(int index, int defaultValue)
        => index < Parameters.Length ? Parse(Parameters[index]) : defaultValue;

    public string GetParameterAsString(int index)
        => Parameters[index];

    public void SendSequence(string sequence)
        => _asciiCodeContext.SendSequence(sequence);
}
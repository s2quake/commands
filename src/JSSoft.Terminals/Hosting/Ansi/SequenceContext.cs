// <copyright file="SequenceContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;

namespace JSSoft.Terminals.Hosting.Ansi;

internal sealed class SequenceContext(string parameter, AsciiCodeContext asciiCodeContext)
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
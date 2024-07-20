// <copyright file="CommandTextWriterExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Extensions;

internal static class CommandTextWriterExtensions
{
    public static void WriteLineIf(this CommandTextWriter @this, bool condition)
    {
        if (condition == true)
        {
            @this.WriteLine();
        }
    }

    public static void WriteLineIf(
        this CommandTextWriter @this, string value, Func<string, bool> predicate)
    {
        if (predicate(value) == true)
        {
            @this.WriteLine(value);
        }
    }

    public static void WriteLinesIf(this CommandTextWriter @this, int count, bool condition)
    {
        if (condition == true)
        {
            for (var i = 0; i < count; i++)
            {
                @this.WriteLine();
            }
        }
    }

    public static void WriteIf(
        this CommandTextWriter @this, string value, Func<string, bool> predicate)
    {
        if (predicate(value) == true)
        {
            @this.Write(value);
        }
    }

    public static void WriteIndentLine(this CommandTextWriter @this, string value)
    {
        WriteLineIndent(@this, value, @this.Indent);
    }

    public static void WriteLineIndent(this CommandTextWriter @this, string value, int indent)
    {
        using var indentScope = @this.IndentScope(indent);
        var width = @this.Width - @this.TotalIndentSpaces;
        var lines = CommandTextWriter.Wrap(value, width);
        @this.WriteLine(lines);
    }
}

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

namespace JSSoft.Commands.Extensions;

static class CommandTextWriterExtensions
{
    public static void WriteLineIf(this CommandTextWriter @this, bool condition)
    {
        if (condition == true)
        {
            @this.WriteLine();
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

    public static void WriteLineIf(this CommandTextWriter @this, string value, Func<string, bool> predicate)
    {
        if (predicate(value) == true)
        {
            @this.WriteLine(value);
        }
    }

    public static void WriteIf(this CommandTextWriter @this, string value, Func<string, bool> predicate)
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
        using var _ = @this.IndentScope(indent);
        var width = @this.Width - @this.TotalIndentSpaces;
        var lines = CommandTextWriter.Wrap(value, width);
        @this.WriteLine(lines);
    }
}

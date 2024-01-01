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

namespace JSSoft.Terminals.Extensions;

public static class ITerminalSelectionExtensions
{
    public static void Select(this ITerminalSelection @this, TerminalSelection selection)
    {
        @this.Clear();
        @this.Add(selection);
    }

    public static bool TrySelect(this ITerminalSelection @this, TerminalSelection selection)
    {
        if (selection != TerminalSelection.Empty)
        {
            @this.Clear();
            @this.Add(selection);
            return true;
        }
        return false;
    }

    public static bool TryClear(this ITerminalSelection @this)
    {
        if (@this.Count > 0)
        {
            @this.Clear();
            return true;
        }
        return false;
    }

    public static bool IsSelected(this ITerminalSelection @this, TerminalCoord coord)
    {
        if (@this.Any())
        {
            foreach (var item in @this)
            {
                var c1 = item.BeginCoord;
                var c2 = item.EndCoord;
                if (coord >= c1 && coord < c2)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

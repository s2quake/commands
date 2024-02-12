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

public static class ITerminalExtensions
{
    public static void MoveToFirst(this ITerminal @this)
    {
        @this.ProcessText("\x01");
        @this.BringIntoView();
    }

    public static void MoveToLast(this ITerminal @this)
    {
        @this.ProcessText("\x1b[F");
        @this.BringIntoView();
    }

    public static void MoveLeft(this ITerminal @this)
    {
        @this.ProcessText($"\x1b[D");
        @this.BringIntoView();
    }

    public static void MoveRight(this ITerminal @this)
    {
        @this.ProcessText($"\x1b[C");
        @this.BringIntoView();
    }

    public static void NextHistory(this ITerminal @this)
    {
        @this.ProcessText($"\x1b[B");
        @this.BringIntoView();
    }

    public static void PrevHistory(this ITerminal @this)
    {
        @this.ProcessText($"\x1b[A");
        @this.BringIntoView();
    }

    public static void Backspace(this ITerminal @this)
    {
        @this.ProcessText($"\b");
        @this.BringIntoView();
    }

    public static void Delete(this ITerminal @this)
    {
        @this.ProcessText($"\x1b[3~");
        @this.BringIntoView();
    }

    public static bool BringIntoView(this ITerminal @this)
    {
        return @this.BringIntoView(y: @this.CursorCoordinate.Y);
    }

    public static bool TrySelecting(this ITerminal @this, TerminalCoord c1, TerminalCoord c2)
    {
        var bufferWidth = @this.BufferSize.Width;
        var bufferHeight = @this.MaximumBufferHeight;
        if (c1.X >= 0 && c1.Y >= 0 && c1.X < bufferWidth && c1.Y < bufferHeight &&
            c2.X >= 0 && c2.Y >= 0 && c2.X < bufferWidth && c2.Y < bufferHeight)
        {
            @this.Selecting = TerminalSelectionUtility.GetSelection(@this, c1, c2);
            return true;
        }
        return false;
    }

    public static bool IsSelecting(this ITerminal @this, TerminalCoord coord)
    {
        if (@this.Selecting != TerminalSelection.Empty)
        {
            return @this.Selecting.Intersect(coord);
        }
        return false;
    }

    public static ITerminalRow[] GetRows(this ITerminal @this, TerminalSelection selection)
    {
        if (selection != TerminalSelection.Empty)
        {
            var y1 = selection.BeginCoord.Y;
            var y2 = selection.EndCoord.Y + 1;
            var rows = new ITerminalRow[y2 - y1];
            for (var i = y1; i >= 0 && i < y2; i++)
            {
                rows[i - y1] = @this.View[i];
            }
            return rows;
        }
        return [];
    }

    public static ITerminalRow[] GetRows(this ITerminal @this, TerminalSelection[] selections)
    {
        return [.. selections.SelectMany(item => GetRows(@this, item)).Distinct().OrderBy(item => item.Index)];
    }

    public static TerminalCharacterInfo? GetInfo(this ITerminal @this, TerminalIndex index)
    {
        if (index.Width != @this.BufferSize.Width)
            throw new ArgumentException("The width of index and the buffer width of terminal are different.", nameof(index));
        return @this.GetInfo((TerminalCoord)index);
    }

    // public static TerminalCharacterInfo GetCell(this ITerminal @this, TerminalCoord coord)
    //     => GetCell(@this, coord.X, coord.Y);

    // public static TerminalCharacterInfo GetCell(this ITerminal @this, int x, int y)
    // {
    //     if (y >= @this.Rows.Count)
    //         throw new ArgumentOutOfRangeException(nameof(y));
    //     if (x >= @this.BufferSize.Width)
    //         throw new ArgumentOutOfRangeException(nameof(x));
    //     return @this.Rows[y].Cells[x];
    // }

    public static TerminalRect GetBackgroundRect(this ITerminal @this, TerminalCoord coord, int span)
    {
        var font = @this.ActualStyle.Font;
        var width = font.Width;
        var height = font.Height;
        return new TerminalRect(coord.X * width, coord.Y * height, width * span, height);
    }

    public static TerminalPoint GetTransform(this ITerminal @this, int rowIndex)
    {
        var font = @this.ActualStyle.Font;
        // var width = font.Width;
        var height = font.Height;
        return new TerminalPoint(0, rowIndex * height);
    }

    public static void AppendLine(this ITerminal @this, string value) => @this.Append(value + Environment.NewLine);

    public static void AppendLine(this ITerminal @this) => @this.Append(Environment.NewLine);

    public static TerminalColor GetForegroundColor(this ITerminal terminal, TerminalCoord coord)
    {
        var style = terminal.ActualStyle;
        var characterInfo = terminal.GetInfo(coord);
        var displayInfo = characterInfo?.DisplayInfo ?? TerminalDisplayInfo.Empty;
        if (terminal.IsSelecting(coord) == true || terminal.Selections.IsSelected(coord) == true)
            return GetSelectionColor(style, displayInfo);
        return TerminalStyleUtility.GetColor(style, displayInfo.Foreground) ?? style.ForegroundColor;

        static TerminalColor GetSelectionColor(ITerminalStyle style, TerminalDisplayInfo displayInfo) => style.SelectionForegroundColorSource switch
        {
            TerminalColorSource.Origin => style.SelectionForegroundColor,
            TerminalColorSource.NotUsed => TerminalStyleUtility.GetColor(style, displayInfo.Foreground) ?? style.ForegroundColor,
            TerminalColorSource.Invert => TerminalStyleUtility.GetColor(style, displayInfo.Background) ?? style.BackgroundColor,
            TerminalColorSource.Complementary => (TerminalStyleUtility.GetColor(style, displayInfo.Foreground) ?? style.ForegroundColor).ToComplementary(),
            _ => throw new ArgumentOutOfRangeException(nameof(style)),
        };
    }

    public static TerminalColor GetBackgroundColor(this ITerminal terminal, TerminalCoord coord)
    {
        var style = terminal.ActualStyle;
        var characterInfo = terminal.GetInfo(coord);
        var displayInfo = characterInfo?.DisplayInfo ?? TerminalDisplayInfo.Empty;
        if (terminal.IsSelecting(coord) == true || terminal.Selections.IsSelected(coord) == true)
            return GetSelectionColor(style, displayInfo);
        return TerminalStyleUtility.GetColor(style, displayInfo.Background) ?? style.BackgroundColor;

        static TerminalColor GetSelectionColor(ITerminalStyle style, TerminalDisplayInfo displayInfo) => style.SelectionBackgroundColorSource switch
        {
            TerminalColorSource.Origin => style.SelectionBackgroundColor,
            TerminalColorSource.NotUsed => TerminalStyleUtility.GetColor(style, displayInfo.Background) ?? style.BackgroundColor,
            TerminalColorSource.Invert => TerminalStyleUtility.GetColor(style, displayInfo.Foreground) ?? style.ForegroundColor,
            TerminalColorSource.Complementary => (TerminalStyleUtility.GetColor(style, displayInfo.Background) ?? style.BackgroundColor).ToComplementary(),
            _ => throw new ArgumentOutOfRangeException(nameof(style)),
        };
    }
}

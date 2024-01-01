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

using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals;

public static class TerminalSelectionUtility
{
    public static TerminalSelection GetSelection(ITerminal terminal, TerminalCoord c1, TerminalCoord c2)
    {
        var i1 = new TerminalIndex(terminal, c1);
        var i2 = new TerminalIndex(terminal, c2);
        var (s1, s2) = i1 < i2 ? (i1, i2) : (i2, i1);
        var isEnabled1 = terminal.GetInfo(s1) is { } characterInfo1 && characterInfo1.Character != char.MinValue;
        var isEnabled2 = terminal.GetInfo(s2) is { } characterInfo2 && characterInfo2.Character != char.MinValue;
        var bufferWidth = terminal.BufferSize.Width;
        var gap = 5;
        if (isEnabled1 == true && isEnabled2 != true)
        {
            var l2 = s2.MoveBackwardToEndOfString(terminal);
            var distance = TerminalIndex.DistanceOf(l2, s2);
            gap = Math.Min(gap, bufferWidth - (l2.X + 1));
            s2 = distance > gap ? s2.MoveToEndOfLine() : l2;
        }
        else if (isEnabled2 == true && isEnabled1 != true)
        {
            var l1 = s1.MoveBackwardToEndOfString(terminal);
            var distance = TerminalIndex.DistanceOf(l1, s1);
            gap = Math.Min(gap, bufferWidth - (l1.X + 1));
            s1 = distance > gap ? s1.MoveToEndOfLine() : l1;
        }
        else if (isEnabled1 != true && isEnabled2 != true)
        {
            var isRow1Empty = IsRowEmpty(terminal, s1);
            var isRow2Empty = IsRowEmpty(terminal, s2);
            if (isRow1Empty == false)
            {
                var l1 = s1.MoveBackwardToEndOfString(terminal);
                var distance = TerminalIndex.DistanceOf(l1, s1);
                gap = Math.Min(gap, bufferWidth - (l1.X + 1));
                s1 = distance > gap ? s1.MoveToEndOfLine() : l1;
            }
            else
            {
                s1 = s1.MoveToEndOfLine();
            }

            if (isRow2Empty == false)
            {
                var l2 = s2.MoveBackwardToEndOfString(terminal);
                var distance = TerminalIndex.DistanceOf(l2, s2);
                gap = Math.Min(gap, bufferWidth - (l2.X + 1));
                s2 = distance > gap ? s2.MoveToEndOfLine() : l2;
            }
            else
            {
                s2 = s2.MoveToEndOfLine();
            }
        }
        else
        {
            s2++;
        }
        return new TerminalSelection(s1, s2);
    }

    public static void Select(ITerminal terminal, TerminalSelection selection)
    {
        if (selection != TerminalSelection.Empty)
        {
            terminal.Selections.Clear();
            terminal.Selections.Add(selection);
            terminal.Selecting = TerminalSelection.Empty;
        }
    }

    public static TerminalSelection SelectLine(ITerminal terminal, TerminalCoord coord)
    {
        var index = new TerminalIndex(terminal, coord);
        if (terminal.GetInfo(coord) is { } characterInfo)
        {
            var i1 = index.MoveToFirstOfString(terminal);
            var i2 = index.MoveForwardToEndOfString(terminal).MoveToEndOfLine();
            return new TerminalSelection(i1, i2);
        }
        else
        {
            var i1 = index.MoveToFirstOfLine();
            var i2 = index.MoveToEndOfLine();
            return new TerminalSelection(i1, i2);
        }
    }

    public static TerminalSelection SelectWord(ITerminal terminal, TerminalCoord viewCoord)
    {
        var coord = terminal.ViewToWorld(viewCoord);
        var characterInfo = terminal.GetInfo(coord);
        var x1 = terminal.GetInfo(new TerminalCoord(0, coord.Y));
        if (x1 == null && characterInfo == null)
            return SelectWordOfEmptyRow(terminal, coord);
        else if (x1 != null && characterInfo == null)
            return SelectWordOfEmptyCell(terminal, coord);
        else
            return SelectWordOfCell(terminal, coord);
    }

    private static bool IsRowEmpty(ITerminal terminal, TerminalIndex index)
    {
        var i1 = index.MoveToFirstOfLine();
        var i2 = index.MoveToEndOfLine() - 1;
        var c1 = terminal.GetInfo(i1);
        var c2 = terminal.GetInfo(i2);
        return c1 is null && c2 is null;
    }

    private static TerminalSelection SelectWordOfEmptyRow(ITerminal terminal, TerminalCoord coord)
    {
        var index = new TerminalIndex(terminal, coord);
        var i1 = index.MoveToFirstOfLine();
        var i2 = index.MoveToEndOfLine();
        return new TerminalSelection(i1, i2);
    }

    private static TerminalSelection SelectWordOfEmptyCell(ITerminal terminal, TerminalCoord coord)
    {
        var index = new TerminalIndex(terminal, coord);
        var c1 = index.MoveBackward(terminal, (index, info) =>
        {
            if (info is { } d)
                return d.Character == char.MinValue;
            if (info is null)
                return true;
            return index.X != 0;
        }) + 1;
        var i2 = index.MoveToEndOfLine();
        return new TerminalSelection(c1, i2);
    }

    private static TerminalSelection SelectWordOfCell(ITerminal terminal, TerminalCoord coord)
    {
        var index = new TerminalIndex(terminal, coord);
        var characterInfo = (TerminalCharacterInfo)terminal.GetInfo(index)!;
        // var x1 = index - characterInfo.Offset;
        var group = characterInfo.Group;
        var predicate = GetPredicate(characterInfo);
        var i1 = index.MoveBackward(terminal, (index, characterInfo) =>
        {
            if (characterInfo is { } d && d.Group == group)
                return predicate(d);
            if (characterInfo is null)
                return false;
            return false;
        }) + 1;
        var i2 = index.MoveForward(terminal, (index, characterInfo) =>
        {
            if (characterInfo is { } d)
                return predicate(d);
            if (characterInfo is null)
                return false;
            return false;
        });
        return new TerminalSelection(i1, i2);

        static Func<TerminalCharacterInfo, bool> GetPredicate(TerminalCharacterInfo characterInfo)
        {
            if (char.IsLetterOrDigit(characterInfo.Character) == true)
                return item => char.IsLetterOrDigit(item.Character) == true;
            else if (char.IsWhiteSpace(characterInfo.Character) == true)
                return item => char.IsWhiteSpace(item.Character) == true;
            else
                return item => char.IsLetterOrDigit(item.Character) == false && char.IsWhiteSpace(item.Character) == false;
        }
    }
}

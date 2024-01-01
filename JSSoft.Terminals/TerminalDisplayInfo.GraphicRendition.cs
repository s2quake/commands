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

// https://learn.microsoft.com/en-us/windows/console/console-virtual-terminal-sequences
partial struct TerminalDisplayInfo
{
    private delegate void GraphicRenditionSetter(ref TerminalDisplayInfo o);

    private static readonly IReadOnlyDictionary<int, GraphicRenditionSetter> actionByCode = new Dictionary<int, GraphicRenditionSetter>
    {
        { 0, (ref TerminalDisplayInfo o) => o.Reset() },
        { 1, (ref TerminalDisplayInfo o) => o.IsBold = true },
        { 22, (ref TerminalDisplayInfo o) => o.IsBold = false },
        { 4, (ref TerminalDisplayInfo o) => o.IsUnderline = true },
        { 24, (ref TerminalDisplayInfo o) => o.IsUnderline = false },
        { 7, (ref TerminalDisplayInfo o) => o.IsNegative = true },
        { 27, (ref TerminalDisplayInfo o) => o.IsNegative = false },
        { 30, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Black },
        { 31, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Red },
        { 32, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Green },
        { 33, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Yellow },
        { 34, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Blue },
        { 35, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Magenta },
        { 36, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.Cyan },
        { 37, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.White },
        // { 38, (ref TerminalDisplayInfo o) => throw new UnreachableException },
        { 39, (ref TerminalDisplayInfo o) => o.Foreground = null },
        { 40, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Black },
        { 41, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Red },
        { 42, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Green },
        { 43, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Yellow },
        { 44, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Blue },
        { 45, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Magenta },
        { 46, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.Cyan },
        { 47, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.White },
        // { 48, (ref TerminalDisplayInfo o) => throw new UnreachableException },
        { 49, (ref TerminalDisplayInfo o) => o.Background = null },
        { 90, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightBlack },
        { 91, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightRed },
        { 92, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightGreen },
        { 93, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightYellow },
        { 94, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightBlue },
        { 95, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightMagenta },
        { 96, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightCyan },
        { 97, (ref TerminalDisplayInfo o) => o.Foreground = TerminalColorType.BrightWhite },
        { 100, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightBlack },
        { 101, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightRed },
        { 102, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightGreen },
        { 103, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightYellow },
        { 104, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightBlue },
        { 105, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightMagenta },
        { 106, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightCyan },
        { 107, (ref TerminalDisplayInfo o) => o.Background = TerminalColorType.BrightWhite },
    };

    public void Reset()
    {
        Foreground = null;
        Background = null;
        IsBold = false;
        IsUnderline = false;
        IsNegative = false;
    }

    public void SetGraphicRendition(int code)
    {
        if (actionByCode.TryGetValue(code, out var action) == true)
        {
            action.Invoke(ref this);
        }
    }

    public void SetGraphicRendition(int[] codes)
    {
        foreach (var item in codes)
        {
            SetGraphicRendition(item);
        }
    }
}

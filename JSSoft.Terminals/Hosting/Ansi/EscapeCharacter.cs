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

using JSSoft.Terminals.Hosting.Ansi.CSI;
using JSSoft.Terminals.Hosting.Ansi.CursorControls;
using JSSoft.Terminals.Hosting.Ansi.EraseFunctions;

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class EscapeCharacter : IAsciiCode
{
    // Control Sequence Introducer
    private static readonly Dictionary<char, IEscapeSequence> CSISequenceByCharacter = new()
    {
        { 'H', new CursorPosition() },
        { 'f', new CursorPosition() },
        { 'A', new CursorUp() },
        { 'B', new CursorDown() },
        { 'C', new CursorRight() },
        { 'D', new CursorLeft() },
        { 'E', new CursorToBeginningOfNextLine() },
        { 'F', new CursorToBeginningOfPreviousLine() },
        { 'G', new CursorToColumn() },
        { 'm', new GraphicsMode() },
        { 's', new SaveCursorPosition() },
        { 'u', new RestoreCursorPosition() },

        { 'n', new ReportCursorPosition() },
        { 'R', new ReportCursorPosition() },
        { 'x', new ReportCursorPosition() },
        
        { 'J', new EraseInDisplay() },
        { 'K', new EraseInLine() },

        { 'p', new SoftReset() },
        { 'l', new ResetMode() },
        { 'g', new TabClear() },
        { 'h', new CommonPrivateModes2() },
    };

    private static readonly Dictionary<char, IEscapeSequence> ESCSequenceByCharacter = new()
    {
        { '7', new SaveCursorPosition() },
        { '8', new RestoreCursorPosition() },

        { 'c', new FullReset() },
        { 'H', new HorizontalTabSet() },
    };

    public void Process(TerminalLineCollection lines, AsciiCodeContext context)
    {
        var c = context.Text[context.TextIndex + 1];
        if (c == '[')
        {
            var s1 = context.TextIndex + 2;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (CSISequenceByCharacter.ContainsKey(character) == true)
                {
                    var escapeSequence = CSISequenceByCharacter[character];
                    var option = context.Text.Substring(s1, i - s1);
                    var escapeSequenceContext = new EscapeSequenceContext(option, context);
                    escapeSequence.Process(lines, escapeSequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
            }
        }
        else
        {
            var s1 = context.TextIndex + 1;
            for (var i = s1; i < context.Text.Length; i++)
            {
                var character = context.Text[i];
                if (ESCSequenceByCharacter.ContainsKey(character) == true)
                {
                    var escapeSequence = ESCSequenceByCharacter[character];
                    var option = context.Text.Substring(s1, i - s1);
                    var escapeSequenceContext = new EscapeSequenceContext(option, context);
                    escapeSequence.Process(lines, escapeSequenceContext);
                    context.TextIndex = i + 1;
                    return;
                }
            }
        }
        context.TextIndex = context.Text.Length;
    }
}

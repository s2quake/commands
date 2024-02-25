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

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

sealed class InsertCharacter : CSISequenceBase
{
    private const char SpaceCharacter = ' ';

    public InsertCharacter()
        : base('@')
    {
    }

    public override string DisplayName => "CSI Ps @\nCSI Ps SP @";

    protected override void OnProcess(TerminalLineCollection lines, SequenceContext context)
    {
        var index1 = context.Index;
        var beginIndex = context.BeginIndex;
        var font = context.Font;
        var span = TerminalFontUtility.GetGlyphSpan(font, SpaceCharacter);
        var characterInfo = new TerminalCharacterInfo
        {
            Character = SpaceCharacter,
            DisplayInfo = context.DisplayInfo,
            Span = span,
        };
        var index2 = index1.Expect(span);
        var line = lines.Prepare(beginIndex, index2);
        line.InsertCharacter(index2.X, characterInfo);
    }
}

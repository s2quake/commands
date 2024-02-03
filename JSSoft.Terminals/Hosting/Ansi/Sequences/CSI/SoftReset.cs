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

using JSSoft.Terminals.Hosting.Ansi.Modes;

namespace JSSoft.Terminals.Hosting.Ansi.Sequences.CSI;

// ESC [ ? Ⓝ $ p
//   	Request Mode (?) (DECRQM)
//      https://terminalguide.namepad.de/seq/csi_sp__p_t_dollar/
// ESC [ > Ⓝ p
//   	Change Mouse Pointer Auto Hide
//      https://terminalguide.namepad.de/seq/csi_sp__q/
// ESC [ ! p
//   	Soft Reset (DECSTR)	todo
//      https://terminalguide.namepad.de/seq/csi_sp_t_bang/
// ESC [ Ⓝ $ p
//   	Request Mode (RQM)
//      https://terminalguide.namepad.de/seq/csi_sp_t_dollar/
// ESC [ [ Ⓝ ] # p
//   	Alias: Save Rendition Attributes
//      https://terminalguide.namepad.de/seq/csi_sp_t_hash/
// ESC [ Ⓝ + p
//   	??? DECSR
//      https://terminalguide.namepad.de/seq/csi_sp_t_plus/
// ESC [ Ⓝ ; Ⓝ " p
//   	Select VT-XXX Conformance Level (DECSCL)
//      https://terminalguide.namepad.de/seq/csi_sp_t_quote/
sealed class SoftReset : CSISequenceBase
{
    public SoftReset()
        : base('p')
    {
    }

    protected override void OnProcess(TerminalLineCollection lines, SequenceContext context)
    {
        GetAction().Invoke(lines, context);

        Action<TerminalLineCollection, SequenceContext> GetAction() => context.Option switch
        {
            "!" => OnSoftReset,
            _ => (l, c) => { }
            ,
        };
    }

    private void OnSoftReset(TerminalLineCollection lines, SequenceContext context)
    {
    }
}

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

namespace JSSoft.Terminals.Tests;

public class TerminalModes_Test
{
    [Fact]
    public void Set_Test()
    {
        var terminal = new TestTerminal();
        var mode = RandomUtility.NextEnum<TerminalMode>();

        terminal.Modes[mode] = true;
        Assert.True(terminal.Modes[mode]);

        terminal.Modes[mode] = false;
        Assert.False(terminal.Modes[mode]);
    }

    [Fact]
    public void SetUnspecified_FailTest()
    {
        var terminal = new TestTerminal();
        var mode = RandomUtility.NextUnspecifiedEnum<TerminalMode>();

        Assert.Throws<KeyNotFoundException>(() => terminal.Modes[mode] = true);
    }
}

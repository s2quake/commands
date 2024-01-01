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

using System.Text.RegularExpressions;

namespace JSSoft.Commands.Test;

public class RunScriptTest
{
    private readonly CommandParser _parser;

    public RunScriptTest()
    {
        _parser = new CommandParser("run", this);
    }

    [Fact]
    public void TestMethod1()
    {
        _parser.ParseCommandLine("run --filename \"C:\\script.js\"");
        Assert.Equal("C:\\script.js", Filename);
        Assert.Empty(Script);
        Assert.False(List);
        Assert.Empty(Arguments);
    }

    [Fact]
    public void TestMethod2()
    {
        _parser.ParseCommandLine("run log(1);");
        Assert.Equal("log(1);", Script);
        Assert.Equal(Filename, string.Empty);
        Assert.False(List);
        Assert.NotNull(Arguments);
        Assert.Empty(Arguments);
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("run --list");
        Assert.True(List);
        Assert.Equal(Script, string.Empty);
        Assert.Equal(Filename, string.Empty);
        Assert.NotNull(Arguments);
        Assert.Empty(Arguments);
    }

    [Fact]
    public void TestMethod4()
    {
        _parser.ParseCommandLine("run -l");
        Assert.Equal(Script, string.Empty);
        Assert.Equal(Filename, string.Empty);
        Assert.NotNull(Arguments);
        Assert.Empty(Arguments);
    }

    [Fact]
    public void TestMethod4_With_Args()
    {
        _parser.ParseCommandLine("run -l -- db=string port=number async=boolean");
        Assert.True(List);
        Assert.Equal(Script, string.Empty);
        Assert.Equal(Filename, string.Empty);
        Assert.Equal(3, Arguments.Length);
        foreach (var item in Arguments)
        {
            Assert.Matches(".+=.+", item);
        }
    }

    [Fact]
    public void TestMethod5()
    {
        _parser.ParseCommandLine("run log(1); arg1=1 arg2=text");
        Assert.Equal("log(1);", Script);
        Assert.Equal(2, Arguments.Length);
        foreach (var item in Arguments)
        {
            Assert.Matches(".+=.+", item);
        }
    }

    [CommandPropertyRequired(DefaultValue = "")]
    [CommandPropertyCondition(nameof(Filename), "")]
    [CommandPropertyCondition(nameof(List), false)]
    public string Script { get; set; } = string.Empty;

    [CommandProperty]
    [CommandPropertyCondition(nameof(Script), "")]
    [CommandPropertyCondition(nameof(List), false)]
    public string Filename { get; set; } = string.Empty;

    [CommandPropertySwitch("list", 'l')]
    [CommandPropertyCondition(nameof(Script), "")]
    [CommandPropertyCondition(nameof(Filename), "")]
    public bool List { get; set; }

    [CommandPropertyArray]
    public string[] Arguments { get; set; } = [];
}

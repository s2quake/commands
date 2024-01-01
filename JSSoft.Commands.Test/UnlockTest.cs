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

using System;

namespace JSSoft.Commands.Test;

public class UnunlockTest
{
    private readonly CommandParser _parser;

    public UnunlockTest()
    {
        _parser = new TestCommandParser("unlock", this);
    }

    [Fact]
    public void TestMethod1()
    {
        _parser.ParseCommandLine("unlock");
        Assert.Equal("", Path);
    }

    [Fact]
    public void TestMethod2()
    {
        Assert.Throws<CommandLineException>(() => _parser.ParseCommandLine("unlock -m"));
    }

    [Fact]
    public void TestMethod3()
    {
        _parser.ParseCommandLine("unlock current_path");
        Assert.Equal("current_path", Path);
    }

    [CommandPropertyRequired(DefaultValue = "")]
    public string Path { get; set; } = string.Empty;

    #region TestCommandParser

    sealed class TestCommandParser(string name, object instance)
        : CommandParser(name, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }

    #endregion
}

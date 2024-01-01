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

namespace JSSoft.Commands.Test;

public class CommandPropertyArrayTest
{
    [Fact]
    public void Test1()
    {
        var parser = new CommandParser(this);
        parser.Parse("get database=a port=123 userid=abc password=1234 comment=\"connect database to \\\"a\\\"\"");
    }

    [Fact]
    public void Test2()
    {
        var parser = new CommandParser(this);
        parser.Parse("get \"database=a b c\"");

        Assert.Single(Arguments);
        Assert.Equal("database=a b c", Arguments[0]);
    }

    [Fact]
    public void Test3()
    {
        var parser = new CommandParser(this);
        parser.Parse("get \"database=\\\"a b c\\\"\"");

        Assert.Single(Arguments);
        Assert.Equal("database=\"a b c\"", Arguments[0]);
    }

    // [Fact]
    // public void ValueIncludedEqualsTest()
    // {
    //     var parser = new CommandParser(this);
    //     Assert.Throws<CommandLineException>(() => parser.Parse("--value=0"));
    // }

    [Fact]
    public void ValueIncludedEqualsTest2()
    {
        var parser = new CommandParser(this);
        parser.Parse("get value=0");
        Assert.Equal("get", Command);
        Assert.Equal("value=0", Arguments[0]);
    }

    [CommandPropertyRequired]
    public string Command { get; set; } = string.Empty;

    [CommandPropertyArray]
    public string[] Arguments { get; set; } = [];
}

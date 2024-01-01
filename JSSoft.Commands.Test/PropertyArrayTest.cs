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
using System.Linq;

namespace JSSoft.Commands.Test;

public class PropertyArrayTest
{
    [Fact]
    public void TestMethod1()
    {
        var instance = new Instance1();
        var parser = new TestCommandParser("array", instance);
        parser.ParseCommandLine("array");
        Assert.NotNull(instance.Arguments);
        Assert.Empty(instance.Arguments);
    }

    class Instance1
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void TestMethod2()
    {
        var instance = new Instance2();
        var parser = new TestCommandParser("array", instance);
        parser.ParseCommandLine("array");
        Assert.NotNull(instance.Arguments);
        Assert.Empty(instance.Arguments);
        Assert.True(Array.Empty<string>().SequenceEqual(instance.Arguments));
    }

    class Instance2
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    [Fact]
    public void TestMethod3()
    {
        var instance = new Instance3();
        var parser = new TestCommandParser("array", instance);
        Assert.Throws<CommandDefinitionException>(() => parser.ParseCommandLine("array"));
    }

    class Instance3
    {
        [CommandPropertyArray]
        public string[] Arguments1 { get; set; } = [];

        [CommandPropertyArray]
        public string[] Arguments2 { get; set; } = [];
    }

    [Fact]
    public void TestMethod4()
    {
        var instance = new Instance4();
        var parser = new TestCommandParser("array", instance);
        parser.ParseCommandLine("array");
        Assert.NotNull(instance.Arguments);
        Assert.Empty(instance.Arguments);
        Assert.Equivalent(Array.Empty<string>(), instance.Arguments);
    }

    class Instance4
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

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

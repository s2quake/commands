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

using JSSoft.Commands.Extensions;

namespace JSSoft.Commands.Test;

public class UnitTest1
{
    [Fact]
    public void TestMethod1()
    {
        var settings = new Settings();
        var parser = new CommandParser(settings);
        parser.Parse("--list -c");

        Assert.Equal("", settings.List);
        Assert.True(settings.IsCancel);
        Assert.Equal(5005, settings.Port);
    }

    [Fact]
    public void TestMethod2()
    {
        var settings = new Settings();
        var parser = new CommandParser(settings);
        parser.Parse("--list wer -c");

        Assert.Equal("wer", settings.List);
        Assert.True(settings.IsCancel);
        Assert.Equal(5005, settings.Port);
    }

    [Fact]
    public void TestMethod3()
    {
        var settings = new Settings();
        var parser = new CommandParser(settings);
        parser.Parse("--list \"a \\\"b\\\" c\" -c");

        Assert.Equal("a \"b\" c", settings.List);
        Assert.True(settings.IsCancel);
        Assert.Equal(5005, settings.Port);
    }

    [Fact]
    public void TestMethod4()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("test a -m wow");
    }

    [Fact]
    public void TestMethod5()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("push-many a b");
    }

    [Fact]
    public void TestMethod6()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("items");
        Assert.False(commands.IsReverseResult);
    }

    [Fact]
    public void TestMethod7()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("items -r");
        Assert.True(commands.IsReverseResult);
    }

    [Fact]
    public void TestMethod8()
    {
        var commands = new Commands();
        var invoker = new CommandInvoker(commands);
        invoker.Invoke("items --reverse");
        Assert.True(commands.IsReverseResult);
    }

    class Settings
    {
        [CommandProperty(DefaultValue = "")]
        public string List { get; set; } = string.Empty;

        [CommandPropertySwitch('c')]
        public bool IsCancel { get; set; }

        [CommandProperty(InitValue = 5005)]
        public int Port { get; set; }
    }

    class Commands
    {
        [CommandMethod]
        [CommandMethodProperty(nameof(Message))]
        public void Test(string target1, string target2 = "")
        {
            Assert.Equal("a", target1);
            Assert.Empty(target2);
            Assert.Equal("wow", Message);
        }

        [CommandMethod]
        public void PushMany(params string[] items)
        {
            Assert.Equal("a", items[0]);
            Assert.Equal("b", items[1]);
        }

        [CommandMethod("items")]
        [CommandMethodProperty(nameof(IsReverse))]
        public void ShowItems()
        {
            IsReverseResult = IsReverse;
        }

        public bool IsReverseResult { get; set; }

        [CommandPropertySwitch("reverse", 'r')]
        public bool IsReverse { get; set; }

        [CommandPropertyExplicitRequired('m')]
        public string Message { get; set; } = string.Empty;
    }
}

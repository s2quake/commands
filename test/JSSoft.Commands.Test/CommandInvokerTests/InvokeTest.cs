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

#pragma warning disable IDE0049
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands.Test.CommandInvokerTests;

public class InvokeTest
{
    sealed class MethodEventArgs(string methodName) : EventArgs
    {
        public string MethodName { get; } = methodName;
    }

    sealed class InstanceClass
    {
        [CommandMethod]
        public void Test0() => Invoke(nameof(Test0));

        [CommandMethod]
        public void Boolean(Boolean boolean) => Invoke(nameof(Boolean));

        [CommandMethod]
        public void String(String @string) => Invoke(nameof(String));

        [CommandMethod]
        public void SByte(SByte sByte) => Invoke(nameof(SByte));

        [CommandMethod]
        public void Byte(Byte @byte) => Invoke(nameof(Byte));

        [CommandMethod]
        public void Int16(Int16 int16) => Invoke(nameof(Int16));

        [CommandMethod]
        public void UInt16(UInt16 uint16) => Invoke(nameof(UInt16));

        [CommandMethod]
        public void Int32(Int32 int32) => Invoke(nameof(Int32));

        [CommandMethod]
        public void UInt32(UInt32 uint32) => Invoke(nameof(UInt32));

        [CommandMethod]
        public void Int64(Int64 int64) => Invoke(nameof(Int64));

        [CommandMethod]
        public void UInt64(UInt64 uint64) => Invoke(nameof(UInt64));

        [CommandMethod]
        public void Single(Single single) => Invoke(nameof(Single));

        [CommandMethod]
        public void Double(Double @double) => Invoke(nameof(Double));

        [CommandMethod]
        public void Decimal(Decimal @decimal) => Invoke(nameof(Decimal));

        public event EventHandler<MethodEventArgs>? Invoked;

        private void Invoke(string methodName) => Invoked?.Invoke(this, new MethodEventArgs(methodName));
    }

    [Fact]
    public void Invoke_Test0()
    {
        var obj = new InstanceClass();
        var invoker = new CommandInvoker(obj);
        var raised = Assert.Raises<MethodEventArgs>(
            a => obj.Invoked += a,
            a => obj.Invoked -= a,
            () => invoker.Invoke("test0"));
        Assert.NotNull(raised);
        Assert.Equal(nameof(InstanceClass.Test0), raised.Arguments.MethodName);
    }

    public static IEnumerable<object[]> TestData => new object[][]
    {
        [nameof(InstanceClass.Boolean), RandomUtility.NextBoolean()],
        [nameof(InstanceClass.String), RandomUtility.NextWord()],
        [nameof(InstanceClass.SByte), RandomUtility.NextSByte()],
        [nameof(InstanceClass.Byte), RandomUtility.NextByte()],
        [nameof(InstanceClass.Int16), RandomUtility.NextInt16()],
        [nameof(InstanceClass.UInt16), RandomUtility.NextUInt16()],
        [nameof(InstanceClass.Int32), RandomUtility.NextInt32()],
        [nameof(InstanceClass.UInt32), RandomUtility.NextUInt32()],
        [nameof(InstanceClass.Int64), RandomUtility.NextInt64()],
        [nameof(InstanceClass.UInt64), RandomUtility.NextUInt64()],
        [nameof(InstanceClass.Single), RandomUtility.NextSingle()],
        [nameof(InstanceClass.Double), RandomUtility.NextDouble()],
        [nameof(InstanceClass.Decimal), RandomUtility.NextDecimal()],
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void Invoke_Test1(string methodName, object value)
    {
        var obj = new InstanceClass();
        var invoker = new CommandInvoker(obj);
        var m = CommandUtility.ToSpinalCase(methodName);
        var raised = Assert.Raises<MethodEventArgs>(
            a => obj.Invoked += a,
            a => obj.Invoked -= a,
            () => invoker.Invoke($"{m} {value:R}"));
        Assert.NotNull(raised);
        Assert.Equal(methodName, raised.Arguments.MethodName);
    }
}

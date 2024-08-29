// <copyright file="InvokeTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable IDE0049
namespace JSSoft.Commands.Tests.CommandInvokerTests;

public class InvokeTest
{
    private sealed class MethodEventArgs(string methodName) : EventArgs
    {
        public string MethodName { get; } = methodName;
    }

    private sealed class InstanceClass
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

        private void Invoke(string methodName)
            => Invoked?.Invoke(this, new MethodEventArgs(methodName));
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

    public static IEnumerable<object[]> TestData =>
    [
        [nameof(InstanceClass.Boolean), RandomUtility.Boolean()],
        [nameof(InstanceClass.String), RandomUtility.Word()],
        [nameof(InstanceClass.SByte), RandomUtility.SByte()],
        [nameof(InstanceClass.Byte), RandomUtility.Byte()],
        [nameof(InstanceClass.Int16), RandomUtility.Int16()],
        [nameof(InstanceClass.UInt16), RandomUtility.UInt16()],
        [nameof(InstanceClass.Int32), RandomUtility.Int32()],
        [nameof(InstanceClass.UInt32), RandomUtility.UInt32()],
        [nameof(InstanceClass.Int64), RandomUtility.Int64()],
        [nameof(InstanceClass.UInt64), RandomUtility.UInt64()],
        [nameof(InstanceClass.Single), RandomUtility.Single()],
        [nameof(InstanceClass.Double), RandomUtility.Double()],
        [nameof(InstanceClass.Decimal), RandomUtility.Decimal()],
    ];

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

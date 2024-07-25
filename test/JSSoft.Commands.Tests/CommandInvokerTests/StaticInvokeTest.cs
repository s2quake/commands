// <copyright file="StaticInvokeTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable IDE0049
namespace JSSoft.Commands.Tests.CommandInvokerTests;

public class StaticInvokeTest
{
    private sealed class MethodEventArgs(string methodName) : EventArgs
    {
        public string MethodName { get; } = methodName;
    }

    private static class InstanceClass
    {
        [CommandMethod]
        public static void Test0() => Invoke(nameof(Test0));

        [CommandMethod]
        public static void Boolean(Boolean boolean) => Invoke(nameof(Boolean));

        [CommandMethod]
        public static void String(String @string) => Invoke(nameof(String));

        [CommandMethod]
        public static void SByte(SByte sByte) => Invoke(nameof(SByte));

        [CommandMethod]
        public static void Byte(Byte @byte) => Invoke(nameof(Byte));

        [CommandMethod]
        public static void Int16(Int16 int16) => Invoke(nameof(Int16));

        [CommandMethod]
        public static void UInt16(UInt16 uint16) => Invoke(nameof(UInt16));

        [CommandMethod]
        public static void Int32(Int32 int32) => Invoke(nameof(Int32));

        [CommandMethod]
        public static void UInt32(UInt32 uint32) => Invoke(nameof(UInt32));

        [CommandMethod]
        public static void Int64(Int64 int64) => Invoke(nameof(Int64));

        [CommandMethod]
        public static void UInt64(UInt64 uint64) => Invoke(nameof(UInt64));

        [CommandMethod]
        public static void Single(Single single) => Invoke(nameof(Single));

        [CommandMethod]
        public static void Double(Double @double) => Invoke(nameof(Double));

        [CommandMethod]
        public static void Decimal(Decimal @decimal) => Invoke(nameof(Decimal));

        public static event EventHandler<MethodEventArgs>? Invoked;

        private static void Invoke(string methodName)
            => Invoked?.Invoke(null, new MethodEventArgs(methodName));
    }

    [Fact]
    public void Invoke_Test0()
    {
        var invoker = new CommandInvoker(typeof(InstanceClass));
        var raised = Assert.Raises<MethodEventArgs>(
            a => InstanceClass.Invoked += a,
            a => InstanceClass.Invoked -= a,
            () => invoker.Invoke("test0"));
        Assert.NotNull(raised);
        Assert.Equal(nameof(InstanceClass.Test0), raised.Arguments.MethodName);
    }

    public static IEnumerable<object[]> TestData => new object[][]
    {
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
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void Invoke_Test1(string methodName, object value)
    {
        var invoker = new CommandInvoker(typeof(InstanceClass));
        var m = CommandUtility.ToSpinalCase(methodName);
        var raised = Assert.Raises<MethodEventArgs>(
            a => InstanceClass.Invoked += a,
            a => InstanceClass.Invoked -= a,
            () => invoker.Invoke($"{m} {value:R}"));
        Assert.NotNull(raised);
        Assert.Equal(methodName, raised.Arguments.MethodName);
    }
}

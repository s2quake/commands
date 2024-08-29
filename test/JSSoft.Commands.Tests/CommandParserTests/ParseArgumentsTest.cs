// <copyright file="ParseArgumentsTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandParserTests;

public class ParseArgumentsTest()
{
    private class BaseClass
    {
        [CommandProperty]
        public string Text { get; set; } = string.Empty;
    }

    private sealed class BooleanClass : BaseClass
    {
        [CommandPropertyArray]
        public bool[] Arguments { get; set; } = [];
    }

    private sealed class StringClass : BaseClass
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    private sealed class SByteClass : BaseClass
    {
        [CommandPropertyArray]
        public sbyte[] Arguments { get; set; } = [];
    }

    private sealed class ByteClass : BaseClass
    {
        [CommandPropertyArray]
        public byte[] Arguments { get; set; } = [];
    }

    private sealed class Int16Class : BaseClass
    {
        [CommandPropertyArray]
        public short[] Arguments { get; set; } = [];
    }

    private sealed class UInt16Class : BaseClass
    {
        [CommandPropertyArray]
        public ushort[] Arguments { get; set; } = [];
    }

    private sealed class Int32Class : BaseClass
    {
        [CommandPropertyArray]
        public int[] Arguments { get; set; } = [];
    }

    private sealed class UInt32Class : BaseClass
    {
        [CommandPropertyArray]
        public uint[] Arguments { get; set; } = [];
    }

    private sealed class Int64Class : BaseClass
    {
        [CommandPropertyArray]
        public long[] Arguments { get; set; } = [];
    }

    private sealed class UInt64Class : BaseClass
    {
        [CommandPropertyArray]
        public ulong[] Arguments { get; set; } = [];
    }

    private sealed class SingleClass : BaseClass
    {
        [CommandPropertyArray]
        public float[] Arguments { get; set; } = [];
    }

    private sealed class DoubleClass : BaseClass
    {
        [CommandPropertyArray]
        public double[] Arguments { get; set; } = [];
    }

    private sealed class DecimalClass : BaseClass
    {
        [CommandPropertyArray]
        public decimal[] Arguments { get; set; } = [];
    }

    public static IEnumerable<object[]> TestData =>
    [
        [typeof(BooleanClass), RandomUtility.Array(RandomUtility.Boolean, 1, 10)],
        [typeof(StringClass), RandomUtility.Array(RandomUtility.Word, 1, 10)],
        [typeof(SByteClass), RandomUtility.Array(RandomUtility.SByte, 1, 10)],
        [typeof(ByteClass), RandomUtility.Array(RandomUtility.Byte, 1, 10)],
        [typeof(Int16Class), RandomUtility.Array(RandomUtility.Int16, 1, 10)],
        [typeof(UInt16Class), RandomUtility.Array(RandomUtility.UInt16, 1, 10)],
        [typeof(Int32Class), RandomUtility.Array(RandomUtility.Int32, 1, 10)],
        [typeof(UInt32Class), RandomUtility.Array(RandomUtility.UInt32, 1, 10)],
        [typeof(Int64Class), RandomUtility.Array(RandomUtility.Int64, 1, 10)],
        [typeof(UInt64Class), RandomUtility.Array(RandomUtility.UInt64, 1, 10)],
        [typeof(SingleClass), RandomUtility.Array(RandomUtility.Single, 1, 10)],
        [typeof(DoubleClass), RandomUtility.Array(RandomUtility.Double, 1, 10)],
        [typeof(DecimalClass), RandomUtility.Array(RandomUtility.Decimal, 1, 10)],
    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_Test(Type type, Array array)
    {
        var obj = Activator.CreateInstance(type)!;
        var parser = new CommandParser(obj);
        var expression = CommandTestUtility.GetExpression(array);
        var argumentLine = $"{expression}";

        parser.Parse(argumentLine);
        var actualValue = type.GetProperty("Arguments")!.GetValue(obj);

        Assert.Equivalent(array, actualValue);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_WithDashDash_Test(Type type, Array array)
    {
        var obj = Activator.CreateInstance(type)!;
        var parser = new CommandParser(obj);
        var expression = CommandTestUtility.GetExpression(array);
        var argumentLine = $"--text s -- {expression}";

        parser.Parse(argumentLine);
        var actualValue = type.GetProperty("Arguments")!.GetValue(obj);

        Assert.Equivalent(array, actualValue);
    }
}

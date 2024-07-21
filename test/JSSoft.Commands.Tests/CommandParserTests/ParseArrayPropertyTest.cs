// <copyright file="ParseArrayPropertyTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using static JSSoft.Commands.Tests.RandomUtility;

namespace JSSoft.Commands.Tests.CommandParserTests;

public class ParseArrayPropertyTest()
{
    private sealed class InstanceClass
    {
        [CommandProperty]
        public bool[] Booleans { get; set; } = [];

        [CommandProperty]
        public string[] Strings { get; set; } = [];

        [CommandProperty]
        public sbyte[] SBytes { get; set; } = [];

        [CommandProperty]
        public byte[] Bytes { get; set; } = [];

        [CommandProperty]
        public short[] Int16s { get; set; } = [];

        [CommandProperty]
        public ushort[] UInt16s { get; set; } = [];

        [CommandProperty]
        public int[] Int32s { get; set; } = [];

        [CommandProperty]
        public uint[] UInt32s { get; set; } = [];

        [CommandProperty]
        public long[] Int64s { get; set; } = [];

        [CommandProperty]
        public ulong[] UInt64s { get; set; } = [];

        [CommandProperty]
        public float[] Singles { get; set; } = [];

        [CommandProperty]
        public double[] Doubles { get; set; } = [];

        [CommandProperty]
        public decimal[] Decimals { get; set; } = [];
    }

    public static IEnumerable<object[]> TestData =>
    [
        [nameof(InstanceClass.Booleans), "--booleans", Array(Boolean, 1, 10)],
        [nameof(InstanceClass.Strings), "--strings", Array(Word, 1, 10)],
        [nameof(InstanceClass.SBytes), "--sbytes", Array(SByte, 1, 10)],
        [nameof(InstanceClass.Bytes), "--bytes", Array(Byte, 1, 10)],
        [nameof(InstanceClass.Int16s), "--int16s", Array(Int16, 1, 10)],
        [nameof(InstanceClass.UInt16s), "--uint16s", Array(UInt16, 1, 10)],
        [nameof(InstanceClass.Int32s), "--int32s", Array(Int32, 1, 10)],
        [nameof(InstanceClass.UInt32s), "--uint32s", Array(UInt32, 1, 10)],
        [nameof(InstanceClass.Int64s), "--int64s", Array(Int64, 1, 10)],
        [nameof(InstanceClass.UInt64s), "--uint64s", Array(UInt64, 1, 10)],
        [nameof(InstanceClass.Singles), "--singles", Array(Single, 1, 10)],
        [nameof(InstanceClass.Doubles), "--doubles", Array(Double, 1, 10)],
        [nameof(InstanceClass.Decimals), "--decimals", Array(Decimal, 1, 10)],
    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_Test(string name, string option, Array array)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var expression = CommandTestUtility.GetExpression(array, ", ");
        var argumentLine = $"{option} {expression}";
        parser.Parse(argumentLine);
        var actualValue = typeof(InstanceClass).GetProperty(name)!.GetValue(obj);

        Assert.Equivalent(array, actualValue);
    }
}

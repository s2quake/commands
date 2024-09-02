// <copyright file="ParseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using static JSSoft.Commands.Tests.RandomUtility;

namespace JSSoft.Commands.Tests.CommandParserTests;

public class ParseTest(ITestOutputHelper logger)
{
    private sealed class InstanceClass
    {
        [CommandProperty]
        public bool Boolean { get; set; }

        [CommandProperty]
        public string String { get; set; } = string.Empty;

        [CommandProperty]
        public sbyte SByte { get; set; }

        [CommandProperty]
        public byte Byte { get; set; }

        [CommandProperty]
        public short Int16 { get; set; }

        [CommandProperty]
        public ushort UInt16 { get; set; }

        [CommandProperty]
        public int Int32 { get; set; }

        [CommandProperty]
        public uint UInt32 { get; set; }

        [CommandProperty]
        public long Int64 { get; set; }

        [CommandProperty]
        public ulong UInt64 { get; set; }

        [CommandProperty]
        public float Single { get; set; }

        [CommandProperty]
        public double Double { get; set; }

        [CommandProperty]
        public decimal Decimal { get; set; }
    }

    public static IEnumerable<object[]> TestData =>
    [
        [nameof(InstanceClass.Boolean), "--boolean", Boolean()],
        [nameof(InstanceClass.String), "--string", Word(item => item.Contains('\'') is false)],
        [nameof(InstanceClass.SByte), "--sbyte", SByte()],
        [nameof(InstanceClass.Byte), "--byte", Byte()],
        [nameof(InstanceClass.Int16), "--int16", Int16()],
        [nameof(InstanceClass.UInt16), "--uint16", UInt16()],
        [nameof(InstanceClass.Int32), "--int32", Int32()],
        [nameof(InstanceClass.UInt32), "--uint32", UInt32()],
        [nameof(InstanceClass.Int64), "--int64", Int64()],
        [nameof(InstanceClass.UInt64), "--uint64", UInt64()],
        [nameof(InstanceClass.Single), "--single", Single()],
        [nameof(InstanceClass.Double), "--double", Double()],
        [nameof(InstanceClass.Decimal), "--decimal", Decimal()],
    ];

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_Test(string name, string option, object value)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"{option} {value:R}";
        parser.Parse(argumentLine);
        var actualValue = typeof(InstanceClass).GetProperty(name)!.GetValue(obj);

        Assert.Equal(value, actualValue);
    }

    [Fact]
    public void ParseTest_Twice_Test()
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);

        parser.Parse($"--int32 123");

        Assert.Equal(123, obj.Int32);

        parser.Parse($"--int64 321");

        Assert.Equal(0, obj.Int32);
        Assert.Equal(321, obj.Int64);
    }

    [Theory]
    [InlineData("", typeof(CommandParsingException))]
    [InlineData("\"\"", typeof(CommandParsingException))]
    [InlineData("-h", typeof(CommandParsingException))]
    [InlineData("-v", typeof(CommandParsingException))]
    [InlineData("--help", typeof(CommandParsingException))]
    [InlineData("--version", typeof(CommandParsingException))]
    [InlineData("aldksjf lakdsjf", typeof(CommandLineException))]
    public void ParseTest_RandomValue_FailTest(string value, Type exceptionType)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        logger.WriteLine(value);
        Assert.Throws(exceptionType, () => parser.Parse($"{value}"));
    }

    [Fact]
    public void ParseTest_RandomString_Test()
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var s = NextString();
        parser.Parse($"--string \"{s}\"");
        Assert.Equal(s, obj.String);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("True", true)]
    [InlineData("TRUE", true)]
    [InlineData("false", false)]
    [InlineData("False", false)]
    [InlineData("FALSE", false)]
    public void ParseTest_Boolean_Test(string text, bool expectedValue)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"--boolean {text}";
        parser.Parse(argumentLine);
        logger.WriteLine(argumentLine);
        Assert.Equal(expectedValue, obj.Boolean);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_FailTest(string name, string option, object value)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"{value:R} {option}";
        logger.WriteLine(name);
        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_DoubleQuotesWrapped_Test(string name, string option, object value)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"\"{option}\" \"{value:R}\"";
        parser.Parse(argumentLine);
        var actualValue = typeof(InstanceClass).GetProperty(name)!.GetValue(obj);

        Assert.Equal(value, actualValue);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_SingleQuoteWrapped_Test(string name, string option, object value)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"'{option}' '{value:R}'";
        parser.Parse(argumentLine);
        var actualValue = typeof(InstanceClass).GetProperty(name)!.GetValue(obj);

        Assert.Equal(value, actualValue);
    }
}

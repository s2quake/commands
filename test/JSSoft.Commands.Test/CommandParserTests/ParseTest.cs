// <copyright file="ParseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test.CommandParserTests;

public class ParseTest(ITestOutputHelper logger)
{
    sealed class InstanceClass
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

    public static IEnumerable<object[]> TestData => new object[][]
    {
        [nameof(InstanceClass.Boolean), "--boolean", RandomUtility.NextBoolean()],
        [nameof(InstanceClass.String), "--string", RandomUtility.NextWord(item => item.Contains('\'') == false)],
        [nameof(InstanceClass.SByte), "--sbyte", RandomUtility.NextSByte()],
        [nameof(InstanceClass.Byte), "--byte", RandomUtility.NextByte()],
        [nameof(InstanceClass.Int16), "--int16", RandomUtility.NextInt16()],
        [nameof(InstanceClass.UInt16), "--uint16", RandomUtility.NextUInt16()],
        [nameof(InstanceClass.Int32), "--int32", RandomUtility.NextInt32()],
        [nameof(InstanceClass.UInt32), "--uint32", RandomUtility.NextUInt32()],
        [nameof(InstanceClass.Int64), "--int64", RandomUtility.NextInt64()],
        [nameof(InstanceClass.UInt64), "--uint64", RandomUtility.NextUInt64()],
        [nameof(InstanceClass.Single), "--single", RandomUtility.NextSingle()],
        [nameof(InstanceClass.Double), "--double", RandomUtility.NextDouble()],
        [nameof(InstanceClass.Decimal), "--decimal", RandomUtility.NextDecimal()],
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_Test(string name, string option, object value)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"{option} {value:R}";
        var memberDescriptors = from item in CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))
                                where item.MemberName != name
                                select item;

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
        var s = RandomUtility.NextString();
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
        var memberDescriptors = from item in CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))
                                where item.MemberName != name
                                select item;

        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_DoubleQuotesWrapped_Test(string name, string option, object value)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var argumentLine = $"\"{option}\" \"{value:R}\"";
        var memberDescriptors = from item in CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))
                                where item.MemberName != name
                                select item;

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
        var memberDescriptors = from item in CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))
                                where item.MemberName != name
                                select item;

        parser.Parse(argumentLine);
        var actualValue = typeof(InstanceClass).GetProperty(name)!.GetValue(obj);

        Assert.Equal(value, actualValue);
    }
}

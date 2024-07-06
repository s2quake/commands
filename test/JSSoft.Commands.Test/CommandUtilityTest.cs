// <copyright file="CommandUtilityTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test;

public class CommandUtilityTest(ITestOutputHelper output)
{
    [Theory]
    [InlineData("", new string[] { })]
    [InlineData(" ", new string[] { })]
    [InlineData("  ", new string[] { })]
    [InlineData("a", new string[] { "a" })]
    [InlineData("a ", new string[] { "a" })]
    [InlineData(" a ", new string[] { "a" })]
    [InlineData("  a ", new string[] { "a" })]
    [InlineData("  a  ", new string[] { "a" })]
    [InlineData("a b", new string[] { "a", "b" })]
    [InlineData(" a b", new string[] { "a", "b" })]
    [InlineData(" a b ", new string[] { "a", "b" })]
    [InlineData("  a b ", new string[] { "a", "b" })]
    [InlineData("  a  b ", new string[] { "a", "b" })]
    [InlineData("  a  b  ", new string[] { "a", "b" })]
    public void Split_WithSpace_Test(string text, string[] expectedItems)
    {
        output.WriteLine(text);
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("\\t", new string[] { "t" })]
    [InlineData("\\r", new string[] { "r" })]
    [InlineData("\\a", new string[] { "a" })]
    [InlineData("\\ ", new string[] { " " })]
    [InlineData("\\\\", new string[] { "\\" })]
    public void Split_WithEscapCharacter_Test(string text, string[] expectedItems)
    {
        output.WriteLine(text);
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("'a'", new string[] { "a" })]
    [InlineData(" 'a'", new string[] { "a" })]
    [InlineData(" 'a' ", new string[] { "a" })]
    [InlineData("  'a' ", new string[] { "a" })]
    [InlineData("  'a'  ", new string[] { "a" })]
    [InlineData("'''a'''", new string[] { "a" })]
    [InlineData("''\\'a\\'''", new string[] { "'a'" })]
    [InlineData("'a' 'b'", new string[] { "a", "b" })]
    [InlineData(" 'a' 'b'", new string[] { "a", "b" })]
    [InlineData(" 'a' 'b' ", new string[] { "a", "b" })]
    [InlineData(" 'a'  'b' ", new string[] { "a", "b" })]
    [InlineData("  'a'  'b' ", new string[] { "a", "b" })]
    [InlineData("  'a'   'b' ", new string[] { "a", "b" })]
    [InlineData("  'a'   'b'  ", new string[] { "a", "b" })]
    [InlineData(" '''a'   'b'''  ", new string[] { "a", "b" })]
    [InlineData(" '''a c'   'b'''''  ", new string[] { "a c", "b" })]
    public void Split_WithQuote_Test(string text, string[] expectedItems)
    {
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("'a ''b'", new string[] { "a b" })]
    [InlineData(" 'a ''b'", new string[] { "a b" })]
    [InlineData(" 'a ''b' ", new string[] { "a b" })]
    [InlineData(" 'a ''''b' ", new string[] { "a b" })]
    [InlineData(" 'a 'b ", new string[] { "a b" })]
    [InlineData(" 'a 'b'' ", new string[] { "a b" })]
    [InlineData(" 'a 'b''c ", new string[] { "a bc" })]
    [InlineData(" 'a 'b''c'd' ", new string[] { "a bcd" })]
    [InlineData(" 'a 'b''c\\''d' ", new string[] { "a bc'd" })]
    public void Split_WithQuote_WithAttached_Test(string text, string[] expectedItems)
    {
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("'")]
    [InlineData("'''")]
    [InlineData(" '''")]
    [InlineData(" ''' ")]
    [InlineData(" 'a'' ")]
    [InlineData(" 'a'' a")]
    [InlineData("\"")]
    [InlineData(" \"")]
    [InlineData(" \" ")]
    [InlineData(" \" \" \"")]
    [InlineData("\" adf ")]
    [InlineData("\" adf \"\"")]
    [InlineData(" \" adf \"\"")]
    [InlineData(" \" adf \"\" ")]
    public void Split_WithQuote_FailTest(string text)
    {
        Assert.Throws<ArgumentException>(() => CommandUtility.Split(text));
        Assert.False(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("\"a\"", new string[] { "a" })]
    [InlineData(" \"a\"", new string[] { "a" })]
    [InlineData(" \"a\" ", new string[] { "a" })]
    [InlineData("  \"a\" ", new string[] { "a" })]
    [InlineData("  \"a\"  ", new string[] { "a" })]
    public void Split_WithDoubleQuotes_Test(string text, string[] expectedItems)
    {
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("\"a\\\\\"", new string[] { "a\\" })]
    [InlineData("\"a\\\"\"", new string[] { "a\"" })]
    public void Split_WithEscapeCharacter_InDoubleQuotes_Test(string text, string[] expectedItems)
    {
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData("\"a\"\"b\"c ", new string[] { "abc" })]
    [InlineData(" \"a\"\"b\"c ", new string[] { "abc" })]
    [InlineData(" \"a\"\"b\"c   ", new string[] { "abc" })]
    [InlineData("  \"a\"\"b\"c ", new string[] { "abc" })]
    [InlineData("  \"a\"\"b\"c  ", new string[] { "abc" })]
    public void Split_WithDoubleQuotes_WithAttached_Test(string text, string[] expectedItems)
    {
        var actualItems = CommandUtility.Split(text);
        Assert.Equivalent(expectedItems, actualItems, strict: true);
        Assert.True(CommandUtility.TrySplit(text, out var _));
    }

    [Theory]
    [InlineData(new string[] { "" }, "")]
    [InlineData(new string[] { " " }, "\" \"")]
    [InlineData(new string[] { " ", "" }, "\" \" ")]
    [InlineData(new string[] { " ", " " }, "\" \" \" \"")]
    [InlineData(new string[] { "\\ ", " " }, "\"\\ \" \" \"")]
    [InlineData(new string[] { "", "" }, " ")]
    [InlineData(new string[] { "", "", "" }, "  ")]
    [InlineData(new string[] { "a", "b" }, "a b")]
    [InlineData(new string[] { "a", "b c" }, "a \"b c\"")]
    public void Join_Test(string[] items, string expectedText)
    {
        var actualText = CommandUtility.Join(items);
        Assert.Equal(expectedText, actualText);
    }

    [Theory]
    [InlineData("a", "a", new string[] { })]
    [InlineData(" a", "a", new string[] { })]
    [InlineData("a b", "a", new string[] { "b" })]
    [InlineData("a \"b\"", "a", new string[] { "b" })]
    [InlineData("\"a\" b", "a", new string[] { "b" })]
    [InlineData("\"a\" \"b\"", "a", new string[] { "b" })]
    [InlineData("a 'b'", "a", new string[] { "b" })]
    [InlineData("'a' b", "a", new string[] { "b" })]
    [InlineData("'a' 'b'", "a", new string[] { "b" })]
    [InlineData("\\a 'b'", "a", new string[] { "b" })]
    [InlineData("\\l\\s 'b' \"123 123\"", "ls", new string[] { "b", "123 123" })]
    public void SplitCommandLine_Test(string commandLine, string expectedCommandName, string[] expectedCommandArguments)
    {
        var (actualCommandName, actualCommandArguments) = CommandUtility.SplitCommandLine(commandLine);
        Assert.Equal(expectedCommandName, actualCommandName);
        Assert.Equivalent(expectedCommandArguments, actualCommandArguments);
        Assert.True(CommandUtility.TrySplitCommandLine(commandLine, out var _1, out var _2));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("'")]
    [InlineData(" '")]
    [InlineData("\"")]
    [InlineData(" \"")]
    public void SplitCommandLine_FailTest(string commandLine)
    {
        Assert.Throws<ArgumentException>(nameof(commandLine), () => CommandUtility.SplitCommandLine(commandLine));
        Assert.False(CommandUtility.TrySplitCommandLine(commandLine, out var _1, out var _2));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(" a", false)]
    [InlineData(" a ", false)]
    [InlineData("-", false)]
    [InlineData(" -", false)]
    [InlineData(" - ", false)]
    [InlineData("-c", false)]
    [InlineData(" -c", false)]
    [InlineData(" -c ", false)]
    [InlineData("-c\\", false)]
    [InlineData("-c\"", false)]
    [InlineData("-cc", true)]
    [InlineData("-cqq", true)]
    [InlineData("-cq", true)]
    [InlineData("-cqa", true)]
    [InlineData(" -cqa", false)]
    [InlineData(" -cqa ", false)]
    public void IsMultipleSwitch_Test(string text, bool expectedResult)
    {
        var actualResult = CommandUtility.IsMultipleSwitch(text);
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(" a", false)]
    [InlineData(" a ", false)]
    [InlineData("--", false)]
    [InlineData(" --", false)]
    [InlineData(" -- ", false)]
    [InlineData("--c", false)]
    [InlineData(" --c", false)]
    [InlineData(" --c ", false)]
    [InlineData("--c\\", false)]
    [InlineData("--c\"", false)]
    [InlineData("--cc", true)]
    [InlineData("--cqq", true)]
    [InlineData("--cq", true)]
    [InlineData("--cqa", true)]
    [InlineData(" --cqa", false)]
    [InlineData(" --cqa ", false)]
    [InlineData("--ca-a", true)]
    [InlineData("--ca-", false)]
    [InlineData("--ca--", false)]
    [InlineData("--ca--a", false)]
    [InlineData("-", false)]
    [InlineData(" -", false)]
    [InlineData(" - ", false)]
    [InlineData("-c", true)]
    [InlineData("-\"", false)]
    [InlineData("-_", false)]
    [InlineData(" -c", false)]
    [InlineData(" -c ", false)]
    [InlineData(" -\" ", false)]
    [InlineData(" -' ", false)]
    public void IsOption_Test(string text, bool expectedResult)
    {
        var actualResult = CommandUtility.IsOption(text);
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("A", "a")]
    [InlineData(" A", " a")]
    [InlineData(" A ", " a ")]
    [InlineData("a", "a")]
    [InlineData("ID", "id")]
    [InlineData("IDs", "ids")]
    [InlineData("SpinalCase", "spinal-case")]
    [InlineData("spinalCase", "spinal-case")]
    [InlineData("PAT", "pat")]
    [InlineData("actualText", "actual-text")]
    public void ToSpinalCase_Test(string text, string expectedText)
    {
        var actualText = CommandUtility.ToSpinalCase(text);
        Assert.Equal(expectedText, actualText);
    }

    [Theory]
    [InlineData("", "\"\"")]
    [InlineData(" ", "\" \"")]
    [InlineData("\"", "\"\\\"\"")]
    [InlineData("'", "\"'\"")]
    [InlineData("\\", "\"\\\\\"")]
    [InlineData("a", "\"a\"")]
    public void WrapDoubleQuotes(string text, string expectedText)
    {
        var actualText = CommandUtility.WrapDoubleQuotes(text);
        Assert.Equal(expectedText, actualText);
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(" ", true)]
    [InlineData("\"", true)]
    [InlineData("'", true)]
    [InlineData("\\", true)]
    [InlineData("a", false)]
    [InlineData("a b", true)]
    public void TryWrapDoubleQuotes(string text, bool expectedValue)
    {
        var actualValue = CommandUtility.TryWrapDoubleQuotes(text, out var _);
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(typeof(bool))]
    [InlineData(typeof(string))]
    [InlineData(typeof(byte))]
    [InlineData(typeof(sbyte))]
    [InlineData(typeof(short))]
    [InlineData(typeof(ushort))]
    [InlineData(typeof(int))]
    [InlineData(typeof(uint))]
    [InlineData(typeof(long))]
    [InlineData(typeof(ulong))]
    [InlineData(typeof(float))]
    [InlineData(typeof(double))]
    [InlineData(typeof(decimal))]
    public void IsSupportedType_PrimitiveType_Test(Type type)
    {
        Assert.True(CommandUtility.IsSupportedType(type));
    }

    [Theory]
    [InlineData(typeof(bool[]))]
    [InlineData(typeof(string[]))]
    [InlineData(typeof(byte[]))]
    [InlineData(typeof(sbyte[]))]
    [InlineData(typeof(short[]))]
    [InlineData(typeof(ushort[]))]
    [InlineData(typeof(int[]))]
    [InlineData(typeof(uint[]))]
    [InlineData(typeof(long[]))]
    [InlineData(typeof(ulong[]))]
    [InlineData(typeof(float[]))]
    [InlineData(typeof(double[]))]
    [InlineData(typeof(decimal[]))]
    public void IsSupportedType_ArrayType_Test(Type type)
    {
        Assert.True(CommandUtility.IsSupportedType(type));
    }

    [Theory]
    [InlineData(typeof(AttributeTargets))]
    [InlineData(typeof(ConsoleKey))]
    public void IsSupportedType_EnumType_Test(Type type)
    {
        Assert.True(CommandUtility.IsSupportedType(type));
    }

    [Theory]
    [InlineData(typeof(bool?))]
    [InlineData(typeof(byte?))]
    [InlineData(typeof(sbyte?))]
    [InlineData(typeof(short?))]
    [InlineData(typeof(ushort?))]
    [InlineData(typeof(int?))]
    [InlineData(typeof(uint?))]
    [InlineData(typeof(long?))]
    [InlineData(typeof(ulong?))]
    [InlineData(typeof(float?))]
    [InlineData(typeof(double?))]
    [InlineData(typeof(decimal?))]
    public void IsSupportedType_NullableType_Test(Type type)
    {
        Assert.True(CommandUtility.IsSupportedType(type));
    }

    [Theory]
    [InlineData(typeof(bool?[]))]
    [InlineData(typeof(string?[]))]
    [InlineData(typeof(byte?[]))]
    [InlineData(typeof(sbyte?[]))]
    [InlineData(typeof(short?[]))]
    [InlineData(typeof(ushort?[]))]
    [InlineData(typeof(int?[]))]
    [InlineData(typeof(uint?[]))]
    [InlineData(typeof(long?[]))]
    [InlineData(typeof(ulong?[]))]
    [InlineData(typeof(float?[]))]
    [InlineData(typeof(double?[]))]
    [InlineData(typeof(decimal?[]))]
    public void IsSupportedType_NullableArrayType_Test(Type type)
    {
        Assert.True(CommandUtility.IsSupportedType(type));
    }
}

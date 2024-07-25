// <copyright file="Property_Custom_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using static JSSoft.Commands.Tests.RandomUtility;

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_Custom_Test
{
    [TypeConverter(typeof(CustomTypeTypeConverter))]
    private struct CustomType
    {
        public int Value { get; set; }
    }

    private sealed class CustomTypeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(
            ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
            => destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string text)
            {
                return new CustomType() { Value = int.Parse(text) };
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object? ConvertTo(
            ITypeDescriptorContext? context,
            CultureInfo? culture,
            object? value,
            Type destinationType)
        {
            if (value is CustomType customType)
            {
                return customType.Value.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    private sealed class ParseClass
    {
        [CommandProperty]
        public CustomType Member1 { get; set; }
    }

    private static class StaticParseClass
    {
        [CommandProperty]
        public static CustomType Member1 { get; set; }
    }

    private sealed class MethodClass
    {
        public CustomType Member1 { get; set; }

        [CommandMethod]
        public void Test1(CustomType member1)
        {
            Member1 = member1;
        }
    }

    private static class StaticMethodClass
    {
        public static CustomType Member1 { get; set; }

        [CommandMethod]
        public static void Test1(CustomType member1)
        {
            Member1 = member1;
        }
    }

    [Fact]
    public void Base_Member1_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(ParseClass));
        var memberDescriptor = memberDescriptors[nameof(ParseClass.Member1)];
        Assert.IsType<CommandPropertyDescriptor>(memberDescriptor);
        Assert.Equal("member1", memberDescriptor.Name);
        Assert.Equal(char.MinValue, memberDescriptor.ShortName);
        Assert.Equal("--member1", memberDescriptor.DisplayName);
        Assert.Equal(DBNull.Value, memberDescriptor.InitValue);
        Assert.Equal(DBNull.Value, memberDescriptor.DefaultValue);
        Assert.False(memberDescriptor.IsRequired);
        Assert.True(memberDescriptor.IsExplicit);
        Assert.False(memberDescriptor.IsSwitch);
        Assert.False(memberDescriptor.IsVariables);
        Assert.Equal(typeof(CustomType), memberDescriptor.MemberType);
        Assert.Equal(nameof(ParseClass.Member1), memberDescriptor.MemberName);
        Assert.True(memberDescriptor.IsGeneral);
    }

    [Fact]
    public void Base_Member1_Parse_Test()
    {
        var expectedValue = Int32();
        var instance = new ParseClass();
        var parser = new CommandParser(instance);
        parser.Parse($"--member1 {expectedValue}");
        Assert.Equal(expectedValue, instance.Member1.Value);
    }

    [Fact]
    public void Base_Member1_Parse_FailTest()
    {
        var word = Word(item => !item.Contains('\''));
        var instance = new ParseClass();
        var parser = new CommandParser(instance);
        Assert.Throws<CommandLineException>(() => parser.Parse($"--member1 {word}"));
    }

    [Fact]
    public void Base_Member1_StaticParse_Test()
    {
        var expectedValue = Int32();
        var parser = new CommandParser(typeof(StaticParseClass));
        parser.Parse($"--member1 {expectedValue}");
        Assert.Equal(expectedValue, StaticParseClass.Member1.Value);
    }

    [Fact]
    public void Base_Member1_StaticParse_FailTest()
    {
        var word = Word(item => !item.Contains('\''));
        var parser = new CommandParser(typeof(StaticParseClass));
        Assert.Throws<CommandLineException>(() => parser.Parse($"--member1 {word}"));
    }

    [Fact]
    public void Base_Member1_Invoke_Test()
    {
        var expectedValue = Int32();
        var instance = new MethodClass();
        var invoker = new CommandInvoker(instance);
        invoker.Invoke($"test1 --member1 {expectedValue}");
        Assert.Equal(expectedValue, instance.Member1.Value);
    }

    [Fact]
    public void Base_Member1_Invoke_FailTest()
    {
        var word = Word(item => !item.Contains('\''));
        var instance = new MethodClass();
        var invoker = new CommandInvoker(instance);
        Assert.Throws<CommandLineException>(() => invoker.Invoke($"test1 --member1 {word}"));
    }

    [Fact]
    public void Base_Member1_StaticInvoke_Test()
    {
        var expectedValue = Int32();
        var invoker = new CommandInvoker(typeof(StaticMethodClass));
        invoker.Invoke($"test1 --member1 {expectedValue}");
        Assert.Equal(expectedValue, StaticMethodClass.Member1.Value);
    }

    [Fact]
    public void Base_Member1_StaticInvoke_FailTest()
    {
        var word = Word(item => !item.Contains('\''));
        var invoker = new CommandInvoker(typeof(StaticMethodClass));
        Assert.Throws<CommandLineException>(() => invoker.Invoke($"test1 --member1 {word}"));
    }
}

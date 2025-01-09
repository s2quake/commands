// <copyright file="TypeConverterTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.Globalization;

namespace JSSoft.Commands.Tests;

public sealed class TypeConverterTest
{
    [Fact]
    public void Parse_Test()
    {
        TypeDescriptor.AddAttributes(
            typeof(Data), new TypeConverterAttribute(typeof(DataTypeConverter)));
        var instance = new Instance();
        var parser = new CommandParser(instance);
        parser.Parse("--data 1");

        Assert.Equal(1, instance.Data.Value);
    }

    [Fact]
    public void ParseThrow_Test()
    {
        var instance = new Instance();
        var parser = new CommandParser(instance);
        Assert.ThrowsAny<CommandDefinitionException>(() => parser.Parse("--data 1"));
    }

    [Fact]
    public void ParseExistTypeConverterThrow_Test()
    {
        TypeDescriptor.AddAttributes(
            typeof(CustomData), new TypeConverterAttribute(typeof(CustomDataTypeConverter2)));
        var instance = new CustomInstance();
        var parser = new CommandParser(instance);
        Assert.Throws<CommandLineException>(() => parser.Parse("--data 1"));
    }

    [Fact]
    public void ParseExistAttributeTypeConverter_Test()
    {
        var instance = new CustomInstance();
        var parser = new CommandParser(instance);
        parser.Parse("--data 1");

        Assert.Equal(1, instance.Data.Value);
    }

    private sealed class Instance
    {
        [CommandProperty]
        public Data Data { get; set; }
    }

    private sealed class CustomInstance
    {
        [CommandProperty]
        public CustomData Data { get; set; }
    }

    private struct Data
    {
        public int Value { get; set; }
    }

    private sealed class DataTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string text)
            {
                return new Data() { Value = int.Parse(text) };
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    [TypeConverter(typeof(CustomDataTypeConverter1))]
    private struct CustomData
    {
        public int Value { get; set; }
    }

    private sealed class CustomDataTypeConverter1 : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is string text)
            {
                return new CustomData() { Value = int.Parse(text) };
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    private sealed class CustomDataTypeConverter2 : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object? ConvertFrom(
            ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            throw new NotSupportedException();
        }
    }
}

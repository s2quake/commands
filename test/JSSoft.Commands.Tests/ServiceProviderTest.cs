// <copyright file="ServiceProviderTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.Globalization;

namespace JSSoft.Commands.Tests;

public sealed class ServiceProviderTest
{
    [Fact]
    public void Parse_Test()
    {
        TypeDescriptor.AddAttributes(
            typeof(Data), new TypeConverterAttribute(typeof(DataTypeConverter)));
        var settings = new CommandSettings
        {
            ServiceProvider = new ServiceProvider(),
        };

        var instance = new Instance();
        var parser = new CommandParser(instance, settings);
        parser.Parse("--data 1");

        Assert.Equal(2, instance.Data.Value);
    }

    private sealed class Instance
    {
        [CommandProperty]
        public Data Data { get; set; }
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
                var i = int.Parse(text);
                if (context is not null && context.GetService(typeof(int)) is int sum)
                {
                    i += sum;
                }

                return new Data() { Value = i };
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    private sealed class ServiceProvider : IServiceProvider
    {
        public object? GetService(Type serviceType)
        {
            if (serviceType == typeof(int))
            {
                return 1;
            }

            return null;
        }
    }
}

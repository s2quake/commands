// <copyright file="CustomParameter_Test.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable CA1822

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class CustomParameter_Test
{
    private CustomParameter? _customParameter;

    [CommandMethod]
    [CommandMethodParameter(nameof(customParameter))]
    internal void Method1(CustomParameter customParameter)
    {
        _customParameter = customParameter;
    }

    [Fact]
    public void Base_Method1_CustomParameterTest()
    {
        var declaringType = typeof(CustomParameter_Test);
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(declaringType);
        var methodDescriptor = methodDescriptors[nameof(Method1)];
        var index = 0;
        Assert.Equal(nameof(CustomParameter.String), methodDescriptor.Members[index++].MemberName);
        Assert.Equal(nameof(CustomParameter.Integer), methodDescriptor.Members[index++].MemberName);
        Assert.Equal(nameof(CustomParameter.Name), methodDescriptor.Members[index++].MemberName);
        Assert.Equal(nameof(CustomParameter.Boolean), methodDescriptor.Members[index++].MemberName);
        Assert.Equal(4, index);
    }

    [Fact]
    public void Base_Method1_CustomParameter_InvokeTest()
    {
        var invoker = new CommandInvoker(this);
        _customParameter = null;
        invoker.Invoke("method1 hello --boolean --name world");

        Assert.NotNull(_customParameter);
        Assert.Equal("hello", _customParameter.String);
        Assert.Equal(0, _customParameter.Integer);
        Assert.True(_customParameter.Boolean);
        Assert.Equal("world", _customParameter.Name);
    }

    internal sealed class CustomParameter
    {
        [CommandPropertySwitch]
        public bool Boolean { get; set; }

        [CommandPropertyRequired(DefaultValue = 0)]
        public int Integer { get; set; }

        [CommandProperty]
        public string Name { get; set; } = string.Empty;

        [CommandPropertyRequired]
        public string String { get; set; } = string.Empty;
    }
}

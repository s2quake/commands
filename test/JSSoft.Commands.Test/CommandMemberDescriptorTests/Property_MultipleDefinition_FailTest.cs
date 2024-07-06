// <copyright file="Property_MultipleDefinition_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Reflection;

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_MultipleDefinition_FailTest
{
    sealed class Required_ExplicitRequired_Class
    {
        [CommandPropertyRequired]
        [CommandPropertyExplicitRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Required_ExplicitRequired_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Required_ExplicitRequired_Class)));
    }

    sealed class Required_General_Class
    {
        [CommandPropertyRequired]
        [CommandProperty]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Required_General_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Required_General_Class)));
    }

    sealed class Required_Switch_Class
    {
        [CommandPropertyRequired]
        [CommandPropertySwitch]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Required_Switch_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Required_Switch_Class)));
    }

    sealed class Required_Variables_Class
    {
        [CommandPropertyRequired]
        [CommandPropertyArray]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Required_Variables_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Required_Variables_Class)));
    }

    sealed class ExplicitRequired_Required_Class
    {
        [CommandPropertyExplicitRequired]
        [CommandPropertyRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void ExplicitRequired_Required_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ExplicitRequired_Required_Class)));
    }

    sealed class ExplicitRequired_General_Class
    {
        [CommandPropertyExplicitRequired]
        [CommandProperty]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void ExplicitRequired_General_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ExplicitRequired_General_Class)));
    }

    sealed class ExplicitRequired_Switch_Class
    {
        [CommandPropertyExplicitRequired]
        [CommandPropertySwitch]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void ExplicitRequired_Switch_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ExplicitRequired_Switch_Class)));
    }

    sealed class ExplicitRequired_Variables_Class
    {
        [CommandPropertyExplicitRequired]
        [CommandPropertyArray]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void ExplicitRequired_Variables_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ExplicitRequired_Variables_Class)));
    }

    sealed class General_Required_Class
    {
        [CommandProperty]
        [CommandPropertyRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void General_Required_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(General_Required_Class)));
    }

    sealed class General_ExplicitRequired_Class
    {
        [CommandProperty]
        [CommandPropertyExplicitRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void General_ExplicitRequired_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(General_ExplicitRequired_Class)));
    }

    sealed class General_Switch_Class
    {
        [CommandProperty]
        [CommandPropertySwitch]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void General_Switch_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(General_Switch_Class)));
    }

    sealed class General_Variables_Class
    {
        [CommandProperty]
        [CommandPropertyArray]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void General_Variables_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(General_Variables_Class)));
    }

    sealed class Switch_Required_Class
    {
        [CommandPropertySwitch]
        [CommandPropertyRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Switch_Required_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Switch_Required_Class)));
    }

    sealed class Switch_ExplicitRequired_Class
    {
        [CommandPropertySwitch]
        [CommandPropertyExplicitRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Switch_ExplicitRequired_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Switch_ExplicitRequired_Class)));
    }

    sealed class Switch_General_Class
    {
        [CommandPropertySwitch]
        [CommandPropertyExplicitRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Switch_General_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Switch_General_Class)));
    }

    sealed class Switch_Variables_Class
    {
        [CommandPropertySwitch]
        [CommandPropertyArray]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Switch_Variables_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Switch_Variables_Class)));
    }

    sealed class Variables_Required_Class
    {
        [CommandPropertyArray]
        [CommandPropertyRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Variables_Required_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Variables_Required_Class)));
    }

    sealed class Variables_ExplicitRequired_Class
    {
        [CommandPropertyArray]
        [CommandPropertyExplicitRequired]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Variables_ExplicitRequired_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Variables_ExplicitRequired_Class)));
    }

    sealed class Variables_General_Class
    {
        [CommandPropertyArray]
        [CommandProperty]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Variables_General_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Variables_General_Class)));
    }

    sealed class Variables_Switch_Class
    {
        [CommandPropertyArray]
        [CommandPropertySwitch]
        public string Member { get; set; } = string.Empty;
    }

    [Fact]
    public void Variables_Switch_Class_FailTest()
    {
        Assert.Throws<AmbiguousMatchException>(() => CommandDescriptor.GetMemberDescriptors(typeof(Variables_Switch_Class)));
    }
}

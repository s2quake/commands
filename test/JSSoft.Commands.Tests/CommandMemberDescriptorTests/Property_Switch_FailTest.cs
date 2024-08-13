// <copyright file="Property_Switch_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_Switch_FailTest
{
    private sealed class StringClass
    {
        [CommandPropertySwitch]
        public string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void StringClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(StringClass)));
    }

    private sealed class ByteClass
    {
        [CommandPropertySwitch]
        public byte Member1 { get; set; }
    }

    [Fact]
    public void ByteClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(ByteClass)));
    }

    private sealed class SByteClass
    {
        [CommandPropertySwitch]
        public sbyte Member1 { get; set; }
    }

    [Fact]
    public void SByteClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(SByteClass)));
    }

    private sealed class ShortClass
    {
        [CommandPropertySwitch]
        public short Member1 { get; set; }
    }

    [Fact]
    public void ShortClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(ShortClass)));
    }

    private sealed class UShortClass
    {
        [CommandPropertySwitch]
        public ushort Member1 { get; set; }
    }

    [Fact]
    public void UShortClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(UShortClass)));
    }

    private sealed class IntClass
    {
        [CommandPropertySwitch]
        public int Member1 { get; set; }
    }

    [Fact]
    public void IntClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(IntClass)));
    }

    private sealed class UIntClass
    {
        [CommandPropertySwitch]
        public uint Member1 { get; set; }
    }

    [Fact]
    public void UIntClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(UIntClass)));
    }

    private sealed class LongClass
    {
        [CommandPropertySwitch]
        public long Member1 { get; set; }
    }

    [Fact]
    public void LongClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(LongClass)));
    }

    private sealed class ULongClass
    {
        [CommandPropertySwitch]
        public ulong Member1 { get; set; }
    }

    [Fact]
    public void ULongClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(ULongClass)));
    }

    private sealed class FloatClass
    {
        [CommandPropertySwitch]
        public float Member1 { get; set; }
    }

    [Fact]
    public void FloatClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(FloatClass)));
    }

    private sealed class DoubleClass
    {
        [CommandPropertySwitch]
        public double Member1 { get; set; }
    }

    [Fact]
    public void DoubleClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(DoubleClass)));
    }

    private sealed class DecimalClass
    {
        [CommandPropertySwitch]
        public decimal Member1 { get; set; }
    }

    [Fact]
    public void DecimalClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(DecimalClass)));
    }

    private sealed class BoolArrayClass
    {
        [CommandPropertySwitch]
        public bool[] Member1 { get; set; } = [];
    }

    [Fact]
    public void BoolArrayClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(BoolArrayClass)));
    }
}

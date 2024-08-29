// <copyright file="Property_Unhandled_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMemberDescriptorTests;

public class Property_Unhandled_FailTest
{
    private sealed class StringClass
    {
        [CommandPropertyUnhandled]
        public string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void StringClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(StringClass)));
    }

    private sealed class BoolClass
    {
        [CommandPropertyUnhandled]
        public bool Member1 { get; set; }
    }

    [Fact]
    public void BoolClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(BoolClass)));
    }

    private sealed class ByteClass
    {
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
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
        [CommandPropertyUnhandled]
        public decimal Member1 { get; set; }
    }

    [Fact]
    public void DecimalClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(DecimalClass)));
    }

    private sealed class InvalidTypeArrayClass
    {
        [CommandPropertyUnhandled]
        public System.Threading.CancellationToken[] Member1 { get; set; } = [];
    }

    [Fact]
    public void InvalidTypeArrayClass_FailTest()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMemberDescriptors(typeof(InvalidTypeArrayClass)));
    }
}

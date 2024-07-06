// <copyright file="Property_Variables_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_Variables_FailTest
{
    sealed class StringClass
    {
        [CommandPropertyArray]
        public string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void StringClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(StringClass)));
    }

    sealed class BoolClass
    {
        [CommandPropertyArray]
        public bool Member1 { get; set; }
    }

    [Fact]
    public void BoolClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(BoolClass)));
    }

    sealed class ByteClass
    {
        [CommandPropertyArray]
        public byte Member1 { get; set; }
    }

    [Fact]
    public void ByteClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ByteClass)));
    }

    sealed class SByteClass
    {
        [CommandPropertyArray]
        public sbyte Member1 { get; set; }
    }

    [Fact]
    public void SByteClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(SByteClass)));
    }

    sealed class ShortClass
    {
        [CommandPropertyArray]
        public short Member1 { get; set; }
    }

    [Fact]
    public void ShortClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ShortClass)));
    }

    sealed class UShortClass
    {
        [CommandPropertyArray]
        public ushort Member1 { get; set; }
    }

    [Fact]
    public void UShortClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(UShortClass)));
    }

    sealed class IntClass
    {
        [CommandPropertyArray]
        public int Member1 { get; set; }
    }

    [Fact]
    public void IntClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(IntClass)));
    }

    sealed class UIntClass
    {
        [CommandPropertyArray]
        public uint Member1 { get; set; }
    }

    [Fact]
    public void UIntClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(UIntClass)));
    }

    sealed class LongClass
    {
        [CommandPropertyArray]
        public long Member1 { get; set; }
    }

    [Fact]
    public void LongClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(LongClass)));
    }

    sealed class ULongClass
    {
        [CommandPropertyArray]
        public ulong Member1 { get; set; }
    }

    [Fact]
    public void ULongClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ULongClass)));
    }

    sealed class FloatClass
    {
        [CommandPropertyArray]
        public float Member1 { get; set; }
    }

    [Fact]
    public void FloatClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(FloatClass)));
    }

    sealed class DoubleClass
    {
        [CommandPropertyArray]
        public double Member1 { get; set; }
    }

    [Fact]
    public void DoubleClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(DoubleClass)));
    }

    sealed class DecimalClass
    {
        [CommandPropertyArray]
        public decimal Member1 { get; set; }
    }

    [Fact]
    public void DecimalClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(DecimalClass)));
    }

    sealed class InvalidTypeArrayClass
    {
        [CommandPropertyArray]
        public System.Threading.CancellationToken[] Member1 { get; set; } = [];
    }

    [Fact]
    public void InvalidTypeArrayClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(InvalidTypeArrayClass)));
    }
}

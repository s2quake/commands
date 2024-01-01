// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace JSSoft.Commands.Test.CommandMemberDescriptorTests;

public class Property_Switch_FailTest
{
    sealed class StringClass
    {
        [CommandPropertySwitch]
        public string Member1 { get; set; } = string.Empty;
    }

    [Fact]
    public void StringClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(StringClass)));
    }

    sealed class ByteClass
    {
        [CommandPropertySwitch]
        public byte Member1 { get; set; }
    }

    [Fact]
    public void ByteClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ByteClass)));
    }

    sealed class SByteClass
    {
        [CommandPropertySwitch]
        public sbyte Member1 { get; set; }
    }

    [Fact]
    public void SByteClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(SByteClass)));
    }

    sealed class ShortClass
    {
        [CommandPropertySwitch]
        public short Member1 { get; set; }
    }

    [Fact]
    public void ShortClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ShortClass)));
    }

    sealed class UShortClass
    {
        [CommandPropertySwitch]
        public ushort Member1 { get; set; }
    }

    [Fact]
    public void UShortClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(UShortClass)));
    }

    sealed class IntClass
    {
        [CommandPropertySwitch]
        public int Member1 { get; set; }
    }

    [Fact]
    public void IntClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(IntClass)));
    }

    sealed class UIntClass
    {
        [CommandPropertySwitch]
        public uint Member1 { get; set; }
    }

    [Fact]
    public void UIntClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(UIntClass)));
    }

    sealed class LongClass
    {
        [CommandPropertySwitch]
        public long Member1 { get; set; }
    }

    [Fact]
    public void LongClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(LongClass)));
    }

    sealed class ULongClass
    {
        [CommandPropertySwitch]
        public ulong Member1 { get; set; }
    }

    [Fact]
    public void ULongClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(ULongClass)));
    }

    sealed class FloatClass
    {
        [CommandPropertySwitch]
        public float Member1 { get; set; }
    }

    [Fact]
    public void FloatClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(FloatClass)));
    }

    sealed class DoubleClass
    {
        [CommandPropertySwitch]
        public double Member1 { get; set; }
    }

    [Fact]
    public void DoubleClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(DoubleClass)));
    }

    sealed class DecimalClass
    {
        [CommandPropertySwitch]
        public decimal Member1 { get; set; }
    }

    [Fact]
    public void DecimalClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(DecimalClass)));
    }

    sealed class BoolArrayClass
    {
        [CommandPropertySwitch]
        public bool[] Member1 { get; set; } = [];
    }

    [Fact]
    public void BoolArrayClass_FailTest()
    {
        Assert.Throws<CommandDefinitionException>(() => CommandDescriptor.GetMemberDescriptors(typeof(BoolArrayClass)));
    }
}

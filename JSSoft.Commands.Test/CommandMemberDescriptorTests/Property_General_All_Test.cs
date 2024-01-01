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

public class Property_General_All_Test
{
    sealed class InstanceClass
    {
        [CommandProperty]
        public bool Bool { get; set; }

        [CommandProperty]
        public string String { get; set; } = string.Empty;

        [CommandProperty]
        public TypeCode Enum { get; set; }

        [CommandProperty]
        public AttributeTargets Flag { get; set; }

        [CommandProperty]
        public byte Byte { get; set; }

        [CommandProperty]
        public sbyte SByte { get; set; }

        [CommandProperty]
        public short Short { get; set; }

        [CommandProperty]
        public ushort UShort { get; set; }

        [CommandProperty]
        public int Int { get; set; }

        [CommandProperty]
        public uint UInt { get; set; }

        [CommandProperty]
        public long Long { get; set; }

        [CommandProperty]
        public ulong ULong { get; set; }

        [CommandProperty]
        public float Float { get; set; }

        [CommandProperty]
        public double Double { get; set; }

        [CommandProperty]
        public decimal Decimal { get; set; }
    }

    [Fact]
    public void Base_InstanceClass_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass));
        var actualNames = memberDescriptors.Select(item => item.MemberName);
        string[] expectedNames =
        [
            nameof(InstanceClass.Bool),
            nameof(InstanceClass.String),
            nameof(InstanceClass.Enum),
            nameof(InstanceClass.Flag),
            nameof(InstanceClass.Byte),
            nameof(InstanceClass.SByte),
            nameof(InstanceClass.Short),
            nameof(InstanceClass.UShort),
            nameof(InstanceClass.Int),
            nameof(InstanceClass.UInt),
            nameof(InstanceClass.Long),
            nameof(InstanceClass.ULong),
            nameof(InstanceClass.Float),
            nameof(InstanceClass.Double),
            nameof(InstanceClass.Decimal),
        ];
        Assert.Equivalent(expectedNames, actualNames);
    }

    sealed class InstancesClass
    {
        [CommandProperty]
        public bool[] Bools { get; set; } = [];

        [CommandProperty]
        public string[] Strings { get; set; } = [];

        [CommandProperty]
        public TypeCode[] Enums { get; set; } = [];

        [CommandProperty]
        public AttributeTargets[] Flags { get; set; } = [];

        [CommandProperty]
        public byte[] Bytes { get; set; } = [];

        [CommandProperty]
        public sbyte[] SBytes { get; set; } = [];

        [CommandProperty]
        public short[] Shorts { get; set; } = [];

        [CommandProperty]
        public ushort[] UShorts { get; set; } = [];

        [CommandProperty]
        public int[] Ints { get; set; } = [];

        [CommandProperty]
        public uint[] UInts { get; set; } = [];

        [CommandProperty]
        public long[] Longs { get; set; } = [];

        [CommandProperty]
        public ulong[] ULongs { get; set; } = [];

        [CommandProperty]
        public float[] Floats { get; set; } = [];

        [CommandProperty]
        public double[] Doubles { get; set; } = [];

        [CommandProperty]
        public decimal[] Decimals { get; set; } = [];
    }

    [Fact]
    public void Base_InstancesClass_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(InstancesClass));
        var actualNames = memberDescriptors.Select(item => item.MemberName);
        string[] expectedNames =
        [
            nameof(InstancesClass.Bools),
            nameof(InstancesClass.Strings),
            nameof(InstancesClass.Enums),
            nameof(InstancesClass.Flags),
            nameof(InstancesClass.Bytes),
            nameof(InstancesClass.SBytes),
            nameof(InstancesClass.Shorts),
            nameof(InstancesClass.UShorts),
            nameof(InstancesClass.Ints),
            nameof(InstancesClass.UInts),
            nameof(InstancesClass.Longs),
            nameof(InstancesClass.ULongs),
            nameof(InstancesClass.Floats),
            nameof(InstancesClass.Doubles),
            nameof(InstancesClass.Decimals),
        ];
        Assert.Equivalent(expectedNames, actualNames);
    }
}

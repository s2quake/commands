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

namespace JSSoft.Commands.Test.CommandParserTests;

public class ParseArrayPropertyTest()
{
    sealed class InstanceClass
    {
        [CommandProperty]
        public bool[] Booleans { get; set; } = [];

        [CommandProperty]
        public string[] Strings { get; set; } = [];

        [CommandProperty]
        public sbyte[] SBytes { get; set; } = [];

        [CommandProperty]
        public byte[] Bytes { get; set; } = [];

        [CommandProperty]
        public short[] Int16s { get; set; } = [];

        [CommandProperty]
        public ushort[] UInt16s { get; set; } = [];

        [CommandProperty]
        public int[] Int32s { get; set; } = [];

        [CommandProperty]
        public uint[] UInt32s { get; set; } = [];

        [CommandProperty]
        public long[] Int64s { get; set; } = [];

        [CommandProperty]
        public ulong[] UInt64s { get; set; } = [];

        [CommandProperty]
        public float[] Singles { get; set; } = [];

        [CommandProperty]
        public double[] Doubles { get; set; } = [];

        [CommandProperty]
        public decimal[] Decimals { get; set; } = [];
    }

    public static IEnumerable<object[]> TestData => new object[][]
    {
        [nameof(InstanceClass.Booleans), "--booleans", RandomUtility.NextBooleanArray(1, 10)],
        [nameof(InstanceClass.Strings), "--strings", RandomUtility.NextWordArray(1, 10)],
        [nameof(InstanceClass.SBytes), "--sbytes", RandomUtility.NextSByteArray(1, 10)],
        [nameof(InstanceClass.Bytes), "--bytes", RandomUtility.NextByteArray(1, 10)],
        [nameof(InstanceClass.Int16s), "--int16s", RandomUtility.NextInt16Array(1, 10)],
        [nameof(InstanceClass.UInt16s), "--uint16s", RandomUtility.NextUInt16Array(1, 10)],
        [nameof(InstanceClass.Int32s), "--int32s", RandomUtility.NextInt32Array(1, 10)],
        [nameof(InstanceClass.UInt32s), "--uint32s", RandomUtility.NextUInt32Array(1, 10)],
        [nameof(InstanceClass.Int64s), "--int64s", RandomUtility.NextInt64Array(1, 10)],
        [nameof(InstanceClass.UInt64s), "--uint64s", RandomUtility.NextUInt64Array(1, 10)],
        [nameof(InstanceClass.Singles), "--singles", RandomUtility.NextSingleArray(1, 10)],
        [nameof(InstanceClass.Doubles), "--doubles", RandomUtility.NextDoubleArray(1, 10)],
        [nameof(InstanceClass.Decimals), "--decimals", RandomUtility.NextDecimalArray(1, 10)],
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_Test(string name, string option, Array array)
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var expression = CommandTestUtility.GetExpression(array, ", ");
        var argumentLine = $"{option} {expression}";
        var memberDescriptors = from item in CommandDescriptor.GetMemberDescriptors(typeof(InstanceClass))
                                where item.MemberName != name
                                select item;

        parser.Parse(argumentLine);
        var actualValue = typeof(InstanceClass).GetProperty(name)!.GetValue(obj);

        Assert.Equivalent(array, actualValue);
    }
}

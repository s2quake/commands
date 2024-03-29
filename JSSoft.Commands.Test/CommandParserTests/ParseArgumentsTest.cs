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

public class ParseArgumentsTest()
{
    class BaseClass
    {
        [CommandProperty]
        public string Text { get; set; } = string.Empty;
    }

    sealed class BooleanClass : BaseClass
    {
        [CommandPropertyArray]
        public bool[] Arguments { get; set; } = [];
    }

    sealed class StringClass : BaseClass
    {
        [CommandPropertyArray]
        public string[] Arguments { get; set; } = [];
    }

    sealed class SByteClass : BaseClass
    {
        [CommandPropertyArray]
        public sbyte[] Arguments { get; set; } = [];
    }

    sealed class ByteClass : BaseClass
    {
        [CommandPropertyArray]
        public byte[] Arguments { get; set; } = [];
    }

    sealed class Int16Class : BaseClass
    {
        [CommandPropertyArray]
        public short[] Arguments { get; set; } = [];
    }

    sealed class UInt16Class : BaseClass
    {
        [CommandPropertyArray]
        public ushort[] Arguments { get; set; } = [];
    }

    sealed class Int32Class : BaseClass
    {
        [CommandPropertyArray]
        public int[] Arguments { get; set; } = [];
    }

    sealed class UInt32Class : BaseClass
    {
        [CommandPropertyArray]
        public uint[] Arguments { get; set; } = [];
    }

    sealed class Int64Class : BaseClass
    {
        [CommandPropertyArray]
        public long[] Arguments { get; set; } = [];
    }

    sealed class UInt64Class : BaseClass
    {
        [CommandPropertyArray]
        public ulong[] Arguments { get; set; } = [];
    }

    sealed class SingleClass : BaseClass
    {
        [CommandPropertyArray]
        public float[] Arguments { get; set; } = [];
    }

    sealed class DoubleClass : BaseClass
    {
        [CommandPropertyArray]
        public double[] Arguments { get; set; } = [];
    }

    sealed class DecimalClass : BaseClass
    {
        [CommandPropertyArray]
        public decimal[] Arguments { get; set; } = [];
    }

    public static IEnumerable<object[]> TestData => new object[][]
    {
        [typeof(BooleanClass), RandomUtility.NextBooleanArray(1, 10)],
        [typeof(StringClass), RandomUtility.NextWordArray(1, 10)],
        [typeof(SByteClass), RandomUtility.NextSByteArray(1, 10)],
        [typeof(ByteClass), RandomUtility.NextByteArray(1, 10)],
        [typeof(Int16Class), RandomUtility.NextInt16Array(1, 10)],
        [typeof(UInt16Class), RandomUtility.NextUInt16Array(1, 10)],
        [typeof(Int32Class), RandomUtility.NextInt32Array(1, 10)],
        [typeof(UInt32Class), RandomUtility.NextUInt32Array(1, 10)],
        [typeof(Int64Class), RandomUtility.NextInt64Array(1, 10)],
        [typeof(UInt64Class), RandomUtility.NextUInt64Array(1, 10)],
        [typeof(SingleClass), RandomUtility.NextSingleArray(1, 10)],
        [typeof(DoubleClass), RandomUtility.NextDoubleArray(1, 10)],
        [typeof(DecimalClass), RandomUtility.NextDecimalArray(1, 10)],
    };

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_Test(Type type, Array array)
    {
        var obj = Activator.CreateInstance(type)!;
        var parser = new CommandParser(obj);
        var expression = CommandTestUtility.GetExpression(array);
        var argumentLine = $"{expression}";

        parser.Parse(argumentLine);
        var actualValue = type.GetProperty("Arguments")!.GetValue(obj);

        Assert.Equivalent(array, actualValue);
    }

    [Theory]
    [MemberData(nameof(TestData))]
    public void ParseTest_WithDashDash_Test(Type type, Array array)
    {
        var obj = Activator.CreateInstance(type)!;
        var parser = new CommandParser(obj);
        var expression = CommandTestUtility.GetExpression(array);
        var argumentLine = $"--text s -- {expression}";

        parser.Parse(argumentLine);
        var actualValue = type.GetProperty("Arguments")!.GetValue(obj);

        Assert.Equivalent(array, actualValue);
    }
}

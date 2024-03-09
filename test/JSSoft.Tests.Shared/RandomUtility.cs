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

#pragma warning disable IDE0049

using System.Reflection;
using System.IO;
using System.Collections;

#if JSSOFT_COMMANDS
namespace JSSoft.Commands.Tests;
#endif
#if JSSOFT_TERMINALS
namespace JSSoft.Terminals.Tests;
#endif

public static class RandomUtility
{
    public const int AttemptCount = 10;
    private static readonly string[] words;
    private static readonly System.Random random = new(DateTime.Now.Millisecond);

    static RandomUtility()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = string.Join(".", typeof(RandomUtility).Namespace, "Resources", "words.txt");
        var resourceStream = assembly.GetManifestResourceStream(resourceName)!;
        using var stream = new StreamReader(resourceStream);
        var text = stream.ReadToEnd();
        int i = 0;
        using var sr1 = new StringReader(text);
        while (sr1.ReadLine() is string line1)
        {
            i++;
        }

        words = new string[i];
        i = 0;
        using var sr2 = new StringReader(text);
        while (sr2.ReadLine() is string line2)
        {
            words[i++] = line2;
        }
    }

    public static SByte NextSByte()
    {
        var bytes = new byte[1];
        random.NextBytes(bytes);
        return (SByte)bytes[0];
    }

    public static SByte[] NextSByteArray() => NextSByteArray(0, 10);

    public static SByte[] NextSByteArray(int maxLength) => NextSByteArray(1, maxLength);

    public static SByte[] NextSByteArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new SByte[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextSByte();
        }
        return items;
    }

    public static Byte NextByte()
    {
        var bytes = new byte[1];
        random.NextBytes(bytes);
        return bytes[0];
    }

    public static Byte[] NextByteArray() => NextByteArray(0, 10);

    public static Byte[] NextByteArray(int maxLength) => NextByteArray(1, maxLength);

    public static Byte[] NextByteArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Byte[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextByte();
        }
        return items;
    }

    public static Int16 NextInt16()
    {
        var bytes = new byte[2];
        random.NextBytes(bytes);
        return BitConverter.ToInt16(bytes, 0);
    }

    public static Int16[] NextInt16Array() => NextInt16Array(0, 10);

    public static Int16[] NextInt16Array(int maxLength) => NextInt16Array(1, maxLength);

    public static Int16[] NextInt16Array(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Int16[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextInt16();
        }
        return items;
    }

    public static UInt16 NextUInt16()
    {
        var bytes = new byte[2];
        random.NextBytes(bytes);
        return BitConverter.ToUInt16(bytes, 0);
    }

    public static UInt16[] NextUInt16Array() => NextUInt16Array(0, 10);

    public static UInt16[] NextUInt16Array(int maxLength) => NextUInt16Array(1, maxLength);

    public static UInt16[] NextUInt16Array(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new UInt16[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextUInt16();
        }
        return items;
    }

    public static Int32 NextInt32() => NextInt32(Int32.MinValue, Int32.MaxValue);

    public static Int32 NextInt32(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    public static Int32[] NextInt32Array() => NextInt32Array(0, 10);

    public static Int32[] NextInt32Array(int maxLength) => NextInt32Array(1, maxLength);

    public static Int32[] NextInt32Array(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Int32[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextInt32();
        }
        return items;
    }

    public static UInt32 NextUInt32()
    {
        var bytes = new byte[4];
        random.NextBytes(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    public static UInt32[] NextUInt32Array() => NextUInt32Array(0, 10);

    public static UInt32[] NextUInt32Array(int maxLength) => NextUInt32Array(1, maxLength);

    public static UInt32[] NextUInt32Array(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new UInt32[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextUInt32();
        }
        return items;
    }

    public static Int64 NextInt64()
    {
        var bytes = new byte[8];
        random.NextBytes(bytes);
        return BitConverter.ToInt64(bytes, 0);
    }

    public static Int64[] NextInt64Array() => NextInt64Array(0, 10);

    public static Int64[] NextInt64Array(int maxLength) => NextInt64Array(1, maxLength);

    public static Int64[] NextInt64Array(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Int64[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextInt64();
        }
        return items;
    }

    public static UInt64 NextUInt64()
    {
        var bytes = new byte[8];
        random.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }

    public static UInt64[] NextUInt64Array() => NextUInt64Array(0, 10);

    public static UInt64[] NextUInt64Array(int maxLength) => NextUInt64Array(1, maxLength);

    public static UInt64[] NextUInt64Array(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new UInt64[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextUInt64();
        }
        return items;
    }

    public static Single NextSingle()
    {
#if NET6_0_OR_GREATER
        return random.NextSingle();
#else
        return (float)random.NextDouble() * random.Next();
#endif
    }

    public static Single[] NextSingleArray() => NextSingleArray(0, 10);

    public static Single[] NextSingleArray(int maxLength) => NextSingleArray(1, maxLength);

    public static Single[] NextSingleArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Single[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextSingle();
        }
        return items;
    }

    public static Double NextDouble()
    {
        return random.NextDouble();
    }

    public static Double[] NextDoubleArray() => NextDoubleArray(0, 10);

    public static Double[] NextDoubleArray(int maxLength) => NextDoubleArray(1, maxLength);

    public static Double[] NextDoubleArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Double[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextDouble();
        }
        return items;
    }

    public static Decimal NextDecimal()
    {
        return (Decimal)random.NextDouble();
    }

    public static Decimal[] NextDecimalArray() => NextDecimalArray(0, 10);

    public static Decimal[] NextDecimalArray(int maxLength) => NextDecimalArray(1, maxLength);

    public static Decimal[] NextDecimalArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new Decimal[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextDecimal();
        }
        return items;
    }
    // public static float NextFloat() => NextSingle();

    public static string NextWord() => NextWord(item => true);

    public static string NextWord(Func<string, bool> predicate)
    {
        for (var i = 0; i < AttemptCount; i++)
        {
            var index = NextInt32(0, words.Length);
            var item = words[index];
            if (predicate(item) == true)
            {
                return item;
            }
        }
        throw new InvalidOperationException("No value was found that matches the condition.");
    }

    public static string[] NextWordArray() => NextWordArray(0, 10);

    public static string[] NextWordArray(int maxLength) => NextWordArray(1, maxLength);

    public static string[] NextWordArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new string[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextWord();
        }
        return items;
    }

    public static int NextLength() => NextLength(0, 10);

    public static int NextLength(int maxLength) => NextLength(1, maxLength);

    public static int NextLength(int minLength, int maxLength)
    {
        return random.Next(minLength, maxLength);
    }

    public static T NextEnum<T>() where T : Enum
    {
        if (Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) != null)
            throw new InvalidOperationException("Flag type cannot be used.");

        var values = Enum.GetValues(typeof(T));
        var index = NextInt32(0, values.Length);
        return (T)values.GetValue(index)!;
    }

    public static T NextUnspecifiedEnum<T>() where T : Enum
    {
        if (Attribute.GetCustomAttribute(typeof(T), typeof(FlagsAttribute)) != null)
            throw new InvalidOperationException("Flag type cannot be used.");

        var values = Enum.GetValues(typeof(T)).OfType<T>().ToArray();
        for (var i = 0; i < AttemptCount; i++)
        {
            var value = GetRandomValue(typeof(T));
            if (values.Contains(value) != true)
                return value;
        }

        throw new InvalidOperationException("No value was found that matches the condition.");

        static T GetRandomValue(Type enumType) => enumType switch
        {
            Type t when Enum.GetUnderlyingType(t) == typeof(SByte) => (T)(object)NextSByte(),
            Type t when Enum.GetUnderlyingType(t) == typeof(Byte) => (T)(object)NextByte(),
            Type t when Enum.GetUnderlyingType(t) == typeof(Int16) => (T)(object)NextInt16(),
            Type t when Enum.GetUnderlyingType(t) == typeof(UInt16) => (T)(object)NextUInt16(),
            Type t when Enum.GetUnderlyingType(t) == typeof(Int32) => (T)(object)NextInt32(),
            Type t when Enum.GetUnderlyingType(t) == typeof(UInt32) => (T)(object)NextUInt32(),
            Type t when Enum.GetUnderlyingType(t) == typeof(Int64) => (T)(object)NextInt64(),
            Type t when Enum.GetUnderlyingType(t) == typeof(UInt64) => (T)(object)NextUInt64(),
            _ => throw new NotSupportedException(),
        };
    }

    public static T? RandomOrDefault<T>(this IEnumerable<T> enumerable)
    {
        return RandomOrDefault(enumerable, item => true);
    }

    public static T? RandomOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        var list = enumerable.Where(predicate).ToArray();
        if (list.Length != 0 == false)
            return default;
        var count = list.Length;
        var index = NextInt32(0, count);
        return list[index];
    }

    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return Random(enumerable, item => true);
    }

    public static T Random<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
    {
        if (enumerable.Any() == false)
            throw new InvalidOperationException("there is no random item that matches the condition.");
        return RandomOrDefault(enumerable, predicate)!;
    }

    public static T? WeightedRandomOrDefault<T>(this IEnumerable<T> enumerable, Func<T, int> weightSelector)
    {
        return WeightedRandomOrDefault(enumerable, weightSelector, item => true);
    }

    public static T? WeightedRandomOrDefault<T>(this IEnumerable<T> enumerable, Func<T, int> weightSelector, Func<T, bool> predicate)
    {
        var totalWeight = 0;
        foreach (var item in enumerable.Where(predicate))
        {
            var weight = weightSelector(item);
            if (weight < 0)
                throw new ArgumentException("weight must be greater or equals than zero");
            totalWeight += weight;
        }

        var value = NextInt32(0, totalWeight) + 1;

        totalWeight = 0;
        foreach (var item in enumerable.Where(predicate))
        {
            var weight = weightSelector(item);
            totalWeight += weight;
            if (value <= totalWeight)
                return item;
        }

        return default;
    }

    public static T WeightedRandom<T>(this IEnumerable<T> enumerable, Func<T, int> weightSelector)
    {
        return WeightedRandom(enumerable, weightSelector, item => true);
    }

    public static T WeightedRandom<T>(this IEnumerable<T> enumerable, Func<T, int> weightSelector, Func<T, bool> predicate)
    {
        var item = WeightedRandomOrDefault(enumerable, weightSelector, predicate);
        return item == null ? throw new InvalidOperationException() : item;
    }

    public static string NextString()
    {
        return NextString(false);
    }

    public static string NextString(bool multiline)
    {
        string s = string.Empty;
        int count = NextInt32(1, 20);
        for (int i = 0; i < count; i++)
        {
            s += NextWord();
            if (i > 0 && multiline == true && Within(5) == true)
            {
                s += Environment.NewLine;
            }
            else if (i + 1 != count)
            {
                s += " ";
            }
        }

        return s;
    }

    public static bool NextBoolean()
    {
        return NextInt32(0, 2) == 0;
    }

    public static bool[] NextBooleanArray() => NextBooleanArray(0, 10);

    public static bool[] NextBooleanArray(int maxLength) => NextBooleanArray(1, maxLength);

    public static bool[] NextBooleanArray(int minLength, int maxLength)
    {
        var length = NextLength(minLength, maxLength);
        var items = new bool[length];
        for (var i = 0; i < length; i++)
        {
            items[i] = NextBoolean();
        }
        return items;
    }

    public static bool Within(int percent)
    {
        var value = NextInt32(0, 100);
        return percent >= value;
    }
}

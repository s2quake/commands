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

using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JSSoft.Commands.Test;

public static class RandomUtility
{
    public const int AttemptCount = 10;
    // private static readonly object lockobj = new();
    private static readonly string[] words;
    // private static readonly byte[] longBytes = new byte[8];
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

    // [Obsolete]
    // public static int Next(int min, int max)
    // {
    //     lock (lockobj)
    //     {
    //         count++;
    //         if (count >= 1000)
    //         {
    //             random = new System.Random(DateTime.Now.Millisecond);
    //             count = 0;
    //         }
    //         return random.Next(min, max);
    //     }
    // }

    // [Obsolete]
    // public static int Next(int max)
    // {
    //     lock (lockobj)
    //     {
    //         count++;
    //         if (count >= 1000)
    //         {
    //             random = new System.Random(DateTime.Now.Millisecond);
    //             count = 0;
    //         }
    //         return random.Next(max);
    //     }
    // }

    // [Obsolete]
    // public static long NextLong(long max)
    // {
    //     return NextLong(0, max);
    // }

    // [Obsolete]
    // public static long NextLong(long min, long max)
    // {
    //     var bytes = BitConverter.GetBytes((long)0);
    //     random.NextBytes(bytes);
    //     var value = (long)BitConverter.ToInt64(bytes, 0);
    //     return Math.Abs(value % (max - min)) + min;
    // }

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

    // public static string NextName()
    // {
    //     string name;

    //     while (NameValidator.VerifyName((name = NextWord())) == false)
    //         ;
    //     return name;
    // }

    // public static string NextInvalidName()
    // {
    //     string name;

    //     while (NameValidator.VerifyName((name = NextWord())) == true)
    //         ;
    //     return name;
    // }

    // public static string NextCategoryName()
    // {
    //     string name;

    //     while (NameValidator.VerifyCategoryName((name = NextWord())) == false)
    //         ;
    //     return name;
    // }

    // public static string NextCategoryPath()
    // {
    //     var depth = NextInt32(0, 5);
    //     return NextCategoryPath(depth);
    // }

    // public static string NextCategoryPath(int depth)
    // {
    //     var items = new List<string>();
    //     for (var i = 0; i < depth; i++)
    //     {
    //         items.Add(NextName());
    //     }
    //     var path = string.Join(PathUtility.Separator, items);
    //     if (path == string.Empty)
    //         return PathUtility.Separator;
    //     return path.WrapSeparator();
    // }

    // public static string[] NextCategoryPaths(int depth, int count)
    // {
    //     if (depth < 0)
    //         throw new ArgumentOutOfRangeException(nameof(depth));
    //     if (count < 0)
    //         throw new ArgumentOutOfRangeException(nameof(count));

    //     var c = 0;
    //     var itemList = new List<string>(depth * count);
    //     while (c < count)
    //     {
    //         var parentPath = "/";
    //         var d = NextInt32(0, depth);
    //         for (var i = 0; i < d; i++)
    //         {
    //             var name = NextCategoryName();
    //             var categoryName = new CategoryName(parentPath, name);
    //             itemList.Add(categoryName);
    //             parentPath = categoryName;
    //             c++;
    //         }
    //     }
    //     return [.. itemList];
    // }

    // public static string NextInvalidCategoryPath()
    // {
    //     var depth = NextInt32(0, 5);
    //     return NextInvalidCategoryPath(depth);
    // }

    // public static string NextInvalidCategoryPath(int depth)
    // {
    //     var items = new List<string>();
    //     for (var i = 0; i < depth; i++)
    //     {
    //         items.Add(NextInvalidName());
    //     }
    //     var path = string.Join(PathUtility.Separator, items);
    //     if (path == string.Empty)
    //         return string.Empty;
    //     return path.WrapSeparator();
    // }

    // public static string NextIdentifier()
    // {
    //     string name;

    //     while (IdentifierValidator.Check((name = NextWord())) == false)
    //         ;
    //     return name;
    // }

    // public static string NextInvalidIdentifier()
    // {
    //     string name;

    //     while (IdentifierValidator.Check((name = NextWord())) == true)
    //         ;
    //     return name;
    // }

    public static int NextLength() => NextLength(0, 10);

    public static int NextLength(int maxLength) => NextLength(1, maxLength);

    public static int NextLength(int minLength, int maxLength)
    {
        return random.Next(minLength, maxLength);
    }

    public static T NextEnum<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        var index = NextInt32(0, values.Length);
        return (T)values.GetValue(index)!;
    }

    // [Obsolete]
    // public static T Next<T>()
    // {
    //     return (T)Next(typeof(T));
    // }

    // [Obsolete]
    // public static object Next(Type type)
    // {
    //     if (type == typeof(System.Boolean))
    //     {
    //         if (Next(2) == 0)
    //             return false;
    //         return true;
    //     }
    //     else if (type == typeof(string))
    //     {
    //         return NextIdentifier();
    //     }
    //     else if (type == typeof(float))
    //     {
    //         return (float)random.NextDouble() * random.Next();
    //     }
    //     else if (type == typeof(double))
    //     {
    //         return (double)random.NextDouble() * random.Next();
    //     }
    //     else if (type == typeof(sbyte))
    //     {
    //         var bytes = new byte[1];
    //         random.NextBytes(bytes);
    //         return (sbyte)bytes[0];
    //     }
    //     else if (type == typeof(byte))
    //     {
    //         var bytes = new byte[1];
    //         random.NextBytes(bytes);
    //         return bytes[0];
    //     }
    //     else if (type == typeof(short))
    //     {
    //         var bytes = new byte[2];
    //         random.NextBytes(bytes);
    //         return BitConverter.ToInt16(bytes, 0);
    //     }
    //     else if (type == typeof(ushort))
    //     {
    //         var bytes = new byte[2];
    //         random.NextBytes(bytes);
    //         return BitConverter.ToUInt16(bytes, 0);
    //     }
    //     else if (type == typeof(int))
    //     {
    //         var bytes = new byte[4];
    //         random.NextBytes(bytes);
    //         return BitConverter.ToInt32(bytes, 0);
    //     }
    //     else if (type == typeof(uint))
    //     {
    //         var bytes = new byte[4];
    //         random.NextBytes(bytes);
    //         return BitConverter.ToUInt32(bytes, 0);
    //     }
    //     else if (type == typeof(long))
    //     {
    //         random.NextBytes(longBytes);
    //         return (long)BitConverter.ToInt64(longBytes, 0);
    //     }
    //     else if (type == typeof(ulong))
    //     {
    //         random.NextBytes(longBytes);
    //         return (ulong)BitConverter.ToUInt64(longBytes, 0);
    //     }
    //     else if (type == typeof(System.DateTime))
    //     {
    //         var year = Next(1970, 2050 + 1);
    //         var month = Next(1, 12 + 1);
    //         var day = Next(1, 12 + 1);

    //         var minValue = new DateTime(1970, 1, 1, 0, 0, 0).Ticks;
    //         var maxValue = new DateTime(2050, 12, 31, 0, 0, 0).Ticks;
    //         var value = NextLong(minValue, maxValue) / (long)10000000 * (long)10000000;
    //         return new DateTime(value);
    //     }
    //     else if (type == typeof(System.TimeSpan))
    //     {
    //         return new TimeSpan(NextLong(new TimeSpan(365, 0, 0, 0).Ticks));
    //     }
    //     else if (type == typeof(Guid))
    //     {
    //         return Guid.NewGuid();
    //     }
    //     else if (type.IsEnum == true)
    //     {
    //         string[] names = Enum.GetNames(type);

    //         if (names.Length == 0)
    //             throw new InvalidOperationException();

    //         string name = string.Empty;

    //         if (Attribute.GetCustomAttribute(type, typeof(FlagsAttribute)) is FlagsAttribute)
    //         {
    //             var query = (from item in names
    //                          where Next(3) == 0
    //                          select item).ToArray();

    //             if (query.Any())
    //                 name = string.Join(", ", query);
    //             else
    //                 return names.First();
    //         }
    //         else
    //         {
    //             name = names[Next(names.Length)];
    //         }

    //         return Enum.Parse(type, name);
    //     }

    //     throw new Exception("占쏙옙占쏙옙占쏙옙占쏙옙 占십댐옙 타占쏙옙占쌉니댐옙.");
    // }

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

    // public static long NextBit()
    // {
    //     if (RandomUtility.Within(1) == true)
    //         return 0;
    //     return 1 << random.Next(32);
    // }

    // public static TagInfo NextTags()
    // {
    //     var value = NextInt32(0, 10);

    //     if (value >= 9)
    //     {
    //         return TagInfo.All;
    //     }
    //     else if (value >= 8)
    //     {
    //         return TagInfo.None;
    //     }
    //     else
    //     {
    //         var items = new List<string>();
    //         for (var i = 0; i < NextInt32(1, 4); i++)
    //         {
    //             items.Add(NextIdentifier());
    //         }
    //         return (TagInfo)string.Join($"{TagInfo.Separator} ", items);
    //     }
    // }

    public static bool Within(int percent)
    {
        var value = NextInt32(0, 100);
        return percent >= value;
    }
}

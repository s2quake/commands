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

using System.IO;

namespace JSSoft.Commands.Extensions;

public static class TextWriterExtensions
{
    public static void PrintItems<T>(this TextWriter @this, IEnumerable<T> items)
    {
        var query = from item in typeof(T).GetProperties() ?? []
                    where item.PropertyType.IsArray == false
                    select item;

        var headers = from item in query
                      let displayName = AttributeUtility.GetDisplayName(item)
                      select displayName != string.Empty ? displayName : item.Name;

        var dataBuilder = new TableDataBuilder(headers.ToArray());

        foreach (var item in items)
        {
            dataBuilder.Add(query.Select(i => i.GetValue(item, null)).ToArray());
        }

        @this.PrintTableData(dataBuilder.Data, true);
    }

    public static void Print(this TextWriter writer, TableDataBuilder tableData)
    {
        PrintTableData(writer, tableData.Data, tableData.HasHeader);
    }

    public static void PrintTableData(this TextWriter @this, string[][] itemsArray, bool hasHeader)
    {
        var count = itemsArray.First().Length;

        var lengths = new int[count];

        for (var x = 0; x < count; x++)
        {
            var len = 0;
            for (var y = 0; y < itemsArray.Length; y++)
            {
                len = Math.Max(GetLength(itemsArray[y][x]), len);
            }
            lengths[x] = len + (4 - (len % 4));
        }

        for (var y = 0; y < itemsArray.Length; y++)
        {
            for (var x = 0; x < itemsArray[y].Length; x++)
            {
                var pad = lengths[x] - GetLength(itemsArray[y][x]);
                @this.Write(itemsArray[y][x]);
                @this.Write(string.Empty.PadRight(pad));
                @this.Write(" ");
            }
            @this.WriteLine();

            if (y != 0 || hasHeader == false)
                continue;

            for (var x = 0; x < itemsArray[y].Length; x++)
            {
                var pad = lengths[x];
                @this.Write(string.Empty.PadRight(pad, '-'));
                @this.Write(" ");
            }
            @this.WriteLine();
        }
    }

    public static void Print<T>(this TextWriter @this, T[] items)
    {
        Print<T>(@this, items, (i, w, s) => w.Write(s), item => $"{item}");
    }

    public static void Print<T>(this TextWriter @this, T[] items, Action<T, TextWriter, string> action)
    {
        Print<T>(@this, items, action, item => $"{item}");
    }

    // <summary>
    // linux에 ls 명령처럼 단순 문자열을 위에서 아래로 좌에서 우로 정렬해서 출력하는 기능
    // </summary>
    public static void Print<T>(this TextWriter @this, T[] items, Action<T, TextWriter, string> action, Func<T, string> selector)
    {
        var maxWidth = 80;
        var lineCount = 4;

        while (true)
        {
            var lines = new List<string>[lineCount];
            var objs = new List<T>[lineCount];
            var lengths = new int[lineCount];
            var columns = new List<int>();

            for (var i = 0; i < items.Length; i++)
            {
                var y = i % lineCount;
                var item = selector(items[i]);
                if (lines[y] == null)
                {
                    lines[y] = [];
                    objs[y] = [];
                    lengths[y] = GetLength(item) + 2;
                }
                else
                {
                    lengths[y] += GetLength(item) + 2;
                }

                var c = lines[y].Count;
                lines[y].Add(item);
                objs[y].Add(items[i]);

                if (columns.Count < lines[y].Count)
                    columns.Add(0);

                columns[c] = Math.Max(columns[c], GetLength(item) + 2);
            }

            var canPrint = true;
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;
                var c = 0;
                for (var j = 0; j < lines[i].Count; j++)
                {
                    c += columns[j];
                }
                if (c >= maxWidth)
                    canPrint = false;
            }

            if (canPrint == false)
            {
                lineCount++;
                continue;
            }

            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null)
                    continue;
                for (var j = 0; j < lines[i].Count; j++)
                {
                    var obj = objs[i][j];
                    var text = lines[i][j].PadRight(columns[j]);
                    action(obj, @this, text);
                }
                @this.WriteLine();
            }

            break;
        }
    }

    public static void Print<T>(this TextWriter @this, IDictionary<string, T> items)
    {
        Print<T>(@this, items, (o, a) => a(), item => $"{item}");
    }

    public static void Print<T>(this TextWriter @this, IDictionary<string, T> items, Action<T, Action> action)
    {
        Print<T>(@this, items, action, item => $"{item}");
    }

    /// <summary>
    /// 라벨과 값이 존재하는 아이템을 {0} : {1} 형태로 출력하는 기능
    /// </summary>
    public static void Print<T>(this TextWriter @this, IDictionary<string, T> items, Action<T, Action> action, Func<T, string> selector)
    {
        var maxWidth = items.Keys.Max(item => item.Length);

        foreach (var item in items)
        {
            var key = item.Key.PadRight(maxWidth);
            var text = $"{key} : {selector(item.Value)}";
            action(item.Value, () => @this.WriteLine(text));
        }
    }

    private static int GetLength(string s)
    {
        return s.Length;
    }
}

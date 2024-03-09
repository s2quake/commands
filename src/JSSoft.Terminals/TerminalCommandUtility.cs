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

#pragma warning disable SYSLIB1045

using System.Text;
using System.Text.RegularExpressions;

namespace JSSoft.Terminals;

static class CommandUtility
{
    public const char ItemSperator = ',';
    public const string Delimiter = "--";
    public const string ShortDelimiter = "-";
    public const string OptionPattern = "[a-zA-Z](?:(?<!-)-(?!$)|[a-zA-Z0-9])+";
    public const string ShortOptionPattern = "[a-zA-Z]";
    private const string doubleQuotesPattern = "(?<!\\\\)\"(?:\\\\.|(?<!\\\\)[^\"])*(?:\\\\.\"|(?<!\\\\)\")";
    private const string singleQuotePattern = "'[^']*'";
    private const string stringPattern = "(?:(?<!\\\\)[^\"'\\s,]|(?<=\\\\).)+";
    private const string separatorPattern = "\\s*,*\\s*";
    private static readonly string fullPattern;
    private static readonly IReadOnlyDictionary<string, Func<string, string>> replacerByName = new Dictionary<string, Func<string, string>>
    {
        { "double", UnescapeDoubleQuotes },
        { "single", UnescapeSingleQuote },
        { "string", UnescapeString },
    };
    private static readonly Type[] supportedTypes =
    [
        typeof(bool),
        typeof(string),
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(float),
        typeof(double),
        typeof(decimal),
    ];

    static CommandUtility()
    {
        var patterns = new string[]
        {
            $"(?<double>{doubleQuotesPattern}){separatorPattern}",
            $"(?<single>{singleQuotePattern}){separatorPattern}",
            $"(?<string>{stringPattern}){separatorPattern}",
        };
        fullPattern = string.Join("|", patterns);
    }

    public static bool TrySplit(string argumentLine, out string[] items)
    {
        try
        {
            items = GetMatches(argumentLine);
            return true;
        }
        catch
        {
            items = [];
            return false;
        }
    }

    public static string[] Split(string argumentLine)
    {
        return GetMatches(argumentLine);
    }

    public static string Join(params string[] items)
    {
        var itemList = new List<string>(items.Length);
        foreach (var item in items)
        {
            itemList.Add(item.Contains(' ') == true ? $"\"{item}\"" : item);
        }
        return string.Join(" ", itemList);
    }

    public static bool TrySplitCommandLine(string commandLine, out string commandName, out string[] commandArguments)
    {
        try
        {
            if (TrySplit(commandLine, out var items) == true)
            {
                commandName = items[0];
                commandArguments = items.Skip(1).ToArray();
                return true;
            }
        }
        catch
        {
        }
        commandName = string.Empty;
        commandArguments = [];
        return false;
    }

    public static (string commandName, string[] commandArguments) SplitCommandLine(string commandLine)
    {
        try
        {
            var items = Split(commandLine);
            var commandName = items[0];
            var commandArguments = items.Skip(1).ToArray();
            return (commandName, commandArguments);
        }
        catch (Exception e)
        {
            throw new ArgumentException($"CommandLine '{commandLine}' cannot be split.", nameof(commandLine), e);
        }
    }

    public static MatchCollection MatchCompletion(string text)
    {
        return Regex.Matches(text, fullPattern);
    }

    public static bool IsMultipleSwitch(string argument)
    {
        return Regex.IsMatch(argument, @$"^{ShortDelimiter}{ShortOptionPattern}{{2,}}$");
    }

    public static bool IsOption(string argument)
    {
        return Regex.IsMatch(argument, $"^({Delimiter}{OptionPattern}|{ShortDelimiter}{ShortOptionPattern})$");
    }

    public static string ToSpinalCase(string text)
    {
        return Regex.Replace(text, @"([a-z])([A-Z])", "$1-$2").ToLower();
    }

    public static string WrapDoubleQuotes(string text)
    {
        return $"\"{text.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
    }

    public static bool TryWrapDoubleQuotes(string text, out string wrappedText)
    {
        if (text.Contains('\\') == true || text.Contains('"') == true || text.Contains('\'') == true || text.Contains(' ') == true)
        {
            wrappedText = WrapDoubleQuotes(text);
            return true;
        }
        wrappedText = text;
        return false;
    }

    public static int GetBufferWidth() => Console.IsOutputRedirected == true ? int.MaxValue : Console.BufferWidth;

    public static bool IsSupportedType(Type value)
    {
        if (Nullable.GetUnderlyingType(value) != null &&
            value.GenericTypeArguments.Length == 1 &&
            IsSupportedType(value.GenericTypeArguments[0]) == true)
        {
            return true;
        }
        else if (supportedTypes.Contains(value) == true)
        {
            return true;
        }
        else if (value.IsEnum == true)
        {
            return true;
        }
        else if (value.IsArray == true &&
                value.HasElementType == true &&
                IsSupportedType(value.GetElementType()!) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal static string GetLabelString(string label, int labelWidth, int indentSpaces)
    {
        var text = label.PadRight(labelWidth);
        if (labelWidth <= label.Length)
            return label.PadRight(label.Length + 1);
        var l = text.Length % indentSpaces;
        return text.PadRight(l);
    }

    internal static string ToSpinalCase(Type type)
    {
        var name = Regex.Replace(type.Name, @"(Command)$", string.Empty);
        return Regex.Replace(name, @"([a-z])([A-Z])", "$1-$2").ToLower();
    }

    internal static string GetExecutionName(string name, string[] aliases)
    {
        var sb = new StringBuilder(name.Length + 2 + aliases.Length + aliases.Sum(item => item.Length));
        sb.Append(name);
        if (aliases.Length > 0)
        {
            sb.Append($"({string.Join(",", aliases)})");
        }
        return sb.ToString();
    }

    internal static bool IsEmptyArgs(string[] args) => args.Length == 0 || args[0] == string.Empty;

    internal static string Join(string separator, IEnumerable<string> items)
    {
        return string.Join(separator, items.Where(item => item != string.Empty));
    }

    private static string[] GetMatches(string text)
    {
        var matches = Regex.Matches(text, fullPattern);
        var etc = text;
        for (var i = matches.Count - 1; i >= 0; i--)
        {
            etc = etc.Remove(matches[i].Index, matches[i].Length);
        }
        if (etc.Trim() != string.Empty)
        {
            throw new ArgumentException("", nameof(text));
        }

        var itemList = new List<string>(matches.Count);
        var arrayList = new List<string>();
        var sb = new StringBuilder(text.Length);
        for (var i = 0; i < matches.Count; i++)
        {
            var item = matches[i];
            var (name, value, space, array) = GetMatchInfo(item);
            if (array > 0)
            {
                arrayList.Add(value);
                arrayList.AddRange(Enumerable.Repeat(string.Empty, array - 1));
            }
            else if (arrayList.Count > 0)
            {
                arrayList.Add(value);
                itemList.Add(JoinList(arrayList));
                arrayList.Clear();
            }
            else
            {
                sb.Append(value);
                if (space > 0 || i == matches.Count - 1)
                {
                    itemList.Add(sb.ToString());
                    sb.Clear();
                }
            }
        }
        if (arrayList.Count > 0)
        {
            itemList.Add(JoinList(arrayList));
            arrayList.Clear();
        }
        else if (sb.Length > 0)
        {
            itemList.Add(sb.ToString());
            sb.Clear();
        }

        return [.. itemList];
    }

    private static (string name, string value, int space, int array) GetMatchInfo(Match match)
    {
        if (FindGroup(match, out var group, out var index) == false || group.Name == "etc")
        {
            throw new ArgumentException($"‘{match.Value}’ is an invalid string.: [{match.Index} .. {match.Index + match.Length}]", nameof(match));
        }

        var value = group.Value;
        var value0 = match.Value.Substring(value.Length);
        var value1 = value0.TrimEnd();
        var value2 = value1.TrimEnd(',');
        var value3 = value2.TrimEnd();
        var array = value1.Length - value2.Length;
        var space = value0.Length - value1.Length + (value2.Length - value3.Length);
        var replacer = replacerByName.TryGetValue(group.Name, out var r) == true ? r : (s) => string.Empty;
        return (group.Name, replacer(value), space, array);
    }

    private static string JoinList(IList<string> itemList)
    {
        for (var i = 0; i < itemList.Count; i++)
        {
            var item = itemList[i];
            itemList[i] = item.Contains(' ') == true ? $"'{item}'" : item;
        }
        return string.Join(",", itemList);
    }

    private static bool FindGroup(Match match, out Group group, out int index)
    {
        for (var i = 1; i < match.Groups.Count; i++)
        {
            if (match.Groups[i].Success == true)
            {
                group = match.Groups[i];
                index = i - 1;
                return true;
            }
        }
        group = match.Groups[0];
        index = -1;
        return false;
    }

    private static string UnescapeDoubleQuotes(string text)
    {
        var text1 = Regex.Replace(text, "^\"(.*)\"$", "$1", RegexOptions.Singleline);
        return Regex.Replace(text1, "\\\\(\\\\|\\\")", "$1", RegexOptions.Singleline);
    }

    private static string UnescapeSingleQuote(string text)
    {
        return Regex.Replace(text, "^'(.*)'$", "$1");
    }

    private static string EscapeCharacter(string text)
    {
        return Regex.Replace(text, "\\\\(.)", "$1");
    }

    private static string UnescapeString(string text)
    {
        return Regex.Replace(text, "(\\\\)(.)", "$2");
    }
}

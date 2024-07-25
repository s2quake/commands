// <copyright file="CommandUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#pragma warning disable SYSLIB1045

using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

#if JSSOFT_TERMINALS
namespace JSSoft.Terminals;

static class CommandUtility
#else
namespace JSSoft.Commands;

/// <summary>
/// See this(https://rubular.com/r/Jsdc0Vz2FNiTaM).
/// </summary>
public static partial class CommandUtility
#endif
{
    public const char ItemSperator = ',';
    public const string Delimiter = "--";
    public const string ShortDelimiter = "-";
    public const string OptionPattern = "[a-zA-Z](?:(?<!-)-(?!$)|[a-zA-Z0-9])+";
    public const string ShortOptionPattern = "[a-zA-Z]";
    private const string DoubleQuotesPattern
        = "(?<!\\\\)\"(?:\\\\.|(?<!\\\\)[^\"])*(?:\\\\.\"|(?<!\\\\)\")";

    private const string SingleQuotePattern = "'[^']*'";
    private const string StringPattern = "(?:(?<!\\\\)[^\"'\\s,]|(?<=\\\\).)+";
    private const string SeparatorPattern = "\\s*,*\\s*";
    private static readonly string FullPattern = string.Join(
        "|",
        $"(?<double>{DoubleQuotesPattern}){SeparatorPattern}",
        $"(?<single>{SingleQuotePattern}){SeparatorPattern}",
        $"(?<string>{StringPattern}){SeparatorPattern}");

    private static readonly Dictionary<string, Func<string, string>> ReplacerByName = new()
    {
        { "double", UnescapeDoubleQuotes },
        { "single", UnescapeSingleQuote },
        { "string", UnescapeString },
    };

    private static readonly Type[] SupportedTypes =
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

    public static int BufferWidth
        => Console.IsOutputRedirected == true ? int.MaxValue : Console.BufferWidth;

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

    public static string[] Split(string argumentLine) => GetMatches(argumentLine);

    public static string Join(params string[] items)
    {
        var itemList = new List<string>(items.Length);
        foreach (var item in items)
        {
            itemList.Add(item.Contains(' ') == true ? $"\"{item}\"" : item);
        }

        return string.Join(" ", itemList);
    }

    public static bool TrySplitCommandLine(
        string commandLine, out string commandName, out string[] commandArguments)
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
            // do nothing
        }

        commandName = string.Empty;
        commandArguments = [];
        return false;
    }

    public static (string CommandName, string[] CommandArguments) SplitCommandLine(
        string commandLine)
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
            var message = $"CommandLine '{commandLine}' cannot be split.";
            throw new ArgumentException(message, nameof(commandLine), e);
        }
    }

    public static MatchCollection MatchCompletion(string text) => Regex.Matches(text, FullPattern);

    public static bool IsMultipleSwitch(string argument)
        => Regex.IsMatch(argument, @$"^{ShortDelimiter}{ShortOptionPattern}{{2,}}$");

    public static bool IsOption(string argument)
        => Regex.IsMatch(
            input: argument,
            pattern: $"^({Delimiter}{OptionPattern}|{ShortDelimiter}{ShortOptionPattern})$");

    public static string ToSpinalCase(string text)
        => Regex.Replace(text, @"([a-z])([A-Z])", "$1-$2").ToLower();

    public static string WrapDoubleQuotes(string text)
        => $"\"{text.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";

    public static bool TryWrapDoubleQuotes(string text, out string wrappedText)
    {
        if (text.Contains('\\') == true
            || text.Contains('"') == true
            || text.Contains('\'') == true
            || text.Contains(' ') == true)
        {
            wrappedText = WrapDoubleQuotes(text);
            return true;
        }

        wrappedText = text;
        return false;
    }

    public static bool IsSupportedType(Type value)
    {
        if (Nullable.GetUnderlyingType(value) is not null &&
            value.GenericTypeArguments.Length == 1 &&
            IsSupportedType(value.GenericTypeArguments[0]) == true)
        {
            return true;
        }
        else if (SupportedTypes.Contains(value) == true)
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
            var converter = TypeDescriptor.GetConverter(value);
            return converter.CanConvertFrom(typeof(string));
        }
    }

    internal static string ToSpinalCase(Type type)
    {
        var name = Regex.Replace(type.Name, @"(Command)$", string.Empty);
        return Regex.Replace(name, @"([a-z])([A-Z])", "$1-$2").ToLower();
    }

    internal static string GetExecutionName(string name, string[] aliases)
    {
        var capacity = name.Length + 2 + aliases.Length + aliases.Sum(item => item.Length);
        var sb = new StringBuilder(capacity);
        sb.Append(name);
        if (aliases.Length > 0)
        {
            sb.Append($"({string.Join(",", aliases)})");
        }

        return sb.ToString();
    }

    internal static bool IsEmptyArgs(string[] args) => args.Length == 0 || args[0] == string.Empty;

    internal static string Join(string separator, IEnumerable<string> items)
        => string.Join(separator, items.Where(item => item != string.Empty));

    private static string[] GetMatches(string text)
    {
        var matches = Regex.Matches(text, FullPattern);
        var etc = text;
        for (var i = matches.Count - 1; i >= 0; i--)
        {
            etc = etc.Remove(matches[i].Index, matches[i].Length);
        }

        if (etc.Trim() != string.Empty)
        {
            throw new ArgumentException(string.Empty, nameof(text));
        }

        var itemList = new List<string>(matches.Count);
        var arrayList = new List<string>();
        var sb = new StringBuilder(text.Length);
        for (var i = 0; i < matches.Count; i++)
        {
            var item = matches[i];
            var (_, value, space, array) = GetMatchInfo(item);
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

    private static (string Name, string Value, int Space, int Array) GetMatchInfo(Match match)
    {
        if (FindGroup(match, out var group, out var index) != true || group.Name == "etc")
        {
            var message = $"""
                '{match.Value}' is an invalid string.: 
                [{match.Index} .. {match.Index + match.Length}]
                """;
            throw new ArgumentException(message, nameof(match));
        }

        var value = group.Value;
        var value0 = match.Value.Substring(value.Length);
        var value1 = value0.TrimEnd();
        var value2 = value1.TrimEnd(',');
        var value3 = value2.TrimEnd();
        var array = value1.Length - value2.Length;
        var space = value0.Length - value1.Length + (value2.Length - value3.Length);
        var replacer = GetReplacer(group.Name);
        return (group.Name, replacer(value), space, array);
    }

    private static Func<string, string> GetReplacer(string name)
    {
        if (ReplacerByName.TryGetValue(name, out var replacer) == true)
        {
            return replacer;
        }

        return (s) => string.Empty;
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

    private static string UnescapeSingleQuote(string text) => Regex.Replace(text, "^'(.*)'$", "$1");

    private static string UnescapeString(string text) => Regex.Replace(text, "(\\\\)(.)", "$2");
}

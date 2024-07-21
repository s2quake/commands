// <copyright file="CommandSettings.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;
using static JSSoft.Commands.CommandUtility;

namespace JSSoft.Commands;

public struct CommandSettings
{
    public const int DefaultIndentSpaces = 2;
    public const int DefaultLabelWidth = 15;
    public const string DefaultHelpName = "help";
    public const char DefaultHelpShortName = 'h';
    public const string DefaultVersionName = "version";
    public const char DefaultVersionShortName = 'v';

    private bool? _isAnsiSupported;
    private int? _labelWith;
    private int? _indentSpaces;
    private string? _helpName;
    private char? _helpShortName;
    private string? _versionName;
    private char? _versionShortName;

    public static bool IsConsoleMode { get; set; } = true;

    public bool IsPublicOnly { get; set; }

    public bool IsAnsiSupported
    {
        readonly get => _isAnsiSupported ?? true;
        set => _isAnsiSupported = value;
    }

    public int LabelWidth
    {
        readonly get => _labelWith ?? DefaultLabelWidth;
        set
        {
            if (value < 5)
            {
                var message = $"Value  must be greater than 5.";
                throw new ArgumentOutOfRangeException(nameof(value), message);
            }

            if (value >= BufferWidth)
            {
                var message = $"""
                    Value must be less than BufferWidth({BufferWidth}).
                    """;
                Trace.TraceWarning(message);
            }

            _labelWith = value;
        }
    }

    public int IndentSpaces
    {
        readonly get => _indentSpaces ?? DefaultIndentSpaces;
        set
        {
            if (value == 0)
            {
                var message = $"The length of value must be greater than zero.";
                throw new ArgumentException(message, nameof(value));
            }

            _indentSpaces = value;
        }
    }

    public readonly string IndentString => string.Empty.PadRight(IndentSpaces);

    public string HelpName
    {
        readonly get => _helpName ?? DefaultHelpName;
        set => _helpName = value;
    }

    public char HelpShortName
    {
        readonly get => _helpShortName ?? DefaultHelpShortName;
        set => _helpShortName = value;
    }

    public string VersionName
    {
        readonly get => _versionName ?? DefaultVersionName;
        set => _versionName = value;
    }

    public char VersionShortName
    {
        readonly get => _versionShortName ?? DefaultVersionShortName;
        set => _versionShortName = value;
    }

    public bool AllowEmpty { get; set; }

    internal static TimeSpan AsyncTimeout { get; } = TimeSpan.FromSeconds(1);

    internal static BindingFlags GetBindingFlags(Type type)
    {
        var instanceFlag = TypeUtility.IsStaticClass(type) == true
            ? BindingFlags.Static
            : BindingFlags.Static | BindingFlags.Instance;
        var accessFlag = BindingFlags.Public | BindingFlags.NonPublic;
        return instanceFlag | accessFlag;
    }

    internal static string GetLabelString(string label, int labelWidth, int indentSpaces)
    {
        var text = label.PadRight(labelWidth);
        if (labelWidth <= label.Length)
        {
            return label.PadRight(label.Length + 1);
        }

        var l = text.Length % indentSpaces;
        return text.PadRight(l);
    }

    internal readonly bool IsHelpArg(string arg)
    {
        if (HelpName != string.Empty && arg == $"{Delimiter}{HelpName}")
        {
            return true;
        }

        if (HelpShortName != char.MinValue && arg == $"{ShortDelimiter}{HelpShortName}")
        {
            return true;
        }

        return false;
    }

    internal readonly bool IsVersionArg(string arg)
    {
        if (VersionName != string.Empty && arg == $"{Delimiter}{VersionName}")
        {
            return true;
        }

        if (VersionShortName != char.MinValue && arg == $"{ShortDelimiter}{VersionShortName}")
        {
            return true;
        }

        return false;
    }

    internal readonly bool ContainsHelpOption(string[] args)
    {
        if (HelpName != string.Empty
            && args.Contains($"{Delimiter}{HelpName}") == true)
        {
            return true;
        }

        if (HelpShortName != char.MinValue
            && args.Contains($"{ShortDelimiter}{HelpShortName}") == true)
        {
            return true;
        }

        return false;
    }

    internal readonly bool ContainsVersionOption(string[] args)
    {
        if (VersionName != string.Empty
            && args.Contains($"{Delimiter}{VersionName}") == true)
        {
            return true;
        }

        if (VersionShortName != char.MinValue
            && args.Contains($"{ShortDelimiter}{VersionShortName}") == true)
        {
            return true;
        }

        return false;
    }

    internal readonly string GetLabelString(string label)
        => GetLabelString(label, LabelWidth, IndentSpaces);
}

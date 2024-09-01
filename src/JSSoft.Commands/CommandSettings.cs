// <copyright file="CommandSettings.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

// Reflection should not be used to increase accessibility of classes, methods, or fields
#pragma warning disable S3011

using System.Diagnostics;
using JSSoft.Commands.Extensions;
using static JSSoft.Commands.CommandUtility;

namespace JSSoft.Commands;

public sealed record class CommandSettings
{
    public const int DefaultIndentSpaces = 2;
    public const int DefaultLabelWidth = 15;
    public const string DefaultHelpName = "help";
    public const char DefaultHelpShortName = 'h';
    public const string DefaultVersionName = "version";
    public const char DefaultVersionShortName = 'v';
    private int _labelWith = DefaultLabelWidth;
    private int _indentSpaces = DefaultIndentSpaces;

    public static CommandSettings Default { get; } = new();

    public static bool IsConsoleMode { get; set; } = true;

    public bool IsPublicOnly { get; init; }

    public bool IsAnsiSupported { get; init; } = true;

    public int LabelWidth
    {
        get => _labelWith;
        init
        {
            if (value < 5)
            {
                var message = $"Value  must be greater than 5.";
                throw new ArgumentOutOfRangeException(nameof(value), message);
            }

            if (value >= BufferWidth)
            {
                var message = $"Value must be less than BufferWidth({BufferWidth}).";
                Trace.TraceWarning(message);
            }

            _labelWith = value;
        }
    }

    public int IndentSpaces
    {
        get => _indentSpaces;
        init
        {
            if (value == 0)
            {
                var message = $"The length of value must be greater than zero.";
                throw new ArgumentException(message, nameof(value));
            }

            _indentSpaces = value;
        }
    }

    public string IndentString => string.Empty.PadRight(IndentSpaces);

    public string HelpName { get; init; } = DefaultHelpName;

    public char HelpShortName { get; init; } = DefaultHelpShortName;

    public string VersionName { get; init; } = DefaultVersionName;

    public char VersionShortName { get; init; } = DefaultVersionShortName;

    public bool AllowEmpty { get; init; }

    public ICommandValueValidator ValueValidator { get; init; } = new CommandValueValidator();

    internal static TimeSpan AsyncTimeout { get; } = TimeSpan.FromSeconds(1);

    internal static BindingFlags GetBindingFlags(Type type)
    {
        var instanceFlag = type.IsStaticClass() == true
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

    internal bool IsHelpArg(string arg)
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

    internal bool IsVersionArg(string arg)
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

    internal bool ContainsHelpOption(string[] args)
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

    internal bool ContainsVersionOption(string[] args)
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

    internal string GetLabelString(string label)
        => GetLabelString(label, LabelWidth, IndentSpaces);
}

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

using System.Diagnostics;

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

    public static CommandSettings Default { get; } = new CommandSettings();

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
                throw new ArgumentOutOfRangeException(nameof(value), $"Value  must be greater than 5.");
            if (value >= CommandUtility.GetBufferWidth())
                Trace.TraceWarning($"Value must be less than BufferWidth({CommandUtility.GetBufferWidth()}).");
            _labelWith = value;
        }
    }

    public int IndentSpaces
    {
        readonly get => _indentSpaces ?? DefaultIndentSpaces;
        set
        {
            if (value == 0)
                throw new ArgumentException($"The length of value must be greater than zero.", nameof(value));
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
        var instanceFlag = TypeUtility.IsStaticClass(type) == true ? BindingFlags.Static : BindingFlags.Static | BindingFlags.Instance;
        var accessFlag = BindingFlags.Public | BindingFlags.NonPublic;
        return instanceFlag | accessFlag;
    }

    internal readonly bool IsHelpArg(string arg)
    {
        if (HelpName != string.Empty && arg == $"{CommandUtility.Delimiter}{HelpName}")
            return true;
        if (HelpShortName != char.MinValue && arg == $"{CommandUtility.ShortDelimiter}{HelpShortName}")
            return true;
        return false;
    }

    internal readonly bool IsVersionArg(string arg)
    {
        if (VersionName != string.Empty && arg == $"{CommandUtility.Delimiter}{VersionName}")
            return true;
        if (VersionShortName != char.MinValue && arg == $"{CommandUtility.ShortDelimiter}{VersionShortName}")
            return true;
        return false;
    }

    internal readonly bool ContainsHelpOption(string[] args)
    {
        if (HelpName != string.Empty && args.Contains($"{CommandUtility.Delimiter}{HelpName}") == true)
            return true;
        if (HelpShortName != char.MinValue && args.Contains($"{CommandUtility.ShortDelimiter}{HelpShortName}") == true)
            return true;
        return false;
    }

    internal readonly bool ContainsVersionOption(string[] args)
    {
        if (VersionName != string.Empty && args.Contains($"{CommandUtility.Delimiter}{VersionName}") == true)
            return true;
        if (VersionShortName != char.MinValue && args.Contains($"{CommandUtility.ShortDelimiter}{VersionShortName}") == true)
            return true;
        return false;
    }

    internal readonly string GetLabelString(string label) => GetLabelString(label, LabelWidth, IndentSpaces);

    internal static string GetLabelString(string label, int labelWidth, int indentSpaces)
    {
        var text = label.PadRight(labelWidth);
        if (labelWidth <= label.Length)
            return label.PadRight(label.Length + 1);
        var l = text.Length % indentSpaces;
        return text.PadRight(l);
    }
}

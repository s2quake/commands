// <copyright file="CommandTextWriter.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.CodeDom.Compiler;
using System.IO;
using System.Text;

namespace JSSoft.Commands;

public class CommandTextWriter(TextWriter writer, int width, string tabString)
    : IndentedTextWriter(writer, tabString)
{
    private readonly CommandSettings _settings = default;
    private readonly string _tabString = tabString;
    private int _indent = -1;
    private string _indentString = string.Empty;

    public CommandTextWriter()
        : this(new StringWriter(), settings: default)
    {
    }

    public CommandTextWriter(CommandSettings settings)
        : this(new StringWriter(), settings)
    {
    }

    public CommandTextWriter(TextWriter writer, CommandSettings settings)
        : this(writer, CommandUtility.GetBufferWidth(), settings.IndentString)
    {
        _settings = settings;
    }

    public int Width { get; private set; } = width;

    public bool IsAnsiSupported => _settings.IsAnsiSupported;

    public int TotalIndentSpaces => Indent * _tabString.Length;

    public string TotalIndentString
    {
        get
        {
            if (_indent != Indent)
            {
                _indentString = GetIndentString(_tabString, Indent);
                _indent = Indent;
            }

            return _indentString;
        }
    }

    public static string[] Wrap(string text, int width)
    {
        var lineList = new List<string>();
        var items = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        for (var i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var ss = item.Split();
            var wraps = Wrap(ss, width);
            lineList.AddRange(wraps);
        }

        return [.. lineList];
    }

    public static string[] Wrap(string[] items, int width)
    {
        if (width < 1)
        {
            var message = $"Value '{nameof(width)}' must be greater than 0.";
            throw new ArgumentOutOfRangeException(nameof(width), message);
        }

        var capacity = (((items.Length * 2) + items.Sum(item => item.Length)) / width) + 1;
        var lineList = new List<string>(capacity);
        var line = string.Empty;
        foreach (var item in items)
        {
            var s = line.Length > 0 ? 1 : 0;
            if (line.Length + s + item.Length > width)
            {
                var text = item;
                lineList.Add(line);
                while (text.Length >= width)
                {
                    lineList.Add(text.Substring(0, width));
                    text = text.Remove(0, width);
                }

                line = text;
            }
            else
            {
                var sb = new StringBuilder();
                sb.Append(line);
                sb.Append(string.Empty.PadRight(s));
                sb.Append(item);
                line = sb.ToString();
            }
        }

        if (line != string.Empty)
        {
            lineList.Add(line);
        }

        if (capacity < lineList.Count)
        {
            throw new InvalidOperationException();
        }

        return [.. lineList];
    }

    public override string ToString() => $"{InnerWriter}";

    public void BeginGroup(string s)
    {
        var text = IsAnsiSupported == true ? $"\x1b[1m{s}\x1b[0m" : s;
        WriteLine(text);
        Indent++;
    }

    public void EndGroup()
    {
        Indent--;
        WriteLine();
    }

    public IDisposable Group(string text) => new GroupScopeObject(this, text);

    public IDisposable IndentScope(int indent) => new IndentScopeObject(this, indent);

    public void WriteLine(string[] lines)
    {
        for (var i = 0; i < lines.Length; i++)
        {
            WriteLine(s: lines[i]);
        }
    }

    public void WriteLine(string label, string summary)
    {
        var name = _settings.GetLabelString(label);
        var items = Wrap(summary, Width - (TotalIndentSpaces + name.Length));
        var lines = items.Length != 0 ? items : [string.Empty];
        var spaces = string.Empty.PadRight(name.Length);
        for (var i = 0; i < lines.Length; i++)
        {
            Write(i == 0 ? name : spaces);
            WriteLine(lines[i]);
        }
    }

    private static string GetIndentString(string tabString, int indent)
    {
        var sb = new StringBuilder(tabString.Length * indent);
        for (var i = 0; i < indent; i++)
        {
            sb.Append(tabString);
        }

        return sb.ToString();
    }

    private sealed class GroupScopeObject : IDisposable
    {
        private readonly CommandTextWriter _commandWriter;

        public GroupScopeObject(CommandTextWriter commandWriter, string text)
        {
            _commandWriter = commandWriter;
            _commandWriter.BeginGroup(text);
        }

        void IDisposable.Dispose()
        {
            _commandWriter.EndGroup();
        }
    }

    private sealed class IndentScopeObject : IDisposable
    {
        private readonly CommandTextWriter _commandWriter;
        private readonly int _indent;

        public IndentScopeObject(CommandTextWriter commandWriter, int indent)
        {
            _commandWriter = commandWriter;
            _indent = commandWriter.Indent;
            _commandWriter.Indent = indent;
        }

        void IDisposable.Dispose()
        {
            _commandWriter.Indent = _indent;
        }
    }
}

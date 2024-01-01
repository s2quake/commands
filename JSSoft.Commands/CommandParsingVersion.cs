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

using System.Text;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

[ResourceUsage(typeof(VersionCommandBase))]
sealed class CommandParsingVersion
{
    [CommandPropertySwitch("quiet", 'q')]
    public bool IsQuiet { get; set; }

    public string ExecutionName { get; private set; } = string.Empty;

    public string Version { get; private set; } = string.Empty;

    public string Copyright { get; private set; } = string.Empty;

    public static CommandParsingVersion Create(CommandParsingException e)
    {
        var settings = e.Parser.Settings;
        var args = e.Arguments.Where(item => settings.IsVersionArg(item) == false).ToArray();
        var obj = new CommandParsingVersion();
        var parser = new VersionCommandParser($"{e.Parser.ExecutionName} {e.Arguments[0]}", obj);
        parser.Parse(args);
        obj.ExecutionName = e.Parser.ExecutionName;
        obj.Version = e.Parser.Version;
        obj.Copyright = e.Parser.Copyright;
        return obj;
    }

    public string GetDetailedString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{ExecutionName} {Version}");
        sb.AppendIf(Copyright, item => item != string.Empty);
        return sb.ToString();
    }

    public string GetQuietString() => Version;

    #region VersionCommandParser

    sealed class VersionCommandParser(string commandName, object instance)
        : CommandParser(commandName, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }

    #endregion
}

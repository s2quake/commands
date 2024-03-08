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

namespace JSSoft.Commands;

[ResourceUsage(typeof(HelpCommandBase))]
sealed class CommandInvocationHelp
{
    [CommandPropertyRequired(DefaultValue = "")]
    public string Command { get; set; } = string.Empty;

    [CommandPropertySwitch("detail")]
    public bool IsDetail { get; set; }

    public static CommandInvocationHelp Create(CommandInvocationException e)
    {
        var settings = e.Invoker.Settings;
        var name = e.Arguments[0];
        var args = e.Arguments.Where(item => settings.IsHelpArg(item) == false).ToArray();
        var obj = new CommandInvocationHelp();
        var parser = new HelpCommandParser(name, obj);
        parser.Parse(args);
        return obj;
    }

    #region HelpCommandParser

    sealed class HelpCommandParser(string commandName, object instance)
        : CommandParser(commandName, instance)
    {
        protected override void OnValidate(string[] args)
        {
        }
    }

    #endregion
}

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

public class CommandParser : CommandAnalyzer
{
    public CommandParser(object instance)
        : base(Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly, instance)
    {
    }

    public CommandParser(object instance, CommandSettings settings)
        : base(Assembly.GetEntryAssembly() ?? typeof(CommandParser).Assembly, instance, settings)
    {
    }

    public CommandParser(string name, object instance)
        : base(name, instance)
    {
    }

    public CommandParser(string name, object instance, CommandSettings settings)
        : base(name, instance, settings)
    {
    }

    public CommandParser(Assembly assembly, object instance)
        : base(assembly, instance)
    {
    }

    public CommandParser(Assembly assembly, object instance, CommandSettings settings)
        : base(assembly, instance, settings)
    {
    }

    public void Parse(string[] args)
    {
        OnValidate(args);

        var instance = Instance;
        var commandName = args.Length > 0 ? args[0] : string.Empty;
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(instance);
        var parserContext = new ParseContext(memberDescriptors, args);
        parserContext.SetValue(instance);
    }

    protected virtual void OnValidate(string[] args)
    {
        if (CommandUtility.IsEmptyArgs(args) == true)
            throw new CommandParsingException(this, CommandParsingError.Empty, args);
        if (Settings.ContainsHelpOption(args) == true)
            throw new CommandParsingException(this, CommandParsingError.Help, args);
        if (Settings.ContainsVersionOption(args) == true)
            throw new CommandParsingException(this, CommandParsingError.Version, args);
    }
}

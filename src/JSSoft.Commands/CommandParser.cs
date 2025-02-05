// <copyright file="CommandParser.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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
        OnVerify(args);

        var instance = Instance;
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(instance);
        var settings = Settings;
        var parserContext = ParseContext.Create(memberDescriptors, args, settings);
        parserContext.SetValue(instance, settings);
    }

    protected virtual void OnVerify(string[] args)
    {
        if (CommandUtility.IsEmptyArgs(args) is true && Settings.AllowEmpty is false)
        {
            throw new CommandParsingException(this, CommandParsingError.Empty, args);
        }

        if (Settings.ContainsHelpOption(args) is true)
        {
            throw new CommandParsingException(this, CommandParsingError.Help, args);
        }

        if (Settings.ContainsVersionOption(args) is true)
        {
            throw new CommandParsingException(this, CommandParsingError.Version, args);
        }
    }
}

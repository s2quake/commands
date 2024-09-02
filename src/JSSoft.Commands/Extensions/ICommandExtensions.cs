// <copyright file="ICommandExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace JSSoft.Commands.Extensions;

public static class ICommandExtensions
{
    public static bool TryGetCommand(
        this ICommand @this,
        string commandName,
        [MaybeNullWhen(false)] out ICommand command)
    {
        return @this.Commands.TryGetValue(commandName, out command) is true;
    }

    public static string GetExecutionName(this ICommand @this)
    {
        if (@this.Context is null)
        {
            throw new InvalidOperationException("This command is not in the CommandContext.");
        }

        var executionName = CommandUtility.GetExecutionName(@this.Name, @this.Aliases);
        if (@this.Parent is { } parent)
        {
            executionName = $"{GetExecutionName(parent)} {executionName}";
        }
        else
        {
            if (@this.Context.Name != string.Empty)
            {
                executionName = $"{@this.Context.Name} {executionName}";
            }
        }

        return executionName;
    }

    public static void Verify(this ICommand @this)
    {
        if (CommandUtility.IsName(@this.Name) is false)
        {
            throw new CommandDefinitionException("Invalid command name.");
        }

        if (@this.AllowsSubCommands is false && @this.Commands.Count > 0)
        {
            throw new CommandDefinitionException("This command does not allow subcommands.");
        }

        foreach (var command in @this.Commands)
        {
            if (command.Parent != @this)
            {
                throw new CommandDefinitionException(
                    $"Command '{command}' in the command collection is not equal to the parent " +
                    $"command '{@this}'.");
            }
        }
    }

    public static void SetParent(this ICommand @this, ICommand? parent)
    {
        if (@this == parent)
        {
            throw new ArgumentException("The parent command is equal to the command.");
        }

        if (@this.Parent is { } oldParent)
        {
            oldParent.Commands.Remove(@this);
            if (oldParent.Commands.Contains(@this) is true)
            {
                throw new InvalidOperationException(
                    "The command is not removed from the parent command.");
            }
        }

        if (parent is not null && parent.AllowsSubCommands is false)
        {
            throw new InvalidOperationException($"Parent '{parent}' does not allow sub commands.");
        }

        @this.Parent = parent;

        if (@this.Parent is { } newParent)
        {
            if (newParent.Commands.Contains(@this) is true)
            {
                throw new InvalidOperationException(
                    "The command is already added to the parent command.");
            }

            newParent.Commands.Add(@this);
        }
    }
}

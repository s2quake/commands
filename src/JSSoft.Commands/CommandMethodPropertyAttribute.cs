// <copyright file="CommandMethodPropertyAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class CommandMethodPropertyAttribute(string propertyName, params string[] propertyNames) : Attribute
{
    public string[] PropertyNames { get; } = [propertyName, .. propertyNames];

    internal IEnumerable<CommandMemberDescriptor> GetCommandMemberDescriptors(Type type)
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(type);
        foreach (var item in PropertyNames)
        {
            if (memberDescriptors.Contains(item) == true)
            {
                yield return memberDescriptors[item];
            }
            else
            {
                throw new CommandDefinitionException($"Type '{type}' does not have property '{item}'.", type);
            }
        }
    }
}

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

using System.Collections.Specialized;
using System.Diagnostics;

namespace JSSoft.Commands;

public sealed class CommandMemberDescriptorCollection : IEnumerable<CommandMemberDescriptor>
{
    private readonly OrderedDictionary _memberDescriptorByMemberName;
    private readonly Dictionary<string, CommandMemberDescriptor> _memberDescriptorByName;
    private readonly Dictionary<char, CommandMemberDescriptor> _memberDescriptorByShortName;

    public CommandMemberDescriptorCollection(Type type, IEnumerable<CommandMemberDescriptor> memberDescriptors)
    {
        if (memberDescriptors.Where(item => item.CommandType == CommandType.Variables).Count() > 1)
            throw new CommandDefinitionException($"Attribute '{nameof(CommandPropertyArrayAttribute)}' can be defined in only one property.", type);

        var query = from item in memberDescriptors
                    orderby item.DefaultValue != DBNull.Value
                    orderby item.CommandType
                    select item;
        var items = query.ToArray();
        _memberDescriptorByMemberName = new(items.Length);
        _memberDescriptorByName = new(items.Length);
        _memberDescriptorByShortName = new(items.Length);
        foreach (var item in items)
        {
            if (_memberDescriptorByMemberName.Contains(item.MemberName) == true)
                throw new CommandDefinitionException($"{nameof(CommandMemberDescriptor)} '{item.MemberName}' cannot be added because it already exists.", type);
            _memberDescriptorByMemberName.Add(item.MemberName, item);
        }
        foreach (var item in items)
        {
            if (item.Name == string.Empty)
                continue;
            if (_memberDescriptorByName.ContainsKey(item.Name) == true)
                throw new CommandDefinitionException($"{nameof(CommandMemberDescriptor)} '{item.Name}' cannot be added because it already exists.", type);
            _memberDescriptorByName.Add(item.Name, item);
        }
        foreach (var item in items)
        {
            if (item.ShortName == char.MinValue)
                continue;
            if (_memberDescriptorByShortName.ContainsKey(item.ShortName) == true)
                throw new CommandDefinitionException($"{nameof(CommandMemberDescriptor)} '{item.ShortName}' cannot be added because it already exists.", type);
            _memberDescriptorByShortName.Add(item.ShortName, item);
        }
        Type = type;
        RequirementDescriptors = Enumerable.Where(this, item => item.IsRequired == true).ToArray();
        VariablesDescriptor = memberDescriptors.Where(item => item.CommandType == CommandType.Variables).SingleOrDefault();
        OptionDescriptors = Enumerable.Where(this, item => item.CommandType == CommandType.General || item.CommandType == CommandType.Switch).ToArray();
    }

    public CommandMemberDescriptor? FindByOptionName(string optionName)
    {
        if (optionName.StartsWith(CommandUtility.Delimiter) == true && optionName.Length > 3)
        {
            var name = optionName.Substring(CommandUtility.Delimiter.Length);
            return _memberDescriptorByName.TryGetValue(name, out var value) == true ? value : null;
        }
        else if (optionName.StartsWith(CommandUtility.ShortDelimiter) == true && optionName.Length == 2)
        {
            var name = optionName[1];
            return _memberDescriptorByShortName.TryGetValue(name, out var value) == true ? value : null;
        }
        else if (optionName.Length == 1)
        {
            var name = optionName[0];
            return _memberDescriptorByShortName.TryGetValue(name, out var value) == true ? value : null;
        }
        else
        {
            return _memberDescriptorByName.TryGetValue(optionName, out var value) == true ? value : null;
        }
    }

    public bool Contains(string memberName) => _memberDescriptorByMemberName.Contains(memberName);

    public CommandMemberDescriptor this[string memberName]
    {
        get
        {
            if (_memberDescriptorByMemberName.Contains(memberName) == true)
            {
                return (CommandMemberDescriptor)_memberDescriptorByMemberName[memberName]!;
            }
            throw new CommandMemberNotFoundException(memberName);
        }
    }

    public CommandMemberDescriptor this[int index] => (CommandMemberDescriptor)_memberDescriptorByMemberName[index]!;

    public int Count => _memberDescriptorByMemberName.Count;

    public Type Type { get; }

    public CommandMemberDescriptor? VariablesDescriptor { get; }

    public bool HasOptions => OptionDescriptors.Length != 0;

    public CommandMemberDescriptor[] OptionDescriptors { get; }

    public bool HasRequirements => RequirementDescriptors.Length != 0;

    public CommandMemberDescriptor[] RequirementDescriptors { get; }

    internal IEnumerable<CommandMemberDescriptor> Filter(Type requestType, string[] memberNames)
    {
        if (memberNames.Length == 0)
        {
            foreach (var item in this)
            {
                yield return item;
            }
        }
        else
        {
            var type = Type;
            foreach (var item in memberNames)
            {
                if (Contains(item) == false)
                    throw new CommandDefinitionException($"Type '{type}' does not have property '{item}'.", requestType);
                yield return this[item];
            }
        }
    }

    #region IEnumerable

    IEnumerator<CommandMemberDescriptor> IEnumerable<CommandMemberDescriptor>.GetEnumerator()
    {
        foreach (var item in _memberDescriptorByMemberName.Values)
        {
            if (item is CommandMemberDescriptor memberDescriptor)
            {
                yield return memberDescriptor;
            }
            else
            {
                throw new UnreachableException();
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        foreach (var item in _memberDescriptorByMemberName.Values)
        {
            yield return item;
        }
    }

    #endregion
}

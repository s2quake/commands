// <copyright file="CommandAsICustomCommandDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandContextTests;

public class CommandAsICustomCommandDescriptor
{
    [Fact]
    public void Test1()
    {
        var runCommand = new RunCommand();
        var commandContext = new TestCommandContext(runCommand);

        commandContext.Execute("run --string1 value1 --string2 value2");

        Assert.Equal("value1", runCommand.String1);
        Assert.Equal("value2", runCommand.String2);
    }

    private sealed class CustomMember1
    {
        [CommandProperty]
        public string String1 { get; set; } = string.Empty;
    }

    private sealed class CustomMember2
    {
        [CommandProperty]
        public string String2 { get; set; } = string.Empty;
    }

    private sealed class RunCommand : CommandBase, ICustomCommandDescriptor
    {
        private readonly CustomMember1 _customMember1 = new();
        private readonly CustomMember2 _customMember2 = new();
        private readonly List<object> _optionList;
        private readonly Dictionary<CommandMemberDescriptor, object> _descriptorByInstance;
        private readonly CommandMemberDescriptorCollection _descriptors;

        public RunCommand()
        {
            var itemList = new List<KeyValuePair<CommandMemberDescriptor, object>>();
            _optionList = [_customMember1, _customMember2];
            for (var i = 0; i < _optionList.Count; i++)
            {
                var option = _optionList[i];
                var descriptors = CommandDescriptor.GetMemberDescriptors(option)
                                    .Select(item => KeyValuePair.Create(item, option));
                itemList.AddRange(descriptors);
            }

            _descriptorByInstance = itemList.ToDictionary(item => item.Key, item => item.Value);
            _descriptors = new(GetType(), [.. _descriptorByInstance.Keys]);
        }

        public CommandMemberDescriptorCollection Members => _descriptors;

        public string String1 => _customMember1.String1;

        public string String2 => _customMember2.String2;

        public object GetMemberOwner(CommandMemberDescriptor memberDescriptor)
            => _descriptorByInstance[memberDescriptor];

        protected override void OnExecute()
        {
        }
    }
}

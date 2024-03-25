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

sealed class CommandNode : ICommandNode
{
    public CommandNode(CommandContextBase commandContext)
    {
        CommandContext = commandContext;
        Command = new EmptyCommand();
    }

    public CommandNode(CommandContextBase commandContext, ICommand command)
    {
        CommandContext = commandContext;
        Command = command;
        Category = AttributeUtility.GetCategory(command.GetType());
    }

    public override string ToString() => Name;

    public CommandNode? Parent { get; set; }

    public CommandNodeCollection Children { get; } = [];

    public CommandAliasNodeCollection ChildByAlias { get; } = [];

    public ICommand Command { get; }

    public ICommandUsage? Usage => Command as ICommandUsage;

    public CommandContextBase CommandContext { get; }

    public List<ICommand> CommandList { get; } = [];

    public string Name => Command.Name;

    public string Category { get; } = string.Empty;

    public string[] Aliases => Command != null ? Command.Aliases : [];

    public bool IsEnabled => CommandList.Any(item => item.IsEnabled);

    #region ICommandNode

    IEnumerable<ICommand> ICommandNode.Commands => CommandList;

    ICommandNode? ICommandNode.Parent => Parent;

    IReadOnlyDictionary<string, ICommandNode> ICommandNode.Children => Children;

    IReadOnlyDictionary<string, ICommandNode> ICommandNode.ChildByAlias => ChildByAlias;

    ICommandContext ICommandNode.CommandContext => CommandContext;

    #endregion

    #region EmptyCommand

    sealed class EmptyCommand : ICommand
    {
        public string Name => throw new System.NotImplementedException();

        public string[] Aliases => throw new System.NotImplementedException();

        public bool IsEnabled => throw new System.NotImplementedException();
    }

    #endregion
}

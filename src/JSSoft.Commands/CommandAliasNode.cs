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

sealed class CommandAliasNode(ICommandNode commandNode, string alias) : ICommandNode
{
    private readonly ICommandNode commandNode = commandNode;

    public ICommandNode? Parent => commandNode.Parent;

    public IReadOnlyDictionary<string, ICommandNode> Childs => commandNode.Childs;

    public IReadOnlyDictionary<string, ICommandNode> ChildsByAlias => commandNode.ChildsByAlias;

    public string Name { get; } = alias;

    public string[] Aliases => commandNode.Aliases;

    public string Category => commandNode.Category;

    public ICommand Command => commandNode.Command;

    public ICommandUsage? Usage => commandNode.Usage;

    public ICommandContext CommandContext => commandNode.CommandContext;

    public bool IsEnabled => commandNode.IsEnabled;

    public IEnumerable<ICommand> Commands => commandNode.Commands;
}

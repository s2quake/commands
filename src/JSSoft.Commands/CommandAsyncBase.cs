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

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public abstract class CommandAsyncBase : ICommand, IAsyncExecutable, ICommandHost, ICommandUsage, ICommandUsagePrinter
{
    private readonly CommandUsageDescriptorBase _usageDescriptor;
    private ICommandNode? _node;
    private IProgress<ProgressInfo>? _progress;

    protected CommandAsyncBase()
        : this(Array.Empty<string>())
    {
    }

    protected CommandAsyncBase(string[] aliases)
    {
        Name = CommandUtility.ToSpinalCase(GetType());
        Aliases = aliases;
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
    }

    protected CommandAsyncBase(string name)
        : this(name, [])
    {
    }

    protected CommandAsyncBase(string name, string[] aliases)
    {
        Name = name;
        Aliases = aliases;
        _usageDescriptor = CommandDescriptor.GetUsageDescriptor(GetType());
    }

    public virtual string[] GetCompletions(CommandCompletionContext completionContext)
    {
        return [];
    }

    public string Name { get; }

    public string[] Aliases { get; }

    public virtual bool IsEnabled => true;

    public TextWriter Out => CommandContext.Out;

    public TextWriter Error => CommandContext.Error;

    public ICommandContext CommandContext => _node?.CommandContext ?? throw new InvalidOperationException();

    internal string ExecutionName
    {
        get
        {
            var executionName = CommandUtility.GetExecutionName(Name, Aliases);
            if (CommandContext.ExecutionName != string.Empty)
                return $"{CommandContext.ExecutionName} {executionName}";
            return executionName;
        }
    }

    protected abstract Task OnExecuteAsync(CancellationToken cancellationToken);

    protected IProgress<ProgressInfo> Progress => _progress ?? throw new InvalidOperationException("The progress is not available.");

    protected CommandMemberDescriptor GetDescriptor(string propertyName)
    {
        return CommandDescriptor.GetMemberDescriptors(GetType())[propertyName];
    }

    protected virtual void OnUsagePrint(bool isDetail)
    {
        var settings = CommandContext.Settings;
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(GetType());
        var usagePrinter = new CommandParsingUsagePrinter(this, settings)
        {
            IsDetail = isDetail,
        };
        usagePrinter.Print(Out, memberDescriptors);
    }

    #region IAsyncExecutable

    Task IAsyncExecutable.ExecuteAsync(CancellationToken cancellationToken, IProgress<ProgressInfo> progress)
    {
        _progress = progress;
        try
        {
            return OnExecuteAsync(cancellationToken);
        }
        finally
        {
            _progress = null;
        }
    }

    #endregion

    #region ICommandHost

    ICommandNode? ICommandHost.Node
    {
        get => _node;
        set => _node = value;
    }

    #endregion

    #region ICommandUsage

    string ICommandUsage.ExecutionName => ExecutionName;

    string ICommandUsage.Summary => _usageDescriptor.Summary;

    string ICommandUsage.Description => _usageDescriptor.Description;

    string ICommandUsage.Example => _usageDescriptor.Example;

    #endregion

    #region ICommandUsagePrinter

    void ICommandUsagePrinter.Print(bool isDetail) => OnUsagePrint(isDetail);

    #endregion
}

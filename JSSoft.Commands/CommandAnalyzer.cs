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

namespace JSSoft.Commands;

public abstract class CommandAnalyzer
{
    private readonly string _fullName;
    private readonly string _filename;

    public CommandAnalyzer(string commandName, object instance)
        : this(commandName, instance, CommandSettings.Default)
    {
    }

    public CommandAnalyzer(string commandName, object instance, CommandSettings settings)
    {
        ThrowUtility.ThrowIfEmpty(commandName, nameof(commandName));
        CommandName = commandName;
        Instance = instance;
        Settings = settings;
        _fullName = commandName;
        _filename = commandName;
    }

    public CommandAnalyzer(Assembly assembly, object instance)
        : this(assembly, instance, CommandSettings.Default)
    {
    }

    public CommandAnalyzer(Assembly assembly, object instance, CommandSettings settings)
    {
        _fullName = AssemblyUtility.GetAssemblyLocation(assembly);
        _filename = Path.GetFileName(_fullName);
        CommandName = Path.GetFileNameWithoutExtension(_fullName);
        Instance = instance;
        Settings = settings;
        Version = AssemblyUtility.GetAssemblyVersion(assembly);
        Copyright = AssemblyUtility.GetAssemblyCopyright(assembly);
    }

    public string CommandName { get; }

    public object Instance { get; }

    public string Version { get; set; } = $"{new Version(1, 0)}";

    public string Copyright { get; set; } = string.Empty;

    public string ExecutionName
    {
        get
        {
#if NETCOREAPP || NET
            if (_filename != CommandName)
            {
                return $"dotnet {_filename}";
            }
#elif NETFRAMEWORK
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return $"mono {_filename}";
            }
#endif
            return CommandName;
        }
    }

    public CommandSettings Settings { get; }

    internal void ThrowIfNotVerifyCommandName(string commandName)
    {
        if (VerifyCommandName(commandName) == false)
        {
            throw new ArgumentException($"Command name '{commandName}' is not available.", nameof(commandName));
        }
    }

    internal bool VerifyCommandName(string commandName)
    {
        if (CommandName == commandName)
            return true;
        if (_fullName == commandName)
            return true;
        if (_filename == commandName)
            return true;
        return false;
    }
}

// <copyright file="CommandAnalyzer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands;

public abstract class CommandAnalyzer
{
    private readonly string _fullName;
    private readonly string _filename;

    protected CommandAnalyzer(string commandName, object instance)
        : this(commandName, instance, settings: default)
    {
    }

    protected CommandAnalyzer(string commandName, object instance, CommandSettings settings)
    {
        ThrowUtility.ThrowIfEmpty(commandName, nameof(commandName));
        CommandName = commandName;
        Instance = instance;
        Settings = settings;
        _fullName = commandName;
        _filename = commandName;
    }

    protected CommandAnalyzer(Assembly assembly, object instance)
        : this(assembly, instance, settings: default)
    {
    }

    protected CommandAnalyzer(Assembly assembly, object instance, CommandSettings settings)
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
        if (VerifyCommandName(commandName) != true)
        {
            throw new ArgumentException(
                message: $"Command name '{commandName}' is not available.",
                paramName: nameof(commandName));
        }
    }

    internal bool VerifyCommandName(string commandName)
    {
        if (CommandName == commandName)
        {
            return true;
        }

        if (_fullName == commandName)
        {
            return true;
        }

        if (_filename == commandName)
        {
            return true;
        }

        return false;
    }
}

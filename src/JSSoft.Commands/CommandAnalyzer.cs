// <copyright file="CommandAnalyzer.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using JSSoft.Commands.Extensions;

namespace JSSoft.Commands;

public abstract class CommandAnalyzer
{
    private readonly string _fullName;
    private readonly string _filename;
    private readonly string? _processName
#if NET6_0_OR_GREATER
        = Path.GetFileNameWithoutExtension(Environment.ProcessPath);
#else
        = Path.GetFileNameWithoutExtension(
            System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName);
#endif

    protected CommandAnalyzer(string commandName, object instance)
        : this(commandName, instance, settings: CommandSettings.Default)
    {
    }

    protected CommandAnalyzer(string commandName, object instance, CommandSettings settings)
    {
        if (commandName == string.Empty)
        {
            throw new ArgumentException("Empty string is not allowed.", nameof(commandName));
        }

        CommandName = commandName;
        Instance = instance;
        Settings = settings;
        _fullName = commandName;
        _filename = commandName;
    }

    protected CommandAnalyzer(Assembly assembly, object instance)
        : this(assembly, instance, settings: CommandSettings.Default)
    {
    }

    protected CommandAnalyzer(Assembly assembly, object instance, CommandSettings settings)
    {
        _fullName = assembly.GetAssemblyLocation();
        _filename = Path.GetFileName(_fullName);
        CommandName = Path.GetFileNameWithoutExtension(_fullName);
        Instance = instance;
        Settings = settings;
        Version = assembly.GetAssemblyVersion();
        Copyright = assembly.GetAssemblyCopyright();
    }

    public string CommandName { get; }

    public object Instance { get; }

    public Type InstanceType => Instance is Type type ? type : Instance.GetType();

    public string Version { get; set; } = $"{new Version(1, 0)}";

    public string Copyright { get; set; } = string.Empty;

    public string ExecutionName
    {
        get
        {
#if NETFRAMEWORK
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                return $"mono {_filename}";
            }
#else
            if (_processName == "dotnet")
            {
                return $"dotnet {_filename}";
            }
#endif
            return CommandName;
        }
    }

    public CommandSettings Settings { get; }

    internal void ThrowIfNotVerifyCommandName(string commandName)
    {
        if (CheckCommandName(commandName) is false)
        {
            throw new ArgumentException(
                message: $"Command name '{commandName}' is not available.",
                paramName: nameof(commandName));
        }
    }

    internal bool CheckCommandName(string commandName)
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

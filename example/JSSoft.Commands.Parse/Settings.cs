// <copyright file="Settings.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Parse;

[ResourceUsage]
[CommandStaticProperty(typeof(GlobalSettings))]
internal sealed class Settings
{
    [CommandPropertyRequired]
    public string Path { get; set; } = string.Empty;

    [CommandPropertyRequired]
    [Description("service name")]
    public string ServiceName { get; set; } = string.Empty;

    [CommandPropertyExplicitRequired]
    [Description("path to work")]
    public string WorkingPath { get; set; } = string.Empty;

    [CommandProperty('p', useName: true, InitValue = "10001")]
    [Description("port")]
    public int Port { get; set; }

    [CommandPropertySwitch]
    public bool UseCache { get; set; }

    [CommandProperty("cache-size", DefaultValue = 1024)]
    public int CacheSize { get; set; }

    [CommandPropertyArray]
    [CommandSummary("library paths.")]
    public string[] Libraries { get; set; } = [];
}

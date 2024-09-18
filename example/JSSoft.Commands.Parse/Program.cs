// <copyright file="Program.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Text;
using JSSoft.Commands;
using JSSoft.Commands.Parse;

try
{
    var settings = new Settings();
    var parser = new CommandParser(settings);
    var sb = new StringBuilder();
    parser.Parse(args);
    sb.AppendLine($"{nameof(settings.Path)}: {settings.Path}");
    sb.AppendLine($"{nameof(settings.ServiceName)}: {settings.ServiceName}");
    sb.AppendLine($"{nameof(settings.WorkingPath)}: {settings.WorkingPath}");
    sb.AppendLine($"{nameof(settings.Port)}: {settings.Port}");
    sb.AppendLine($"{nameof(settings.UseCache)}: {settings.UseCache}");
    sb.AppendLine($"{nameof(settings.CacheSize)}: {settings.CacheSize}");
    sb.AppendLine($"{nameof(GlobalSettings.Id)}: {GlobalSettings.Id}");
    sb.AppendLine($"{nameof(GlobalSettings.Password)}: {GlobalSettings.Password}");
    sb.AppendLine($"{nameof(settings.Libraries)}:");
    foreach (var item in settings.Libraries)
    {
        sb.AppendLine($"    {item}");
    }

    Console.WriteLine(sb.ToString());
}
catch (CommandParsingException e)
{
    e.Print(Console.Out);
    Environment.Exit(1);
}

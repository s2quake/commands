// <copyright file="ListDirectoryCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using JSSoft.Commands.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;

namespace JSSoft.Commands.Applications.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[Category("IO")]
sealed class ListDirectoryCommand : CommandBase
{
    public ListDirectoryCommand()
        : base("ls")
    {
    }

    [CommandPropertySwitch('s', useName: true)]
    public bool IsRecursive { get; set; }

    protected override void OnExecute()
    {
        var dir = Directory.GetCurrentDirectory();
        PrintItems(dir);
    }

    private void PrintItems(string dir)
    {
        var items = new List<string[]>();

        {
            var props = new List<string>
            {
                "DateTime",
                "",
                "Name"
            };
            items.Add([.. props]);
        }

        foreach (var item in Directory.GetDirectories(dir))
        {
            var itemInfo = new DirectoryInfo(item);

            var props = new List<string>
            {
                itemInfo.LastWriteTime.ToString("yyyy-MM-dd tt hh:mm"),
                "<DIR>",
                itemInfo.Name
            };
            items.Add([.. props]);
        }

        foreach (var item in Directory.GetFiles(dir))
        {
            var itemInfo = new FileInfo(item);

            var props = new List<string>
            {
                itemInfo.LastWriteTime.ToString("yyyy-MM-dd tt hh:mm"),
                string.Empty,
                itemInfo.Name
            };
            items.Add([.. props]);
        }

        Out.WriteLine();
        Out.PrintTableData([.. items], true);
        Out.WriteLine();

        if (IsRecursive == true)
        {
            foreach (var item in Directory.GetDirectories(dir))
            {
                PrintItems(item);
            }
        }
    }
}

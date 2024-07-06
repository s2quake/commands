// <copyright file="ChangeDirectoryCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using JSSoft.Commands.Applications;

namespace JSSoft.Commands.Repl.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
[Category("IO")]
[method: ImportingConstructor]
sealed class ChangeDirectoryCommand(IApplication application)
    : CommandBase("cd")
{
    [CommandPropertyRequired("dir", DefaultValue = "")]
    public string DirectoryName { get; set; } = string.Empty;

    protected override void OnExecute()
    {
        if (DirectoryName == string.Empty)
        {
            Out.WriteLine(application.CurrentDirectory);
        }
        else if (DirectoryName == "..")
        {
            var parentDirectory = Path.GetDirectoryName(Directory.GetCurrentDirectory())!;
            Directory.SetCurrentDirectory(parentDirectory);
            application.CurrentDirectory = parentDirectory;
        }
        else if (Directory.Exists(DirectoryName) == true)
        {
            var dir = new DirectoryInfo(DirectoryName).FullName;
            Directory.SetCurrentDirectory(dir);
            application.CurrentDirectory = dir;
        }
        else
        {
            throw new DirectoryNotFoundException(string.Format("'{0}'은(는) 존재하지 않는 경로입니다.", DirectoryName));
        }
    }
}

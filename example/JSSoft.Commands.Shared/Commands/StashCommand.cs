// <copyright file="StashCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>
// using System;
// using System.ComponentModel;
// using System.ComponentModel.Composition;

// namespace JSSoft.Commands.Applications.Commands;

// [Export(typeof(ICommand))]
// [ResourceUsage]
// [Category("Git-like")]
// sealed class StashCommand : CommandMethodBase
// {
//     public StashCommand()
//         : base("stash")
//     {
//     }

//     [CommandMethod("show")]
//     [CommandMethodProperty(nameof(Path), nameof(Port))]
//     public void Show(int value, int test = 0)
//     {
//         Console.WriteLine("value : {0}", value);
//         Console.WriteLine("test : {0}", test);
//         Console.WriteLine("port : {0}", Port);
//     }

//     [CommandMethod("save")]
//     [CommandMethodProperty(nameof(Patch), nameof(KeepIndex), nameof(IncludeUntracked), nameof(All), nameof(Quit))]
//     public void Save(string message)
//     {
//         Console.WriteLine(message);
//     }

//     [CommandProperty('p')]
//     public bool Patch { get; set; }

//     [CommandPropertySwitch('k', useName: true)]
//     public bool KeepIndex { get; set; }

//     [CommandPropertySwitch('u', useName: true)]
//     public bool IncludeUntracked { get; set; }

//     [CommandPropertySwitch('a', useName: true)]
//     public bool All { get; set; }

//     [CommandPropertySwitch('q', useName: true)]
//     public bool Quit { get; set; }

//     [CommandProperty]
//     public string Path { get; set; } = string.Empty;

//     [CommandPropertyExplicitRequired('t')]
//     public int Port { get; set; }
// }

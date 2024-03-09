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

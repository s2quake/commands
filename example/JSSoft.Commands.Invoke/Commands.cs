// <copyright file="Commands.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;

namespace JSSoft.Commands.Invoke;

[ResourceUsage]
[CommandStaticMethod(typeof(StaticCommand))]
sealed class Commands
{
    public Commands()
    {
        Message = string.Empty;
    }

    [CommandMethod("init")]
    [CommandMethodStaticProperty(typeof(GlobalSettings))]
    [CommandMethodProperty(nameof(Message))]
    public void Initialize(string path)
    {
        Console.WriteLine("{0} initialized.", path);
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Message))]
    public void Add(params string[] paths)
    {
        foreach (var item in paths)
        {
            Console.WriteLine("{0} added.", item);
        }
    }

    [CommandMethod]
    public void Update(string path = "")
    {
        Console.WriteLine("{0} updated.", path);
    }

    [CommandMethod]
    public void Delete(params string[] paths)
    {
        foreach (var item in paths)
        {
            Console.WriteLine("{0} deleted.", item);
        }
    }

    [CommandMethod]
    [CommandMethodProperty(nameof(Message))]
    public void Commit(string path = "")
    {
        if (Message == string.Empty)
            Console.WriteLine("{0} committed.", path);
        else
            Console.WriteLine("{0} committed. : {1}", path, Message);
    }

    [CommandPropertyExplicitRequired('m', useName: true)]
    [ConsoleModeOnly]
    public string Message { get; set; }
}

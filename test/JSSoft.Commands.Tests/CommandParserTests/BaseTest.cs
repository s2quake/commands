// <copyright file="BaseTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using System.Reflection;

namespace JSSoft.Commands.Tests.CommandParserTests;

public class BaseTest
{
    private sealed class InstanceClass
    {
    }

    [Fact]
    public void Constructor_Arg0_Object_Test()
    {
        var obj = new InstanceClass();
        var parser = new CommandParser(obj);
        var name = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()!.Location);

        Assert.Equal(name, parser.CommandName);
        Assert.Equal(obj, parser.Instance);
    }
}

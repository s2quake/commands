// <copyright file="CommandDescriptorTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class CommandDescriptorTest
{
    [Fact]
    public void Test1()
    {
        var argumentLine = """
            get database=a port=123 userid=abc password=1234 comment=\"connect database to \"a\"\"
            """;
        var parser = new CommandParser(this);
        Assert.Throws<CommandLineException>(() => parser.Parse(argumentLine));
    }
}

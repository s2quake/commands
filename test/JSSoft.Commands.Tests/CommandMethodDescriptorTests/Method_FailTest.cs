// <copyright file="Method_FailTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.CommandMethodDescriptorTests;

public class Method_FailTest
{
    private sealed class Method1Class
    {
        [CommandMethod]
        internal int Method()
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Method1_Test()
    {
        Assert.Throws<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method1Class)));
    }

    private sealed class Method5Class
    {
        [CommandMethod]
        internal void Method(List<int> list)
        {
            throw new NotSupportedException();
        }
    }

    [Fact]
    public void Method5_Test()
    {
        Assert.ThrowsAny<CommandDefinitionException>(
            () => CommandDescriptor.GetMethodDescriptors(typeof(Method5Class)));
    }
}

// <copyright file="CommandMethodDescriptorCollectionTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests;

public class CommandMethodDescriptorCollectionTest
{
    internal static class StaticCommand
    {
        [CommandMethod]
        [Category("Asset")]
        public static void List()
        {
        }

        [CommandMethod]
        public static void Restore()
        {
        }

        [CommandMethod]
        public static void Revert()
        {
        }
    }

    [CommandStaticMethod(typeof(StaticCommand))]
    internal sealed class Commands
    {
        [CommandMethod]
        [Category("Hidden")]
        public void Initialize()
        {
        }

        [CommandMethod]
        public void Add(params string[] paths)
        {
        }

        [CommandMethod]
        [Category("Normal")]
        public void Update(string path = "")
        {
        }

        [CommandMethod]
        public void Delete(params string[] paths)
        {
        }

        [CommandMethod]
        [Category("Normal")]
        public void Commit(string path = "")
        {
        }
    }

    [Fact]
    public void Methods_Order_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Commands));
        var index = 0;
        Assert.Equal(8, methodDescriptors.Count);
        Assert.Equal(nameof(Commands.Initialize), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(Commands.Add), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(Commands.Update), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(Commands.Delete), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(Commands.Commit), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(StaticCommand.List), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(StaticCommand.Restore), methodDescriptors[index++].MethodName);
        Assert.Equal(nameof(StaticCommand.Revert), methodDescriptors[index++].MethodName);
        Assert.Equal(8, index);
    }

    [Fact]
    public void Method_GroupByCategory_Test()
    {
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Commands));
        var groups = methodDescriptors.GroupByCategory();

        Assert.Equal(4, groups.Length);

        Assert.Equal(string.Empty, groups[0].Key);
        Assert.Equal(
            groups[0].Select(item => item.MethodName).ToArray(),
            [
                nameof(Commands.Add),
                nameof(Commands.Delete),
                nameof(StaticCommand.Restore),
                nameof(StaticCommand.Revert),
            ]);
        Assert.Equal("Hidden", groups[1].Key);
        Assert.Equal(
            groups[1].Select(item => item.MethodName).ToArray(),
            [
                nameof(Commands.Initialize),
            ]);
        Assert.Equal("Normal", groups[2].Key);
        Assert.Equal(
            groups[2].Select(item => item.MethodName).ToArray(),
            [
                nameof(Commands.Update),
                nameof(Commands.Commit),
            ]);
        Assert.Equal("Asset", groups[3].Key);
        Assert.Equal(
            groups[3].Select(item => item.MethodName).ToArray(),
            [
                nameof(StaticCommand.List),
            ]);
    }

    [Fact]
    public void Method_GroupByCategory_WithoutHidden_Test()
    {
        var settings = new CommandSettings();
        var methodDescriptors = CommandDescriptor.GetMethodDescriptors(typeof(Commands));
        var groups = methodDescriptors.GroupByCategory(settings.CategoryPredicate);

        Assert.Equal(3, groups.Length);

        Assert.Equal(string.Empty, groups[0].Key);
        Assert.Equal(
            groups[0].Select(item => item.MethodName).ToArray(),
            [
                nameof(Commands.Add),
                nameof(Commands.Delete),
                nameof(StaticCommand.Restore),
                nameof(StaticCommand.Revert),
            ]);
        Assert.Equal("Normal", groups[1].Key);
        Assert.Equal(
            groups[1].Select(item => item.MethodName).ToArray(),
            [
                nameof(Commands.Update),
                nameof(Commands.Commit),
            ]);
        Assert.Equal("Asset", groups[2].Key);
        Assert.Equal(
            groups[2].Select(item => item.MethodName).ToArray(),
            [
                nameof(StaticCommand.List),
            ]);
    }
}

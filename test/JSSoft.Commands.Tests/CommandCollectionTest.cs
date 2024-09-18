// <copyright file="CommandCollectionTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests;

public class CommandCollectionTest
{
    [Category("Hidden")]
    private sealed class HiddenCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do something
        }
    }

    [Category("Normal")]
    private sealed class AddCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do something
        }
    }

    [Category("Asset")]
    private sealed class InitCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do something
        }
    }

    private sealed class DeleteCommand : CommandBase
    {
        protected override void OnExecute()
        {
            // do something
        }
    }

    [Fact]
    public void Commands_Order_Test()
    {
        var items = new ICommand[]
        {
            new HiddenCommand(),
            new AddCommand(),
            new InitCommand(),
            new DeleteCommand(),
        };
        var commands = new CommandCollection(items);
        var index = 0;
        Assert.Equal(4, commands.Count);
        Assert.IsType<HiddenCommand>(commands[index++]);
        Assert.IsType<AddCommand>(commands[index++]);
        Assert.IsType<InitCommand>(commands[index++]);
        Assert.IsType<DeleteCommand>(commands[index++]);
        Assert.Equal(4, index);
    }

    [Fact]
    public void Commands_GroupByCategory_Test()
    {
        var items = new ICommand[]
        {
            new HiddenCommand(),
            new AddCommand(),
            new InitCommand(),
            new DeleteCommand(),
        };
        var commands = new CommandCollection(items);
        var groups = commands.GroupByCategory();

        Assert.Equal(4, groups.Length);

        Assert.Equal(string.Empty, groups[0].Key);
        Assert.Equal(
            groups[0].Select(item => item.GetType()).ToArray(),
            [
                typeof(DeleteCommand),
            ]);
        Assert.Equal("Hidden", groups[1].Key);
        Assert.Equal(
            groups[1].Select(item => item.GetType()).ToArray(),
            [
                typeof(HiddenCommand),
            ]);
        Assert.Equal("Normal", groups[2].Key);
        Assert.Equal(
            groups[2].Select(item => item.GetType()).ToArray(),
            [
                typeof(AddCommand),
            ]);
        Assert.Equal("Asset", groups[3].Key);
        Assert.Equal(
            groups[3].Select(item => item.GetType()).ToArray(),
            [
                typeof(InitCommand),
            ]);
    }

    [Fact]
    public void Commands_GroupByCategory_WithoutHidden_Test()
    {
        var settings = new CommandSettings();
        var items = new ICommand[]
        {
            new HiddenCommand(),
            new AddCommand(),
            new InitCommand(),
            new DeleteCommand(),
        };
        var commands = new CommandCollection(items);
        var groups = commands.GroupByCategory(settings.CategoryPredicate);

        Assert.Equal(3, groups.Length);

        Assert.Equal(string.Empty, groups[0].Key);
        Assert.Equal(
            groups[0].Select(item => item.GetType()).ToArray(),
            [
                typeof(DeleteCommand),
            ]);
        Assert.Equal("Normal", groups[1].Key);
        Assert.Equal(
            groups[1].Select(item => item.GetType()).ToArray(),
            [
                typeof(AddCommand),
            ]);
        Assert.Equal("Asset", groups[2].Key);
        Assert.Equal(
            groups[2].Select(item => item.GetType()).ToArray(),
            [
                typeof(InitCommand),
            ]);
    }
}

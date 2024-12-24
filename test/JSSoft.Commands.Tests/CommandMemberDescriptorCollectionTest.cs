// <copyright file="CommandMemberDescriptorCollectionTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands.Tests;

public class CommandMemberDescriptorCollectionTest
{
    internal static class GlobalSettings
    {
        [CommandProperty]
        [Category("Asset")]
        public static string Id { get; set; } = string.Empty;

        [CommandProperty]
        public static string Password { get; set; } = string.Empty;

        [CommandPropertyRequired]
        public static string LogPath { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired]
        public static string Platform { get; set; } = string.Empty;
    }

    [CommandStaticProperty(typeof(GlobalSettings))]
    private sealed class Settings
    {
        [CommandPropertyRequired]
        public string Path { get; set; } = string.Empty;

        [CommandPropertyRequired]
        public string ServiceName { get; set; } = string.Empty;

        [CommandPropertyExplicitRequired]
        public string WorkingPath { get; set; } = string.Empty;

        [CommandProperty('p', useName: true, InitValue = "10001")]
        [Category]
        public int Port { get; set; }

        [CommandPropertySwitch]
        [Category("Normal")]
        public bool UseCache { get; set; }

        [CommandProperty("cache-size", DefaultValue = 1024)]
        public int CacheSize { get; set; }

        [CommandPropertyArray]
        public string[] Libraries { get; set; } = [];

        [CommandProperty]
        [Category("Normal")]
        public string Comment { get; set; } = string.Empty;
    }

    [Fact]
    public void Members_Order_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(Settings));
        var index = 0;
        Assert.Equal(12, memberDescriptors.Count);
        Assert.Equal(nameof(Settings.WorkingPath), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(GlobalSettings.Platform), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.Path), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.ServiceName), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(GlobalSettings.LogPath), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.Libraries), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.Port), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.UseCache), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.CacheSize), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(Settings.Comment), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(GlobalSettings.Id), memberDescriptors[index++].MemberName);
        Assert.Equal(nameof(GlobalSettings.Password), memberDescriptors[index++].MemberName);
        Assert.Equal(12, index);
    }

    [Fact]
    public void Members_GroupByCategory_Test()
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(Settings));
        var groups = memberDescriptors.GroupOptionsByCategory();

        Assert.Equal(4, groups.Length);

        Assert.Equal(string.Empty, groups[0].Key);
        Assert.Equal<string>(
            groups[0].Select(item => item.MemberName).ToArray(),
            [
                nameof(Settings.CacheSize),
                nameof(GlobalSettings.Password),
            ]);
        Assert.Equal(CategoryAttribute.Default.Category, groups[1].Key);
        Assert.Equal<string>(
            groups[1].Select(item => item.MemberName).ToArray(),
            [
                nameof(Settings.Port),
            ]);
        Assert.Equal("Normal", groups[2].Key);
        Assert.Equal<string>(
            groups[2].Select(item => item.MemberName).ToArray(),
            [
                nameof(Settings.UseCache),
                nameof(Settings.Comment),
            ]);
        Assert.Equal("Asset", groups[3].Key);
        Assert.Equal<string>(
            groups[3].Select(item => item.MemberName).ToArray(),
            [
                nameof(GlobalSettings.Id),
            ]);
    }

    [Fact]
    public void Members_GroupByCategory_WithoutHidden_Test()
    {
        var settings = new CommandSettings();
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(Settings));
        var groups = memberDescriptors.GroupOptionsByCategory(settings.CategoryPredicate);

        Assert.Equal(3, groups.Length);

        Assert.Equal(string.Empty, groups[0].Key);
        Assert.Equal<string>(
            groups[0].Select(item => item.MemberName).ToArray(),
            [
                nameof(Settings.CacheSize),
                nameof(GlobalSettings.Password),
            ]);
        Assert.Equal("Normal", groups[1].Key);
        Assert.Equal<string>(
            groups[1].Select(item => item.MemberName).ToArray(),
            [
                nameof(Settings.UseCache),
                nameof(Settings.Comment),
            ]);
        Assert.Equal("Asset", groups[2].Key);
        Assert.Equal<string>(
            groups[2].Select(item => item.MemberName).ToArray(),
            [
                nameof(GlobalSettings.Id),
            ]);
    }
}

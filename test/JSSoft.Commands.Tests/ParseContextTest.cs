// <copyright file="ParseContextTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests;

public class ParseContextTest
{
    [CommandPropertyExplicitRequired("es")]
    public string ExplicitString { get; set; } = string.Empty;

    [CommandPropertyExplicitRequired("en", DefaultValue = 1)]
    public int ExplicitNumber { get; set; }

    [CommandPropertyRequired]
    public string RequiredString { get; set; } = string.Empty;

    [CommandPropertyRequired]
    public int RequiredNumber { get; set; }

    [CommandProperty('t')]
    public string String { get; set; } = string.Empty;

    [CommandProperty('n', DefaultValue = 1)]
    public int Number { get; set; }

    [CommandPropertySwitch('s')]
    public bool Switch { get; set; }

    [CommandPropertyArray]
    public string[] Items { get; set; } = [];

    public static readonly IEnumerable<object[]> TestMethod1TestData =
    [
        [0, Array.Empty<string>(), nameof(RequiredString)],
        [1, new string[] { "a", }, nameof(RequiredNumber)],
        [2, new string[] { "a", "1" }, string.Empty],
        [3, new string[] { "a", "--es" }, nameof(ExplicitString)],
        [4, new string[] { "--es" }, nameof(ExplicitString)],
        [5, new string[] { "a", "1", "--es", "a" }, string.Empty],
        [6, new string[] { "a", "1", "--es", "a", "--en" }, nameof(ExplicitNumber)],
        [7, new string[] { "a", "1", "--en" }, nameof(ExplicitNumber)],
        [8, new string[] { "a", "1", "--es", "a", "--en", "0" }, nameof(Items)],
        [9, new string[] { "a", "1", "--es", "a", "--en", "--" }, nameof(Items)],
        [10, new string[] { "a", "1", "--es", "a", "--en", "0", "--" }, nameof(Items)],
        [11, new string[] { "-s" }, nameof(RequiredString)],
        [12, new string[] { "-t" }, nameof(String)],
        [13, new string[] { "a", "1", "--es", "a", "--en", "-n" }, nameof(Number)],
        [14, new string[] { "a", "1", "--es", "a", "--en", "-n", "0" }, nameof(Items)],
        [15, new string[] { "a", "1", "--es", "a", "--en", "-n", "0", "--" }, nameof(Items)],
        [16, new string[] { "a", "1", "--es", "a", "--en", "-n", "0", "-s" }, nameof(Items)],
        [17, new string[] { "a", "1", "--es", "a", "--en", "-n", "0", "-s", "--" }, nameof(Items)],
    ];

    [Theory]
    [MemberData(nameof(TestMethod1TestData))]
    public void TestMethod1(int index, string[] args, string expectedPropertyName)
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(ParseContextTest));
        var parseContext = new ParseContext(memberDescriptors);
        parseContext.Next(args);
        var descriptor = parseContext.Descriptor;
        var actualPropertyName = descriptor?.MemberDescriptor.MemberName ?? string.Empty;
        Assert.Equal(expectedPropertyName, actualPropertyName);
        Assert.True(index >= 0);
    }

    public static readonly IEnumerable<object[]> TestMethod2TestData =
    [
        [
            0,
            new string[] { "--" },
            "Cannot use '--'. All required options must be set.",
        ],
        [
            1,
            new string[] { "a", "1", "--es", "a", "--en", "--", "--" },
            "The '--' option cannot be used because the mode is already set to ",
        ],
        [
            2,
            new string[] { "a", "1", "--es", "--" },
            "Cannot use '--'. Value of the '--es' must be specified",
        ],
    ];

    [Theory]
    [MemberData(nameof(TestMethod2TestData))]
    public void TestMethod2_Throw(int index, string[] args, string message)
    {
        var memberDescriptors = CommandDescriptor.GetMemberDescriptors(typeof(ParseContextTest));
        var parseContext = new ParseContext(memberDescriptors);
        var exception = Assert.Throws<ArgumentException>(() => parseContext.Next(args));
        Assert.StartsWith(message, exception.Message);
        Assert.True(index >= 0);
    }
}

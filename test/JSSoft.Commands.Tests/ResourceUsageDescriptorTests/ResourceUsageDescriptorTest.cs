// <copyright file="ResourceUsageDescriptorTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Tests.ResourceUsageDescriptorTests;

public class ResourceUsageDescriptorTest
{
    [Fact]
    public void ResourceStaticProperties_Test()
    {
        var type = typeof(ResourceStaticProperties);
        var usageDescriptor = CommandDescriptor.GetUsage(type);

        Assert.Equal(
            expected: "Resource Static Properties",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "Resource Static Properties Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "Resource Static Properties Example",
            actual: usageDescriptor.Example);
    }

    [Fact]
    public void ResourceProperties_Test()
    {
        var type = typeof(ResourceProperties);
        var usageDescriptor = CommandDescriptor.GetUsage(type);

        Assert.Equal(
            expected: "Resource Properties",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "Resource Properties Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "Resource Properties Example",
            actual: usageDescriptor.Example);
    }

    [Fact]
    public void ResourceStaticMethods_Method1_Test()
    {
        var type = typeof(ResourceStaticMethods);
        var methodInfo = type.GetMethod(nameof(ResourceStaticMethods.Method1))!;
        var usageDescriptor = CommandDescriptor.GetUsage(methodInfo);

        Assert.Equal(
            expected: "Method1 Summary",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "Method1 Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "Method1 Example",
            actual: usageDescriptor.Example);
    }

    [Fact]
    public void ResourceStaticMethods_Method1_text_Test()
    {
        var type = typeof(ResourceStaticMethods);
        var methodInfo = type.GetMethod(nameof(ResourceStaticMethods.Method1))!;
        var parameterInfo = methodInfo.GetParameters()[0];
        var usageDescriptor = CommandDescriptor.GetUsage(parameterInfo);

        Assert.Equal(
            expected: "text Summary",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "text Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "text Example",
            actual: usageDescriptor.Example);
    }

    [Fact]
    public void ResourceStaticMethods_Method1_value_Test()
    {
        var type = typeof(ResourceStaticMethods);
        var methodInfo = type.GetMethod(nameof(ResourceStaticMethods.Method1))!;
        var parameterInfo = methodInfo.GetParameters()[1];
        var usageDescriptor = CommandDescriptor.GetUsage(parameterInfo);

        Assert.Equal(
            expected: "value Summary",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "value Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "value Example",
            actual: usageDescriptor.Example);
    }

    [Fact]
    public void ResourceStaticMethods_Method2Async_Test()
    {
        var type = typeof(ResourceStaticMethods);
        var methodInfo = type.GetMethod(nameof(ResourceStaticMethods.Method2Async))!;
        var usageDescriptor = CommandDescriptor.GetUsage(methodInfo);

        Assert.Equal(
            expected: "Method2Async Summary",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "Method2Async Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "Method2Async Example",
            actual: usageDescriptor.Example);
    }

    [Fact]
    public void ResourceStaticMethods_Method2Async_text_Test()
    {
        var type = typeof(ResourceStaticMethods);
        var methodInfo = type.GetMethod(nameof(ResourceStaticMethods.Method2Async))!;
        var parameterInfo = methodInfo.GetParameters()[0];
        var usageDescriptor = CommandDescriptor.GetUsage(parameterInfo);

        Assert.Equal(
            expected: "text Summary",
            actual: usageDescriptor.Summary);
        Assert.Equal(
            expected: "text Description",
            actual: usageDescriptor.Description);
        Assert.Equal(
            expected: "text Example",
            actual: usageDescriptor.Example);
    }
}

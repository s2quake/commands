// <copyright file="StepProgressTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Tests;

public class StepProgressTest
{
    private const int TestTimeout = 1000;

    [Fact]
    public void Base_Test()
    {
        var progress = new StepProgress<int>(1);
        Assert.Equal(1, progress.Length);
        Assert.Equal(-1, progress.Step);
        Assert.False(progress.IsCompleted);

        progress.Next(0);
        Assert.Equal(0, progress.Step);
        Assert.False(progress.IsCompleted);

        progress.Complete(1);
        Assert.Equal(1, progress.Step);
        Assert.True(progress.IsCompleted);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Base_InvalidLength_Throw(int length)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new StepProgress<int>(length));
    }

    [Fact]
    public void ForEach_Test()
    {
        var items = Enumerable.Range(0, 10).ToArray();
        var actualList = new List<int>(items.Length);
        var progress = new StepProgress<int>(items.Length);
        var autoResetEvent = new AutoResetEvent(false);
        progress.ProgressChanged += (s, e) =>
        {
            actualList.Add(e);
            autoResetEvent.Set();
        };

        for (var i = 0; i < items.Length; i++)
        {
            progress.Next(i);
            Assert.True(autoResetEvent.WaitOne(1000));
        }

        Assert.False(progress.IsCompleted);
        Assert.Equal(items.Length - 1, progress.Step);

        progress.Complete(10);

        Assert.Equal(items.Length, progress.Step);
        Assert.Equal(items.Concat([10]), actualList);
    }

    [Fact(Timeout = TestTimeout)]
    public async Task ForEach_Parallel_TestAsync()
    {
        var items = Enumerable.Range(0, 10).ToArray();
        var actualList = new List<int>(items.Length);
        var progress = new StepProgress<int>(items.Length);
        progress.ProgressChanged += (s, e) => actualList.Add(e);

        Parallel.ForEach(items, progress.Next);
        await TaskUtility.WaitIfAsync(() => actualList.Count < items.Length);

        Assert.False(progress.IsCompleted);
        Assert.Equal(items.Length - 1, progress.Step);

        progress.Complete(10);
        await TaskUtility.WaitIfAsync(() => actualList.Count < items.Length + 1);
        Assert.Equal(items.Length, progress.Step);
        Assert.Equal(items.Concat([10]), actualList.Order());
    }

    [Fact]
    public void ForEach_OverStep_Throw()
    {
        const int completionValue = 10;
        var progress = new StepProgress<int>(completionValue);

        for (var i = 0; i < completionValue; i++)
        {
            progress.Next(i);
        }

        Assert.Throws<InvalidOperationException>(() => progress.Next(completionValue));
    }

    [Fact]
    public void ForEach_Completion_Twice_Throw()
    {
        const int completionValue = 10;
        var progress = new StepProgress<int>(completionValue);

        for (var i = 0; i < completionValue; i++)
        {
            progress.Next(i);
        }

        progress.Complete(10);

        Assert.Throws<InvalidOperationException>(() => progress.Complete(10));
    }

    [Fact]
    public void ForEach_Complete_NotCompleted_Throw()
    {
        const int completionValue = 10;
        var length = RandomUtility.Int32(1, 10);
        var progress = new StepProgress<int>(completionValue);

        for (var i = 0; i < length; i++)
        {
            progress.Next(i);
        }

        Assert.Throws<InvalidOperationException>(() => progress.Complete(completionValue));
    }
}

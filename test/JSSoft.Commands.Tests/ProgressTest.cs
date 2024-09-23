// <copyright file="ProgressTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;

namespace JSSoft.Commands.Tests;

public class ProgressTest
{
    private const int TestTimeout = 1000;

    [Fact]
    public void PreProgress_Test()
    {
        var length = RandomUtility.Int32(1, 100);
        var i1 = RandomUtility.Int32(1, 9999);
        var i2 = RandomUtility.Int32(i1 + 1, 10000);
        var begin = i1 / 10000.0;
        var end = i2 / 10000.0;
        var progress = new Progress<ProgressInfo>();
        var preProgress = progress.PreProgress(begin, end, length);
        var autoResetEvent = new AutoResetEvent(false);
        var items = new List<double>(length);

        progress.ProgressChanged += (s, e) =>
        {
            items.Add(e.Value);
            autoResetEvent.Set();
        };

        for (var i = 0; i < length; i++)
        {
            preProgress.Next($"{i}");
            Assert.True(autoResetEvent.WaitOne(1000));
        }

        Assert.Equal(length, items.Count);
        for (var i = 0; i < length; i++)
        {
            var expectedPercentage = begin + ((end - begin) / length * i);
            var expectedValue = Math.Round(expectedPercentage, 4);
            var actualValue = Math.Round(items[i], 4);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void PreProgress_OnePercent_Test()
    {
        var length = 100;
        var begin = 0.0;
        var end = 1.0;
        var progress = new Progress<ProgressInfo>();
        var preProgress = progress.PreProgress(begin, end, length);
        var autoResetEvent = new AutoResetEvent(false);
        var items = new List<double>(length);

        progress.ProgressChanged += (s, e) =>
        {
            items.Add(e.Value);
            autoResetEvent.Set();
        };

        for (var i = 0; i < length; i++)
        {
            preProgress.Next($"{i}");
            Assert.True(autoResetEvent.WaitOne(1000));
        }

        Assert.Equal(length, items.Count);
        for (var i = 0; i < length; i++)
        {
            var expectedValue = Math.Round(i / 100.0, 4);
            var actualValue = Math.Round(items[i], 4);
            Assert.Equal(expectedValue, actualValue);
        }
    }

    [Fact]
    public void PreProgress_OutOfValue_ThrowTest()
    {
        var progress = new Progress<ProgressInfo>();
        Assert.Throws<ArgumentException>(() => progress.PreProgress(-0.1, 0.1, 10));
        Assert.Throws<ArgumentException>(() => progress.PreProgress(0.1, 0.0, 10));
        Assert.Throws<ArgumentException>(() => progress.PreProgress(0.0, 1.1, 10));
    }

    [Fact]
    public void PostProgress_Test()
    {
        var length = RandomUtility.Int32(1, 100);
        var i1 = RandomUtility.Int32(1, 9999);
        var i2 = RandomUtility.Int32(i1 + 1, 10000);
        var begin = i1 / 10000.0;
        var end = i2 / 10000.0;
        var progress = new Progress<ProgressInfo>();
        var postProgress = progress.PostProgress(begin, end, length);
        var autoResetEvent = new AutoResetEvent(false);
        var items = new List<double>(length);

        progress.ProgressChanged += (s, e) =>
        {
            items.Add(e.Value);
            autoResetEvent.Set();
        };

        for (var i = 0; i < length; i++)
        {
            postProgress.Next($"{i}");
            Assert.True(autoResetEvent.WaitOne(1000));
        }

        Assert.Equal(length, items.Count);
        for (var i = 0; i < length; i++)
        {
            var expectedPercentage = begin + ((end - begin) / length * (i + 1));
            var expectedValue = Math.Round(expectedPercentage, 4);
            var actualValue = Math.Round(items[i], 4);
            Assert.Equal(expectedValue, actualValue);
        }

        postProgress.Complete("completed");
        Assert.True(autoResetEvent.WaitOne(1000));
        Assert.Equal(end, items[length]);
    }

    [Fact]
    public void PostProgress_OnePercentage_Test()
    {
        var length = 100;
        var begin = 0.0;
        var end = 1.0;
        var progress = new Progress<ProgressInfo>();
        var postProgress = progress.PostProgress(begin, end, length);
        var autoResetEvent = new AutoResetEvent(false);
        var items = new List<double>(length);

        progress.ProgressChanged += (s, e) =>
        {
            items.Add(e.Value);
            autoResetEvent.Set();
        };

        for (var i = 0; i < length; i++)
        {
            postProgress.Next($"{i}");
            Assert.True(autoResetEvent.WaitOne(1000));
        }

        Assert.Equal(length, items.Count);
        for (var i = 0; i < length; i++)
        {
            var expectedValue = Math.Round((i + 1) / 100.0, 4);
            var actualValue = Math.Round(items[i], 4);
            Assert.Equal(expectedValue, actualValue);
        }

        postProgress.Complete("completed");
        Assert.True(autoResetEvent.WaitOne(1000));
        Assert.Equal(1.0, items[length]);
    }

    [Fact]
    public void PostProgress_OutOfValue_ThrowTest()
    {
        var progress = new Progress<ProgressInfo>();
        Assert.Throws<ArgumentException>(() => progress.PostProgress(-0.1, 0.1, 10));
        Assert.Throws<ArgumentException>(() => progress.PostProgress(0.1, 0.0, 10));
        Assert.Throws<ArgumentException>(() => progress.PostProgress(0.0, 1.1, 10));
    }
}

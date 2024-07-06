// <copyright file="ProgressExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Extensions;

public static class ProgressExtensions
{
    public static void Report(this IProgress<ProgressInfo> @this, double value, string text)
    {
        @this.Report(new ProgressInfo() { Value = value, Text = text });
    }

    public static void Complete(this IProgress<ProgressInfo> @this, string text)
    {
        @this.Report(new ProgressInfo() { Value = double.MaxValue, Text = text });
    }
}

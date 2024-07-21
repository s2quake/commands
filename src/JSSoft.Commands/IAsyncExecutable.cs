// <copyright file="IAsyncExecutable.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands;

public interface IAsyncExecutable
{
    Task ExecuteAsync(CancellationToken cancellationToken, IProgress<ProgressInfo> progress);
}

// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace JSSoft.Terminals.Pty;

/// <summary>
/// Options for spawning a new pty process.
/// </summary>
public sealed class PtyOptions
{
    private int _width;
    private int _height;

    /// <summary>
    /// Gets or sets the number of initial rows.
    /// </summary>
    public int Height
    {
        get => _height;
        set
        {
            if (value < 0 || value > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value));
            _height = value;
        }
    }

    /// <summary>
    /// Gets or sets the number of initial columns.
    /// </summary>
    public int Width
    {
        get => _width;
        set
        {
            if (value < 0 || value > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value));
            _width = value;
        }
    }

    /// <summary>
    /// Gets or sets the working directory for the spawned process.
    /// </summary>
    public string WorkingDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path to the process to be spawned.
    /// </summary>
    public string App { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the command line arguments to the process.
    /// </summary>
    public string[] CommandLine { get; set; } = [];

    /// <summary>
    /// Gets or sets the process' environment variables.
    /// </summary>
    public IReadOnlyDictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();

    public static PtyOptions Default { get; } = new();
}

// <copyright file="PtyOptions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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

    public static PtyOptions Default { get; } = new();

    /// <summary>
    /// Gets or sets the number of initial rows.
    /// </summary>
    public int Height
    {
        get => _height;
        set
        {
            if (value < 0 || value > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

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
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

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
    public IReadOnlyDictionary<string, string> EnvironmentVariables { get; set; }
        = new Dictionary<string, string>();
}

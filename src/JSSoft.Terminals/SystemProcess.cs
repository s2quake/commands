// <copyright file="SystemProcess.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Terminals;

public sealed class SystemProcess
{
    private readonly ProcessStartInfo _startInfo = new()
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        RedirectStandardInput = true,
        StandardOutputEncoding = Encoding.UTF8,
        StandardErrorEncoding = Encoding.UTF8,
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        StandardInputEncoding = Encoding.UTF8,
#endif
    };

    public SystemProcess(string filename)
        : this(filename, Directory.GetCurrentDirectory())
    {
    }

    public SystemProcess(string filename, string workingDirectory)
    {
        FileName = filename;
        WorkingDirectory = workingDirectory;
    }

    public string FileName
    {
        get => _startInfo.FileName;
        set => _startInfo.FileName = value;
    }

    public string WorkingDirectory
    {
        get => _startInfo.WorkingDirectory;
        set => _startInfo.WorkingDirectory = value;
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public Collection<string> ArgumentList => _startInfo.ArgumentList;
#else
    public List<string> ArgumentList { get; } = new();
#endif

    public async Task<int> StartAsync(CancellationToken cancellationToken)
    {
#if !NETCOREAPP2_1_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
        _startInfo.Arguments = string.Join(" ", ArgumentList.Select(item => $"\"{item}\""));
#endif
        using var process = new Process()
        {
            StartInfo = _startInfo,
        };
        process.OutputDataReceived += (s, e) => OnOutputDataReceived(e);
        process.ErrorDataReceived += (s, e) => OnErrorDataReceived(e);
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
#if NET5_0_OR_GREATER
        await process.WaitForExitAsync(cancellationToken);
#else
        await Task.Run(() => process.WaitForExit());
#endif
        return process.ExitCode;
    }

    private void OnErrorDataReceived(DataReceivedEventArgs e)
    {
        // Error.WriteLine(e.Data);
    }

    private void OnOutputDataReceived(DataReceivedEventArgs e)
    {
        // Out.WriteLine(e.Data);
    }
}

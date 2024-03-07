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

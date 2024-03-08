// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

namespace JSSoft.Commands;

sealed class CommandProgress(IProgress<ProgressInfo> progress)
        : IProgress<sbyte>
    , IProgress<byte>
    , IProgress<short>
    , IProgress<ushort>
    , IProgress<int>
    , IProgress<uint>
    , IProgress<long>
    , IProgress<ulong>
    , IProgress<float>
    , IProgress<double>
    , IProgress<decimal>
    , IProgress<ProgressInfo>
{
    private readonly IProgress<ProgressInfo> _progress = progress;

    private void Report(long value)
    {
        if (value == long.MaxValue)
            _progress.Report(new ProgressInfo() { Value = double.MaxValue, Text = string.Empty });
        else
            _progress.Report(new ProgressInfo() { Value = value / 100.0, Text = string.Empty });
    }

    private void Report(ulong value)
    {
        if (value == ulong.MaxValue)
            _progress.Report(new ProgressInfo() { Value = double.MaxValue, Text = string.Empty });
        else
            _progress.Report(new ProgressInfo() { Value = value / 100.0, Text = string.Empty });
    }

    #region Implementations

    void IProgress<sbyte>.Report(sbyte value) => Report(value == sbyte.MaxValue ? long.MaxValue : value);

    void IProgress<byte>.Report(byte value) => Report(value == byte.MaxValue ? ulong.MaxValue : value);

    void IProgress<short>.Report(short value) => Report(value == short.MaxValue ? long.MaxValue : value);

    void IProgress<ushort>.Report(ushort value) => Report(value == ushort.MaxValue ? ulong.MaxValue : value);

    void IProgress<int>.Report(int value) => Report(value == int.MaxValue ? long.MaxValue : value);

    void IProgress<uint>.Report(uint value) => Report(value == uint.MaxValue ? ulong.MaxValue : value);

    void IProgress<long>.Report(long value) => Report(value);

    void IProgress<ulong>.Report(ulong value) => Report(value);

    void IProgress<float>.Report(float value)
    {
        if (value == float.MaxValue)
            _progress.Report(new ProgressInfo() { Value = double.MaxValue, Text = string.Empty });
        else
            _progress.Report(new ProgressInfo() { Value = value, Text = string.Empty });
    }

    void IProgress<double>.Report(double value) => _progress.Report(new ProgressInfo() { Value = value, Text = string.Empty });

    void IProgress<decimal>.Report(decimal value)
    {
        if (value == decimal.MaxValue)
            _progress.Report(new ProgressInfo() { Value = double.MaxValue, Text = string.Empty });
        else
            _progress.Report(new ProgressInfo() { Value = checked((double)value), Text = string.Empty });
    }

    void IProgress<ProgressInfo>.Report(ProgressInfo value) => _progress.Report(value);

    #endregion
}

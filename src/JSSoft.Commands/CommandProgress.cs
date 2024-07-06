// <copyright file="CommandProgress.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

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

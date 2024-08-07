// <copyright file="TerminalColor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public struct TerminalColor : IEquatable<TerminalColor>
{
    private byte _a;
    private byte _r;
    private byte _g;
    private byte _b;

    public override readonly bool Equals(object? obj)
    {
        if (obj is TerminalColor color)
        {
            return _a == color._a && _r == color._r && _g == color._g && _b == color._b;
        }

        return base.Equals(obj);
    }

    public override readonly int GetHashCode()
        => _a.GetHashCode() ^ _r.GetHashCode() ^ _g.GetHashCode() ^ _b.GetHashCode();

    public override readonly string ToString()
    {
        if (Name != string.Empty)
        {
            return Name;
        }
        else if (A == 255)
        {
            return $"#{R:x2}{G:x2}{B:x2}";
        }

        return $"#{R:x2}{G:x2}{B:x2}{A:x2}";
    }

    public readonly uint ToUInt32()
        => ((uint)A << 24) | ((uint)R << 16) | ((uint)G << 8) | B;

    public readonly TerminalColor ToComplementary()
    {
        var bytes = BitConverter.GetBytes(ToUInt32() ^ 0xffffff);
        return FromArgb(a: A, r: bytes[1], g: bytes[2], b: bytes[3]);
    }

    public static TerminalColor FromLightness(byte value)
        => new() { _a = 255, _r = value, _g = value, _b = value };

    public static TerminalColor FromRgb(byte r, byte g, byte b)
        => new() { _a = 255, _r = r, _g = g, _b = b };

    public static TerminalColor FromArgb(byte a, byte r, byte g, byte b)
        => new() { _a = a, _r = r, _g = g, _b = b };

    public static TerminalColor FromAColor(byte a, TerminalColor color)
        => FromArgb(a, color.R, color.G, color.B);

    public static TerminalColor FromScArgb(float a, float r, float g, float b)
        => FromArgb((byte)(a * 255.0f), (byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f));

    public static TerminalColor FromScAColor(float a, TerminalColor color)
        => FromArgb((byte)(a * 255.0f), color.R, color.G, color.B);

    public static TerminalColor FromScRgb(float r, float g, float b)
        => FromRgb((byte)(r * 255.0f), (byte)(g * 255.0f), (byte)(b * 255.0f));

    public static TerminalColor FromValues(float[] values)
        => FromScArgb(values[0], values[1], values[2], values[3]);

    public static TerminalColor FromUInt32(uint value)
        => FromArgb(
            (byte)((value >> 24) & 0xff),
            (byte)((value >> 16) & 0xff),
            (byte)((value >> 8) & 0xff),
            (byte)(value & 0xff)
        );

    internal static TerminalColor FromArgb(byte a, byte r, byte g, byte b, string name)
        => new() { _a = a, _r = r, _g = g, _b = b, Name = name };

    public byte A
    {
        readonly get => _a;
        set
        {
            _a = value;
            Name = string.Empty;
        }
    }

    public byte R
    {
        readonly get => _r;
        set
        {
            _r = value;
            Name = string.Empty;
        }
    }

    public byte G
    {
        readonly get => _g;
        set
        {
            _g = value;
            Name = string.Empty;
        }
    }

    public byte B
    {
        readonly get => _b;
        set
        {
            _b = value;
            Name = string.Empty; ;
        }
    }

    public float ScA
    {
        readonly get => _a / 255.0f;
        set
        {
            if (value < 0 || value > 1.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _a = (byte)(value * 255.0f);
            Name = string.Empty;
        }
    }

    public float ScR
    {
        readonly get => _r / 255.0f;
        set
        {
            if (value < 0 || value > 1.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _r = (byte)(value * 255.0f);
            Name = string.Empty;
        }
    }

    public float ScG
    {
        readonly get => _g / 255.0f;
        set
        {
            if (value < 0 || value > 1.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _g = (byte)(value * 255.0f);
            Name = string.Empty;
        }
    }

    public float ScB
    {
        readonly get => _b / 255.0f;
        set
        {
            if (value < 0 || value > 1.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _b = (byte)(value * 255.0f);
            Name = string.Empty;
        }
    }

    public string Name { get; private set; }

    public static TerminalColor Empty { get; } = new TerminalColor();

    public static TerminalColor White { get; } = new TerminalColor();

    public static bool operator ==(TerminalColor color1, TerminalColor color2)
        => color1._a == color2._a && color1._r == color2._r && color1._g == color2._g && color1._b == color2._b;

    public static bool operator !=(TerminalColor color1, TerminalColor color2)
        => color1._a != color2._a || color1._r != color2._r || color1._g != color2._g || color1._b != color2._b;

    public static implicit operator uint(TerminalColor color) => color.ToUInt32();

    readonly bool IEquatable<TerminalColor>.Equals(TerminalColor other)
        => _a == other._a && _r == other._r && _g == other._g && _b == other._b;
}

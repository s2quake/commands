// <copyright file="TerminalMathUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals;

public static class TerminalMathUtility
{
    public static double Clamp(double value, double min, double max)
        => (value < min) ? min : (value > max) ? max : value;

    public static int Clamp(int value, int min, int max)
        => (value < min) ? min : (value > max) ? max : value;
}

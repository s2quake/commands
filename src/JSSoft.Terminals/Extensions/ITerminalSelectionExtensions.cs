// <copyright file="ITerminalSelectionExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Extensions;

public static class ITerminalSelectionExtensions
{
    public static void Select(this ITerminalSelectionCollection @this, TerminalSelection selection)
    {
        @this.Clear();
        @this.Add(selection);
    }

    public static bool TrySelect(this ITerminalSelectionCollection @this, TerminalSelection selection)
    {
        if (selection != TerminalSelection.Empty)
        {
            @this.Clear();
            @this.Add(selection);
            return true;
        }
        return false;
    }

    public static bool TryClear(this ITerminalSelectionCollection @this)
    {
        if (@this.Count > 0)
        {
            @this.Clear();
            return true;
        }
        return false;
    }

    public static bool IsSelected(this ITerminalSelectionCollection @this, TerminalCoord coord)
    {
        if (@this.Any())
        {
            foreach (var item in @this)
            {
                var c1 = item.BeginCoord;
                var c2 = item.EndCoord;
                if (coord >= c1 && coord < c2)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

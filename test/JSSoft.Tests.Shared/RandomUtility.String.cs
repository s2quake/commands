// <copyright file="RandomUtility.String.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if JSSOFT_COMMANDS
namespace JSSoft.Commands.Tests;
#endif

#if JSSOFT_TERMINALS
namespace JSSoft.Terminals.Tests;
#endif

public static partial class RandomUtility
{
    public static string NextString()
    {
        return NextString(false);
    }

    public static string NextString(bool multiline)
    {
        string s = string.Empty;
        int count = Int32(1, 20);
        for (int i = 0; i < count; i++)
        {
            s += Word();
            if (i > 0 && multiline == true && Within(5) == true)
            {
                s += Environment.NewLine;
            }
            else if (i + 1 != count)
            {
                s += " ";
            }
        }

        return s;
    }
}

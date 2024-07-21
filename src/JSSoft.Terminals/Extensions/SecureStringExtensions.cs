// <copyright file="SecureStringExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Security;

namespace JSSoft.Terminals.Extensions;

internal static class SecureStringExtensions
{
    public static void InsertAt(this SecureString secureString, int index, string text)
    {
        for (var i = 0; i < text.Length; i++)
        {
            secureString.InsertAt(i + index, text[i]);
        }
    }
}

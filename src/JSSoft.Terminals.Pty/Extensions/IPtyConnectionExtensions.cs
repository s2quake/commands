// <copyright file="IPtyConnectionExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Text;

namespace JSSoft.Terminals.Pty.Extensions;

public static class IPtyConnectionExtensions
{
    public static void Write(this IPtyConnection @this, string text)
    {
        var buffer = Encoding.UTF8.GetBytes(text);
        @this.Write(buffer, buffer.Length);
    }
}

// <copyright file="PtyProvider.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Diagnostics;

namespace JSSoft.Terminals.Pty.Unix;

internal abstract class PtyProvider : IPtyProvider
{
    public abstract IPtyConnection StartTerminal(PtyOptions options, TraceSource trace);
}

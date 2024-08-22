// <copyright file="IsExternalInit.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NET5_0_OR_GREATER
#pragma warning disable
using System.ComponentModel;

namespace System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit { }

#endif // !NET5_0_OR_GREATER

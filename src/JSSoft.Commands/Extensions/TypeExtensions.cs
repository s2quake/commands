// <copyright file="TypeExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands.Extensions;

internal static class TypeExtensions
{
    public static bool IsStaticClass(this Type @this)
    {
        return @this.GetConstructor(Type.EmptyTypes) is null
            && @this.IsAbstract == true
            && @this.IsSealed == true;
    }
}

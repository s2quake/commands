// <copyright file="MemberInfoExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;

namespace JSSoft.Commands.Extensions;

public static class MemberInfoExtensions
{
    public static Type GetDeclaringType(this MemberInfo @this)
    {
        if (@this.DeclaringType is { } declaringType)
        {
            return declaringType;
        }

        var message = $"""
            '{nameof(MemberInfo.DeclaringType)}' of '{@this}' cannot be null.
            """;
        throw new ArgumentException(message, nameof(@this));
    }
}

// <copyright file="CommandValueValidator.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NETSTANDARD
using System.ComponentModel.DataAnnotations;
#endif

namespace JSSoft.Commands;

internal sealed class CommandValueValidator : ICommandValueValidator
{
    public void Validate(CommandMemberInfo memberInfo, object instance, object? value)
    {
#if !NETSTANDARD
        if (memberInfo.GetAttributes<ValidationAttribute>(true) is { } attributes)
        {
            var context = new ValidationContext(instance) { MemberName = memberInfo.Name };
#if NET6_0 || NET7_0
            Validator.ValidateValue(value!, context, attributes);
#else
            Validator.ValidateValue(value, context, attributes);
#endif
        }
#endif
    }
}

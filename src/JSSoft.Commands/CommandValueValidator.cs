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
    public void Validate(ParseDescriptor parseDescriptor, object instance, object? value)
    {
#if !NETSTANDARD
        var memberInfo = parseDescriptor.MemberDescriptor.MemberInfo;
        if (memberInfo.GetAttributes<ValidationAttribute>(true) is { } attributes)
        {
            var items = new Dictionary<object, object?>()
            {
                { typeof(ParseDescriptor), parseDescriptor },
            };
            var context = new ValidationContext(instance, items) { MemberName = memberInfo.Name };
#if NET6_0 || NET7_0
            Validator.ValidateValue(value!, context, attributes);
#else
            Validator.ValidateValue(value, context, attributes);
#endif
        }
#endif
    }
}

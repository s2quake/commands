// <copyright file="ICommandValueValidator.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public interface ICommandValueValidator
{
    void Validate(ParseDescriptor parseDescriptor, object instance, object? value);
}

// <copyright file="CommandPropertyArrayAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandPropertyArrayAttribute : CommandPropertyBaseAttribute
{
    public override CommandType CommandType => CommandType.Variables;

    [Obsolete("In the Variables property, DefaultValue is not used.")]
    public new object DefaultValue => base.DefaultValue;
}

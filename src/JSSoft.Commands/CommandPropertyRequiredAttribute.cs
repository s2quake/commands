// <copyright file="CommandPropertyRequiredAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public sealed class CommandPropertyRequiredAttribute : CommandPropertyBaseAttribute
{
    public CommandPropertyRequiredAttribute()
    {
    }

    public CommandPropertyRequiredAttribute(string name)
        : base(name)
    {
    }

    public override CommandType CommandType => CommandType.Required;

    [Obsolete("In the Required property, InitValue is not used.")]
    public new object InitValue => base.InitValue;
}

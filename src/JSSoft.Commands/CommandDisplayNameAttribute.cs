// <copyright file="CommandDisplayNameAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.All)]
public class CommandDisplayNameAttribute(string displayName) : DisplayNameAttribute(displayName)
{
}

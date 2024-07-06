// <copyright file="ICustomCommandDescriptor.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Commands;

public interface ICustomCommandDescriptor
{
    CommandMemberDescriptorCollection GetMembers();

    object GetMemberOwner(CommandMemberDescriptor memberDescriptor);
}

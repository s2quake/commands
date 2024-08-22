// <copyright file="RequiredMemberAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NET7_0_OR_GREATER
#pragma warning disable
#pragma warning disable MEN002 // Line is too long
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field
                | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class RequiredMemberAttribute : Attribute { }


#pragma warning restore MEN002 // Line is too long
#endif // !NET7_0_OR_GREATER

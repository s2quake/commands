// <copyright file="SetsRequiredMembersAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NET7_0_OR_GREATER
#pragma warning disable
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
internal sealed class SetsRequiredMembersAttribute : Attribute { }

#endif // !NET7_0_OR_GREATER

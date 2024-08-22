// <copyright file="CompilerFeatureRequiredAttribute.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

#if !NET7_0_OR_GREATER
#pragma warning disable
// https://stackoverflow.com/questions/74447497/is-it-possible-to-use-the-c11-required-modifier-with-net-framework-4-8-and

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
internal sealed class CompilerFeatureRequiredAttribute : Attribute
{
    public CompilerFeatureRequiredAttribute(string featureName)
    {
        FeatureName = featureName;
    }

    public string FeatureName { get; }
    public bool IsOptional { get; init; }

    public const string RefStructs = nameof(RefStructs);
    public const string RequiredMembers = nameof(RequiredMembers);
}

#endif // !NET7_0_OR_GREATER

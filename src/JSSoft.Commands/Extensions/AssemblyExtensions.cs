// <copyright file="AssemblyExtensions.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands.Extensions;

internal static class AssemblyExtensions
{
    public static string GetAssemblyVersion(this Assembly @this)
        => GetValue<AssemblyInformationalVersionAttribute>(
            assembly: @this,
            getter: assembly => assembly.InformationalVersion);

    public static string GetAssemblyFileVersion(this Assembly @this)
        => GetValue<AssemblyFileVersionAttribute>(@this, assembly => assembly.Version);

    public static string GetAssemblyCopyright(this Assembly @this)
        => GetValue<AssemblyCopyrightAttribute>(@this, assembly => assembly.Copyright);

    public static string GetAssemblyProduct(this Assembly @this)
        => GetValue<AssemblyProductAttribute>(@this, assembly => assembly.Product);

    public static string GetAssemblyCompany(this Assembly @this)
        => GetValue<AssemblyCompanyAttribute>(@this, assembly => assembly.Company);

    public static string GetAssemblyName(this Assembly @this)
    {
        if (@this.GetName() is { } assemblyName && assemblyName.Name is { } name)
        {
            return name;
        }

        return string.Empty;
    }

    public static string GetAssemblyLocation(this Assembly @this)
    {
#pragma warning disable IL3000
        if (@this.Location != string.Empty)
        {
            return @this.Location;
        }
#pragma warning restore IL3000
        return Path.Combine(AppContext.BaseDirectory, @this.GetName().Name!);
    }
}

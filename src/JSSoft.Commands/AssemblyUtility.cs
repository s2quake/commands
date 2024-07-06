// <copyright file="AssemblyUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JSSoft.Commands;

static class AssemblyUtility
{
    public static string GetAssemblyVersion(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>() is { } assemblyInformationalVersionAttribute)
        {
            return assemblyInformationalVersionAttribute.InformationalVersion;
        }
        return string.Empty;
    }

    public static string GetAssemblyFileVersion(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<AssemblyFileVersionAttribute>() is { } assemblyFileVersionAttribute)
        {
            return assemblyFileVersionAttribute.Version;
        }
        return string.Empty;
    }

    public static string GetAssemblyCopyright(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<AssemblyCopyrightAttribute>() is { } assemblyCopyrightAttribute)
        {
            return assemblyCopyrightAttribute.Copyright;
        }
        return string.Empty;
    }

    public static string GetAssemblyProduct(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<AssemblyProductAttribute>() is { } assemblyProductAttribute)
        {
            return assemblyProductAttribute.Product;
        }
        return string.Empty;
    }

    public static string GetAssemblyCompany(Assembly assembly)
    {
        if (assembly.GetCustomAttribute<AssemblyCompanyAttribute>() is { } assemblyCompanyAttribute)
        {
            return assemblyCompanyAttribute.Company;
        }
        return string.Empty;
    }

    public static string GetAssemblyName(Assembly assembly)
    {
        if (assembly.GetName() is { } assemblyName && assemblyName.Name != null)
        {
            return assemblyName.Name;
        }
        return string.Empty;
    }

    public static string GetAssemblyLocation(Assembly assembly)
    {
#pragma warning disable IL3000
        if (assembly.Location != string.Empty)
            return assembly.Location;
#pragma warning restore IL3000
        return Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name!);
    }

    public static IEnumerable<KeyValuePair<string, string?>> GetMetadatas(Assembly assembly)
    {
        var attributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
        foreach (var item in attributes)
        {
            yield return new KeyValuePair<string, string?>(item.Key, item.Value);
        }
    }

    // https://learn.microsoft.com/en-us/dotnet/standard/assembly/identify
    public static bool CheckAssembly(string path)
    {
        try
        {
            AssemblyName.GetAssemblyName(path);
            return true;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
        catch (BadImageFormatException)
        {
            return false;
        }
        catch (FileLoadException)
        {
            return false;
        }
    }
}

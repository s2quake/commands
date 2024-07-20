// <copyright file="AssemblyUtility.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;
using static JSSoft.Commands.AttributeUtility;

namespace JSSoft.Commands;

internal static class AssemblyUtility
{
    public static string GetAssemblyVersion(Assembly assembly)
        => GetValue<AssemblyInformationalVersionAttribute>(
            assembly: assembly,
            getter: assembly => assembly.InformationalVersion);

    public static string GetAssemblyFileVersion(Assembly assembly)
        => GetValue<AssemblyFileVersionAttribute>(assembly, assembly => assembly.Version);

    public static string GetAssemblyCopyright(Assembly assembly)
        => GetValue<AssemblyCopyrightAttribute>(assembly, assembly => assembly.Copyright);

    public static string GetAssemblyProduct(Assembly assembly)
        => GetValue<AssemblyProductAttribute>(assembly, assembly => assembly.Product);

    public static string GetAssemblyCompany(Assembly assembly)
        => GetValue<AssemblyCompanyAttribute>(assembly, assembly => assembly.Company);

    public static string GetAssemblyName(Assembly assembly)
    {
        if (assembly.GetName() is { } assemblyName && assemblyName.Name is { } name)
        {
            return name;
        }

        return string.Empty;
    }

    public static string GetAssemblyLocation(Assembly assembly)
    {
#pragma warning disable IL3000
        if (assembly.Location != string.Empty)
        {
            return assembly.Location;
        }
#pragma warning restore IL3000
        return Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name!);
    }

    public static IEnumerable<KeyValuePair<string, string?>> GetMetadatas(Assembly assembly)
    {
        var attributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();
        foreach (var attribute in attributes)
        {
            yield return new KeyValuePair<string, string?>(attribute.Key, attribute.Value);
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

// Released under the MIT License.
// 
// Copyright (c) 2024 Jeesu Choi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

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

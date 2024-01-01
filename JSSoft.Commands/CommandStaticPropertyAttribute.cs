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

using System.Diagnostics;

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CommandStaticPropertyAttribute : Attribute
{
    public CommandStaticPropertyAttribute(string staticTypeName, params string[] propertyNames)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotStaticClass(staticTypeName);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        StaticTypeName = staticTypeName;
        StaticType = Type.GetType(staticTypeName)!;
        PropertyNames = propertyNames;
    }

    public CommandStaticPropertyAttribute(Type staticType, params string[] propertyNames)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotStaticClass(staticType);
        }
        catch (Exception e)
        {
            Trace.TraceWarning(e.Message);
        }

        StaticType = staticType;
        StaticTypeName = staticType.AssemblyQualifiedName!;
        PropertyNames = propertyNames;
    }

    public string StaticTypeName { get; } = string.Empty;

    public string[] PropertyNames { get; } = [];

    public Type? StaticType { get; }

    internal Type GetStaticType(Type requestType)
    {
        try
        {
            TypeUtility.ThrowIfTypeIsNotStaticClass(StaticTypeName);
        }
        catch (Exception e)
        {
            throw new CommandDefinitionException(e.Message, requestType, innerException: e);
        }
        return StaticType!;
    }
}

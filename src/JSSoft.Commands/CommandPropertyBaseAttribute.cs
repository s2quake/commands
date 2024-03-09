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

namespace JSSoft.Commands;

[AttributeUsage(AttributeTargets.Property)]
public abstract class CommandPropertyBaseAttribute : Attribute
{
    internal CommandPropertyBaseAttribute()
    {
        AllowName = true;
    }

    internal CommandPropertyBaseAttribute(string name)
    {
        ThrowUtility.ThrowIfInvalidName(name);

        Name = name;
    }

    internal CommandPropertyBaseAttribute(string name, char shortName)
    {
        ThrowUtility.ThrowIfInvalidName(name);
        ThrowUtility.ThrowIfInvalidShortName(shortName);

        Name = name;
        ShortName = shortName;
    }

    internal CommandPropertyBaseAttribute(char shortName)
        : this(shortName, useName: false)
    {
    }

    internal CommandPropertyBaseAttribute(char shortName, bool useName)
    {
        ThrowUtility.ThrowIfInvalidShortName(shortName);

        ShortName = shortName;
        AllowName = useName;
    }

    public string Name { get; } = string.Empty;

    public char ShortName { get; }

    public bool AllowName { get; }

    public object DefaultValue { get; set; } = DBNull.Value;

    public object InitValue { get; set; } = DBNull.Value;

    public abstract CommandType CommandType { get; }

    internal bool IsRequired => CommandType == CommandType.Required || CommandType == CommandType.ExplicitRequired;

    internal bool IsExplicit => CommandType == CommandType.General || CommandType == CommandType.ExplicitRequired || CommandType == CommandType.Switch;

    internal bool IsSwitch => CommandType == CommandType.Switch;

    internal bool IsVariables => CommandType == CommandType.Variables;

    internal string GetName(string defaultName)
    {
        if (Name != string.Empty)
            return Name;
        return AllowName == true ? CommandUtility.ToSpinalCase(defaultName) : string.Empty;
    }
}

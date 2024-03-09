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

public sealed class CommandLineException : SystemException
{
    public CommandLineException(string message)
        : base(message)
    {
    }

    public CommandLineException(string message, CommandMemberDescriptor memberDescriptor)
        : base(message)
    {
        Descriptor = memberDescriptor;
    }

    public CommandLineException(CommandLineError error, string message, CommandMemberDescriptor memberDescriptor)
        : base(message)
    {
        Error = error;
        Descriptor = memberDescriptor;
    }

    public CommandLineException(CommandLineError error, string message, CommandMemberDescriptor memberDescriptor, Exception? innerException)
        : base(message, innerException)
    {
        Error = error;
        Descriptor = memberDescriptor;
    }

    public CommandLineException(string message, CommandMemberDescriptor memberDescriptor, Exception innerException)
        : base(message, innerException)
    {
        Descriptor = memberDescriptor;
    }

    public CommandLineException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public CommandMemberDescriptor? Descriptor { get; }

    public CommandLineError Error { get; }

    public static void ThrowIfValueMissing(CommandMemberDescriptor memberDescriptor)
    {
        var error = CommandLineError.ValueMissing;
        var message = $"Option '{memberDescriptor.DisplayName}' value is missing.";
        throw new CommandLineException(error, message, memberDescriptor);
    }

    public static void ThrowIfInvalidValue(CommandMemberDescriptor memberDescriptor, string value)
    {
        var error = CommandLineError.InvalidValue;
        var message = $"Value '{value}' cannot be used for option '{memberDescriptor.DisplayName}'.";
        throw new CommandLineException(error, message, memberDescriptor);
    }
}

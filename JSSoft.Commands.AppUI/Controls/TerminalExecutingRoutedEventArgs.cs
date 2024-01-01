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
using Avalonia.Interactivity;
using JSSoft.Terminals;

namespace JSSoft.Commands.AppUI.Controls;

public sealed class TerminalExecutingRoutedEventArgs : RoutedEventArgs
{
    private readonly TerminalExecutingEventArgs _e;

    internal TerminalExecutingRoutedEventArgs(TerminalExecutingEventArgs e, RoutedEvent? routedEvent)
        : base(routedEvent)
    {
        _e = e;
    }

    public string Command => _e.Command;

    public bool IsHandled => _e.IsHandled;

    public Guid GetToken() => _e.GetToken();

    public void Success(Guid token) => _e.Success(token);

    public void Fail(Guid token, Exception exception) => _e.Fail(token, exception);
}

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

namespace JSSoft.Terminals;

public sealed class TerminalExecutingEventArgs(string command, Action<Exception?> action) : EventArgs
{
    private readonly Action<Exception?> _action = action;

    public string Command { get; } = command;

    public bool IsHandled { get; private set; }

    public Guid GetToken()
    {
        if (Token != Guid.Empty)
            throw new InvalidOperationException("Token has already been created.");
        Token = Guid.NewGuid();
        return Token;
    }

    public void Success(Guid token)
    {
        if (token == Guid.Empty || token != Token)
            throw new InvalidOperationException("Invalid token.");
        if (IsHandled == true)
            throw new InvalidOperationException("command expired.");
        IsHandled = true;
        _action(null);
    }

    public void Fail(Guid token, Exception exception)
    {
        if (token == Guid.Empty || token != Token)
            throw new InvalidOperationException("Invalid token.");
        if (IsHandled == true)
            throw new InvalidOperationException("command expired.");
        if (exception == null)
            throw new ArgumentNullException(nameof(exception));
        IsHandled = true;
        _action(exception);
    }

    internal Guid Token { get; private set; }
}

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

using System.ComponentModel;
using System.IO;
using JSSoft.Terminals.Input;
using JSSoft.Terminals.Serializations;

namespace JSSoft.Terminals;

public interface ITerminal : INotifyPropertyChanged
{
    string Command { get; set; }

    string Prompt { get; set; }

    int CursorPosition { get; set; }

    bool IsReadOnly { get; set; }

    bool IsExecuting { get; }

    TerminalCompletor Completor { get; set; }

    TerminalColorType? ForegroundColor { get; set; }

    TerminalColorType? BackgroundColor { get; set; }

    ITerminalStyle ActualStyle { get; }

    ITerminalStyle? Style { get; set; }

    ITerminalScroll Scroll { get; }

    IInputHandler? InputHandler { get; set; }

    bool IsFocused { get; set; }

    string CompositionString { get; set; }

    int MaximumBufferHeight { get; set; }

    TerminalSize BufferSize { get; }

    TerminalSize Size { get; }

    IReadOnlyList<ITerminalRow> View { get; }

    TerminalCoord CursorCoordinate { get; }

    TerminalCoord OriginCoordinate { get; set; }

    TerminalCoord ViewCoordinate { get; set; }

    ITerminalSelection Selections { get; }

    TerminalSelection Selecting { get; set; }

    TextWriter Out { get; set; }

    TextWriter Error { get; set; }

    TextReader In { get; set; }

    TerminalPoint ViewToWorld(TerminalPoint position);

    TerminalCoord ViewToWorld(TerminalCoord viewCoord);

    TerminalCoord PositionToCoordinate(TerminalPoint position);

    TerminalCharacterInfo? GetInfo(TerminalCoord coord);

    bool BringIntoView(int y);

    string Copy();

    void Paste(string text);

    void Update(params ITerminalRow[] rows);

    void Append(string value);

    void Reset(TerminalCoord coord);

    void ResetColor();

    void Delete();

    void Backspace();

    void NextCompletion();

    void PrevCompletion();

    void NextHistory();

    void PrevHistory();

    void Execute();

    void Cancel();

    TerminalDataInfo Save();

    void Load(TerminalDataInfo data);

    event EventHandler? CancellationRequested;

    event EventHandler<TerminalExecutingEventArgs>? Executing;

    event EventHandler<TerminalExecutedEventArgs>? Executed;

    event EventHandler<TerminalUpdateEventArgs>? Updated;
}

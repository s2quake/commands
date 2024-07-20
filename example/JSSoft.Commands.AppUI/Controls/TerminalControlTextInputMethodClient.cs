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
using Avalonia;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace JSSoft.Commands.AppUI.Controls;

sealed class TerminalControlTextInputMethodClient : TextInputMethodClient
{
    private TerminalControl? _parent;
    private TerminalPresenter? _presenter;

    public override Visual TextViewVisual => _presenter!;

    public override string SurroundingText
    {
        get
        {
            if (_presenter is null || _parent is null)
            {
                return "";
            }

            // if (_parent.CaretIndex != _presenter.CaretIndex)
            // {
            //     _presenter.SetCurrentValue(TerminalPresenter.CaretIndexProperty, _parent.CaretIndex);
            // }

            // if (_parent.Text != _presenter.Text)
            // {
            //     _presenter.SetCurrentValue(TerminalPresenter.TextProperty, _parent.Text);
            // }

            // var lineIndex = _presenter.TextLayout.GetLineIndexFromCharacterIndex(_presenter.CaretIndex, false);

            // var textLine = _presenter.TextLayout.TextLines[lineIndex];

            // var lineText = GetTextLineText(textLine);

            // return lineText;
            return string.Empty;
        }
    }

    public override Rect CursorRectangle
    {
        get
        {
            if (_parent is null || _presenter is null)
            {
                return default;
            }

            var transform = _presenter.TransformToVisual(_parent);

            if (transform is null)
            {
                return default;
            }

            // return _presenter.GetCursorRectangle().TransformToAABB(transform.Value);
            return default;
        }
    }

    public override TextSelection Selection
    {
        get
        {
            if (_presenter is null || _parent is null)
            {
                return default;
            }

            // var lineIndex = _presenter.TextLayout.GetLineIndexFromCharacterIndex(_parent.CaretIndex, false);

            // var textLine = _presenter.TextLayout.TextLines[lineIndex];

            // var lineStart = textLine.FirstTextSourceIndex;

            // var selectionStart = Math.Max(0, _parent.SelectionStart - lineStart);

            // var selectionEnd = Math.Max(0, _parent.SelectionEnd - lineStart);

            // return new TextSelection(selectionStart, selectionEnd);
            return default;
        }
        set
        {
            if (_parent is null || _presenter is null)
            {
                return;
            }

            // var lineIndex = _presenter.TextLayout.GetLineIndexFromCharacterIndex(_parent.CaretIndex, false);

            // var textLine = _presenter.TextLayout.TextLines[lineIndex];

            // var lineStart = textLine.FirstTextSourceIndex;

            // var selectionStart = lineStart + value.Start;
            // var selectionEnd = lineStart + value.End;

            // _parent.SelectionStart = selectionStart;
            // _parent.SelectionEnd = selectionEnd;

            // RaiseSelectionChanged();
        }
    }

    public override bool SupportsPreedit => true;

    public override bool SupportsSurroundingText => true;

    public void SetPresenter(TerminalPresenter? presenter, TerminalControl? parent)
    {
        if (_parent is not null)
        {
            _parent.PropertyChanged -= OnParentPropertyChanged;
        }

        _parent = parent;

        if (_parent is not null)
        {
            _parent.PropertyChanged += OnParentPropertyChanged;
        }

        var oldPresenter = _presenter;

        // if (oldPresenter is not null)
        // {
        //     oldPresenter.ClearValue(TerminalPresenter.PreeditTextProperty);

        //     oldPresenter.CaretBoundsChanged -= (s, e) => RaiseCursorRectangleChanged();
        // }

        _presenter = presenter;

        // if (_presenter is not null)
        // {
        //     _presenter.CaretBoundsChanged += (s, e) => RaiseCursorRectangleChanged();
        // }

        RaiseTextViewVisualChanged();

        RaiseCursorRectangleChanged();
    }

    public override void SetPreeditText(string? preeditText) => SetPreeditText(preeditText, null);

    public override void SetPreeditText(string? preeditText, int? cursorPos)
    {
        if (_presenter is null || _parent is null)
        {
            return;
        }
        // Console.WriteLine($"preeditText: {preeditText}");
        // _presenter.SetCurrentValue(TerminalPresenter.PreeditTextProperty, preeditText);
        // _presenter.SetCurrentValue(TerminalPresenter.PreeditTextCursorPositionProperty, cursorPos);
    }

    //     private static string GetTextLineText(TextLine textLine)
    //     {
    //         var builder = StringBuilderCache.Acquire(textLine.Length);

    //         foreach (var run in textLine.TextRuns)
    //         {
    //             if (run.Length > 0)
    //             {
    // #if NET6_0_OR_GREATER
    //                 builder.Append(run.Text.Span);
    // #else
    //                     builder.Append(run.Text.Span.ToArray());
    // #endif
    //             }
    //         }

    //         var lineText = builder.ToString();

    //         StringBuilderCache.Release(builder);

    //         return lineText;
    //     }

    private void OnParentPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // if (e.Property == TextBox.TextProperty)
        // {
        //     RaiseSurroundingTextChanged();
        // }

        // if (e.Property == TextBox.SelectionStartProperty || e.Property == TextBox.SelectionEndProperty)
        // {
        //     RaiseSelectionChanged();
        // }
    }
}

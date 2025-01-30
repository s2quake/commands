// <copyright file="TerminalControlTextInputMethodClient.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using Avalonia;
using Avalonia.Input.TextInput;

namespace JSSoft.Commands.AppUI.Controls;

internal sealed class TerminalControlTextInputMethodClient : TextInputMethodClient
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
                return string.Empty;
            }

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

            return default;
        }
    }

    public override bool SupportsPreedit => true;

    public override bool SupportsSurroundingText => true;

    public override TextSelection Selection { get; set; }

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

        _presenter = presenter;
        RaiseTextViewVisualChanged();
        RaiseCursorRectangleChanged();
    }

    public override void SetPreeditText(string? preeditText) => SetPreeditText(preeditText, null);

    public override void SetPreeditText(string? preeditText, int? cursorPos)
    {
        // do nothing
    }

    private void OnParentPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        // do nothing
    }
}

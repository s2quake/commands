﻿// <copyright file="TerminalInputHandlerContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Threading;
using System.Threading.Tasks;
using JSSoft.Terminals.Extensions;

namespace JSSoft.Terminals.Input;

internal sealed class TerminalInputHandlerContext(ITerminal terminal) : InputHandlerContext(terminal)
{
    private const double ClickThreshold = 0.5;
    private TerminalPoint _downPosition;
    private TerminalCoord _downCoord;
    private int _downCount;
    private double _downTime;
    private TerminalPoint _dragPosition;
    private DateTime _dragTime;
    private double _scrollValue;
    private bool _isDragging;
    private CancellationTokenSource? _cancellationTokenSource;

    protected override void OnBeginDrag(IPointerEventData eventData)
    {
        if (eventData.IsMouseLeftButton is true && _downCoord != TerminalCoord.Invalid)
        {
            _cancellationTokenSource = _cancellationTokenSource ??= BeginScrollObservation(ObserveScroll);
            _dragTime = DateTime.Now;
            _scrollValue = Terminal.Scroll.Value;
            _isDragging = true;
        }

        static CancellationTokenSource BeginScrollObservation(Action action)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(action, cancellationTokenSource.Token, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            return cancellationTokenSource;
        }
    }

    protected override void OnDrag(IPointerEventData eventData)
    {
        if (eventData.IsMouseLeftButton is true && _downCoord != TerminalCoord.Invalid)
        {
            var terminal = Terminal;
            var downCoord = _downCoord;
            var position1 = terminal.ViewToWorld(eventData.Position);
            var position2 = position1.Y < 0 ? TerminalPoint.Empty : position1;
            var coord = terminal.PositionToCoordinate(position2);
            terminal.Selections.TryClear();
            terminal.TrySelecting(downCoord, coord);
            _dragPosition = eventData.Position;
        }
    }

    protected override void OnEndDrag(IPointerEventData eventData)
    {
        if (eventData.IsMouseLeftButton is true && _downCoord != TerminalCoord.Invalid)
        {
            var terminal = Terminal;
            terminal.Selections.TrySelect(terminal.Selecting);
            terminal.Selecting = TerminalSelection.Empty;
            _downCoord = TerminalCoord.Invalid;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }
    }

    protected override void OnPointerDown(IPointerEventData eventData)
    {
        if (eventData.IsMouseLeftButton is true)
        {
            var terminal = Terminal;
            var newPosition = terminal.ViewToWorld(eventData.Position);
            var newCoord = terminal.PositionToCoordinate(newPosition);
            var newTime = eventData.Timestamp;
            var downCount = GetDownCount(_downCount, _downTime, newTime, _downPosition, newPosition);
            _downPosition = newPosition;
            _downCoord = newCoord;
            _downCount = downCount;
            _downTime = newTime;

            if (newCoord != TerminalCoord.Invalid)
            {
                if (downCount == 1)
                {
                    terminal.Selecting = TerminalSelection.Empty;
                }
                else if (downCount == 2)
                {
                    terminal.Selections.Clear();
                    terminal.Selecting = TerminalSelectionUtility.SelectWord(terminal, newCoord);
                }
                else if (downCount == 3)
                {
                    terminal.Selections.Clear();
                    terminal.Selecting = TerminalSelectionUtility.SelectLine(terminal, newCoord);
                }
            }
        }
    }

    protected override void OnPointerUp(IPointerEventData eventData)
    {
        if (eventData.IsMouseLeftButton is true)
        {
            var terminal = Terminal;
            var downCount = _downCount;
            var position = terminal.ViewToWorld(eventData.Position);
            var newCoord = terminal.PositionToCoordinate(position);
            var oldCoord = _downCoord;
            if (oldCoord == newCoord && _isDragging is false)
            {
                if (downCount == 1)
                {
                    terminal.Selections.Clear();
                    terminal.Selecting = TerminalSelection.Empty;
                }
                else if (downCount == 2 || downCount == 3)
                {
                    terminal.Selections.Select(terminal.Selecting);
                    terminal.Selecting = TerminalSelection.Empty;
                }
            }

            _isDragging = false;
        }
    }

    protected override void OnDispose()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
        base.OnDispose();
    }

    private async void ObserveScroll()
    {
        while (_cancellationTokenSource?.IsCancellationRequested is false)
        {
            var terminal = Terminal;
            var dragTime = DateTime.Now;
            var timeSpan = dragTime - _dragTime;
            var oldScrollValue = terminal.Scroll.Value;
            if (_dragPosition.Y <= 0)
            {
                ScrollValue(terminal, -timeSpan.Milliseconds / 100.0);
            }
            else if (_dragPosition.Y >= terminal.Size.Height - 1)
            {
                ScrollValue(terminal, timeSpan.Milliseconds / 100.0);
            }

            _dragTime = dragTime;
            await Task.Delay(100);
        }

        void ScrollValue(ITerminal terminal, double offset)
        {
            var scroll = terminal.Scroll;
            var oldScrollValue = scroll.Value;
            _scrollValue += offset;
            _scrollValue = TerminalMathUtility.Clamp(_scrollValue, scroll.Minimum, scroll.Maximum);
            scroll.Value = (int)_scrollValue;
            if (oldScrollValue != scroll.Value)
            {
                var downCoord = _downCoord;
                var position1 = terminal.ViewToWorld(_dragPosition);
                var position2 = position1.Y < 0 ? TerminalPoint.Empty : position1;
                var coord = terminal.PositionToCoordinate(position2);
                terminal.Selections.TryClear();
                terminal.TrySelecting(downCoord, coord);
            }
        }
    }

    private static int GetDownCount(int count, double oldTime, double newTime, TerminalPoint oldPosition, TerminalPoint newPosition)
    {
        var diffTime = newTime - oldTime;
        if (diffTime > ClickThreshold || oldPosition != newPosition)
        {
            return 1;
        }

        return count % 3 + 1;
    }
}

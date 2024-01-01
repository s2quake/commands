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

// using System.ComponentModel;
// using System.Text.RegularExpressions;

// namespace JSSoft.Terminals.Input;

// sealed class PowershellInputHandlerContext : InputHandlerContext
// {
//     private const double ClickThreshold = 0.5;
//     private TerminalPoint _downPosition;
//     private TerminalCoord _downPoint;
//     private TerminalSelection _dragRange;
//     private TerminalSelection _downRange;
//     private double _time;
//     private int _downCount;

//     public PowershellInputHandlerContext(ITerminal terminal)
//         : base(terminal)
//     {
//         Terminal.PropertyChanged += Terminal_PropertyChanged;
//     }

//     protected override void OnBeginDrag(IPointerEventData eventData)
//     {
//         var downPoint = _downPoint;
//         if (eventData.IsMouseLeftButton == true && downPoint != TerminalCoord.Invalid)
//         {
//             var position = Terminal.ViewToWorld(eventData.Position);
//             var point = Terminal.PositionToCoordinate(position);
//             if (point != TerminalCoord.Invalid)
//             {
//                 Terminal.Selecting = new TerminalSelection(downPoint, point);
//             }
//         }
//     }

//     protected override void OnDrag(IPointerEventData eventData)
//     {
//         var downPoint = _downPoint;
//         if (eventData.IsMouseLeftButton == true && downPoint != TerminalCoord.Invalid)
//         {
//             var position = Terminal.ViewToWorld(eventData.Position);
//             var point = Terminal.PositionToCoordinate(position);
//             if (point != TerminalCoord.Invalid)
//             {
//                 _dragRange = new TerminalSelection(downPoint, point);
//                 UpdateSelecting();
//             }
//         }
//     }

//     protected override void OnEndDrag(IPointerEventData eventData)
//     {
//         var downPoint = _downPoint;
//         if (eventData.IsMouseLeftButton == true && downPoint != TerminalCoord.Invalid)
//         {
//             Terminal.Selections.Clear();
//             if (Terminal.Selecting != TerminalSelection.Empty)
//                 Terminal.Selections.Add(Terminal.Selecting);
//             Terminal.Selecting = TerminalSelection.Empty;
//             _downPoint = TerminalCoord.Invalid;
//         }
//     }

//     protected override void OnPointerDown(IPointerEventData eventData)
//     {
//         if (eventData.IsMouseLeftButton == true)
//         {
//             OnLeftPointerDown(eventData);
//         }
//     }

//     protected override void OnPointerUp(IPointerEventData eventData)
//     {
//         if (eventData.IsMouseLeftButton == true)
//         {
//             OnLeftPointerUp(eventData);
//         }
//     }

//     protected override void OnDispose()
//     {
//         Terminal.PropertyChanged -= Terminal_PropertyChanged;
//         base.OnDispose();
//     }

//     private void SelectWord(TerminalCoord point)
//     {
//         var terminal = Terminal;
//         var row = terminal.Rows[point.Y];
//         var cell = row.Cells[point.X];
//         if (row.IsEmpty == true)
//             SelectWordOfEmptyRow(row);
//         else if (cell.Character == char.MinValue)
//             SelectWordOfEmptyCell(row.Index);
//         else
//             SelectWordOfCell(cell);
//     }

//     private void SelectLine(TerminalCoord point)
//     {
//         // var terminal = Terminal;
//         // var row = terminal.Rows[point.Y];
//         // if (row.IsEmpty == false)
//         // {
//         //     var cell = row.Cells[0];
//         //     var index = cell.TextIndex;
//         //     var text = terminal.Text + char.MinValue;
//         //     var matches = Regex.Matches(terminal.Text, @"^|$", RegexOptions.Multiline).Cast<Match>();
//         //     var match1 = matches.Where(item => item.Index <= index).Last();
//         //     var match2 = matches.Where(item => item.Index > index).First();
//         //     var p1 = terminal.CharacterInfos[match1.Index].Coordinate;
//         //     var p2 = terminal.CharacterInfos[match2.Index].Coordinate;
//         //     var p3 = new TerminalCoord(0, p1.Y);
//         //     var p4 = new TerminalCoord(terminal.BufferSize.Width, p2.Y);
//         //     _downRange = new TerminalSelection(p3, p4);
//         //     UpdateSelecting();
//         // }
//         // else
//         // {
//         //     var p1 = new TerminalCoord(0, point.Y);
//         //     var p2 = new TerminalCoord(terminal.BufferSize.Width, point.Y);
//         //     _downRange = new TerminalSelection(p1, p2);
//         //     UpdateSelecting();
//         // }
//     }

//     private void SelectGroup(TerminalCoord point)
//     {
//         // var terminal = Grid;
//         // var row = terminal.Rows[point.Y];
//         // var cell = row.Cells[point.X];
//         // var index = cell.TextIndex;
//         // var character = cell.Character;
//         // var patterns = new string[] { @"\[[^\]]*\]", @"\{[^\}]*\}", @"\([^\)]*\)", @"\<[^\>]*\>" };
//         // var pattern = string.Join("|", patterns);
//         // var matches = Regex.Matches(terminal.Text, pattern).Cast<Match>();
//         // var match = matches.FirstOrDefault(item => item.Index == index);
//         // if (match != null)
//         // {
//         //     var p1 = terminal.CharacterInfos[match.Index].Coordinate;
//         //     var p2 = terminal.CharacterInfos[match.Index + match.Length].Coordinate;
//         //     var range = new TerminalSelection(p1, p2);
//         //     Grid.Selections.Clear();
//         //     Grid.Selections.Add(range);
//         // }
//     }

//     private void SelectWordOfEmptyRow(ITerminalRow row)
//     {
//         var terminal = Terminal;
//         var p1 = new TerminalCoord(0, row.Index);
//         var p2 = new TerminalCoord(terminal.BufferSize.Width, row.Index);
//         _downRange = new TerminalSelection(p1, p2);
//         UpdateSelecting();
//     }

//     private void SelectWordOfEmptyCell(int y)
//     {
//         // var terminal = Terminal;
//         // var row = terminal.Rows[y];
//         // var cells = row.Cells;
//         // var p1 = TerminalSelectionUtility.LastPoint(row, true);
//         // var p2 = new TerminalCoord(terminal.BufferSize.Width, row.Index);
//         // _downRange = new TerminalSelection(p1, p2);
//         UpdateSelecting();
//     }

//     private void SelectWordOfCell(TerminalCharacterInfo cell)
//     {
//         // var terminal = Grid;
//         // var text = terminal.Text;
//         // var index = cell.TextIndex;
//         // var character = cell.Character;
//         // var pattern = GetPattern();
//         // var matches = Regex.Matches(text, pattern).Cast<Match>();
//         // var match = matches.First(item => index >= item.Index && index < item.Index + item.Length);
//         // var i1 = match.Index;
//         // var i2 = i1 + match.Length;
//         // var c1 = terminal.CharacterInfos[i1];
//         // var c2 = terminal.CharacterInfos[i2];
//         // var p1 = c1.Coordinate;
//         // var p2 = c2.Coordinate;
//         // _downRange = new TerminalSelection(p1, p2);
//         // UpdateSelecting();

//         // string GetPattern()
//         // {
//         //     if (char.IsLetterOrDigit(character) == true)
//         //         return @"(?:\w+\.(?=\w))*\w+";
//         //     else if (char.IsWhiteSpace(character) == true)
//         //         return @"\s+";
//         //     else
//         //         return @"[^\w\s]";
//         // }
//     }

//     private bool OnLeftPointerDown(IPointerEventData eventData)
//     {
//         var terminal = Terminal;
//         var newPosition = Terminal.ViewToWorld(eventData.Position);
//         var newPoint1 = Terminal.PositionToCoordinate(newPosition);
//         var newPoint2 = new TerminalCoord(newPoint1.X + 1, newPoint1.Y);
//         var newTime = eventData.Timestamp;
//         var downCount = GetDownCount(_downCount, _time, newTime, _downPosition, newPosition);
//         if (Terminal.IsFocused == false)
//         {
//             Terminal.IsFocused = true;
//             return true;
//         }
//         _downPosition = newPosition;
//         _downPoint = newPoint1;
//         _downCount = downCount;
//         _dragRange = new TerminalSelection(newPoint1, newPoint1);
//         _time = newTime;

//         if (newPoint1 != TerminalCoord.Invalid)
//         {
//             var row = terminal.Rows[newPoint1.Y];
//             if (downCount == 1)
//             {
//                 Terminal.Selecting = TerminalSelection.Empty;
//                 Terminal.Selections.Clear();
//                 _downRange = new TerminalSelection(newPoint1, newPoint1);
//                 Terminal.Selecting = new TerminalSelection(newPoint1, newPoint2);
//             }
//             else if (downCount == 2)
//             {
//                 SelectWord(newPoint1);
//             }
//             else if (downCount == 3)
//             {
//                 SelectLine(newPoint1);
//             }
//         }
//         return true;
//     }

//     private bool OnLeftPointerUp(IPointerEventData eventData)
//     {
//         var position = Terminal.ViewToWorld(eventData.Position);
//         var newPoint = Terminal.PositionToCoordinate(position);
//         var oldPoint = _downPoint;
//         if (oldPoint == newPoint)
//         {
//             Terminal.Selections.Clear();
//             if (Terminal.Selecting != TerminalSelection.Empty)
//                 Terminal.Selections.Add(Terminal.Selecting);
//             Terminal.Selecting = TerminalSelection.Empty;
//             if (_downCount == 2)
//             {
//                 SelectGroup(newPoint);
//             }
//             return true;
//         }
//         return false;
//     }

//     private void UpdateSelecting()
//     {
//         var p1 = _downRange.BeginCoord < _dragRange.BeginCoord ? _downRange.BeginCoord : _dragRange.BeginCoord;
//         var p2 = _downRange.EndCoord > _dragRange.EndCoord ? _downRange.EndCoord : _dragRange.EndCoord;
//         Terminal.Selections.Clear();
//         Terminal.Selecting = new TerminalSelection(p1, p2);
//     }

//     private void Terminal_PropertyChanged(object? sender, PropertyChangedEventArgs e)
//     {
//         switch (e.PropertyName)
//         {
//             case nameof(ITerminal.BufferSize):
//             case nameof(ITerminal.MaximumBufferHeight):
//                 {
//                     Terminal.Selections.Clear();
//                 }
//                 break;
//         }
//     }

//     private static int GetDownCount(int count, double oldTime, double newTime, TerminalPoint oldPosition, TerminalPoint newPosition)
//     {
//         var diffTime = newTime - oldTime;
//         if (diffTime > ClickThreshold || oldPosition != newPosition)
//             return 1;
//         return ++count;
//     }
// }

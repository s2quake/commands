// // Released under the MIT License.
// // 
// // Copyright (c) 2024 Jeesu Choi
// // 
// // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// // documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// // rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// // persons to whom the Software is furnished to do so, subject to the following conditions:
// // 
// // The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// // Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// // WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// // COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// // OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// // 

// using System.Collections;

// namespace JSSoft.Terminals.Hosting;

// sealed class TerminalBlockCollection : IEnumerable<TerminalBlock>
// {
//     private readonly Queue<TerminalBlock> _poolQueue = [];
//     private readonly List<TerminalBlock> _itemList;
//     private readonly Terminal _terminal;
//     private readonly TerminalArray<TerminalBlock> _indexes = [];

//     public TerminalBlockCollection(Terminal terminal)
//     {
//         CurrentBlock = new TerminalBlock(terminal);
//         _itemList = [CurrentBlock];
//         _terminal = terminal;
//         CurrentBlock.Index = _itemList.IndexOf(CurrentBlock);
//         UpdateTransform(index: 0, y: 0);
//         CurrentBlock.TextChanged += Block_TextChanged;
//     }

//     public TerminalBlock CurrentBlock { get; private set; }

//     public int Count => _itemList.Count;

//     public int Height { get; private set; }

//     public TerminalBlock this[int index] => _itemList[index];

//     public void Clear()
//     {
//         foreach (var item in _itemList)
//         {
//             _poolQueue.Enqueue(item);
//         }
//         for (var i = _itemList.Count - 1; i >= 0; i--)
//         {
//             var item = _itemList[i];
//             item.TextChanged -= Block_TextChanged;
//             item.Index = -1;
//             item.Clear();
//         }
//         _itemList.Clear();
//         CurrentBlock = _poolQueue.TryDequeue(out var outputBlock) == true ? outputBlock : new TerminalBlock(_terminal);
//         _itemList.Add(CurrentBlock);
//         CurrentBlock.Index = _itemList.IndexOf(CurrentBlock);
//         UpdateTransform(index: 0, y: 0);
//         CurrentBlock.TextChanged += Block_TextChanged;
//         Updated?.Invoke(this, EventArgs.Empty);
//     }

//     public int IndexOf(TerminalBlock item) => _itemList.IndexOf(item);

//     public void Update()
//     {
//         var y = 0;
//         for (var i = 0; i < Count; i++)
//         {
//             var item = this[i];
//             item.Update();
//             item.TryTransform(y);
//             y += item.Height;
//             _indexes.Expand(y);
//             _indexes.SetRange(index: item.Top, length: item.Height, value: item);
//         }
//         Height = y;
//         _indexes.Take(Height);
//         Updated?.Invoke(this, EventArgs.Empty);
//     }

//     public TerminalCharacterInfo? GetInfo(int x, int y)
//     {
//         if (y < 0 || y >= _indexes.Count)
//             return null;
//         var block = _indexes[y];
//         var x1 = x;
//         var y1 = y - block.Top;
//         if (y1 < block.Lines.Count && block.Lines[y1] is {} line && line.Length > 0)
//             return line[x1];
//         return null;
//     }

//     public TerminalLine? GetLine(int y)
//     {
//         if (y < 0 || y >= _indexes.Count)
//             return null;
//         var block = _indexes[y];
//         var y1 = y - block.Top;
//         if (y1 < block.Lines.Count)
//             return block.Lines[y1];
//         return null;
//     }

//     public event EventHandler? Updated;

//     private void UpdateTransform(int index, int y)
//     {
//         for (var i = index; i < Count; i++)
//         {
//             var item = this[i];
//             item.TryTransform(y);
//             y += item.Height;
//             _indexes.Expand(y);
//             _indexes.SetRange(index: item.Top, length: item.Height, value: item);
//         }
//         Height = y;
//         _indexes.Take(Height);
//     }

//     private void Block_TextChanged(object? sender, EventArgs e)
//     {
//         if (sender is TerminalBlock block)
//         {
//             var index = block.Index;
//             var y = block.Top;
//             UpdateTransform(index, y);
//             Updated?.Invoke(this, EventArgs.Empty);
//         }
//     }

//     #region IEnumerable

//     IEnumerator<TerminalBlock> IEnumerable<TerminalBlock>.GetEnumerator()
//     {
//         foreach (var item in _itemList)
//         {
//             yield return item;
//         }
//     }

//     IEnumerator IEnumerable.GetEnumerator()
//     {
//         foreach (var item in _itemList)
//         {
//             yield return item;
//         }
//     }

//     #endregion
// }

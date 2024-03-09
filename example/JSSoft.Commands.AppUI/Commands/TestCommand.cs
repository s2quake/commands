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

using System.ComponentModel.Composition;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
[ResourceUsage]
sealed class TestCommand() : CommandBase
{
    protected override void OnExecute()
    {
        Out.WriteLine("0123456\r1");
        Out.WriteLine("0123456\v1");
        Out.WriteLine("0123456\t1");
        Out.WriteLine("012345\t1");
        Out.WriteLine("01234\t1");
        Out.WriteLine("0123\t1");
        Out.WriteLine("012\t1");
        Out.WriteLine("01\t1");
        Out.WriteLine("0\t1");
        Out.WriteLine("lsadkjflksajfl;kasjfl;kasjf;lasdkjfl;sdakjfl;sadkjfl;asdkjfl;adskjfl;asdkjflas;dkjflads;kfja;lsdkjf\r123");
        Out.WriteLine("lsadkjflksajfl;kasjfl;kasjf;lasdkjfl;sdakjfl;sadkjfl;asdkjfl;adskjfl;asdkjflas;dkjflads;\b1");
        Out.WriteLine("ab\bd");
        Out.WriteLine("\x1b[H123asdfasfsafasdf");
        Out.WriteLine("\x1b[#B123");
        Out.WriteLine("123\x1b[#C\x1b[#C4");
        Out.WriteLine("\x1b[1;2;3;4;5;6;7;B123");
        Out.WriteLine("\x1b[1B\x1b[10Casdf");
        Out.WriteLine("123\x1b[100Gabc");
        Out.WriteLine("\x1b[H");
    }
}

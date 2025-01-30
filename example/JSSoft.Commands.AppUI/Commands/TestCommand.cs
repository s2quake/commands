// <copyright file="TestCommand.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>
#pragma warning disable MEN002 // Line is too long

using System.ComponentModel.Composition;

namespace JSSoft.Commands.AppUI.Commands;

[Export(typeof(ICommand))]
internal sealed class TestCommand() : CommandBase
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

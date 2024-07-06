﻿// <copyright file="DCSSequenceCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

namespace JSSoft.Terminals.Hosting.Ansi;

sealed class DCSSequenceCollection : SequenceCollection
{
    public DCSSequenceCollection()
        : base("\x001bP")
    {
    }

    protected override bool Test(char character)
    {
        return true;
    }
}
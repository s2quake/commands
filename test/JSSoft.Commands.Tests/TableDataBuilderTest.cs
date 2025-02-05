// <copyright file="TableDataBuilderTest.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.IO;

namespace JSSoft.Commands.Tests;

public sealed class TableDataBuilderTest
{
    [Fact]
    public void Data_Test()
    {
        var tableData = new TableDataBuilder(2);
        tableData.Add(["1", "2"]);
        var data = tableData.Data;
        var header = tableData.HasHeader;
        using var sw = new StringWriter();
        TextWriterExtensions.PrintTableData(sw, data, header);
        Assert.Equal("1    2    \n", sw.ToString());
    }

    [Fact]
    public void EmptyData_Test()
    {
        var tableData = new TableDataBuilder(2);
        var data = tableData.Data;
        var header = tableData.HasHeader;
        using var sw = new StringWriter();
        TextWriterExtensions.PrintTableData(sw, data, header);
        Assert.Empty(sw.ToString());
    }
}

// <copyright file="ITerminalSelectionCollection.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.Collections.Specialized;
using System.ComponentModel;

namespace JSSoft.Terminals;

public interface ITerminalSelectionCollection : IList<TerminalSelection>, INotifyPropertyChanged, INotifyCollectionChanged
{    
    void SelectAll();
}

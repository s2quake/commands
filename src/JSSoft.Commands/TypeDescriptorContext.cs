// <copyright file="TypeDescriptorContext.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System.ComponentModel;

namespace JSSoft.Commands;

internal sealed class TypeDescriptorContext(
    IServiceProvider serviceProvider) : ITypeDescriptorContext
{
    IContainer ITypeDescriptorContext.Container => null!;

    public object Instance => null!;

    PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => null!;

    public object? GetService(Type serviceType) => serviceProvider.GetService(serviceType);

    void ITypeDescriptorContext.OnComponentChanged()
    {
    }

    bool ITypeDescriptorContext.OnComponentChanging() => false;
}

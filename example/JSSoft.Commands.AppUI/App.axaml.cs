// <copyright file="App.axaml.cs" company="JSSoft">
//   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
//   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// </copyright>

using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace JSSoft.Commands.AppUI;

/// <summary>
/// Definition of the <see cref="App"/> class.
/// </summary>
public partial class App : Application, IServiceProvider
{
    private readonly CompositionContainer _container;

    public App()
    {
        _container = new CompositionContainer(new AssemblyCatalog(typeof(App).Assembly));
    }

    public static new App Current
        => Application.Current as App ?? throw new InvalidOperationException();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    public void RegisterService<T>(T service)
    {
        _container.ComposeExportedValue(service);
    }

    public T? GetService<T>()
    {
        return _container.GetExportedValue<T>();
    }

    public object? GetService(Type serviceType)
    {
        var contractName = AttributedModelServices.GetContractName(serviceType);
        return _container.GetExportedValue<object?>(contractName);
    }
}

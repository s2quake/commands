using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace JSSoft.Commands.AppUI;

public partial class App : Application, IServiceProvider
{
    private readonly CompositionContainer _container;

    public App()
    {
        _container = new CompositionContainer(new AssemblyCatalog(typeof(App).Assembly));
    }

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

    public override void RegisterServices()
    {
        base.RegisterServices();
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
        return _container.GetExportedValue<object?>(AttributedModelServices.GetContractName(serviceType));
    }

    public static new App Current => Application.Current as App ?? throw new InvalidOperationException();
}

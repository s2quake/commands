using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace JSSoft.Commands.AppUI;

partial class TerminalStyles : ResourceDictionary
{
    public TerminalStyles()
    {
        AvaloniaXamlLoader.Load(this);
        App.Current.RegisterService(this);
    }
}

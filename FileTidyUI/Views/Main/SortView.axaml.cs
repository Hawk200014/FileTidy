using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Xilium.CefGlue.Avalonia;

namespace FileTidyUI.Views.Main;

public partial class SortView : UserControl
{
    public SortView()
    {
        InitializeComponent();

        var browserWrapper = this.FindControl<Decorator>("browserWrapper");

        var browser = new AvaloniaCefBrowser();
        browser.Address = "";


        browserWrapper.Child = browser;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
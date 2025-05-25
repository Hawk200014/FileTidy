using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FileTidyUI.Views.Main;

public partial class SortView : UserControl
{
    public SortView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FileTidyUI.Views.Main;

public partial class DupplicateView : UserControl
{
    public DupplicateView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
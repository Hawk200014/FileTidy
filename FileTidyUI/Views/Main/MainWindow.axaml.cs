using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FileTidyUI.Views.Dialogs;
using ReactiveUI;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FileTidyUI.Views.Main;

public partial class MainWindow : Window
{

    public static Window window;

    public MainWindow()
    {
        InitializeComponent();

        window = this;

    }

}

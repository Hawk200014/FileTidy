using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FileTidyUI.ViewModels;
using FileTidyUI.Views.Dialogs;
using ReactiveUI;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FileTidyUI.Views.Main;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        Opened += OnOpened;
        Closing += OnClosing;
    }

    private void OnClosing(object? sender, EventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.SaveWindowPosition(this.Position);
            vm.SaveWindowSize(this.Width, this.Height);
        }
    }

    private void OnOpened(object? sender, EventArgs e)
    {

        if (DataContext is MainViewModel vm)
        {
            this.Position = vm.WindowPosition;
            this.Width = vm.WindowWidth;
            this.Height = vm.WindowHeight;
        }
    }
}



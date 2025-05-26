using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using FileTidyUI.ViewModels;
using FileTidyUI.Views.Main;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileTidyUI;

public partial class App : Application
{
    private MainViewModel _mainViewModel;

    public static IServiceProvider ServiceProvider { get; set; }

    public App()
    {
        _mainViewModel = ServiceProvider.GetService<MainViewModel>() ?? throw new InvalidOperationException("MainViewModel not found in service provider.");
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void RegisterServices()
    {
        base.RegisterServices();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = _mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}

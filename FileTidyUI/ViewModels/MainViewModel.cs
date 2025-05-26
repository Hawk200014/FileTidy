using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FileTidyBase;
using FileTidyBase.Controller;
using FileTidyBase.Models;
using FileTidyDatabase;
using ReactiveUI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace FileTidyUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    public int WindowWidth { get; set; } = 800;
    public int WindowHeight { get; set; } = 600;
    public PixelPoint WindowPosition { get; set;  } = new PixelPoint(100, 100);


    private static ILogger Log => Serilog.Log.ForContext<MainViewModel>();
    public ReactiveCommand<Window, Unit> SelectChaosFolderCommand { get; } 
    public ReactiveCommand<Unit,Unit> ExecuteButtonCommand { get; }

    private readonly FileBaseController _fileBaseController;
    private readonly SettingsController _settingsController;
    private string _chaosFolderPath = "";

    private Dictionary<string, List<string>> _fileTypes = new()
    {
        { "Img", new List<string>{".PNG" , ".WEBP", ".JPG", ".JPEG"} },
        { "PDF", new List<string>{".PDF"} },
        { "Video", new List<string>{".MP4", ".AVI", ".MKV"} },
        { "Text", new List<string>{".TXT"} },
        { "All", new List<string>{".PNG" , ".WEBP", ".JPG", ".PDF", ".MP4", ".AVI", ".MKV", ".TXT" } }
    };

    public IEnumerable<string> FileTypeKeys => _fileTypes.Keys;

    private string _selectedFileTypeKey = "PDF";
    public string SelectedFileTypeKey
    {
        get => _selectedFileTypeKey;
        set => this.RaiseAndSetIfChanged(ref _selectedFileTypeKey, value);
    }


    public string ChaosFolderPath
    {
        get => _chaosFolderPath;
        set => this.RaiseAndSetIfChanged(ref _chaosFolderPath, value);
    }

    public MainViewModel(FileBaseController fileBaseController, SettingsController settingsController)
    {
        _fileBaseController = fileBaseController;
        _settingsController = settingsController;
        ExecuteButtonCommand = ReactiveCommand.CreateFromTask(ExecuteButtoAsync);
        SelectChaosFolderCommand = ReactiveCommand.CreateFromTask<Window>(SelectChaosFolderAsync);

        LoadWindowProperties();
    }



    private async Task SelectChaosFolderAsync(Window window)
    {
        Log.Here().Debug("SelectChaosFolderAsync called");
        var topLevel = TopLevel.GetTopLevel(window);

        // Start async operation to open the dialog.
        Log.Here().Debug("Opening folder picker dialog");
        var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Open Chaos Folder",
            AllowMultiple = false
        });

        Log.Here().Debug("Folder picker dialog closed");
        Log.Here().Debug("Number of folders selected: " + folder.Count);

        if (folder.Count >= 1)
        {
            Log.Here().Debug("Folder selected: " + folder[0].Path.AbsolutePath);
            ChaosFolderPath = folder[0].Path.AbsolutePath;
        }
    }

    private async Task ExecuteButtoAsync()
    {

        _fileBaseController.ExecuteActions();
        LoadFiles();
    }

    public void LoadFiles()
    {

    }

    #region Window Position and Size

    public void SaveWindowPosition(PixelPoint position)
    {
        _settingsController.SetSettingAsync("WindowPositionX", position.X.ToString()).Wait();
        _settingsController.SetSettingAsync("WindowPositionY", position.Y.ToString()).Wait();
    }

    public void SaveWindowSize(double width, double height)
    {
        _settingsController.SetSettingAsync("WindowWidth", width.ToString()).Wait();
        _settingsController.SetSettingAsync("WindowHeight", height.ToString()).Wait();
    }

    public PixelPoint GetSavedWindowPosition()
    {
        string x = _settingsController.GetSettingAsync("WindowPositionX").Result;
        string y = _settingsController.GetSettingAsync("WindowPositionY").Result;
        if (int.TryParse(x, out int posX) && int.TryParse(y, out int posY))
        {
            return new PixelPoint(posX, posY);
        }
        return new PixelPoint(100, 100); // Default position
    }


    public (double Width, double Height) GetSavedWindowSize()
    {
        string width = _settingsController.GetSettingAsync("WindowWidth").Result;
        string height = _settingsController.GetSettingAsync("WindowHeight").Result;
        if (double.TryParse(width, out double w) && double.TryParse(height, out double h))
        {
            return (w, h);
        }
        return (800, 600); // Default size
    }

    private void LoadWindowProperties()
    {
        this.WindowPosition = GetSavedWindowPosition();
        var size = GetSavedWindowSize();
        this.WindowWidth = (int)size.Width;
        this.WindowHeight = (int)size.Height;
    }

    #endregion
}

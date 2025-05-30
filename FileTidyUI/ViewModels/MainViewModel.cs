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



    private static ILogger Log => Serilog.Log.ForContext<MainViewModel>();
    public ReactiveCommand<Window, Unit> SelectChaosFolderCommand { get; } 
    public ReactiveCommand<Unit,Unit> ExecuteButtonCommand { get; }

    public ReactiveCommand<Unit, Unit> LoadFilesCommand { get; }

    private readonly FileBaseController _fileBaseController;
    private readonly SettingsController _settingsController;
    private string _chaosFolderPath = "";
    private FileBaseModel? _activeModel;

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
        LoadFilesCommand = ReactiveCommand.CreateFromTask(LoadFiles);

        LoadWindowProperties();
    }

    private async Task LoadFiles()
    {
        Log.Here().Debug("LoadFiles called");
        if (string.IsNullOrEmpty(ChaosFolderPath))
        {
            Log.Here().Warning("Chaos folder path is empty. Cannot load files.");
            return;
        }
        Log.Here().Debug("Loading files from chaos folder: " + ChaosFolderPath);
        try
        {
            await _fileBaseController.LoadFilesAsync(ChaosFolderPath, _fileTypes[SelectedFileTypeKey]);
            Log.Here().Information("Files loaded successfully from: " + ChaosFolderPath);
            _activeModel = _fileBaseController.GetFile() // Update the active model after loading files
        }
        catch (Exception ex)
        {
            Log.Here().Error(ex, "Error loading files from chaos folder: " + ChaosFolderPath);
        }
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
        await LoadFiles();
    }



    #region Window Position and Size

    public int WindowWidth { get; set; } = 800;
    public int WindowHeight { get; set; } = 600;
    public PixelPoint WindowPosition { get; set; } = new PixelPoint(100, 100);

    public void SaveWindowPosition(PixelPoint position)
    {
        _settingsController.SetSettingAsync(ESettings.WindowPositionX.ToString(), position.X.ToString()).Wait();
        _settingsController.SetSettingAsync(ESettings.WindowPositionY.ToString(), position.Y.ToString()).Wait();
    }

    public void SaveWindowSize(double width, double height)
    {
        _settingsController.SetSettingAsync(ESettings.WindowWidth.ToString(), width.ToString()).Wait();
        _settingsController.SetSettingAsync(ESettings.WindowHeight.ToString(), height.ToString()).Wait();
    }

    public PixelPoint GetSavedWindowPosition()
    {
        string x = _settingsController.GetSettingAsync(ESettings.WindowPositionX.ToString()).Result;
        string y = _settingsController.GetSettingAsync(ESettings.WindowPositionY.ToString()).Result;
        if (int.TryParse(x, out int posX) && int.TryParse(y, out int posY))
        {
            return new PixelPoint(posX, posY);
        }
        return new PixelPoint(100, 100); // Default position
    }


    public (double Width, double Height) GetSavedWindowSize()
    {
        string width = _settingsController.GetSettingAsync(ESettings.WindowWidth.ToString()).Result;
        string height = _settingsController.GetSettingAsync(ESettings.WindowHeight.ToString()).Result;
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

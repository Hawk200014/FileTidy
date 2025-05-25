using Avalonia.Controls;
using Avalonia.Platform.Storage;
using FileTidyBase;
using FileTidyBase.Models;
using ReactiveUI;
using Serilog;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace FileTidyUI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private static ILogger Log => Serilog.Log.ForContext<MainViewModel>();
    public ReactiveCommand<Window, Unit> SelectChaosFolderCommand { get; }


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

    public MainViewModel()
    {
        SelectChaosFolderCommand = ReactiveCommand.CreateFromTask<Window>(SelectChaosFolderAsync);
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
}

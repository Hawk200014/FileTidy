using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Xilium.CefGlue;
using Xilium.CefGlue.Avalonia;
using Xilium.CefGlue.Common.Handlers;
using FileTidyBase.Controller;
using FileTidyBase.Models;
using System.Xml.Linq;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Layout;
using FileTidyUI.Controls;
using System.ComponentModel;
using ReactiveUI;
using FileTidyUI.Views.Dialogs;
using Avalonia.ReactiveUI;
using System.Windows.Input;

namespace FileTidyUI.Views.Main;

public partial class MainView : UserControl
{
    private string className = "MainView";
    private AvaloniaCefBrowser browser;
    private int _index = 0;
    private FileBaseModel? _activeFileModel;

    private bool _isEditSortFolderMode = false;

    private Dictionary<string, List<string>> _fileTypes = new()
    {
        { "Img", new List<string>{".PNG" , ".WEBP", ".JPG"} },
        { "PDF", new List<string>{".PDF"} },
        { "Video", new List<string>{".MP4", ".AVI", ".MKV"} },
        { "Text", new List<string>{".TXT"} },
        { "All", new List<string>{".PNG" , ".WEBP", ".JPG", ".PDF", ".MP4", ".AVI", ".MKV", ".TXT" } }
    };

    private string _chaosFolderPath = "";
    private List<string> _allowedFileTypes = new();

    private FileBaseController _fileBaseController = new();


    public MainView()
    {
        InitializeComponent();

        var browserWrapper = this.FindControl<Decorator>("browserWrapper");

        browser = new AvaloniaCefBrowser();
        browser.Address = "";
        

        browserWrapper.Child = browser;



        FillFormatCombobox();

    }

    private void FillFormatCombobox()
    {
        ComboBox? filetypes = this.FindControl<ComboBox>("fileType");

        if (filetypes == null) return;

        filetypes.Items.Clear();

        foreach (var fileType in _fileTypes.Keys)
        {
            ComboBoxItem item = new ComboBoxItem()
            {
                Content = fileType
            };
            filetypes.Items.Add(item);
        }
    }

    private void SetBrowserWidthAndHeight()
    {
        string method = "SetBrowserWidthAndHeight";
        
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (browserParent != null)
            {
                browser.Height = browserParent.Bounds.Height;
                browser.Width = browserParent.Bounds.Width;
            }
            else
            {
                browser.Height = 600; // Default height
                browser.Width = 800; // Default width
            }

            browser.IsVisible = true;
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + " Sets Browser bounds to " + browser.Height + ", " + browser.Width);
        });
    }

    private void OnBrowserLoadStart(object sender, Xilium.CefGlue.Common.Events.LoadStartEventArgs e)
    {
        if (e.Frame.Browser.IsPopup || !e.Frame.IsMain)
        {
            return;
        }
    }


    #region handler
    public async void FolderSelectClick(object sender, RoutedEventArgs args)
    {

        string method = "FolderSelectClick";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + "Open Dialog");
        var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Open Chaos Folder",
            AllowMultiple = false
        });


        System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + "Number Folder Selected " + folder.Count);

        if (folder.Count >= 1)
        {
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + "Folder Selected " + folder[0].Path.AbsolutePath);
            SetChaosFolderName(folder[0].Path.AbsolutePath);
        }
    }

    public void AddSortFolderClick(object sender, RoutedEventArgs args)
    {
        string method = "AddSortFolderClick";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        var stackPanel = this.FindControl<StackPanel>("sortFolderStackPanel");


        SortFolderModel sortFolder = new SortFolderModel()
        {
            FolderPath = _chaosFolderPath,
            Name = "SortFolder" + stackPanel.Children.Count
        };

        var button = new SortButton(sortFolder)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch
        };

        SortFolderDialog dialog = new SortFolderDialog();

        dialog.ShowDialog(MainWindow.window);

        button.Click += SortButtonClick;

        stackPanel.Children.Add(button);

    }


    public void SortButtonClick(object sender, RoutedEventArgs args)
    {
        string method = "SortButtonClick";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if(_activeFileModel == null) return;
        SortButton button = sender as SortButton;
        if (button != null)
        {
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + "Sort Folder Clicked with guid: " + button.GetGuid().ToString());
            _activeFileModel.NewFilePath = button.GetFolderPath();
            //stackPanel.Children.Remove(button);
        }
    }

    

    

    public void NameChangedEvent(object sender, RoutedEventArgs args)
    {
        string method = "NameChangedEvent";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if (_activeFileModel != null)
        {
            string extension = _activeFileModel.GetFileType();

            if(!fileNameTB.Text.EndsWith(extension))
            {
                fileNameTB.Text = fileNameTB.Text.Trim() + extension; 
            }

            if ( fileNameTB.Text.Trim() == _activeFileModel.FileName)
            {
                _activeFileModel.NewFileName = "";

            }
            else
            {
                _activeFileModel.NewFileName = fileNameTB.Text.Trim();
            }


            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": File Name " + _activeFileModel.NewFileName);
        }
    }

    public void FileIndexUp(object sender, RoutedEventArgs args)
    {
        string method = "FileIndexUp";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if (_index < _fileBaseController.GetFileCount())
        {
            SetFileIndexNumber(_index + 1);
        }
    }

    public void DragStartEvent(object sender, VectorEventArgs args)
    {
        browser.IsVisible = false;
    }

    public void DragCompleteEvent(object sender, VectorEventArgs args)
    { 
        string method = "DragCompleteEvent";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        SetBrowserWidthAndHeight();
        
    }

    public void FileIndexDown(object sender, RoutedEventArgs args)
    {
        string method = "FileIndexDown";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if (_index > 1)
        {
            SetFileIndexNumber(_index - 1);
        }
    }

    public async void FileTypeIndexChanged(object sender, SelectionChangedEventArgs args)
    {
        if (fileType is null || fileType.SelectedValue is null) return;
        string method = "FileTypeIndexChanged";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        string selected = (fileType.SelectedValue as ComboBoxItem )?.Content?.ToString() ?? "";
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Filetype selected " + selected);
        if (_fileTypes.TryGetValue(selected, out List<string>? values))
        {
            SetAllowedFileTypes(values);
        }
    }
    #endregion


    #region Setter

    public void SetChaosFolderName(string name)
    {
        string method = "SetChaosFolderName";
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + " Sets Chaos Folder to " + name);
        chaosFolderPathTB.Text = name;
        _chaosFolderPath = name;
        LoadFiles();
    }

    public void SetAllowedFileTypes(List<string> types)
    {
        string method = "SetAllowedFileTypes";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        _allowedFileTypes = types;
        System.Diagnostics.Debug.WriteLine(className + ":" + method +": Sets Allowed Types " + types);
        LoadFiles();
    }

    public async void SetFiles(List<string> files)
    {
        string method = "SetFiles";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);

        foreach (var file in files)
        {
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Add File " + file);
            _fileBaseController.AddFile(file);
        }

        SetFileMaxNumber(files.Count);
        SetFileIndexNumber(1);

        bool first = true;

        foreach (FileBaseModel filebase in _fileBaseController.GetAllFiles())
        {
            
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Loads Fileinfo " + filebase.FilePath);
            await filebase.GetFileInfo();
            AddProgressbarValue(1);
            if (first)
            {
                first = false;
                UpdateFileInfo();
            }
        }

        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Sets Files " + files);

        ResetProgressbar();
    }

    public void SetFileIndexNumber(int index)
    {
        string method = "SetFileIndexNumber";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if(_fileBaseController.GetFileCount() == 0)
        {
            index = 0;
        }
        _index = index;
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            fileIndexNumber.Content = index;
        });
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Sets FileIndexNumer to " + index);
        UpdateFileInfo();
        LoadBrowser(index);
    }

    public void UpdateFileInfo()
    {
        string method = "UpdateFileInfo";
        bool valid = _fileBaseController.GetFileCount() != 0;
       
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (valid)
            {

                FileBaseModel file = _fileBaseController.GetFile(_index - 1);
                fileNameTB.Text = file.FileName;
                filePathTB.Text = file.FilePath;
                fileTypeTB.Text = file.FileType;
                fileHashTB.Text = file.FileContentHashValue;
                fileSizeTB.Text = file.FileSize;
                fileActionTB.Text = file.Action;
                _activeFileModel = file;
                System.Diagnostics.Debug.WriteLine(className + ":" + method + " Set Active Filemodel to " + _activeFileModel.FileName);
                browser.IsVisible = true;
            }
            else
            {
                fileNameTB.Text = "";
                filePathTB.Text = "";
                fileTypeTB.Text = "";
                fileHashTB.Text = "";
                fileSizeTB.Text = "";
                fileActionTB.Text = "";
                _activeFileModel = null;
                System.Diagnostics.Debug.WriteLine(className + ":" + method + " Set Active Filemodel to null");
                browser.Address = "";
                browser.IsVisible = false;
            }
        });
    }

    private void LoadBrowser(int index)
    {
        SetBrowserWidthAndHeight();
        if (_fileBaseController.GetFileCount() > index - 1 && _fileBaseController.GetFileCount() != 0)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                browser.Address = @"file:///" + _fileBaseController.GetFile(index-1).FilePath;
            });
        }
    }

    public void SetFileMaxNumber(int max)
    {
        string method = "SetFileMaxNumber";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            fileMaxNumber.Content = "" + max;
        });
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Sets MaxFileNumer to " + max);
    }

    #endregion

    #region Getter

    public string GetChaosFolderName()
    {
        string method = "GetChaosFolderName";
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Gets " + _chaosFolderPath);
        return _chaosFolderPath;
    }

    public List<string> GetAllowedFileTypes()
    {
        string method = "GetAllowedFileTypes";
        //System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Gets " + _allowedFileTypes);
        return _allowedFileTypes;
    }

    #endregion


    public void LoadFiles()
    {
        string method = "LoadFiles";
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Resetting Files");
        _fileBaseController.RemoveAllFiles();
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Chaosfolderpath is empty: " + string.IsNullOrEmpty(_chaosFolderPath));
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": allowedfiletypes count: " + _allowedFileTypes.Count);
        if (string.IsNullOrEmpty(_chaosFolderPath) || _allowedFileTypes.Count == 0) return;
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Added to Threadpool");
        ThreadPool.QueueUserWorkItem(LoadFilesAsync);
    }

    public void LoadFilesAsync(object? stateInfo)
    {
        string method = "LoadFilesAsync";
        string[] files = Directory.GetFiles(GetChaosFolderName(), "*", SearchOption.TopDirectoryOnly);
        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": all files: " + files.Length);

        files = files.Where(file => GetAllowedFileTypes()
            .Any(extension => file.ToUpper().EndsWith(extension.ToUpper())))
            .ToArray();

        InitProgressbar("Loading Files", files.Length);

        System.Diagnostics.Debug.WriteLine(className + ":" + method + ": filtered files: " + files.Length);

        SetFiles(files.ToList());
    }

    public void InitProgressbar(string progressName, int maxValue)
    {
        string method = "InitProgressbar";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            progressBarLbl.Content = progressName;
            progressBar.IsIndeterminate = false;
            progressBar.Maximum = maxValue;
            progressBar.Minimum = 0;
            progressBar.Value = 0;
            progressBar.IsVisible = true;
            progressBarLbl.IsVisible = true;
        });
    }

    public void ResetProgressbar()
    {
        string method = "ResetProgressbar";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            progressBarLbl.Content = "Idle";
            progressBar.IsIndeterminate = true;
            progressBar.Value = 0;
            progressBarLbl.IsVisible = false;
            progressBar.IsVisible = false;
        });
    }

    public void SetProgressbarValue(int value)
    {
        string method = "SetProgressbarValue";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            progressBar.Value = value;
        });
    }

    public void AddProgressbarValue(int value)
    {
        string method = "AddProgressbarValue";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            progressBar.Value += value;
        });
    }


    private class BrowserLifeSpanHandler : LifeSpanHandler
    {
        protected override bool OnBeforePopup(
            CefBrowser browser,
            CefFrame frame,
            string targetUrl,
            string targetFrameName,
            CefWindowOpenDisposition targetDisposition,
            bool userGesture,
            CefPopupFeatures popupFeatures,
            CefWindowInfo windowInfo,
            ref CefClient client,
            CefBrowserSettings settings,
            ref CefDictionaryValue extraInfo,
            ref bool noJavascriptAccess)
        {
            var bounds = windowInfo.Bounds;
            Dispatcher.UIThread.Post(() =>
            {
                var window = new Window();
                var popupBrowser = new AvaloniaCefBrowser();
                popupBrowser.Address = targetUrl;
                window.Content = popupBrowser;
                window.Position = new PixelPoint(bounds.X, bounds.Y);
                window.Height = bounds.Height;
                window.Width = bounds.Width;
                window.Title = targetUrl;
                window.Show();
            });
            return true;
        }
    }

    public void FileDeleteActionClick(object sender, RoutedEventArgs args)
    {
        string method = "FileDeleteActionClick";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if (_fileBaseController.GetFileCount() > _index - 1)
        {
            FileBaseModel file = _fileBaseController.GetFile(_index - 1);
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Delete File " + file.FilePath);
            file.SetDeleteAction();
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": File Action " + file.Action);
            UpdateFileInfo();
        }
    }

    public void FileResetActionClick(object sender, RoutedEventArgs args)
    {
        string method = "FileResetActionClick";
        System.Diagnostics.Debug.WriteLine(className + ":" + method);
        if (_fileBaseController.GetFileCount() > _index - 1)
        {
            FileBaseModel file = _fileBaseController.GetFile(_index - 1);
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": Reset File " + file.FilePath);
            file.ResetAction();
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ": File Action " + file.Action);

            UpdateFileInfo();
        }
    }


}

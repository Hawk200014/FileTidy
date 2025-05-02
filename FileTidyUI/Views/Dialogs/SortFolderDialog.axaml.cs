using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FileTidyBase.Models;
using FileTidyUI.ResultSets;
using System;

namespace FileTidyUI.Views.Dialogs;

public partial class SortFolderDialog : Window, IDialog<SortFolderResultSet>
{
    public string className = "SortFolderDialog";

    private SortFolderResultSet _resultSet { get; set; } = new(ACTION.NONE);
    private bool _IsEditMode { get; set; } = false;

    public SortFolderDialog(SortFolderModel? model = null)
    {
        InitializeComponent();

        if(model != null)
        {
            SetSortFolderValues(model);
            deleteBtn.IsVisible = true;
            _IsEditMode = true;
        }
        else
        {
            deleteBtn.IsVisible = false;
            sortFolderGuidTB.Text = Guid.NewGuid().ToString();
        }
    }

    private void SetSortFolderValues(SortFolderModel model)
    {
        sortFolderGuidTB.Text = model.GUID.ToString();
        sortFolderNameTB.Text = model.Name;
        sortPathTB.Text = model.FolderPath;
    }

    public async void SortPathSelectButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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
        if (folder.Count >= 1)
        {
            System.Diagnostics.Debug.WriteLine(className + ":" + method + ":" + "Folder Selected " + folder[0].Path.AbsolutePath);
            sortPathTB.Text= folder[0].Path.AbsolutePath;
        }
    }

    public async void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _resultSet.SetAction(ACTION.DELETE);
        this.Close();
    }

    public async void OKButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
        _resultSet.SetAction(_IsEditMode ? ACTION.EDIT : ACTION.ADD);
        this.Close();
    }

    public async void CancleButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _resultSet.SetAction(ACTION.NONE);
        this.Close();
    }

    public SortFolderResultSet GetValue()
    {
        if(_resultSet.GetAction() != ACTION.DELETE && string.IsNullOrEmpty(sortPathTB.Text))
        {
            throw new Exception("Sort Path is empty");
        }
        if (_resultSet.GetAction() != ACTION.DELETE && string.IsNullOrEmpty(sortFolderNameTB.Text))
        {
            throw new Exception("Sort Folder Name is empty");
        }
        if (string.IsNullOrEmpty(sortFolderGuidTB.Text))
        {
            throw new Exception("Sort Folder GUID is empty");
        }

        SortFolderModel sortFolderModel = new SortFolderModel
        {
            FolderPath = sortPathTB.Text,
            Name = sortFolderNameTB.Text,
            GUID = Guid.Parse( sortFolderGuidTB.Text)
        };

        _resultSet.SetResult(sortFolderModel);

        return _resultSet;
    }
}
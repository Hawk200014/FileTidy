using FileTidyBase.Models;
using FileTidyDatabase;
using Serilog; // Add this using directive
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyBase.Controller;

/// <summary>
/// Provides functionality for managing a collection of file models, including adding, removing,  updating, and
/// executing actions on files.
/// </summary>
/// <remarks>This controller maintains an internal list of <see cref="FileBaseModel"/> objects and
/// provides  methods to manipulate and query the collection. It supports operations such as adding files  by file
/// path or model, removing files, updating file properties, and executing actions defined  within the file
/// models.</remarks>
public class FileBaseController
{
    private static ILogger Log => Serilog.Log.ForContext<FileBaseController>();

    /// <summary>
    /// Gets or sets the collection of file models.
    /// </summary>
    private List<FileBaseModel> _files { get; set; } = new List<FileBaseModel>();

    private SettingsController _settingsController;
    private int _selectIndex = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileBaseController"/> class.
    /// </summary>
    public FileBaseController(SettingsController settingsController)
    {
        Log.Here().Debug("Constructing FileBaseController");
        this._settingsController = settingsController;
    }

    /// <summary>
    /// Gets the total number of files currently managed by the instance.
    /// </summary>
    /// <returns>The total count of files as an integer. Returns 0 if no files are present.</returns>
    public int GetFileCount()
    {
        Log.Here().Debug("Getting file count: {Count}", _files.Count);
        return _files.Count;
    }

    /// <summary>
    /// Removes all files from the collection.
    /// </summary>
    /// <remarks>This method clears the entire collection of files, leaving it empty.  Use this method
    /// when you need to reset or discard all files in the collection.</remarks>
    public void RemoveAllFiles()
    {
        Log.Here().Debug("Removing all files from collection");
        _files.Clear();
    }

    /// <summary>
    /// Retrieves all files currently stored in the system.
    /// </summary>
    /// <returns>A list of <see cref="FileBaseModel"/> objects representing the files.  The list will be empty if no files
    /// are available.</returns>
    public List<FileBaseModel> GetAllFiles()
    {
        Log.Here().Debug("Retrieving all files. Count: {Count}", _files.Count);
        return _files;
    }

    /// <summary>
    /// Adds a file to the collection.
    /// </summary>
    /// <remarks>The file is represented internally as a <see cref="FileBaseModel"/> instance and
    /// added to the collection. Ensure that <paramref name="filePath"/> points to a valid file path.</remarks>
    /// <param name="filePath">The full path of the file to add. Cannot be null or empty.</param>
    public void AddFile(string filePath)
    {
        Log.Here().Debug("Adding file by path: {FilePath}", filePath);
        var file = new FileBaseModel(filePath);
        _files.Add(file);
    }

    /// <summary>
    /// Adds a file to the collection.
    /// </summary>
    /// <remarks>If the <paramref name="file"/> parameter is <see langword="null"/>, the method does
    /// nothing.</remarks>
    /// <param name="file">The file to add. Must not be <see langword="null"/>.</param>
    public void AddFile(FileBaseModel file)
    {
        if (file != null)
        {
            Log.Here().Debug("Adding file model: {FilePath}", file.FilePath);
            _files.Add(file);
        }
        else
        {
            Log.Here().Debug("Attempted to add null file model");
        }
    }

    /// <summary>
    /// Removes the file with the specified file path from the collection.
    /// </summary>
    /// <remarks>If no file with the specified path exists in the collection, the method performs no
    /// action.</remarks>
    /// <param name="filePath">The path of the file to remove. Cannot be null or empty.</param>
    public void RemoveFile(string filePath)
    {
        Log.Here().Debug("Removing file by path: {FilePath}", filePath);
        var file = _files.FirstOrDefault(f => f.FilePath == filePath);
        if (file != null)
        {
            _files.Remove(file);
            Log.Here().Debug("File removed: {FilePath}", filePath);
        }
        else
        {
            Log.Here().Debug("File not found for removal: {FilePath}", filePath);
        }
    }

    /// <summary>
    /// Removes the specified file from the collection.
    /// </summary>
    /// <remarks>If the specified file is <see langword="null"/>, the method does nothing.</remarks>
    /// <param name="file">The file to remove. Must not be <see langword="null"/>.</param>
    public void RemoveFile(FileBaseModel file)
    {
        if (file != null)
        {
            Log.Here().Debug("Removing file model: {FilePath}", file.FilePath);
            _files.Remove(file);
        }
        else
        {
            Log.Here().Debug("Attempted to remove null file model");
        }
    }

    /// <summary>
    /// Executes the predefined actions for each file in the collection.
    /// </summary>
    /// <remarks>This method iterates through all files in the internal collection and invokes their
    /// respective <see cref="File.ExecuteAction"/> method. Ensure that the collection is properly initialized and
    /// populated before calling this method.</remarks>
    public void ExecuteActions()
    {
        Log.Here().Debug("Executing actions for all files. Count: {Count}", _files.Count);
        foreach (var file in _files)
        {
            Log.Here().Debug("Executing action for file: {FilePath}", file.FilePath);
            file.ExecuteAction();
        }
    }

    /// <summary>
    /// Updates the details of an existing file in the collection based on its file path.
    /// </summary>
    /// <remarks>If a file with the same file path as the provided <paramref name="file"/> exists in
    /// the collection, its details are replaced with the details from the provided file. If no matching file is
    /// found, the method performs no action.</remarks>
    /// <param name="file">The file model containing the updated details. The <see cref="FileBaseModel.FilePath"/> property must match
    /// an existing file in the collection.</param>
    public void UpdateFile(FileBaseModel file)
    {
        if (file != null)
        {
            Log.Here().Debug("Updating file: {FilePath}", file.FilePath);
            var existingFile = _files.FirstOrDefault(f => f.FilePath == file.FilePath);
            if (existingFile != null)
            {
                existingFile = file;
                Log.Here().Debug("File updated: {FilePath}", file.FilePath);
            }
            else
            {
                Log.Here().Debug("File not found for update: {FilePath}", file.FilePath);
            }
        }
        else
        {
            Log.Here().Debug("Attempted to update null file model");
        }
    }

    /// <summary>
    /// Updates the file path of an existing file to a new specified path.
    /// </summary>
    /// <remarks>If no file with the specified <paramref name="filePath"/> exists, the method performs
    /// no action.</remarks>
    /// <param name="filePath">The current file path of the file to be updated. Must match an existing file's path.</param>
    /// <param name="newFilePath">The new file path to assign to the file.</param>
    public void SetNewFilePath(string filePath, string newFilePath)
    {
        Log.Here().Debug("Setting new file path. Old: {OldFilePath}, New: {NewFilePath}", filePath, newFilePath);
        var file = _files.FirstOrDefault(f => f.FilePath == filePath);
        if (file != null)
        {
            file.SetNewFilePath(newFilePath);
            Log.Here().Debug("File path updated. New: {NewFilePath}", newFilePath);
        }
        else
        {
            Log.Here().Debug("File not found for path update: {FilePath}", filePath);
        }
    }

    /// <summary>
    /// Retrieves the file at the specified index from the collection.
    /// </summary>
    /// <param name="index">The zero-based index of the file to retrieve. Must be within the valid range of the collection.</param>
    /// <returns>The <see cref="FileBaseModel"/> at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown if <paramref name="index"/> is less than 0 or greater than or equal to the number of files in the
    /// collection.</exception>
    public FileBaseModel? GetFile()
    {
        if (_selectIndex == -1) return null;
        Log.Here().Debug("Retrieving file at index: {Index}", _selectIndex);
        if (_selectIndex >= 0 && _selectIndex < _files.Count)
        {
            Log.Here().Debug("File retrieved: {FilePath}", _files[_selectIndex].FilePath);
            return _files[_selectIndex];
        }
        else
        {
            Log.Here().Fatal("Index out of range: {Index}", _selectIndex);
            throw new IndexOutOfRangeException("Index out of range");
        }
    }

    public FileBaseModel? GetNextFile()
    {
        _selectIndex++;
        if (_selectIndex == _files.Count)
        {
            _selectIndex = 0;
        }

        return GetFile();
    }

    public FileBaseModel? GetPreviousFile()
    {
        _selectIndex--;
        if (_selectIndex < 0)
        {
            _selectIndex = _files.Count - 1;
        }
        return GetFile();
    }

    public async Task LoadFilesAsync(string chaosFolderPath, string extension)
    {
        bool searchChilds = _settingsController.GetSettingAsync(ESettings.SearchChildFolders.ToString()).Result == "true";

        string[] files = Directory.GetFiles(chaosFolderPath, "*.*", searchChilds ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
            .Where(f => list.Contains(Path.GetExtension(f).ToLower())).ToArray();

        Log.Here().Debug("Loading files from: {ChaosFolderPath}, SearchChilds: {SearchChilds}, FileCount: {FileCount}");

        List<FileBaseModel> fileBaseModels = new List<FileBaseModel>();

        foreach (var file in files)
        {
            try
            {
                Log.Here().Debug("Adding file: {FilePath}", file);
                var fileModel = new FileBaseModel(file);
                fileBaseModels.Add(fileModel);
                await fileModel.GetFileInfo();
            }
            catch (Exception ex)
            {
                Log.Here().Error(ex, "Error adding file: {FilePath}", file);
            }
        }

        this._files = fileBaseModels;
        if (_files.Count > 0)
        {
            _selectIndex = 0;
            Log.Here().Debug("Files loaded successfully. Count: {Count}", _files.Count);
        }
        else
        {
            _selectIndex = -1;
            Log.Here().Warning("No files found in the specified directory: {ChaosFolderPath}", chaosFolderPath);
        }
    }
}

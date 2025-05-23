using FileTidyBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyBase
{
    /// <summary>
    /// Provides a base class for managing file-based operations within a specified directory.
    /// </summary>
    /// <remarks>The <see cref="BaseController{T}"/> class is designed to handle file management tasks, such
    /// as retrieving files from a directory, filtering them by extensions, and maintaining a collection of file models.
    /// It provides functionality to set the directory path, retrieve files, and clear the file list. Derived classes
    /// can extend this functionality as needed.</remarks>
    /// <typeparam name="T">The type of the file model that the controller operates on. Must derive from <see cref="FileBaseModel"/>.</typeparam>
    public abstract class BaseController<T> where T : FileBaseModel
    {
        #region Fields
        private static Serilog.ILogger Log => Serilog.Log.ForContext<BaseController<T>>();
        private string _directoryPath { get; set; }
        private string[] _extensions { get; set; }
        public List<T> Items { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController{T}"/> class with the specified directory path
        /// and file extensions.
        /// </summary>
        /// <remarks>The <paramref name="directoryPath"/> parameter specifies the directory that the
        /// controller will manage, while the <paramref name="extensions"/> parameter defines the file types to include
        /// in operations.</remarks>
        /// <param name="directoryPath">The path to the directory that the controller will operate on. Cannot be null or empty.</param>
        /// <param name="extensions">An array of file extensions to filter the items. Cannot be null or empty.</param>
        public BaseController(string directoryPath, string[] extensions)
        {
            Log.Here().Debug("Constructing BaseController");
            Items = new List<T>();
            Log.Here().Debug("Directory Path:" + directoryPath);
            _directoryPath = directoryPath;
            Log.Here().Debug("Extensions:" + string.Join(",", extensions));
            _extensions = extensions;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the directory path to be used by the application.
        /// </summary>
        /// <remarks>The specified directory path is stored for later use by the application.  Ensure that
        /// the provided path is valid and accessible to avoid runtime issues.</remarks>
        /// <param name="directoryPath">The full path to the directory. This value cannot be null or empty.</param>
        public void SetDirectoryPath(string directoryPath)
        {
            Log.Here().Debug("Setting Directory Path:" + directoryPath);
            _directoryPath = directoryPath;
        }

        /// <summary>
        /// Retrieves files from the specified directory and adds them to the collection of items.
        /// </summary>
        /// <remarks>Only files with extensions matching the predefined set are included. The method does
        /// not perform any asynchronous operations  despite returning a <see cref="Task"/>; it completes
        /// immediately.</remarks>
        /// <param name="searchInChildFolder">A value indicating whether to include files from child directories.  If <see langword="true"/>, files from
        /// child directories are included; otherwise, only files from the top-level directory are processed.</param>
        /// <returns></returns>
        public Task GetFilesInDirectory(bool searchInChildFolder = false)
        {
            Log.Here().Debug("Getting files in directory:" + _directoryPath);
            Log.Here().Debug("Search in child folder:" + searchInChildFolder);
            SearchOption searchOption = searchInChildFolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            Directory.GetFiles(_directoryPath, "*.*", searchOption)
                .Where(f => _extensions.Contains(Path.GetExtension(f)))
                .ToList()
                .ForEach(f =>
                {
                    var model = CreateModel(f);
                    Items.Add(model);
                });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Clears all items from the file list.
        /// </summary>
        /// <remarks>This method removes all entries from the file list.  After calling this method, the
        /// list will be empty.</remarks>
        public void ClearFileList()
        {
            Log.Here().Debug("Clearing file list");
            Items.Clear();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Creates an instance of the model using the specified file path.
        /// </summary>
        /// <param name="f">The file path used to initialize the model. Must not be null or empty.</param>
        /// <returns>An instance of the model of type <typeparamref name="T"/> initialized with the specified file path.</returns>
        private T CreateModel(string f)
        {
            return (T)new FileBaseModel(f);
        }
        #endregion
    }
}

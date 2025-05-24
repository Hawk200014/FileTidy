using FileTidyBase.Models;
using FileTidyDatabase;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyBase.Controller
{
    /// <summary>
    /// Provides functionality for managing a collection of sort folders, including adding and removing sort folders by path, name, or GUID.
    /// </summary>
    /// <remarks>
    /// This controller maintains an internal list of <see cref="SortFolderModel"/> objects and provides methods to manipulate the collection.
    /// </remarks>
    public class SortFolderController
    {
        private static ILogger Log => Serilog.Log.ForContext<SortFolderController>();

        private FileTidyDbContext _context;

        /// <summary>
        /// Gets or sets the collection of sort folders.
        /// </summary>
        private List<SortFolderModel> _sortFolders { get; set; } = new List<SortFolderModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SortFolderController"/> class.
        /// </summary>
        public SortFolderController(FileTidyDbContext context)
        {
            Log.Here().Debug("Constructing SortFolderController");
            this._context = context;
            _sortFolders = context.SortFolders.ToList();
        }

        /// <summary>
        /// Adds a new sort folder to the collection.
        /// </summary>
        /// <param name="folderPath">The path of the folder to add. Cannot be null or empty.</param>
        /// <param name="name">The name of the sort folder. Cannot be null or empty.</param>
        public void AddSortFolder(string folderPath, string name)
        {
            Log.Here().Debug("Adding sort folder. Path: {FolderPath}, Name: {Name}", folderPath, name);
            var sortFolder = new SortFolderModel()
            {
                FolderPath = folderPath,
                Name = name
            };
            _sortFolders.Add(sortFolder);
            _context.SortFolders.Add(sortFolder);
            _context.SaveChanges();
            Log.Here().Debug("Sort folder added. Path: {FolderPath}, Name: {Name}", folderPath, name);
        }

        /// <summary>
        /// Removes a sort folder from the collection by its GUID.
        /// </summary>
        /// <param name="guid">The GUID of the sort folder to remove.</param>
        public void RemoveSortFolder(Guid guid)
        {
            Log.Here().Debug("Removing sort folder by GUID: {GUID}", guid);
            var sortFolder = _sortFolders.FirstOrDefault(f => f.GUID == guid);
            if (sortFolder != null)
            {
               RemoveSortFolder(sortFolder);
                Log.Here().Debug("Sort folder removed. GUID: {GUID}", guid);
            }
            else
            {
                Log.Here().Debug("Sort folder not found for removal. GUID: {GUID}", guid);
            }
        }

        /// <summary>
        /// Removes a sort folder from the collection by its folder path.
        /// </summary>
        /// <param name="folderPath">The path of the folder to remove. Cannot be null or empty.</param>
        public void RemoveSortFolder(string folderPath)
        {
            Log.Here().Debug("Removing sort folder by path: {FolderPath}", folderPath);
            var sortFolder = _sortFolders.FirstOrDefault(f => f.FolderPath == folderPath);
            if (sortFolder != null)
            {
                RemoveSortFolder(sortFolder);
                Log.Here().Debug("Sort folder removed. Path: {FolderPath}", folderPath);
            }
            else
            {
                Log.Here().Debug("Sort folder not found for removal. Path: {FolderPath}", folderPath);
            }
        }

        /// <summary>
        /// Removes the specified sort folder from the collection.
        /// </summary>
        /// <param name="sortFolder">The sort folder to remove. Must not be null.</param>
        public void RemoveSortFolder(SortFolderModel sortFolder)
        {
            if (sortFolder != null)
            {
                Log.Here().Debug("Removing sort folder by model. Path: {FolderPath}, Name: {Name}, GUID: {GUID}", sortFolder.FolderPath, sortFolder.Name, sortFolder.GUID);
                _sortFolders.Remove(sortFolder);
                _context.SortFolders.Remove(sortFolder);
                _context.SaveChanges();
                Log.Here().Debug("Sort folder removed by model. Path: {FolderPath}, Name: {Name}, GUID: {GUID}", sortFolder.FolderPath, sortFolder.Name, sortFolder.GUID);
            }
            else
            {
                Log.Here().Debug("Attempted to remove null sort folder model.");
            }
        }
    }
}

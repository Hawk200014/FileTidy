using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Serilog; // Added for logging

namespace FileTidyBase.Models
{
    /// <summary>
    /// Represents a base model for file operations, providing properties and methods to manage file metadata and
    /// actions such as moving or deleting files.
    /// </summary>
    /// <remarks>This class encapsulates file-related information, such as file name, size, type, and content
    /// hash, and provides functionality to perform actions like moving or deleting the file. It ensures that the file
    /// exists before performing operations and validates inputs for critical methods.</remarks>
    public class FileBaseModel
    {
        private static ILogger Log => Serilog.Log.ForContext<FileBaseModel>();

        #region Fields
        private string _fileName = "";
        private string _fileContentHashValue = "";
        private string _fileSize = "";
        private string _fileType = "";
        private string _filePath = "";
        private string _newFilePath = "";
        private string _newFileName = "";
        private string _action = "";
        private FileInfo _fileInfo;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileBaseModel"/> class with the specified file path.
        /// </summary>
        /// <param name="filePath">The full path to the file. The file must exist at the specified path.</param>
        /// <exception cref="FileNotFoundException">Thrown if the file specified by <paramref name="filePath"/> does not exist.</exception>
        public FileBaseModel(string filePath)
        {
            Log.Here().Debug("Constructing FileBaseModel for path: {FilePath}", filePath);
            if (!System.IO.File.Exists(filePath))
            {
                Log.Here().Error("File does not exist: {FilePath}", filePath);
                throw new FileNotFoundException("File does not exist");
            }
            this.FilePath = filePath;
            _fileInfo = new FileInfo(this.FilePath);
        }
        #endregion

        #region Properties
        public string FileName
        {
            get { return _fileName; }
        }

        public string FileContentHashValue
        {
            get { return _fileContentHashValue; }
        }

        public string FileType
        {
            get { return _fileType; }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public string FileSize
        {
            get { return _fileSize; }
        }

        public string Action
        {
            get { return _action; }
        }

        public string NewFilePath
        {
            get { return _newFilePath; }
            set { _newFilePath = value; }
        }

        public string NewFileName
        {
            get { return _newFileName; }
            set { _newFileName = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Sets a new file path for the current file, ensuring the new path is valid and does not already exist.
        /// </summary>
        /// <remarks>This method updates the internal state to reference a new file path. Ensure that the
        /// current file exists and that the new file path does not conflict with an existing file before calling this
        /// method.</remarks>
        /// <param name="newFilePath">The new file path to be assigned. Must not be null, empty, or point to an existing file.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="newFilePath"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown if the current file specified by <see cref="FilePath"/> does not exist.</exception>
        /// <exception cref="Exception">Thrown if a file already exists at the specified <paramref name="newFilePath"/>.</exception>
        public void SetNewFilePath(string newFilePath)
        {
            Log.Here().Debug("Setting new file path. Old: {OldFilePath}, New: {NewFilePath}", this.FilePath, newFilePath);
            if (string.IsNullOrEmpty(newFilePath))
            {
                Log.Here().Error("New file path is null or empty");
                throw new ArgumentException("New file path is null or empty");
            }
            if (!System.IO.File.Exists(this.FilePath))
            {
                Log.Here().Error("File does not exist: {FilePath}", this.FilePath);
                throw new FileNotFoundException("File does not exist");
            }
            if (System.IO.File.Exists(newFilePath))
            {
                Log.Here().Error("New file path already exists: {NewFilePath}", newFilePath);
                throw new Exception("New file path already exists");
            }
            this._newFilePath = newFilePath;
            Log.Here().Debug("New file path set: {NewFilePath}", newFilePath);
        }

        /// <summary>
        /// Retrieves and processes file information, including name, type, size, and content hash.
        /// </summary>
        /// <remarks>This method performs multiple operations to gather and process file details.  It
        /// includes asynchronous processing for calculating the file's content hash. Ensure that the necessary file
        /// context is available before invoking this method.</remarks>
        /// <returns></returns>
        public async Task GetFileInfo()
        {
            Log.Here().Debug("Getting file info for: {FilePath}", this.FilePath);
            GetFileName();
            SetFileType();
            GetFileSize();
            await GetFileContentHash();
            Log.Here().Debug("File info retrieved: Name={FileName}, Type={FileType}, Size={FileSize}, Hash={FileContentHashValue}",
                _fileName, _fileType, _fileSize, _fileContentHashValue);
        }

        /// <summary>
        /// Gets the action associated with the current instance.
        /// </summary>
        /// <returns>The action as a string. This value may be null or empty if no action has been set.</returns>
        public string GetAction()
        {
            Log.Here().Debug("Getting action: {Action}", this._action);
            return this._action;
        }

        /// <summary>
        /// Sets the current action to "Move".
        /// </summary>
        /// <remarks>This method updates the internal state to indicate that the "Move" action is
        /// active.</remarks>
        public void SetMoveAction()
        {
            Log.Here().Debug("Setting action to Move for: {FilePath}", this.FilePath);
            this._action = "Move";
        }

        /// <summary>
        /// Sets the action to "Delete", indicating that the current operation will perform a delete action.
        /// </summary>
        /// <remarks>This method updates the internal state to reflect a delete operation.  It should be
        /// called when the desired action is to remove or delete an entity or resource.</remarks>
        public void SetDeleteAction()
        {
            Log.Here().Debug("Setting action to Delete for: {FilePath}", this.FilePath);
            this._action = "Delete";
        }

        /// <summary>
        /// Resets the current action to its default state.
        /// </summary>
        /// <remarks>This method clears the current action by setting it to an empty string. It can be
        /// used to reinitialize the action before assigning a new value.</remarks>
        public void ResetAction()
        {
            Log.Here().Debug("Resetting action for: {FilePath}", this.FilePath);
            this._action = "";
        }

        /// <summary>
        /// Executes the action specified by the current state of the object.
        /// </summary>
        /// <remarks>The action to be executed is determined by the value of the internal state. Supported
        /// actions include "Move" and "Delete". If the action is not recognized, no operation is performed.</remarks>
        public void ExecuteAction()
        {
            Log.Here().Debug("Executing action: {Action} for: {FilePath}", this._action, this.FilePath);
            switch (this._action)
            {
                case "Move":
                    MoveFile();
                    break;
                case "Delete":
                    DeleteFile();
                    break;
                default:
                    Log.Here().Debug("No action executed for: {FilePath}", this.FilePath);
                    break;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Computes and stores the MD5 hash of the file content specified by the <see cref="FilePath"/> property.
        /// </summary>
        /// <remarks>This method reads the file content asynchronously and calculates its MD5 hash.  The
        /// resulting hash is stored internally in a lowercase hexadecimal string format.</remarks>
        /// <returns></returns>
        private async Task GetFileContentHash()
        {
            Log.Here().Debug("Computing file content hash for: {FilePath}", this.FilePath);
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(this.FilePath))
                {
                    byte[] contentHashBytes = await md5.ComputeHashAsync(stream);
                    this._fileContentHashValue = BitConverter.ToString(contentHashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
            Log.Here().Debug("File content hash computed: {Hash}", this._fileContentHashValue);
        }

        /// <summary>
        /// Deletes the file at the specified file path.
        /// </summary>
        /// <remarks>This method checks for the existence of the file before attempting to delete it.  If
        /// the file does not exist, an exception is thrown.</remarks>
        /// <exception cref="Exception">Thrown if the file does not exist at the specified file path.</exception>
        private void DeleteFile()
        {
            Log.Here().Debug("Deleting file: {FilePath}", _filePath);
            if (!System.IO.File.Exists(_filePath))
            {
                Log.Here().Error("File does not exist for deletion: {FilePath}", _filePath);
                throw new Exception("File does not exist");
            }
            System.IO.File.Delete(_filePath);
            Log.Here().Debug("File deleted: {FilePath}", _filePath);
        }

        /// <summary>
        /// Moves the current file to a new location and optionally renames it.
        /// </summary>
        /// <remarks>This method moves the file specified by <see cref="FilePath"/> to the directory
        /// specified by the new file path. If <see cref="NewFileName"/> is not null or empty, the file will be renamed
        /// to the specified name during the move operation. After the move, the <see cref="FilePath"/> property is
        /// updated to reflect the new location.</remarks>
        /// <exception cref="Exception">Thrown if the new file path is null or empty, the current file does not exist, or a file already exists at
        /// the new file path.</exception>
        private void MoveFile()
        {
            Log.Here().Debug("Moving file: {FilePath} to {NewFilePath} (NewFileName: {NewFileName})", this.FilePath, _newFilePath, NewFileName);
            if (string.IsNullOrEmpty(_newFilePath))
            {
                Log.Here().Error("New file path is null or empty for move operation");
                throw new Exception("New file path is null or empty");
            }
            if (!System.IO.File.Exists(this.FilePath))
            {
                Log.Here().Error("File does not exist for move: {FilePath}", this.FilePath);
                throw new Exception("File does not exist");
            }
            if (System.IO.File.Exists(_newFilePath))
            {
                Log.Here().Error("New file path already exists for move: {NewFilePath}", _newFilePath);
                throw new Exception("New file path already exists");
            }
            string filename = string.IsNullOrEmpty(NewFileName) ? _fileName : NewFileName;
            System.IO.File.Move(this.FilePath, _newFilePath + filename);
            Log.Here().Debug("File moved from {OldFilePath} to {NewFilePath}{FileName}", this.FilePath, _newFilePath, filename);
            this._filePath = _newFilePath;
            this._newFilePath = "";
        }

        /// <summary>
        /// Updates the file size information for the current file.
        /// </summary>
        /// <remarks>This method retrieves the size of the file represented by the current <see
        /// cref="_fileInfo"/> instance and stores it as a string in the <see cref="_fileSize"/> field.</remarks>
        private void GetFileSize()
        {
            this._fileSize = _fileInfo.Length.ToString();
            Log.Here().Debug("File size set: {FileSize} for {FilePath}", this._fileSize, this.FilePath);
        }

        public string GetFileType()
        {
            return _fileType;
        }

        private void SetFileType()
        {
            this._fileType = _fileInfo.Extension;
            Log.Here().Debug("File type set: {FileType} for {FilePath}", this._fileType, this.FilePath);
        }

        private void GetFileName()
        {
            this._fileName = _fileInfo.Name;
            Log.Here().Debug("File name set: {FileName} for {FilePath}", this._fileName, this.FilePath);
        }

        #endregion
    }
}

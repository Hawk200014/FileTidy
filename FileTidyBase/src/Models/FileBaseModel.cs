using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FileTidyBase.Models
{
    public class FileBaseModel
    {
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
        public FileBaseModel(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new Exception("File does not exist");
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


        public void SetNewFilePath(string newFilePath)
        {
            if (string.IsNullOrEmpty(newFilePath))
                throw new Exception("New file path is null or empty");
            if (!System.IO.File.Exists(this.FilePath))
                throw new Exception("File does not exist");
            if (System.IO.File.Exists(newFilePath))
                throw new Exception("New file path already exists");
            this._newFilePath = newFilePath;
        }


        public async Task GetFileInfo()
        {
            GetFileName();
            SetFileType();
            GetFileSize();
            await GetFileContentHash();
        }

        public string GetAction()
        {
            return this._action;
        }

        public void SetMoveAction()
        {
            this._action = "Move";
        }

        public void SetDeleteAction()
        {
            this._action = "Delete";
        }

        public void ResetAction()
        {
            this._action = "";
        }

        public void ExecuteAction()
        {
            switch (this._action)
            {
                case "Move":
                    MoveFile();
                    break;
                case "Delete":
                    DeleteFile();
                    break;
                default:
                    break; ;
            }
        }
        #endregion

        #region Private Methods
        private async Task GetFileContentHash()
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(this.FilePath))
                {
                    byte[] contentHashBytes = await md5.ComputeHashAsync(stream);
                    this._fileContentHashValue = BitConverter.ToString(contentHashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private void DeleteFile()
        {
            if (!System.IO.File.Exists(_filePath))
                throw new Exception("File does not exist");
            System.IO.File.Delete(_filePath);
        }

        private void MoveFile()
        {
            if (string.IsNullOrEmpty(_newFilePath))
                throw new Exception("New file path is null or empty");
            if (!System.IO.File.Exists(this.FilePath))
                throw new Exception("File does not exist");
            if (System.IO.File.Exists(_newFilePath))
                throw new Exception("New file path already exists");
            string filename = string.IsNullOrEmpty(NewFileName) ? _fileName : NewFileName;
            System.IO.File.Move(this.FilePath, _newFilePath + filename);
            this._filePath = _newFilePath;
            this._newFilePath = "";
        }

        private void GetFileSize()
        {
            this._fileSize = _fileInfo.Length.ToString();
        }

        public string GetFileType()
        {
            return _fileType;
        }

        private void SetFileType()
        {
            this._fileType = _fileInfo.Extension;
        }

        private void GetFileName()
        {
            this._fileName = _fileInfo.Name;
        }

        #endregion
    }
}

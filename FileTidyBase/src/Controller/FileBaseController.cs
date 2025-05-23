using FileTidyBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyBase.Controller
{
    public class FileBaseController
    {
        private List<FileBaseModel> _files { get; set; } = new List<FileBaseModel>();

        public FileBaseController()
        {
        }

        public int GetFileCount()
        {
            return _files.Count;
        }

        public void RemoveAllFiles()
        {
            _files.Clear();
        }

        public List<FileBaseModel> GetAllFiles()
        {
            return _files;
        }

        public void AddFile(string filePath)
        {
            var file = new FileBaseModel(filePath);
            _files.Add(file);
        }

        public void AddFile(FileBaseModel file)
        {
            if (file != null)
            {
                _files.Add(file);
            }
        }

        public void RemoveFile(string filePath)
        {
            var file = _files.FirstOrDefault(f => f.FilePath == filePath);
            if (file != null)
            {
                _files.Remove(file);
            }
        }

        public void RemoveFile(FileBaseModel file)
        {
            if (file != null)
            {
                _files.Remove(file);
            }
        }

        public void ExecuteActions()
        {
            foreach (var file in _files)
            {
                file.ExecuteAction();
            }
        }

        public void UpdateFile(FileBaseModel file)
        {
            if (file != null)
            {
                var existingFile = _files.FirstOrDefault(f => f.FilePath == file.FilePath);
                if (existingFile != null)
                {
                    existingFile = file;
                }
            }
        }

        public void SetNewFilePath(string filePath, string newFilePath)
        {
            var file = _files.FirstOrDefault(f => f.FilePath == filePath);
            if (file != null)
            {
                file.SetNewFilePath(newFilePath);
            }
        }

        public FileBaseModel GetFile(int index)
        {
            if (index >= 0 && index < _files.Count)
            {
                return _files[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index out of range");
            }
        }
    }
}

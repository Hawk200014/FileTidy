using FileTidyBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyBase
{
    public abstract class BaseController<T> where T : FileBaseModel
    {
        private string _directoryPath { get; set; }
        private string[] _extensions { get; set; }
        public List<T> Items { get; set; }

        public BaseController(string directoryPath, string[] extensions)
        {
            Items = new List<T>();
            _directoryPath = directoryPath;
            _extensions = extensions;
        }

        public void SetDirectoryPath(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public Task GetFilesInDirectory()
        {
            Directory.GetFiles(_directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => _extensions.Contains(Path.GetExtension(f)))
                .ToList()
                .ForEach(f =>
                {
                    var model = CreateModel(f);
                    Items.Add(model);
                });
            return Task.CompletedTask;
        }

        public void ClearFileList()
        {
            Items.Clear();
        }

        private T CreateModel(string f)
        {
            return (T)new FileBaseModel(f);
        }
    }
}

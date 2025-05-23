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
        private static Serilog.ILogger Log => Serilog.Log.ForContext<BaseController<T>>();
        private string _directoryPath { get; set; }
        private string[] _extensions { get; set; }
        public List<T> Items { get; set; }

        public BaseController(string directoryPath, string[] extensions)
        {
            Log.Here().Debug("Constructing BaseController");
            Items = new List<T>();
            Log.Here().Debug("Directory Path:" + directoryPath);
            _directoryPath = directoryPath;
            Log.Here().Debug("Extensions:" + string.Join(",", extensions));
            _extensions = extensions;
        }

        public void SetDirectoryPath(string directoryPath)
        {
            Log.Here().Debug("Setting Directory Path:" + directoryPath);
            _directoryPath = directoryPath;
        }

        public Task GetFilesInDirectory(bool searchInChildFolder = false)
        {
            Log.Here().Debug("Getting files in directory:" + _directoryPath);
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

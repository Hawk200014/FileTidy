using System;

namespace FileTidyBase.Models
{
    public class SortFolderModel
    {
        public string FolderPath { get; set; }
        public string Name { get; set; }

        private Guid _guid;
        public Guid GUID
        {
            get => _guid;
            set => _guid = value == Guid.Empty ? Guid.NewGuid() : value;
        }

        public SortFolderModel()
        {
            _guid = Guid.NewGuid();
        }
    }
}
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SortFolderModel"/> class.
        /// </summary>
        /// <remarks>This constructor generates a new unique identifier for the instance by assigning a
        /// new <see cref="Guid"/> to the internal field.</remarks>
        public SortFolderModel()
        {
            _guid = Guid.NewGuid();
        }
    }
}
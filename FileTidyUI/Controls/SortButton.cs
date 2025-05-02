using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileTidyBase.Models;
using Avalonia.Interactivity;

namespace FileTidyUI.Controls
{
    class SortButton : Button
    {
        private SortFolderModel _sortFolderModel;

        // `SortButton` will be styled as a standard `Button` control.
        protected override Type StyleKeyOverride => typeof(Button);

        public SortButton(SortFolderModel sortFolderModel)
        {
            
            _sortFolderModel = sortFolderModel;
            this.Content = _sortFolderModel.Name;
            
        }

        public Guid GetGuid()
        {
            return _sortFolderModel.GUID;
        }

        public string GetFolderPath()
        {
            return _sortFolderModel.FolderPath;
        }

        public string GetName()
        {
            return _sortFolderModel.Name;
        }

        public SortFolderModel GetSortFolderModel()
        {
            return _sortFolderModel;
        }

        public void Update(SortFolderModel sortFolderModel)
        {
            _sortFolderModel = sortFolderModel;
            this.Content = _sortFolderModel.Name;
        }
    }
}

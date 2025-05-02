using FileTidyBase.Models;

namespace FileTidyUI.ResultSets
{
    public class SortFolderResultSet : IResultSet<SortFolderModel>
    {
        private SortFolderModel? _result;
        private ACTION _action;

        public SortFolderResultSet( ACTION action, SortFolderModel? model = null)
        {
            _result = model;
            _action = action;
        }
        public SortFolderModel? GetResult()
        {
            return _result;
        }
        public ACTION GetAction()
        {
            return _action;
        }
        public void SetResult(SortFolderModel result)
        {
            _result = result;
        }
        public void SetAction(ACTION action)
        {
            _action = action;
        }
    }
}

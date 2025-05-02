using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyUI.ResultSets
{
    public interface IResultSet<T>
    {
        public T? GetResult();
        public ACTION GetAction();
        public void SetResult(T result);
        public void SetAction(ACTION action);
    }
}

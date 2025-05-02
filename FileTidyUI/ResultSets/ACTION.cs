using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyUI.ResultSets
{
    public enum ACTION
    {
        /// <summary>
        /// No action
        /// </summary>
        NONE = 0,
        /// <summary>
        /// Add a new sort folder
        /// </summary>
        ADD = 1,
        /// <summary>
        /// Edit an existing sort folder
        /// </summary>
        EDIT = 2,
        /// <summary>
        /// Delete an existing sort folder
        /// </summary>
        DELETE = 3,
    }
}

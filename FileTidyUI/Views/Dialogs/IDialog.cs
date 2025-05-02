using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyUI.Views.Dialogs
{
    public interface IDialog<T>
    {
        /// <summary>
        /// Close the dialog.
        /// </summary>
        public void Close();

        /// <summary>
        /// Gets the value of the dialog.
        /// </summary>
        /// <returns>The Value of the dialog</returns>
        public T GetValue();
    }
}

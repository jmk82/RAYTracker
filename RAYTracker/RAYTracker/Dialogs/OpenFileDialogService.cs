using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace RAYTracker.Dialogs
{
    public class OpenFileDialogService : IOpenFileDialogService
    {
        public string ShowOpenFileDialog()
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();

            return dialog.FileName;
        }
    }
}

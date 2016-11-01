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
            dialog.Filter = "XML- ja tekstitiedostot (*.xml, *.txt)|*.xml;*.txt|Kaikki tiedostot (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.ShowDialog();

            return dialog.FileName;
        }
    }
}

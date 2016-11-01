using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAYTracker.Dialogs
{
    public class SaveFileDialogService : ISaveFileDialogService
    {
        public string ShowSaveFileDialog()
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "XML-tiedostot (*.xml)|*.xml|Kaikki tiedostot (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.AddExtension = true;
            dialog.ShowDialog();

            return dialog.FileName;
        }
    }
}

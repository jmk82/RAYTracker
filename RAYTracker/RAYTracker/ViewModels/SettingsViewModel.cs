using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAYTracker.ViewModels
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private IOpenFileDialogService _openFileDialogService;
        private string _sessionXMLFilename;

        public string SessionXMLFilename
        {
            get
            {
                return _sessionXMLFilename;
            }
            set
            {
                _sessionXMLFilename = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand ChooseFileCommand { get; set; }
        public RelayCommand SaveSettingsCommand { get; set; }

        public SettingsViewModel(IOpenFileDialogService openFileDialogService)
        {
            _openFileDialogService = openFileDialogService;
            SessionXMLFilename = Properties.Settings.Default.SessionXMLFilename;

            ChooseFileCommand = new RelayCommand(ChooseFile);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        private void SaveSettings()
        {
            if (!string.IsNullOrEmpty(_sessionXMLFilename))
            {
                Properties.Settings.Default.SessionXMLFilename = SessionXMLFilename;
                Properties.Settings.Default.Save();

                Messenger.Default.Send(new NotificationMessage("CloseSettingsWindow"));
            }
        }

        private void ChooseFile()
        {
            var filename = _openFileDialogService.ShowOpenFileDialog();

            if (!string.IsNullOrEmpty(filename))
            {
                SessionXMLFilename = filename;
            }
        }
    }
}

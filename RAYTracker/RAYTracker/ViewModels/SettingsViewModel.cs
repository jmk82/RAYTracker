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

        private string _tournamentXMLFilename;
        public string TournamentXMLFilename
        {
            get
            {
                return _tournamentXMLFilename;
            }
            set
            {
                _tournamentXMLFilename = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand ChooseSessionFileCommand { get; set; }
        public RelayCommand ChooseTournamentFileCommand { get; set; }

        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public SettingsViewModel(IOpenFileDialogService openFileDialogService)
        {
            _openFileDialogService = openFileDialogService;
            SessionXMLFilename = Properties.Settings.Default.SessionXMLFilename;
            TournamentXMLFilename = Properties.Settings.Default.TournamentXMLFilename;

            ChooseSessionFileCommand = new RelayCommand(ChooseSessionFile);
            ChooseTournamentFileCommand = new RelayCommand(ChooseTournamentFile);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            SessionXMLFilename = Properties.Settings.Default.SessionXMLFilename;
            TournamentXMLFilename = Properties.Settings.Default.TournamentXMLFilename;
            Messenger.Default.Send(new NotificationMessage("CloseSettingsWindow"));
        }

        private void SaveSettings()
        {
            if (!string.IsNullOrEmpty(SessionXMLFilename))
            {
                Properties.Settings.Default.SessionXMLFilename = SessionXMLFilename;
                Properties.Settings.Default.Save();
            }

            if (!string.IsNullOrEmpty(TournamentXMLFilename))
            {
                Properties.Settings.Default.TournamentXMLFilename = TournamentXMLFilename;
                Properties.Settings.Default.Save();
            }

            Messenger.Default.Send(new NotificationMessage("CloseSettingsWindow"));
        }

        private void ChooseSessionFile()
        {
            var filename = _openFileDialogService.ShowOpenFileDialog();

            if (!string.IsNullOrEmpty(filename))
            {
                SessionXMLFilename = filename;
            }
        }

        private void ChooseTournamentFile()
        {
            var filename = _openFileDialogService.ShowOpenFileDialog();

            if (!string.IsNullOrEmpty(filename))
            {
                TournamentXMLFilename = filename;
            }
        }
    }
}

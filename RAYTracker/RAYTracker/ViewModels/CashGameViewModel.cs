using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RAYTracker.Dialogs;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Repository;
using RAYTracker.Domain.Utils;
using System;
using System.Collections.Generic;

namespace RAYTracker.ViewModels
{
    public class CashGameViewModel : ViewModelBase
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (value != null)
                {
                    _fileName = value;
                    LoadPlayingSessions();
                }
            }
        }

        private string _userSessionId;

        public string UserSessionId
        {
            get { return _userSessionId; }
            set
            {
                _userSessionId = value;
                RaisePropertyChanged(nameof(UserSessionId));
            }
        }

        private void LoadPlayingSessions()
        {
            PlayingSessions = _cashGameService.GetPlayingSessionsFromFile(_fileName);
        }

        private IList<PlayingSession> _playingSessions;

        public IList<PlayingSession> PlayingSessions
        {
            get { return _playingSessions; }
            set
            {
                _playingSessions = value;
                RaisePropertyChanged(nameof(PlayingSessions));
            }
        }

        public IList<Session> Sessions;
        private ICashGameService _cashGameService;
        private ISessionRepository _sessionRepository;
        private IOpenFileDialogService _openFileDialogService;
        private IWaitDialogService _waitDialogService;

        private PlayingSession _selectedPlayingSession;
        

        public PlayingSession SelectedPlayingSession
        {
            get { return _selectedPlayingSession; }
            set
            {
                _selectedPlayingSession = value;
                RaisePropertyChanged(nameof(SelectedPlayingSession));
            }
        }

        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand FetchFromServerCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand SaveSessionsCommand { get; set; }

        public CashGameViewModel(ICashGameService cashGameService,
            ISessionRepository sessionRepository,
            IOpenFileDialogService openFileDialogService,
            IWaitDialogService waitDialogService)
        {
            _cashGameService = cashGameService;
            _sessionRepository = sessionRepository;
            _openFileDialogService = openFileDialogService;
            _waitDialogService = waitDialogService;

            OpenFileCommand = new RelayCommand(OpenFile);
            FetchFromServerCommand = new RelayCommand(FetchFromServer);
            ClearCommand = new RelayCommand(() =>
            {
                UserSessionId = "";
                RaisePropertyChanged(nameof(UserSessionId));
            });
            SaveSessionsCommand = new RelayCommand(SaveSessions);

            var thisDay = DateTime.Now;
            StartDate = new DateTime(thisDay.Year, thisDay.Month, 1);
            EndDate = thisDay;
            UserSessionId = "Liitä wcusersessionid tähän";

            LoadSessions();
        }

        private void LoadSessions()
        {
            _sessionRepository.ReadXml();
            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
        }

        private void SaveSessions()
        {
            _sessionRepository.SaveAsXml();
        }

        private async void FetchFromServer()
        {
            _waitDialogService.ShowWaitDialog();
             var sessions = await _cashGameService.FetchSessionsFromServer(_userSessionId, StartDate, EndDate);
            _sessionRepository.Add(sessions);
            _waitDialogService.CloseWaitDialog();

            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private void OpenFile()
        {
            var fileName = _openFileDialogService.ShowOpenFileDialog();
            
            if (!string.IsNullOrEmpty(fileName))
            {
                var sessions = _cashGameService.GetSessionsFromFile(fileName);
                _sessionRepository.Add(sessions);
            }

            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
        }
    }
}

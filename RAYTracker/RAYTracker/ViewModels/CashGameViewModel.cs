using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Dialogs;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using RAYTracker.Domain.Repository;
using RAYTracker.Domain.Utils;
using RAYTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
                if (_userSessionId != "")
                {
                    Messenger.Default.Send(new UserSessionIdChangedMessage(value, this));
                }
                RaisePropertyChanged();
            }
        }

        private IList<PlayingSession> _playingSessions;

        public IList<PlayingSession> PlayingSessions
        {
            get { return _playingSessions; }
            set
            {
                _playingSessions = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        private IList<Session> _sessions;

        public IList<Session> Sessions
        {
            get { return _sessions; }
            set
            {
                _sessions = value;
                RaisePropertyChanged();
            }
        }

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
                RaisePropertyChanged();
                Console.WriteLine("ssssss");
            }
        }

        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand FetchFromServerCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand SaveSessionsCommand { get; set; }
        public RelayCommand ClearSessionsCommand { get; set; }
        public RelayCommand FilterCommand { get; set; }
        public RelayCommand<bool> ShowSessionsOnlyCommand { get; set; }

        public FilterViewModel FilterViewModel { get; set; }
        private IFilterWindowService _filterWindowService;

        public CashGameViewModel(ICashGameService cashGameService,
            ISessionRepository sessionRepository,
            IOpenFileDialogService openFileDialogService,
            IWaitDialogService waitDialogService,
            IFilterWindowService filterWindowService,
            FilterViewModel filterViewModel)
        {
            _cashGameService = cashGameService;
            _sessionRepository = sessionRepository;
            _openFileDialogService = openFileDialogService;
            _waitDialogService = waitDialogService;
            _filterWindowService = filterWindowService;

            OpenFileCommand = new RelayCommand(OpenFile);
            FetchFromServerCommand = new RelayCommand(FetchFromServer);
            ClearCommand = new RelayCommand(() => UserSessionId = "");
            SaveSessionsCommand = new RelayCommand(SaveSessions);
            ClearSessionsCommand = new RelayCommand(ClearSessions);
            ShowSessionsOnlyCommand = new RelayCommand<bool>(ShowSessionsOnly);

            FilterCommand = new RelayCommand(FilterSessions);
            FilterViewModel = filterViewModel;

            var thisDay = DateTime.Now;
            StartDate = new DateTime(thisDay.Year, thisDay.Month, 1);
            EndDate = thisDay;
            UserSessionId = "Liitä wcusersessionid tähän";

            Messenger.Default.Register<UserSessionIdChangedMessage>(this,
                message =>
                {
                    if (message.Sender != this && message.NewUserSessionId != _userSessionId)
                    {
                        UserSessionId = message.NewUserSessionId;
                    }
                });

            LoadStoredSessions();
        }

        private void ClearSessions()
        {
            _sessionRepository.RemoveAll();
            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
            SelectedPlayingSession = null;
        }

        private void ShowSessionsOnly(bool isChecked)
        {
            if (isChecked)
            { 
                Console.WriteLine("session only, playing sessions: " + PlayingSessions.Count);

                List<Session> sessions = new List<Session>();

                foreach (var playingSession in PlayingSessions)
                {
                    sessions.AddRange(playingSession.Sessions);
                }

                SelectedPlayingSession = new PlayingSession { Sessions = sessions };
            }
        }

        private void LoadStoredSessions()
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

        private void LoadPlayingSessions()
        {
            PlayingSessions = PlayingSession.GroupToPlayingSessions(_cashGameService.GetSessionsFromFile(_fileName));
        }

        private void FilterSessions()
        {
            var gameTypes = _sessionRepository.GetAllGameTypes().OrderByDescending(gt => gt.Name).ToList();
            
            FilterViewModel.SetWrappedGameTypes(gameTypes);

            _filterWindowService.ShowWindow(FilterViewModel);

            var selectedGameTypes = FilterViewModel.GameTypes
                .Where(gt => gt.IsSelected)
                .Select(gt => gt.GameType);
            
            var filter = new CashGameFilter { GameTypes = selectedGameTypes };

            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetFiltered(filter));
        }

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

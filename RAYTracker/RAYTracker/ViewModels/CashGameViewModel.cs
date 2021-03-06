﻿using GalaSoft.MvvmLight;
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
    public sealed class CashGameViewModel : ViewModelBase
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
                Messenger.Default.Send(new NotificationMessage("PlayingSessionsUpdated"));
            }
        }

        private PlayingSession _selectedPlayingSession;

        public PlayingSession SelectedPlayingSession
        {
            get { return _selectedPlayingSession; }
            set
            {
                _selectedPlayingSession = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand FetchFromServerCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }
        public RelayCommand SaveSessionsCommand { get; set; }
        public RelayCommand ClearSessionsCommand { get; set; }
        public RelayCommand FilterCommand { get; set; }
        public RelayCommand<bool> ShowSessionsOnlyCommand { get; set; }

        private ICashGameService _cashGameService;
        private ISessionRepository _sessionRepository;
        private IOpenFileDialogService _openFileDialogService;
        private ISaveFileDialogService _saveFileDialogService;
        private IWaitDialogService _waitDialogService;
        private IFilterWindowService _filterWindowService;
        private IInfoDialogService _infoDialogService;

        public FilterViewModel FilterViewModel { get; set; }
        public CashGameFilter Filter { get; set; }
        public InfoDialogViewModel InfoDialogViewModel { get; set; }

        public CashGameViewModel(ICashGameService cashGameService,
            ISessionRepository sessionRepository,
            IOpenFileDialogService openFileDialogService,
            ISaveFileDialogService saveFileDialogService,
            IWaitDialogService waitDialogService,
            IFilterWindowService filterWindowService,
            IInfoDialogService infoDialogService)
        {
            _cashGameService = cashGameService;
            _sessionRepository = sessionRepository;
            _openFileDialogService = openFileDialogService;
            _saveFileDialogService = saveFileDialogService;
            _waitDialogService = waitDialogService;
            _filterWindowService = filterWindowService;
            _infoDialogService = infoDialogService;

            OpenFileCommand = new RelayCommand(OpenFile);
            FetchFromServerCommand = new RelayCommand(FetchFromServer);
            ClearCommand = new RelayCommand(() => UserSessionId = "");
            SaveSessionsCommand = new RelayCommand(SaveSessions);
            ClearSessionsCommand = new RelayCommand(ClearSessions);
            ShowSessionsOnlyCommand = new RelayCommand<bool>(ShowSessionsOnly);

            FilterCommand = new RelayCommand(FilterSessions);

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

            if (_sessionRepository.GetAll().Count > 0)
            {
                StartDate = PlayingSessions.Max(s => s.StartTime);
                FilterViewModel = new FilterViewModel(_sessionRepository.GetAll().Min(s => s.StartTime), _sessionRepository.GetAll().Max(s => s.EndTime));
            }
            else
            {
                StartDate = new DateTime(2010, 11, 1);
                FilterViewModel = new FilterViewModel();
            }
            
            EndDate = DateTime.Now;
            Filter = new CashGameFilter();
        }

        private void UpdateFilterDates()
        {
            Filter.StartDate = _sessionRepository.GetAll().Min(s => s.StartTime);
            Filter.EndDate = _sessionRepository.GetAll().Max(s => s.EndTime);
            Filter.GameTypes = _sessionRepository.GetAllGameTypes();

            FilterViewModel.StartDate = Filter.StartDate;
            FilterViewModel.OriginalStartDate = Filter.StartDate;

            FilterViewModel.EndDate = Filter.EndDate;
            FilterViewModel.OriginalEndDate = Filter.EndDate;
        }

        private void ClearSessions()
        {
            bool confirmed = false;
            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "InfoDialogConfirmed") confirmed = true;
            });

            _infoDialogService.ShowDialog(new InfoDialogViewModel("Haluatko varmasti tyhjentää kaikki käteispelitiedot?", showCancelButton: true));

            Messenger.Default.Unregister<NotificationMessage>(this);

            if (confirmed)
            {
                _sessionRepository.RemoveAll();
                PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
                SelectedPlayingSession = null;
                _infoDialogService.ShowDialog(new InfoDialogViewModel("Käteissessiot tyhjennetty"));
            }
        }

        private void ShowSessionsOnly(bool isChecked = true)
        {
            if (isChecked)
            {
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
            var settings = new UserSettings();

            try
            {
                var sessions = _sessionRepository.ReadXml(settings.SessionXMLFilename);
                _sessionRepository.Add(sessions);
            }
            catch (InvalidOperationException)
            {

            }

            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
        }

        private void SaveSessions()
        {
            var settings = new UserSettings();
            var filename = _saveFileDialogService.ShowSaveFileDialog();
            if (!string.IsNullOrEmpty(filename))
            {
                _sessionRepository.SaveAsXml(filename);
                _infoDialogService.ShowDialog(new InfoDialogViewModel("Käteissessiot tallennettu tiedostoon " + filename));
            }
        }

        private async void FetchFromServer()
        {
            _waitDialogService.ShowWaitDialog();

            string message = "";
            var settings = new UserSettings();

            try
            {
                var sessions = await _cashGameService.FetchSessionsFromServer(_userSessionId, StartDate, EndDate);
                var newSessions = _sessionRepository.Add(sessions);

                message += "Palvelimelta haettu " + sessions.Count + " sessiota, joista uusia " + newSessions;

                if (settings.SaveAutomaticallyAfterFetch && newSessions > 0)
                {
                    _sessionRepository.SaveAsXml(settings.SessionXMLFilename);
                    message += "\nTiedosto " + settings.SessionXMLFilename + " päivitetty.";
                }
            }
            catch (Exception ex)
            {
                message += ex.Message;
            }
            
            _waitDialogService.CloseWaitDialog();

            _infoDialogService.ShowDialog(new InfoDialogViewModel(message));

            PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
        }

        private void LoadPlayingSessions()
        {
            PlayingSessions = PlayingSession.GroupToPlayingSessions(_cashGameService.GetSessionsFromFile(_fileName));
        }

        private void FilterSessions()
        {
            if (_sessionRepository.GetAll().Count == 0) return;

            var gameTypes = _sessionRepository.GetAllGameTypes().OrderByDescending(gt => gt.Name).ToList();

            FilterViewModel.SetWrappedGameTypes(gameTypes);
            FilterViewModel.SetFilter(Filter);
            if (FilterViewModel.StartDate == null && FilterViewModel.EndDate == null)
            {
                UpdateFilterDates();
            }

            _filterWindowService.ShowWindow(FilterViewModel);

            var selectedGameTypes = FilterViewModel.GameTypes
                .Where(gt => gt.IsSelected)
                .Select(gt => gt.GameType);

            Filter = new CashGameFilter
            {
                GameTypes = new List<GameType>(selectedGameTypes),
                StartDate = FilterViewModel.StartDate,
                EndDate = FilterViewModel.EndDate
            };

            PlayingSessions = _sessionRepository.GetFilteredPlayingSessions(Filter);
            ShowSessionsOnly();
        }

        private void OpenFile()
        {
            var fileName = _openFileDialogService.ShowOpenFileDialog();
            var message = "";

            if (!string.IsNullOrEmpty(fileName))
            {
                bool xmlImportSuccess = false;
                IList<Session> sessions = new List<Session>();
                int sessionsAdded = 0;

                try
                {
                    sessions = _sessionRepository.ReadXml(fileName);
                    sessionsAdded = _sessionRepository.Add(sessions);
                    xmlImportSuccess = true;
                }
                catch (InvalidOperationException)
                {
                }

                // ei ollut xml-tiedosto, yritetään lukea tekstitiedostona
                if (!xmlImportSuccess)
                {
                    sessions = _cashGameService.GetSessionsFromFile(fileName);
                    sessionsAdded = _sessionRepository.Add(sessions);
                }

                message += "Tiedostosta löytyi " + sessions.Count() + " sessiota, joista uusia " + sessionsAdded;
                _infoDialogService.ShowDialog(new InfoDialogViewModel(message));

                PlayingSessions = PlayingSession.GroupToPlayingSessions(_sessionRepository.GetAll());
                UpdateFilterDates();
            }
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Dialogs;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Repository;
using RAYTracker.Domain.Utils;
using RAYTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.ViewModels
{
    public sealed class TournamentViewModel : ViewModelBase
    {
        private ITournamentService _tournamentService;
        private ITournamentRepository _tournamentRepository;
        private IOpenFileDialogService _openFileDialogService;
        private IWaitDialogService _waitDialogService;
        private IInfoDialogService _infoDialogService;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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

        public RelayCommand FetchFromServerCommand { get; set; }
        public RelayCommand SaveTournamentsCommand { get; set; }
        public RelayCommand ClearTournamentsCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }

        private IList<Tournament> _tournaments;

        public IList<Tournament> Tournaments
        {
            get { return _tournaments; }
            set
            {
                _tournaments = value;
                RaisePropertyChanged();
                CalculateStats();
            }
        }

        private string _stats;

        public string Stats
        {
            get { return _stats; }
            set
            {
                _stats = value;
                RaisePropertyChanged();
            }
        }

        public TournamentViewModel(ITournamentService tournamentService,
            ITournamentRepository tournamentRepository,
            IOpenFileDialogService openFileDialogService,
            IWaitDialogService waitDialogService,
            IInfoDialogService infoDialogService)
        {
            _tournamentService = tournamentService;
            _tournamentRepository = tournamentRepository;
            _openFileDialogService = openFileDialogService;
            _waitDialogService = waitDialogService;
            _infoDialogService = infoDialogService;

            FetchFromServerCommand = new RelayCommand(FetchFromServer);
            ClearTournamentsCommand = new RelayCommand(ClearTournaments);
            ClearCommand = new RelayCommand(() => UserSessionId = "");
            SaveTournamentsCommand = new RelayCommand(SaveTournaments);

            UserSessionId = "Liitä wcusersessionid tähän";

            Messenger.Default.Register<UserSessionIdChangedMessage>(this,
                message =>
                {
                    if (message.Sender != this && message.NewUserSessionId != _userSessionId)
                    {
                        UserSessionId = message.NewUserSessionId;
                    }
                });

            LoadStoredTournaments();

            if (_tournamentRepository.GetAll().Count > 0)
            {
                StartDate = Tournaments.Max(t => t.StartTime);
            }
            else
            {
                StartDate = new DateTime(2010, 11, 1);
            }
            
            EndDate = DateTime.Now;
        }

        private void LoadStoredTournaments()
        {
            _tournamentRepository.ReadXml();
            Tournaments = _tournamentRepository.GetAll();
        }

        private void SaveTournaments()
        {
            var filename = _tournamentRepository.SaveAsXml();

            _infoDialogService.ShowInfoDialog(new InfoDialogViewModel("Turnaukset tallennettu tiedostoon " + filename));
        }

        private void ClearTournaments()
        {
            _tournamentRepository.RemoveAll();
            Tournaments = _tournamentRepository.GetAll();
        }

        private async void FetchFromServer()
        {
            _waitDialogService.ShowWaitDialog();
            var tournaments = await _tournamentService.FetchTournamentsFromServerAsync(_userSessionId, StartDate, EndDate);
            _tournamentRepository.Add(tournaments);
            _waitDialogService.CloseWaitDialog();

            Tournaments = _tournamentRepository.GetAll();
        }

        private void CalculateStats()
        {
            if (Tournaments.Count == 0)
            {
                Stats = "Ei turnauksia";
                return;
            }

            var buyIns = Tournaments.Sum(t => t.TotalBuyIn);
            var winnings = Tournaments.Sum(t => t.Win);
            var average = Tournaments.Average(t => t.TotalBuyIn);

            var roi = (double)(winnings - buyIns) / (double)buyIns * 100.0;

            Stats = "Turnauksia pelattu: " + Tournaments.Count + " kpl";
            Stats += "\nKeskimääräinen sisäänosto (sis. uudelleenostot): " + average.ToString("0.00") + " €";
            Stats += "\nSisäänostot: \t" + buyIns + " €\nVoitot: \t\t" + winnings + " €";
            Stats += "\nROI: \t\t" + roi.ToString("0.00") + " %";
            Stats += "\nTulos: \t\t" + (winnings - buyIns) + " €";
            RaisePropertyChanged("Stats");
        }
    }
}

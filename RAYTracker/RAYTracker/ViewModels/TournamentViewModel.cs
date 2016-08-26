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
    public class TournamentViewModel : ViewModelBase
    {
        private ITournamentService _tournamentService;
        private ITournamentRepository _tournamentRepository;
        private IOpenFileDialogService _openFileDialogService;
        private IWaitDialogService _waitDialogService;

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

        public string Stats { get; set; }

        public TournamentViewModel(ITournamentService tournamentService,
            ITournamentRepository tournamentRepository,
            IOpenFileDialogService openFileDialogService,
            IWaitDialogService waitDialogService)
        {
            _tournamentService = tournamentService;
            _tournamentRepository = tournamentRepository;
            _openFileDialogService = openFileDialogService;
            _waitDialogService = waitDialogService;

            FetchFromServerCommand = new RelayCommand(FetchFromServer);
            ClearTournamentsCommand = new RelayCommand(ClearTournaments);
            ClearCommand = new RelayCommand(() => UserSessionId = "");

            var thisDay = DateTime.Now;
            StartDate = new DateTime(thisDay.Year, 7, 1);
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

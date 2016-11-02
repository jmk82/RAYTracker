using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using RAYTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.ViewModels
{
    public sealed class FilterViewModel : ViewModelBase
    {
        private IList<GameTypeWrapper> _gameTypes;

        public IList<GameTypeWrapper> GameTypes
        {
            get { return _gameTypes; }
            set
            {
                _gameTypes = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? OriginalStartDate { get; set; }
        public DateTime? OriginalEndDate { get; set; }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? _startDate;
        private DateTime? _endDate;

        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand SelectAllGamesCommand { get; set; }
        public RelayCommand ClearGameSelectionsCommand { get; set; }
        public RelayCommand ResetDatesCommand { get; set; }

        public FilterViewModel()
        {
            GameTypes = new List<GameTypeWrapper>();
            
            CloseWindowCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CloseFilterWindow"));
            });

            SelectAllGamesCommand = new RelayCommand(() => ChangeGameSelection(true));
            ClearGameSelectionsCommand = new RelayCommand(() => ChangeGameSelection(false));
            ResetDatesCommand = new RelayCommand(ResetDates);
        }

        [PreferredConstructor]
        public FilterViewModel(DateTime originalStart, DateTime originalEnd) : this()
        {
            OriginalStartDate = originalStart;
            OriginalEndDate = originalEnd;
        }

        private void ResetDates()
        {
            StartDate = OriginalStartDate;
            EndDate = OriginalEndDate;
        }

        public void ChangeGameSelection(bool selectAll)
        {
            List<GameTypeWrapper> newGameTypes = new List<GameTypeWrapper>();

            foreach (var gameTypeWrapper in GameTypes)
            {
                newGameTypes.Add(new GameTypeWrapper { GameType = gameTypeWrapper.GameType, IsSelected = selectAll });
            }

            GameTypes = newGameTypes;
        }

        public void SetWrappedGameTypes(IList<GameType> gameTypes)
        {
            GameTypes.Clear();

            foreach (var gameType in gameTypes)
            {
                GameTypes.Add(new GameTypeWrapper { GameType = gameType, IsSelected = true });
            }
        }

        public void SetFilter(CashGameFilter filter)
        {
            if (filter.GameTypes == null) return;

            foreach (var gameType in GameTypes)
            {
                if (filter.GameTypes.Contains(gameType.GameType))
                {
                    gameType.IsSelected = true;
                }
                else
                {
                    gameType.IsSelected = false;
                }
            }

            if (filter.StartDate != null) StartDate = filter.StartDate.Value;
            if (filter.EndDate != null) EndDate = filter.EndDate.Value;
        }
    }
}

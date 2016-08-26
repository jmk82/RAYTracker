using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Domain.Model;
using RAYTracker.Helpers;
using System;
using System.Collections.Generic;

namespace RAYTracker.ViewModels
{
    public class FilterViewModel : ViewModelBase
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

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand SelectAllGamesCommand { get; set; }
        public RelayCommand ClearGameSelectionsCommand { get; set; }

        public FilterViewModel()
        {
            GameTypes = new List<GameTypeWrapper>();
            
            CloseWindowCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CloseFilterWindow"));
            });

            SelectAllGamesCommand = new RelayCommand(() =>
            {
                List<GameTypeWrapper> newGameTypes = new List<GameTypeWrapper>();

                foreach (var gameTypeWrapper in GameTypes)
                {
                    newGameTypes.Add(new GameTypeWrapper { GameType = gameTypeWrapper.GameType, IsSelected = true });
                }

                GameTypes = newGameTypes;
            });

            ClearGameSelectionsCommand = new RelayCommand(() =>
            {
                List<GameTypeWrapper> newGameTypes = new List<GameTypeWrapper>();

                foreach (var gameTypeWrapper in GameTypes)
                {
                    newGameTypes.Add(new GameTypeWrapper { GameType = gameTypeWrapper.GameType, IsSelected = false });
                }

                GameTypes = newGameTypes;
            });
        }

        public void SetWrappedGameTypes(IList<GameType> gameTypes)
        {
            GameTypes.Clear();

            foreach (var gameType in gameTypes)
            {
                GameTypes.Add(new GameTypeWrapper { GameType = gameType, IsSelected = true });
            }
        }
    }
}

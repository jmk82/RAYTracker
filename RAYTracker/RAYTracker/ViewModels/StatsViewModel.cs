using System.Collections;
using GalaSoft.MvvmLight;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;

namespace RAYTracker.ViewModels
{
    public sealed class StatsViewModel: ViewModelBase
    {
        private IEnumerable<GameTypeReport> _reportsByGameType;

        private bool _separateTurbosAndAntes;

        public bool SeparateTurbosAndAntes
        {
            get { return _separateTurbosAndAntes; }
            set
            {
                _separateTurbosAndAntes = value;
                RaisePropertyChanged();
                Messenger.Default.Send(new NotificationMessage("PlayingSessionsUpdated"));
            }
        }


        public IEnumerable<GameTypeReport> ReportsByGameType
        {
            get { return _reportsByGameType; }
            set
            {
                _reportsByGameType = value;
                RaisePropertyChanged();
            }
        }

        public StatsViewModel()
        {
            ReportsByGameType = new List<GameTypeReport>();
        }

        public void GenerateReport(IList<PlayingSession> playingSessions)
        {
            var sessions = playingSessions.SelectMany(s => s.Sessions).ToList();
            ReportsByGameType = Reporter.GameTypeReport(sessions, _separateTurbosAndAntes);
        }
    }
}

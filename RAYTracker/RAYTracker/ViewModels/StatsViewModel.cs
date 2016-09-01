using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.ViewModels
{
    public sealed class StatsViewModel: ViewModelBase
    {
        private IEnumerable<GameTypeReport> _reportByGameType;
        private IEnumerable<DailyReport> _dailyReport;
        private IEnumerable<MonthlyReport> _monthlyReport;
        private IEnumerable<YearlyReport> _yearlyReport;
        private bool _separateTurbosAndAntes;

        public IEnumerable<GameTypeReport> ReportByGameType
        {
            get { return _reportByGameType; }
            set
            {
                _reportByGameType = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<DailyReport> DailyReport
        {
            get { return _dailyReport; }
            set
            {
                _dailyReport = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<MonthlyReport> MonthlyReport
        {
            get { return _monthlyReport; }
            set
            {
                _monthlyReport = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<YearlyReport> YearlyReport
        {
            get { return _yearlyReport; }
            set
            {
                _yearlyReport = value;
                RaisePropertyChanged();
            }
        }

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

        public StatsViewModel()
        {
            ReportByGameType = new List<GameTypeReport>();
        }

        public void GenerateReports(IList<PlayingSession> playingSessions)
        {
            var sessions = playingSessions.SelectMany(s => s.Sessions).ToList();
            ReportByGameType = Reporter.GameTypeReport(sessions, _separateTurbosAndAntes);
            DailyReport = Reporter.DailyReport(sessions);
            MonthlyReport = Reporter.MonthlyReport(sessions);
            YearlyReport = Reporter.YearlyReport(sessions);
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;

namespace RAYTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public CashGameViewModel CashGameViewModel { get; set; }
        public ReportViewModel ReportViewModel { get; set; }
        public TournamentViewModel TournamentViewModel { get; set; }
        public StatsViewModel StatsViewModel { get; set; }

        public RelayCommand CreateReportCommand { get; set; }

        public MainViewModel(CashGameViewModel cashGameVM, ReportViewModel reportVM, TournamentViewModel tournamentViewModel, StatsViewModel statsViewModel)
        {
            CashGameViewModel = cashGameVM;
            ReportViewModel = reportVM;
            TournamentViewModel = tournamentViewModel;
            StatsViewModel = statsViewModel;

            CreateReportCommand = new RelayCommand(CreateCashReport);

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "PlayingSessionsUpdated")
                {
                    CreateCashReport();
                    CreateStatsReport();
                }
            });

            CreateCashReport();
            CreateStatsReport();
        }

        private void CreateStatsReport()
        {
            StatsViewModel.GenerateReports(CashGameViewModel.PlayingSessions);
        }

        public void CreateCashReport()
        {
            var sessions = new List<Session>();
            foreach (var playingSession in CashGameViewModel.PlayingSessions)
            {
                sessions.AddRange(playingSession.Sessions);
            }

            ReportViewModel.Report = Reporter.GetSimpleSessionTotalReport(sessions, CashGameViewModel.PlayingSessions);
        }
    }
}

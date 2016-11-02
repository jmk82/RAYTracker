using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;
using System.Windows;
using System;
using RAYTracker.Dialogs;

namespace RAYTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public CashGameViewModel CashGameViewModel { get; set; }
        public ReportViewModel ReportViewModel { get; set; }
        public TournamentViewModel TournamentViewModel { get; set; }
        public StatsViewModel StatsViewModel { get; set; }
        private SettingsViewModel _settingsViewModel;
        private ISettingsWindowService _settingsWindowService { get; set; }

        public RelayCommand CreateReportCommand { get; set; }
        public RelayCommand ExitApplicationCommand { get; set; }
        public RelayCommand ShowSettingsDialog { get; set; }

        public MainViewModel(CashGameViewModel cashGameVM, ReportViewModel reportVM, TournamentViewModel tournamentViewModel, StatsViewModel statsViewModel,
            SettingsViewModel settingsViewModel, ISettingsWindowService settingsWindowService)
        {
            CashGameViewModel = cashGameVM;
            ReportViewModel = reportVM;
            TournamentViewModel = tournamentViewModel;
            StatsViewModel = statsViewModel;
            _settingsWindowService = settingsWindowService;
            _settingsViewModel = settingsViewModel;

            CreateReportCommand = new RelayCommand(CreateCashReport);

            ExitApplicationCommand = new RelayCommand(() => Application.Current.Shutdown());

            ShowSettingsDialog = new RelayCommand(ShowSettings);

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

        private void ShowSettings()
        {
            _settingsViewModel.LoadSettings();
            _settingsWindowService.showWindow(_settingsViewModel);
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

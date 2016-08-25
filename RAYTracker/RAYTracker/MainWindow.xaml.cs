using Microsoft.Win32;
using RAYTracker.Domain;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;

namespace RAYTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly Program _program;

        public MainWindow()
        {
            InitializeComponent();

            //_program = new Program();
        }

        //private void openFileButton_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        _program.Filename = openFileDialog.FileName;
        //        FileNameTextBox.Text = "Avattu tiedosto: " + openFileDialog.FileName;
        //        ImportButton.IsEnabled = true;
        //    }
        //}

        //private void importButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _program.ImportFromFile();
        //    PlayingSessionDataGrid.ItemsSource = _program.PlayingSessions;
        //}

        //private void sessionDataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        //{
        //    var selectedSession = (PlayingSession)PlayingSessionDataGrid.CurrentItem;

        //    if (selectedSession != null)
        //    {
        //        SessionDataGrid.ItemsSource = selectedSession.Sessions;
        //    }
        //}

        //private void userSessionIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    UserSessionIdTextBox.Clear();
        //}

        //private async void fetchFromServerbutton_Click(object sender, RoutedEventArgs e)
        //{
        //    var sessionId = UserSessionIdTextBox.Text.Trim();

        //    if (sessionId.Length != 32)
        //    {
        //        MessageBox.Show("Virheellinen wcusersessionid!");
        //        return;
        //    }

        //    IList<Session> tableSessions;
        //    IList<PlayingSession> sessions;

        //    try
        //    {
        //        tableSessions = await _program.FetchSessionsFromServer(sessionId, StartDatePicker.Text, EndDatePicker.Text);
        //        sessions = PlayingSession.GroupToPlayingSessions(tableSessions);
        //        _program.PlayingSessions = sessions;
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return;
        //    }

        //    PlayingSessionDataGrid.ItemsSource = sessions;

        //    string message = Reporter.GetSimpleSessionTotalReport(tableSessions, sessions);

        //    MessageBox.Show(message);
        //}

        //private void TimeReportButton_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (var column in ReportDataGrid.Columns)
        //    {
        //        column.Visibility = Visibility.Hidden;
        //    }

        //    ReportDataGrid.Columns[3].Visibility = Visibility.Visible;
        //    ReportDataGrid.Columns[4].Visibility = Visibility.Visible;

        //    if (TimeReportComboBox.SelectedIndex == 0)
        //    {
        //        var data = Reporter.DailyReport(_program.Sessions);
        //        ReportDataGrid.Columns[0].Visibility = Visibility.Visible;
        //        ReportDataGrid.ItemsSource = data;
        //    }
        //    else if (TimeReportComboBox.SelectedIndex == 1)
        //    {
        //        var data = Reporter.MonthlyReport(_program.Sessions);
        //        ReportDataGrid.Columns[1].Visibility = Visibility.Visible;
        //        ReportDataGrid.ItemsSource = data;
        //    }

        //    else if (TimeReportComboBox.SelectedIndex == 2)
        //    {
        //        var data = Reporter.YearlyReport(_program.Sessions);
        //        ReportDataGrid.Columns[2].Visibility = Visibility.Visible;
        //        ReportDataGrid.ItemsSource = data;
        //    }
        //}

        //private void ReportByGameTypeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    foreach (var column in ReportDataGrid.Columns)
        //    {
        //        column.Visibility = Visibility.Hidden;
        //    }

        //    ReportDataGrid.Columns[3].Visibility = Visibility.Visible;
        //    ReportDataGrid.Columns[4].Visibility = Visibility.Visible;

        //    var data = Reporter.GameTypeReport(_program.Sessions);
        //    ReportDataGrid.Columns[5].Visibility = Visibility.Visible;
        //    ReportDataGrid.Columns[6].Visibility = Visibility.Visible;
        //    ReportDataGrid.Columns[5].DisplayIndex = 0;
        //    ReportDataGrid.ItemsSource = data;
        //}

        //private void UserSessionIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (UserSessionIdTextBox.Text.Length == 32)
        //    {
        //        FetchFromServerbutton.IsEnabled = true;
        //        FetchFromServerbutton.IsDefault = true;
        //    }
        //}
    }
}

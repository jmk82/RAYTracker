using RAYTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RAYTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Program _program;

        public MainWindow()
        {
            InitializeComponent();

            _program = new Program();
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            _program.OpenFile();
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            _program.ImportFromFile();
            SessionDataGrid.ItemsSource = _program.Sessions;
        }

        private void sessionDataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            var selectedSession = (Session)SessionDataGrid.CurrentItem;

            if (selectedSession != null)
            {
                TableSessionDataGrid.ItemsSource = selectedSession.TableSessions;
            }
        }

        private void userSessionIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UserSessionIdTextBox.Clear();
        }

        private void fetchFromServerbutton_Click(object sender, RoutedEventArgs e)
        {
            var sessionId = UserSessionIdTextBox.Text.Trim();

            if (sessionId.Length != 32)
            {
                MessageBox.Show("Virheellinen wcusersessionid!");
                return;
            }

            IList<TableSession> tableSessions;
            IList<Session> sessions;

            try
            {
                tableSessions = _program.FetchTableSessionsFromServer(sessionId, StartDatePicker.Text, EndDatePicker.Text);
                sessions = new SessionGenerator().GroupToSessions(tableSessions);
                _program.Sessions = sessions;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            SessionDataGrid.ItemsSource = sessions;

            string message = Reporter.GetSimpleSessionTotalReport(tableSessions, sessions);

            MessageBox.Show(message);
        }

        private void TimeReportButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var column in ReportDataGrid.Columns)
            {
                column.Visibility = Visibility.Hidden;
            }

            ReportDataGrid.Columns[3].Visibility = Visibility.Visible;
            ReportDataGrid.Columns[4].Visibility = Visibility.Visible;

            if (TimeReportComboBox.SelectedIndex == 0)
            {
                var data = Reporter.DailyReport(_program.TableSessions);
                ReportDataGrid.Columns[0].Visibility = Visibility.Visible;
                ReportDataGrid.ItemsSource = data;
            }
            else if (TimeReportComboBox.SelectedIndex == 1)
            {
                var data = Reporter.MonthlyReport(_program.TableSessions);
                ReportDataGrid.Columns[1].Visibility = Visibility.Visible;
                ReportDataGrid.ItemsSource = data;
            }

            else if (TimeReportComboBox.SelectedIndex == 2)
            {
                var data = Reporter.YearlyReport(_program.TableSessions);
                ReportDataGrid.Columns[2].Visibility = Visibility.Visible;
                ReportDataGrid.ItemsSource = data;
            }
        }

        private void ReportByGameTypeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var column in ReportDataGrid.Columns)
            {
                column.Visibility = Visibility.Hidden;
            }

            ReportDataGrid.Columns[3].Visibility = Visibility.Visible;
            ReportDataGrid.Columns[4].Visibility = Visibility.Visible;

            var data = Reporter.GameTypeReport(_program.TableSessions);
            ReportDataGrid.Columns[5].Visibility = Visibility.Visible;
            ReportDataGrid.Columns[6].Visibility = Visibility.Visible;
            ReportDataGrid.Columns[5].DisplayIndex = 0;
            ReportDataGrid.ItemsSource = data;
        }
    }
}

using RAYTracker.Model;
using System;
using System.Collections.Generic;
using System.Windows;

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
    }
}

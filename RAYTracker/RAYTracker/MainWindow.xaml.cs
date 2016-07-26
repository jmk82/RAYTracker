using System;
using System.Linq;
using System.Windows;

namespace RAYTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Program p;
        private SessionViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            p = new Program();
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            p.OpenFile();
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            p.Import();

            sessionDataGrid.ItemsSource = p.GetSessions();
            //_viewModel = new SessionViewModel(p);
            //DataContext = _viewModel;
            
        }

        private void sessionDataGrid_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            var selectedSession = (Session)sessionDataGrid.CurrentItem;

            if (selectedSession != null)
            {
                tableSessionDataGrid.ItemsSource = selectedSession.TableSessions;
            }
        }

        private void tableSessionDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // TODO: show session detail view here somehow
            MessageBox.Show("Clicked: " + (TableSession) tableSessionDataGrid.CurrentItem);
        }

        private void userSessionIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            userSessionIdTextBox.Clear();
        }

        private void fetchFromServerbutton_Click(object sender, RoutedEventArgs e)
        {
            var sessionId = userSessionIdTextBox.Text.Trim();
            if (sessionId.Length != 32)
            {
                MessageBox.Show("Not a valid length for wcusersessionid!");
                return;
            }
            DataFetcher fetcher = new DataFetcher(sessionId);
            if (startDatePicker.Text != "")
            {
                var startDateTokens = startDatePicker.Text.Split('.');
                var startDate = startDateTokens[2] + "-" + startDateTokens[1] + "-" + startDateTokens[0];
                fetcher.StartDate = startDate;
            }


            if (endDatePicker.Text != "")
            {
                var endDateTokens = endDatePicker.Text.Split('.');
                var endDate = endDateTokens[2] + "-" + endDateTokens[1] + "-" + endDateTokens[0];
                fetcher.EndDate = endDate; 
            }

            FetchedDataParser fp = new FetchedDataParser(fetcher);

            var tableSessions = fp.ParseTableSessions(fp.GetFetchedDataLines());

            if (tableSessions == null)
            {
                return;
            }

            SessionImporter importer = new SessionImporter();
            var sessions = importer.CreateSessions(tableSessions);

            sessionDataGrid.ItemsSource = sessions;

            var result = tableSessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
            var timePlayed = sessions.Sum(s => s.Duration.TotalMinutes);

            string message = tableSessions.Count + " table sessions imported!\nFound total " + sessions.Count +
                             " playing sessions.";
            message += "\nTotal result: " + result + " €";
            message += "\nTime played: " + (timePlayed / 60.0).ToString("N2") + " hours";
            message += "\nHourly rate: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " €/h";
            message += "\nTotal hands played: " + tableSessions.Sum(t => t.HandsPlayed);

            MessageBox.Show(message);

            //MessageBox.Show("Created table sessions: " + tableSessions.Count);
        }
    }
}

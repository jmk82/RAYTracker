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
    }
}

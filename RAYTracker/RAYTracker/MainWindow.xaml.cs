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

            _viewModel = new SessionViewModel(p);
            DataContext = _viewModel;
        }
    }
}

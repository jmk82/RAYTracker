using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace RAYTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
                _fileName = openFileDialog.FileName;
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            Reader reader = new Reader(_fileName);
            var lines = reader.GetAllLinesAsStrings();
            Parser parser = new Parser(reader);

            var tableSessions = new List<TableSession>();

            foreach (var line in lines)
            {
                tableSessions.Add(parser.CreateTableSession(parser.ParseLine(line)));
            }

            SessionImporter importer = new SessionImporter();
            var sessions = importer.CreateSessions(tableSessions);

            string message = tableSessions.Count + " table sessions imported!\nFound total " + sessions.Count +
                             " playing sessions.";

            var result = tableSessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
            var timePlayed = sessions.Sum(s => s.Duration);

            message += "\nResult: " + result;
            message += "\nTime played: " + timePlayed / 60.0 + " hours";
            message += "\nHourly rate: " + (double)result /(timePlayed/60.0) + " €/h";

            MessageBox.Show(message);
        }
    }
}

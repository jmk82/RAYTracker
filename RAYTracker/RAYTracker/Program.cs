using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows;

namespace RAYTracker
{
    public class Program
    {
        private string _filename;
        private MainWindow mw = (MainWindow) Application.Current.MainWindow;

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
                _filename = openFileDialog.FileName;
                mw.textBox.Text = "Opened file(s): " + openFileDialog.FileName;
        }

        public void Import()
        {
            Reader reader = new Reader(_filename);
            Parser parser = new Parser(reader);

            var lines = reader.GetAllLinesAsStrings();
            var tableSessions = new List<TableSession>();

            var start = DateTime.Now;

            foreach (var line in lines)
            {
                tableSessions.Add(parser.CreateTableSession(parser.ParseLine(line)));
            }

            SessionImporter importer = new SessionImporter();
            var sessions = importer.CreateSessions(tableSessions);

            var result = tableSessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
            var timePlayed = sessions.Sum(s => s.Duration);

            var stop = DateTime.Now;

            string message = tableSessions.Count + " table sessions imported!\nFound total " + sessions.Count +
                             " playing sessions.";
            message += "\nResult: " + result;
            message += "\nTime played: " + timePlayed / 60.0 + " hours";
            message += "\nHourly rate: " + (double)result / (timePlayed / 60.0) + " €/h";
            message += "\nTime elapsed: " + (stop - start).TotalMilliseconds + " ms";

            MessageBox.Show(message);
        }
    }
}

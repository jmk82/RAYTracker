using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RAYTracker
{
    public class Program
    {
        private string _filename;
        private MainWindow mw = (MainWindow) Application.Current.MainWindow;
        public List<TableSession> TableSessions;
        public IList<Session> Sessions { get; set; }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
                _filename = openFileDialog.FileName;
                mw.textBox.Text = "Opened file(s): " + openFileDialog.FileName;
        }

        public IList<Session> GetSessions()
        {
            return Sessions;
        }

        public void Import()
        {
            Reader reader = new Reader(_filename);
            Parser parser = new Parser(reader);

            var lines = reader.GetAllLinesAsStrings();
            TableSessions = new List<TableSession>();

            var start = DateTime.Now;

            foreach (var line in lines)
            {
                TableSessions.Add(parser.CreateTableSession(parser.ParseLine(line)));
            }

            SessionImporter importer = new SessionImporter();
            Sessions = importer.CreateSessions(TableSessions);

            var result = TableSessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
            var timePlayed = Sessions.Sum(s => s.Duration.TotalMinutes);

            var stop = DateTime.Now;

            string message = TableSessions.Count + " table sessions imported!\nFound total " + Sessions.Count +
                             " playing sessions.";
            message += "\nResult: " + result;
            message += "\nTime played: " + timePlayed / 60.0 + " hours";
            message += "\nHourly rate: " + (double)result / (timePlayed / 60.0) + " €/h";
            message += "\nTime elapsed: " + (stop - start).TotalMilliseconds + " ms";

            //MessageBox.Show(message);
        }
    }
}

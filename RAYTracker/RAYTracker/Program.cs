using Microsoft.Win32;
using RAYTracker.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RAYTracker
{
    public class Program
    {
        public string Filename { get; set; }
        public IList<TableSession> TableSessions;
        public IList<Session> Sessions { get; set; }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;

            if (openFileDialog.ShowDialog() == true)
            {
                Filename = openFileDialog.FileName;
                mw.FileNameTextBox.Text = "Avattu tiedosto: " + openFileDialog.FileName;
                mw.ImportButton.IsEnabled = true;
            }
        }

        public void ImportFromFile()
        {
            Reader reader = new Reader(Filename);
            FileParser fileParser = new FileParser(reader);

            var lines = reader.GetAllLinesAsStrings();
            TableSessions = new List<TableSession>();

            string errors = "";
            int errorCount = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                try
                {
                    TableSessions.Add(fileParser.CreateTableSession(fileParser.ParseLine(lines[i])));
                }
                catch (SystemException e) when (e is FormatException || e is IndexOutOfRangeException)
                {
                    errors += ("Virhe rivillä " + i + " (" + lines[i] + "). Rivi ohitettu.\n");
                    errorCount++;
                }
            }

            SessionGenerator generator = new SessionGenerator();
            Sessions = generator.GroupToSessions(TableSessions);

            string message = Reporter.GetSimpleSessionTotalReport(TableSessions, Sessions);
            message += "\n\nVirheitä: " + errorCount + "!:\n" + errors;

            MessageBox.Show(message);
        }

        public IList<TableSession> FetchTableSessionsFromServer(string sessionId, string startDate, string endDate)
        {
            DataFetcher fetcher = new DataFetcher(sessionId);

            if (startDate != "")
            {
                var startDateTokens = startDate.Split('.');
                startDate = startDateTokens[2] + "-" + startDateTokens[1] + "-" + startDateTokens[0];
                fetcher.StartDate = startDate;
            }

            if (endDate != "")
            {
                var endDateTokens = endDate.Split('.');
                endDate = endDateTokens[2] + "-" + endDateTokens[1] + "-" + endDateTokens[0];
                fetcher.EndDate = endDate;
            }

            FetchedDataParser fp = new FetchedDataParser(fetcher);

            var tableSessions = fp.ParseTableSessions(fp.GetFetchedDataLines());
            TableSessions = tableSessions;

            return tableSessions;
        }
    }
}

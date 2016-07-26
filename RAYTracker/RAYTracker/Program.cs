using Microsoft.Win32;
using RAYTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RAYTracker
{
    public class Program
    {
        public string Filename { get; set; }
        public List<TableSession> TableSessions;
        public IList<Session> Sessions { get; set; }

        public Program()
        {
            
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;

            if (openFileDialog.ShowDialog() == true)
                Filename = openFileDialog.FileName;
                mw.FileNameTextBox.Text = "Avattu tiedosto: " + openFileDialog.FileName;
        }

        public void ImportFromFile()
        {
            Reader reader = new Reader(Filename);
            FileParser fileParser = new FileParser(reader);

            var lines = reader.GetAllLinesAsStrings();
            TableSessions = new List<TableSession>();

            foreach (var line in lines)
            {
                TableSessions.Add(fileParser.CreateTableSession(fileParser.ParseLine(line)));
            }

            SessionGenerator generator = new SessionGenerator();
            Sessions = generator.GroupToSessions(TableSessions);

            //string message = Reporter.GetSimpleSessionTotalReport(TableSessions, Sessions);

            //MessageBox.Show(message);
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

            return tableSessions;
        }
    }
}

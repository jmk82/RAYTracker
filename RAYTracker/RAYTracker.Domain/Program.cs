using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using RAYTracker.Domain.Utils;

namespace RAYTracker.Domain
{
    public class Program
    {
        public string Filename { get; set; }
        public IList<Session> Sessions;
        public IList<PlayingSession> PlayingSessions { get; set; }

        public void ImportFromFile()
        {
            Reader reader = new Reader(Filename);
            FileParser fileParser = new FileParser(reader);

            var lines = reader.GetAllLinesAsStrings();
            Sessions = new List<Session>();

            string errors = "";
            int errorCount = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                try
                {
                    Sessions.Add(fileParser.CreateTableSession(fileParser.ParseLine(lines[i])));
                }
                catch (SystemException e) when (e is FormatException || e is IndexOutOfRangeException)
                {
                    errors += ("Virhe rivillä " + i + " (" + lines[i] + "). Rivi ohitettu.\n");
                    errorCount++;
                }
            }

            PlayingSessions = PlayingSession.GroupToPlayingSessions(Sessions);

            string message = Reporter.GetSimpleSessionTotalReport(Sessions, PlayingSessions);

            if (errorCount != 0)
            {
                message += "\n\nVirheitä: " + errorCount + "!:\n" + errors; 
            }

            MessageBox.Show(message);
        }

        public async Task<IList<Session>> FetchSessionsFromServer(string sessionId, string startDate, string endDate)
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
            var data = await fetcher.GetCashSessionsAsync();
            DataParser fp = new DataParser(data);

            var sessions = fp.ParseSessions();
            Sessions = sessions;

            return sessions;
        }
    }
}

﻿using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Utils
{
    public class CashGameService : ICashGameService
    {
        public IList<PlayingSession> GetPlayingSessionsFromFile(string fileName)
        {
            Reader reader = new Reader(fileName);
            FileParser fileParser = new FileParser(reader);

            var lines = reader.GetAllLinesAsStrings();
            var sessions = new List<Session>();

            string errors = "";
            int errorCount = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                try
                {
                    sessions.Add(fileParser.CreateTableSession(fileParser.ParseLine(lines[i])));
                }
                catch (SystemException e) when (e is FormatException || e is IndexOutOfRangeException)
                {
                    errors += ("Virhe rivillä " + i + " (" + lines[i] + "). Rivi ohitettu.\n");
                    errorCount++;
                }
            }

            var playingSessions = PlayingSession.GroupToPlayingSessions(sessions);

            return playingSessions;
        }

        public async Task<IList<PlayingSession>> FetchFromServer(string sessionId, DateTime startDate, DateTime endDate)
        {
            DataFetcher fetcher = new DataFetcher(sessionId);

            var startDateString = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
            fetcher.StartDate = startDateString;

            var endDateString = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;
            fetcher.EndDate = endDateString;

            var data = await fetcher.GetCashSessionsAsync();
            DataParser dp = new DataParser(data);

            var sessions = dp.ParseSessions();
            var playingSessions = PlayingSession.GroupToPlayingSessions(sessions);

            return playingSessions;
        }
    }
}

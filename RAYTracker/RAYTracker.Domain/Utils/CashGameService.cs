using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Utils
{
    public class CashGameService : ICashGameService
    {
        public IList<Session> GetSessionsFromFile(string fileName)
        {
            Reader reader = new Reader(fileName);
            FileParser fileParser = new FileParser(reader);

            var lines = reader.GetAllLinesAsStrings();
            var sessions = new List<Session>();

            foreach (string line in lines)
            {
                sessions.Add(fileParser.CreateTableSession(fileParser.ParseLine(line)));
            }
            Console.WriteLine(sessions.Count);
            return sessions;
        }

        public async Task<IList<Session>> FetchSessionsFromServer(string sessionId, DateTime startDate, DateTime endDate)
        {
            DataFetcher fetcher = new DataFetcher(sessionId);

            var startDateString = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
            fetcher.StartDate = startDateString;

            var endDateString = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;
            fetcher.EndDate = endDateString;

            var data = await fetcher.GetCashSessionsAsync();
            DataParser dp = new DataParser(data);

            var sessions = dp.ParseSessions();

            return sessions;
        }
    }
}

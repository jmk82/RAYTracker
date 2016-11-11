using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Utils
{
    public class TournamentService : ITournamentService
    {
        public async Task<IList<Tournament>> FetchTournamentsFromServerAsync(string sessionId, DateTime startDate, DateTime endDate)
        {
            DataFetcher fetcher = new DataFetcher(sessionId);

            var startDateString = startDate.Year + "-" + startDate.Month + "-" + startDate.Day;
            fetcher.StartDate = startDateString;

            var endDateString = endDate.Year + "-" + endDate.Month + "-" + endDate.Day;
            fetcher.EndDate = endDateString;

            var data = await fetcher.GetTournamentsAsync();
            DataParser parser = new DataParser(data);

            var tournaments = parser.ParseTournaments();

            return tournaments;
        }
    }
}

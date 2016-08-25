using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Utils
{
    public interface ITournamentService
    {
        Task<IList<Tournament>> FetchTournamentsFromServerAsync(string sessionId, DateTime startDate, DateTime endDate);
    }
}
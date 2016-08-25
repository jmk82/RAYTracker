using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RAYTracker.Domain.Model;

namespace RAYTracker.Domain.Utils
{
    public interface ICashGameService
    {
        IList<PlayingSession> GetPlayingSessionsFromFile(string fileName);
        Task<IList<PlayingSession>> FetchFromServer(string sessionId, DateTime startDate, DateTime endDate);
    }
}
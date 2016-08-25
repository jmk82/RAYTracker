using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Utils
{
    public interface ICashGameService
    {
        IList<Session> GetSessionsFromFile(string fileName);
        IList<PlayingSession> GetPlayingSessionsFromFile(string fileName);
        Task<IList<Session>> FetchSessionsFromServer(string sessionId, DateTime startDate, DateTime endDate);
        Task<IList<PlayingSession>> FetchFromServer(string sessionId, DateTime startDate, DateTime endDate);
    }
}
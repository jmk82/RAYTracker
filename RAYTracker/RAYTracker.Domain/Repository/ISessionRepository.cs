using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ISessionRepository
    {
        IList<Session> GetAll();
        IList<GameType> GetAllGameTypes();
        IList<Session> GetFilteredSessions(CashGameFilter filter);
        IList<PlayingSession> GetFilteredPlayingSessions(CashGameFilter filter);
        int Add(IList<Session> sessions);
        IList<Session> ReadXml(string filename);
        string SaveAsXml(string filename);
        void RemoveAll();
    }
}
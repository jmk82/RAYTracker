using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ISessionRepository
    {
        IList<Session> GetAll();
        IList<GameType> GetAllGameTypes();
        IList<Session> GetFiltered(CashGameFilter filter);
        int Add(IList<Session> sessions);
        void ReadXml(string filename);
        string SaveAsXml(string filename);
        void RemoveAll();
    }
}
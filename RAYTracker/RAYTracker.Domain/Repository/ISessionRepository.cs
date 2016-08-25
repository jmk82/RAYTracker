using RAYTracker.Domain.Model;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ISessionRepository
    {
        IList<Session> GetAll();
        void Add(IList<Session> sessions);
        void ReadXml();
        void SaveAsXml();
    }
}
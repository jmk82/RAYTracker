using RAYTracker.Domain.Model;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ITournamentRepository
    {
        IList<Tournament> GetAll();
        int Add(IList<Tournament> tournaments);
        void ReadXml();
        string SaveAsXml();
        void RemoveAll();
    }
}

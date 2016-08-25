using RAYTracker.Domain.Model;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ITournamentRepository
    {
        IList<Tournament> GetAll();
        void Add(IList<Tournament> tournaments);
        void ReadXml();
        void SaveAsXml();
        void RemoveAll();
    }
}

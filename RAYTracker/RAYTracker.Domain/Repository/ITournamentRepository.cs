using RAYTracker.Domain.Model;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public interface ITournamentRepository
    {
        IList<Tournament> GetAll();
        int Add(IList<Tournament> tournaments);
        IList<Tournament> ReadXml(string filename);
        string SaveAsXml(string filename);
        void RemoveAll();
    }
}

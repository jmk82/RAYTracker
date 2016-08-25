using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;

namespace RAYTracker.Domain.Repository
{
    public class TournamentRepository : ITournamentRepository
    {
        private IList<Tournament> _tournaments;

        public TournamentRepository()
        {
            _tournaments = new List<Tournament>();
        }

        public IList<Tournament> GetAll()
        {
            return _tournaments;
        }

        public void Add(IList<Tournament> tournaments)
        {
            var counter = 0;
            foreach (var tournament in tournaments)
            {
                // TODO: Add contains check + implement Tournament.Equals!!
                _tournaments.Add(tournament);
                counter++;
            }
            Console.WriteLine("Added " + counter + " tournaments");
        }

        public void ReadXml()
        {
            throw new NotImplementedException();
        }

        public void SaveAsXml()
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            _tournaments.Clear();
        }
    }
}

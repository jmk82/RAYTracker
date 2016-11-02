using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RAYTracker.Domain.Repository
{
    public class TournamentRepository : ITournamentRepository
    {
        private IList<Tournament> _tournaments;
        private IList<string> _tournamentNames;

        public TournamentRepository()
        {
            _tournaments = new List<Tournament>();
            _tournamentNames = new List<string>();
        }

        public IList<Tournament> GetAll()
        {
            return _tournaments;
        }

        public int Add(IList<Tournament> tournaments)
        {
            var addedTournaments = 0;

            foreach (var tournament in tournaments)
            {
                if (!_tournaments.Contains(tournament))
                {
                    _tournaments.Add(tournament);
                    addedTournaments++;

                    if (!_tournamentNames.Contains(tournament.Name))
                    {
                        _tournamentNames.Add(tournament.Name);
                    }
                }
            }

            return addedTournaments;
        }

        public IList<Tournament> ReadXml(string filename)
        {
            XmlSerializer reader = new XmlSerializer(typeof(List<Tournament>));
            IList<Tournament> tournaments = new List<Tournament>();

            try
            {
                StreamReader file = new StreamReader(filename);
                tournaments = (IList<Tournament>)reader.Deserialize(file);
            }
            catch (Exception)
            {
            }

            return tournaments;
        }

        public string SaveAsXml(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return null;

            XmlSerializer writer = new XmlSerializer(typeof(List<Tournament>));
            FileStream file;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                file = File.Create(filename);
            }
            catch (Exception)
            {
                throw;
            }

            writer.Serialize(file, _tournaments.ToList());
            file.Close();

            return file.Name;
        }

        public void RemoveAll()
        {
            _tournaments.Clear();
        }
    }
}

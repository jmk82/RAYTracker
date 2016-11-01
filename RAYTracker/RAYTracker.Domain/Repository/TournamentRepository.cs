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
        private string _xmlFile;

        public TournamentRepository()
        {
            _tournaments = new List<Tournament>();
            _tournamentNames = new List<string>();
            var path = Environment.GetFolderPath((Environment.SpecialFolder.ApplicationData));
            var dirName = @"\RAYTracker";
            Directory.CreateDirectory(path + dirName);
            _xmlFile = path + dirName + "//TournamentData.xml";
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

        public void ReadXml()
        {
            XmlSerializer reader = new XmlSerializer(typeof(List<Tournament>));

            try
            {
                StreamReader file = new StreamReader(_xmlFile);
                Add((IList<Tournament>)reader.Deserialize(file));
            }
            catch (FileNotFoundException)
            {
            }
        }

        public string SaveAsXml()
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<Tournament>));
            FileStream file = File.Create(_xmlFile);
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

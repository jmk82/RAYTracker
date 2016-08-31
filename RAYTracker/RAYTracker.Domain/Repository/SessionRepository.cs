using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RAYTracker.Domain.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private IList<Session> _sessions;
        private IList<GameType> _gameTypes;
        private string _xmlFile;

        public SessionRepository()
        {
            _sessions = new List<Session>();
            _gameTypes = new List<GameType>();
            var path = Environment.GetFolderPath((Environment.SpecialFolder.ApplicationData));
            var dirName = @"\RAYTracker";
            Directory.CreateDirectory(path + dirName);
            _xmlFile = path + dirName + "//SessionData.xml";
        }

        public IList<Session> GetAll()
        {
            return _sessions;
        }

        public IList<GameType> GetAllGameTypes()
        {
            return _gameTypes;
        }

        public IList<Session> GetFiltered(CashGameFilter filter)
        {
            var sessions = _sessions;

            if (filter.GameTypes != null)
            {
                sessions = sessions.Where(s => filter.GameTypes.Contains(s.GameType)).ToList();
            }

            if (filter.StartDate != null)
            {
                sessions = sessions.Where(s => s.StartTime >= filter.StartDate.Value).ToList();
            }

            if (filter.EndDate != null)
            {
                sessions = sessions.Where(s => s.EndTime <= filter.EndDate.Value).ToList();
            }

            return sessions;
        }

        public void Add(IList<Session> sessions)
        {
            foreach (var session in sessions)
            {
                if (!_sessions.Contains(session))
                {
                    _sessions.Add(session);
                    if (!_gameTypes.Contains(session.GameType))
                    {
                        _gameTypes.Add(session.GameType);
                    }
                }
            }
        }

        public void ReadXml()
        {
            XmlSerializer reader = new XmlSerializer(typeof(List<Session>));

            try
            {
                StreamReader file = new StreamReader(_xmlFile);
                Add((IList<Session>)reader.Deserialize(file));
            }
            catch (FileNotFoundException)
            { 
            }
        }

        public void SaveAsXml()
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<Session>));
            FileStream file = File.Create(_xmlFile);
            writer.Serialize(file, _sessions.ToList());
            file.Close();
        }

        public void RemoveAll()
        {
            _sessions.Clear();
            _gameTypes.Clear();
        }
    }
}
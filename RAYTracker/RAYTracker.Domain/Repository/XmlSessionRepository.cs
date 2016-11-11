using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace RAYTracker.Domain.Repository
{
    public class XmlSessionRepository : ISessionRepository
    {
        private IList<Session> _sessions;
        private IList<GameType> _gameTypes;

        public XmlSessionRepository()
        {
            _sessions = new List<Session>();
            _gameTypes = new List<GameType>();
        }

        public IList<Session> GetAll()
        {
            return _sessions;
        }

        public IList<GameType> GetAllGameTypes()
        {
            return _gameTypes;
        }

        public IList<Session> GetFilteredSessions(CashGameFilter filter)
        {
            var sessions = _sessions;

            if (filter.GameTypes != null)
            {
                sessions = sessions.Where(s => filter.GameTypes.Contains(s.GameType)).ToList();
            }

            if (filter.StartDate != null)
            {
                sessions = sessions.Where(s => s.StartTime.Date >= filter.StartDate.Value.Date).ToList();
            }

            if (filter.EndDate != null)
            {
                sessions = sessions.Where(s => s.StartTime.Date <= filter.EndDate.Value.Date).ToList();
            }

            return sessions;
        }

        public IList<PlayingSession> GetFilteredPlayingSessions(CashGameFilter filter)
        {
            var sessions = _sessions;

            if (filter.GameTypes != null)
            {
                sessions = _sessions.Where(s => filter.GameTypes.Contains(s.GameType)).ToList();
            }

            var playingSessions = PlayingSession.GroupToPlayingSessions(sessions);

            if (filter.StartDate != null)
            {
                playingSessions = playingSessions.Where(s => s.StartTime.Date >= filter.StartDate.Value.Date).ToList();
            }

            if (filter.EndDate != null)
            {
                playingSessions = playingSessions.Where(s => s.StartTime.Date <= filter.EndDate.Value.Date).ToList();
            }

            return playingSessions;
        }

        public int Add(IList<Session> sessions)
        {
            var addedSessions = 0;

            foreach (var session in sessions)
            {
                if (!_sessions.Contains(session))
                {
                    _sessions.Add(session);
                    addedSessions++;

                    if (!_gameTypes.Contains(session.GameType))
                    {
                        _gameTypes.Add(session.GameType);
                    }
                }
            }

            return addedSessions;
        }

        public IList<Session> ReadXml(string filename)
        {
            XmlSerializer reader = new XmlSerializer(typeof(List<Session>));
            IList<Session> sessions = new List<Session>();

            try
            {
                StreamReader file = new StreamReader(filename);
                sessions = (IList<Session>)reader.Deserialize(file);
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
            {
            }

            return sessions;
        }

        public string SaveAsXml(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return null;

            XmlSerializer writer = new XmlSerializer(typeof(List<Session>));
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
           
            writer.Serialize(file, _sessions.ToList());
            file.Close();

            return file.Name;
        }

        public void RemoveAll()
        {
            _sessions.Clear();
            _gameTypes.Clear();
        }
    }
}
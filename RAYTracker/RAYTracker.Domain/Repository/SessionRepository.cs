using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RAYTracker.Domain.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private IList<Session> _sessions;
        private string _xmlFile;

        public SessionRepository()
        {
            _sessions = new List<Session>();
            _xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SessionData.xml";
        }

        public IList<Session> GetAll()
        {
            return _sessions;
        }

        public void Add(IList<Session> sessions)
        {
            var counter = 0;
            foreach (var session in sessions)
            {
                if (!_sessions.Contains(session))
                {
                    _sessions.Add(session);
                    counter++;
                }
            }
            Console.WriteLine("Added " + counter + " sessions");
        }

        public void ReadXml()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Session>));
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(_xmlFile);
                _sessions = (IList<Session>)reader.Deserialize(file);
            }
            catch (FileNotFoundException ex)
            {
                
            }
            
        }

        public void SaveAsXml()
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Session>));
            System.IO.FileStream file = System.IO.File.Create(_xmlFile);

            writer.Serialize(file, _sessions.ToList());
            file.Close();
        }

        public void RemoveAll()
        {
            _sessions.Clear();
        }
    }
}

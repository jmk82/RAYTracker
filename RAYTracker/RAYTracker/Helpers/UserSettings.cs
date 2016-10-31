using RAYTracker.Domain.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAYTracker.Helpers
{
    public class UserSettings
    {
        public string SessionXMLFilename { get; set; }
        public string TournamentXMLFilename { get; set; }

        public UserSettings()
        {
            var sessionFilename = Properties.Settings.Default.SessionXMLFilename;
            var tournamentFilename = Properties.Settings.Default.TournamentXMLFilename;

            if (!string.IsNullOrWhiteSpace(sessionFilename))
            {
                SessionXMLFilename = Properties.Settings.Default.SessionXMLFilename;
            }
            else
            {
                var path = Environment.GetFolderPath((Environment.SpecialFolder.ApplicationData));
                var dirName = @"\RAYTracker";
                Directory.CreateDirectory(path + dirName);
                SessionXMLFilename = path + dirName + "//SessionData.xml";
            }
        }
    }
}
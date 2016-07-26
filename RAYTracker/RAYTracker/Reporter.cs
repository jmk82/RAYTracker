using RAYTracker.Model;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker
{
    public class Reporter
    {
        public static string GetSimpleSessionTotalReport(IList<TableSession> tableSessions, IList<Session> sessions)
        {
            var result = tableSessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
            var timePlayed = sessions.Sum(s => s.Duration.TotalMinutes);

            var message = tableSessions.Count + " table sessions imported!\nFound total " + sessions.Count +
                          " playing sessions.";
            message += "\nTotal result: " + result + " €";
            message += "\nTime played: " + (timePlayed / 60.0).ToString("N2") + " hours";
            message += "\nHourly rate: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " €/h";
            message += "\nTotal hands played: " + tableSessions.Sum(t => t.HandsPlayed);

            return message;
        }
    }
}

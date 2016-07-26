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

            var message = tableSessions.Count + " istuntoa tuotu!\nYhteensä " + sessions.Count +
                          " sessiota.";
            message += "\nTulos: " + result + " €";
            message += "\nPelattu " + (timePlayed / 60.0).ToString("N2") + " hours";
            message += "\nTulos tuntia kohti: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " €/h";
            message += "\nKäsiä pelattu yhteensä: " + tableSessions.Sum(t => t.HandsPlayed);

            return message;
        }

        public static string DailyReport(IList<TableSession> tableSessions)
        {
            var report = tableSessions.GroupBy(t => t.StartTime.Date, t => t.Result);



            return report.ToString();
        }
    }
}

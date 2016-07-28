using RAYTracker.Model;
using RAYTracker.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

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
            message += "\nPelattu " + (timePlayed / 60.0).ToString("N2") + " tuntia";
            message += "\nTulos tuntia kohti: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " €/h";
            message += "\nKäsiä pelattu yhteensä: " + tableSessions.Sum(t => t.HandsPlayed);

            return message;
        }

        public static IEnumerable<DailyReport> DailyReport(IList<TableSession> tableSessions)
        {
            var data = tableSessions.GroupBy(t => t.StartTime.Date).OrderBy(t => t.Key)
                .Select(t => new DailyReport
                {
                    Time = t.Key.Date,
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                });

            return data;
        }

        public static IEnumerable<MonthlyReport> MonthlyReport(IList<TableSession> tableSessions)
        {
            var data = tableSessions.GroupBy(t => new { t.StartTime.Year, t.StartTime.Month}).OrderBy(t => t.Key.Year).ThenBy(t => t.Key.Month)
                .Select(t => new MonthlyReport
                {
                    Month = t.Key.Year.ToString() + "/" + t.Key.Month.ToString(),
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                });

            return data;
        }

        public static IEnumerable<YearlyReport> YearlyReport(IList<TableSession> tableSessions)
        {
            var data = tableSessions.GroupBy(t => t.StartTime.Year).OrderBy(t => t.Key)
                .Select(t => new YearlyReport
                {
                    Year = t.Key,
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                });

            return data;
        }

        public static IEnumerable<GameTypeReport> GameTypeReport(IList<TableSession> tableSessions)
        {
            var data = tableSessions.GroupBy(t => t.GameType.Name).OrderBy(t => t.Key)
                .Select(t => new GameTypeReport
                {
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed),
                    GameType = t.Key,
                    BbPerHundred = CountBbPerHundred(t)
                });

            return data;
        }

        private static decimal CountBbPerHundred(IGrouping<string, TableSession> t)
        {
            var totalWinnings = t.Sum(s => s.Result);
            var bb = t.ElementAt(0).GameType.BigBlind;
            var totalHands = t.Sum(s => s.HandsPlayed);

            return (decimal)((double)(totalWinnings / bb) / (totalHands * 0.01));
        }
    }
}

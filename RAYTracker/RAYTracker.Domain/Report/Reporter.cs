using RAYTracker.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.Domain.Report
{
    public static class Reporter
    {
        public static string GetSimpleSessionTotalReport(IList<Session> sessions, IList<PlayingSession> playingSessions)
        {
            var result = sessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
            var timePlayed = playingSessions.Sum(s => s.Duration.TotalMinutes);

            var message = sessions.Count + " istuntoa tuotu!\nYhteensä " + playingSessions.Count + " sessiota.";
                message += "\nTulos: " + result + " €";
                message += "\nPelattu " + (timePlayed / 60.0).ToString("N2") + " tuntia";
                message += "\nTulos tuntia kohti: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " €/h";
                message += "\nKäsiä pelattu yhteensä: " + sessions.Sum(t => t.HandsPlayed);

            return message;
        }

        public static IEnumerable<DailyReport> DailyReport(IList<Session> sessions)
        {
            var data = sessions.GroupBy(t => t.StartTime.Date).OrderBy(t => t.Key)
                .Select(t => new DailyReport
                {
                    Time = t.Key.Date,
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                });

            return data;
        }

        public static IEnumerable<MonthlyReport> MonthlyReport(IList<Session> sessions)
        {
            var data = sessions.GroupBy(t => new { t.StartTime.Year, t.StartTime.Month}).OrderBy(t => t.Key.Year).ThenBy(t => t.Key.Month)
                .Select(t => new MonthlyReport
                {
                    Month = t.Key.Year.ToString() + "/" + t.Key.Month.ToString(),
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                });

            return data;
        }

        public static IEnumerable<YearlyReport> YearlyReport(IList<Session> sessions)
        {
            var data = sessions.GroupBy(t => t.StartTime.Year).OrderBy(t => t.Key)
                .Select(t => new YearlyReport
                {
                    Year = t.Key,
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                });

            return data;
        }

        public static IEnumerable<GameTypeReport> GameTypeReport(IList<Session> sessions)
        {
            var data = sessions.GroupBy(t => t.GameType.FullName()).OrderByDescending(t => t.Key)
                .Select(t => new GameTypeReport
                {
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed),
                    GameType = t.Key,
                    BbPerHundred = CountBbPerHundred(t)
                });

            return data;
        }

        private static decimal CountBbPerHundred(IGrouping<string, Session> t)
        {
            var totalWinnings = t.Sum(s => s.Result);
            var bb = t.First().GameType.BigBlind;
            var totalHands = t.Sum(s => s.HandsPlayed);

            return (decimal)((double)(totalWinnings / bb) / (totalHands * 0.01));
        }

        public static IEnumerable<Session> Top10Sessions(IList<Session> sessions)
        {
            return sessions.OrderByDescending(s => s.Result).Take(10);
        }
    }
}

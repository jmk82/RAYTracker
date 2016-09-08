using RAYTracker.Domain.Model;
using System;
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
            var tableTimePlayed = sessions.Sum(s => s.Duration.TotalHours);
            var winningSessions = sessions.Count(s => s.Result > 0);
            var winningPlayingSessions = playingSessions.Count(s => s.Result > 0);

            var report = sessions.Count + " sessiota.\nYhteens‰ " + playingSessions.Count + " pelikertaa.";
                report += "\nTulos: " + result + " Ä";
                report += "\nPelattu " + (timePlayed / 60.0).ToString("N2") + " tuntia";
                report += "\nTulos tuntia kohti: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " Ä/h";
                report += "\nTulos pˆyt‰tuntia kohti: " + ((double)result / (tableTimePlayed)).ToString("N2") + " Ä/h";
                report += "\nK‰si‰ yhteens‰: " + sessions.Sum(t => t.HandsPlayed);
                report += "\nVoitollisia sessioita: " + winningSessions + " (" + 
                    ((double) winningSessions/sessions.Count*100.0).ToString("N2") + " %)";
                report += "\nVoitollisia pelikertoja: " + winningPlayingSessions + " (" +
                    ((double) winningPlayingSessions / playingSessions.Count * 100.0).ToString("N2") + " %)";

            return report;
        }

        public static IEnumerable<DailyReport> DailyReport(IList<Session> sessions)
        {
            // V‰hennet‰‰n v‰liaikaisesti kuusi tuntia aloitusajasta, jotta klo 00-06 v‰lill‰ aloitetut sessiot kirjataan viel‰ edelt‰v‰n p‰iv‰n tulokseen
            var dayStartMargin = new TimeSpan(6,0,0);
            foreach (var session in sessions)
            {
                session.StartTime = session.StartTime - dayStartMargin;
            }

            var data = sessions.GroupBy(t => t.StartTime.Date).OrderBy(t => t.Key)
                .Select(t => new DailyReport
                {
                    Time = t.Key.Date,
                    Result = t.Sum(s => s.Result),
                    Hands = t.Sum(s => s.HandsPlayed)
                }).ToList();

            // Ja lis‰t‰‰n v‰hennetty aika takaisin
            foreach (var session in sessions)
            {
                session.StartTime = session.StartTime + dayStartMargin;
            }

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

        public static IEnumerable<GameTypeReport> GameTypeReport(IList<Session> sessions, bool separateTurboAndAnteGames)
        {
            Func<Session, string> groupByFunc;
            if (separateTurboAndAnteGames)
            {
                groupByFunc = t => t.GameType.FullName();
            }
            else
            {
                groupByFunc = t => t.GameType.Name;
            }

            var data = sessions.GroupBy(groupByFunc).OrderByDescending(t => t.Key)
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
    }
}

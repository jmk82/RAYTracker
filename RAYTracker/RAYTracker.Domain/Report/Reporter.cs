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

            var report = sessions.Count + " sessiota.\nYhteens� " + playingSessions.Count + " pelikertaa.";
                report += "\nTulos: " + result + " �";
                report += "\nPelattu " + (timePlayed / 60.0).ToString("N2") + " tuntia";
                report += "\nTulos tuntia kohti: " + ((double)result / (timePlayed / 60.0)).ToString("N2") + " �/h";
                report += "\nTulos p�yt�tuntia kohti: " + ((double)result / (tableTimePlayed)).ToString("N2") + " �/h";
                report += "\nK�si� yhteens�: " + sessions.Sum(t => t.HandsPlayed);
                report += "\nVoitollisia sessioita: " + winningSessions + " (" + 
                    ((double) winningSessions/sessions.Count*100.0).ToString("N2") + " %)";
                report += "\nVoitollisia pelikertoja: " + winningPlayingSessions + " (" +
                    ((double) winningPlayingSessions / playingSessions.Count * 100.0).ToString("N2") + " %)";
                report += "\nSuurin downswing (sessiot): " + FindLargestDrop(sessions) + " �";
                report += "\nSuurin downswing (pelikerrat): " + FindLargestDrop(playingSessions
                    .Select(p => new Session { EndTime = p.EndTime, Result = p.Result })) + " �";

            return report;
        }

        public static IEnumerable<DailyReport> DailyReport(IList<PlayingSession> playingSessions)
        {
            var data = playingSessions.GroupBy(p => p.StartTime.Date).OrderBy(g => g.Key)
                .Select(g => new DailyReport
                {
                    Time = g.Key.Date,
                    Result = g.Sum(p => p.Result),
                    Hands = g.Sum(p => p.HandsPlayed),
                    Hours = g.Sum(p => p.Duration.TotalHours)
                });

            return data;
        }

        public static IEnumerable<MonthlyReport> MonthlyReport(IList<PlayingSession> playingSessions)
        {
            var data = playingSessions.GroupBy(p => new { p.StartTime.Year, p.StartTime.Month }).OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyReport
                {
                    Month = g.Key.Year.ToString() + "/" + g.Key.Month.ToString(),
                    Result = g.Sum(p => p.Result),
                    Hands = g.Sum(p => p.HandsPlayed),
                    Hours = g.Sum(p => p.Duration.TotalHours)
                });

            return data;
        }

        public static IEnumerable<YearlyReport> YearlyReport(IList<PlayingSession> playingSessions)
        {
            var data = playingSessions.GroupBy(p => p.StartTime.Year).OrderBy(g => g.Key)
                .Select(g => new YearlyReport
                {
                    Year = g.Key,
                    Result = g.Sum(p => p.Result),
                    Hands = g.Sum(p => p.HandsPlayed),
                    Hours = g.Sum(p => p.Duration.TotalHours)
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

        public static decimal FindLargestDrop(IEnumerable<Session> sessions)
        {
            var sessionArray = sessions.OrderBy(s => s.EndTime).ToArray();
            decimal biggestDrop = 0M;
            decimal currentMax = 0M;
            decimal currentMin = 0M;
            decimal cumResult = 0M;

            for (int i = 0; i < sessionArray.Length; i++)
            {
                cumResult += sessionArray[i].Result;

                if (cumResult < currentMin)
                {
                    currentMin = cumResult;
                    biggestDrop = Math.Max(biggestDrop, currentMax - currentMin);
                }
                else if (cumResult > currentMax)
                {
                    currentMax = cumResult;
                    currentMin = cumResult;
                }
            }

            return biggestDrop;
        }
    }
}

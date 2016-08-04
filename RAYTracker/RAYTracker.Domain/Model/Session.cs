using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.Domain.Model
{
    public class Session
    {
        public string TableName { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan SessionDuration { get; set; }
        public DateTime EndTime { get; set; }
        public GameType GameType { get; set; }
        public decimal TotalBetsMade { get; set; }
        public decimal TotalWonAmount { get; set; }
        public int HandsPlayed { get; set; }
        public decimal ChipsBought { get; set; }
        public decimal ChipsCashedOut { get; set; }
        public decimal Result { get; set; }

        public static IList<Session> OrderSessions(ICollection<Session> sessions)
        {
            var orderedSessions = sessions.OrderBy(t => t.StartTime).ThenBy(t => t.EndTime).ToList();

            return orderedSessions;
        }
    }
}

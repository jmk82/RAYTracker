using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker
{
    public class Session
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public IList<TableSession> TableSessions { get; set; }
        public decimal Result { get; set; }

        public Session(DateTime start, DateTime end, IList<TableSession> sessions)
        {
            StartTime = start;
            EndTime = end;
            Duration = end - start;
            TableSessions = sessions;
            Result = TableSessions.Sum(t => t.ChipsCashedOut - t.ChipsBought);
        }

        public override string ToString()
        {
            return "Start: " + StartTime + " End: " + EndTime + " TableSessions: " + TableSessions.Count;
        }
    }
}

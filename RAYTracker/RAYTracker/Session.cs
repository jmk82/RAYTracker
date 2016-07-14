using System;
using System.Collections.Generic;

namespace RAYTracker
{
    public class Session
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public IList<TableSession> TableSessions { get; set; }

        public Session(DateTime start, DateTime end, IList<TableSession> sessions)
        {
            StartTime = start;
            EndTime = end;
            TableSessions = sessions;
        }

        public override string ToString()
        {
            return "Start: " + StartTime + " End: " + EndTime + " TableSessions: " + TableSessions.Count;
        }
    }
}

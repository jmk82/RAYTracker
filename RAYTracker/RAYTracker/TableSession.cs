using System;

namespace RAYTracker
{
    public class TableSession
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public override string ToString()
        {
            return "Start: " + StartTime + " End: " + EndTime;
        }
    }
}

using System;

namespace RAYTracker.Helpers
{
    public class SessionsDatesMessage
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public SessionsDatesMessage(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
    }
}

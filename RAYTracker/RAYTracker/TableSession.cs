using System;

namespace RAYTracker
{
    public class TableSession
    {
        public string TableName { get; set; }
        public DateTime StartTime { get; set; }
        public int SessionDuration { get; set; }
        public DateTime EndTime { get; set; }
        public string GameType { get; set; }
        public decimal TotalBetsMade { get; set; }
        public decimal TotalWonAmount { get; set; }
        public decimal HandsPlayed { get; set; }
        public decimal ChipsBought { get; set; }
        public decimal ChipsCashedOut { get; set; }

        public override string ToString()
        {
            return "Start: " + StartTime + " End: " + EndTime;
        }
    }
}

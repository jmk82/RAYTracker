using System;

namespace RAYTracker.Domain.Model
{
    public class Tournament
    {
        public string Name { get; set; }
        public int FinalPosition { get; set; }
        public string Type { get; set; }
        public DateTime StartTime { get; set; }
        public decimal TotalBuyIn { get; set; }
        public string BuyIn { get; set; }
        public decimal Win { get; set; }
    }
}

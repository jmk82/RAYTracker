namespace RAYTracker.Domain.Report
{
    public class GameTypeReport : Report
    {
        public string GameType { get; set; }
        public string Bb { get; set; }
        public decimal BbPerHundred { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.Domain.Model
{
    public class Tournament : IEquatable<Tournament>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int FinalPosition { get; set; }
        public string Type { get; set; }
        public DateTime StartTime { get; set; }
        public decimal TotalBuyIn { get; set; }
        public string BuyIn { get; set; }
        public decimal Win { get; set; }

        public static IList<Tournament> OrderTournaments(ICollection<Tournament> tournaments)
        {
            return tournaments.OrderBy(t => t.StartTime).ToList();
        }

        public bool Equals(Tournament other)
        {
            return this.Id == other.Id;
        }
    }
}

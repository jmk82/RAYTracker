using System;

namespace RAYTracker.Domain.Model
{
    public class GameType : IEquatable<GameType>
    {
        public string Name { get; set; }
        public decimal BigBlind { get; set; }
        public bool HasAnte { get; set; }
        public bool IsTurbo { get; set; }

        public string FullName()
        {
            var fullName = Name;

            if (IsTurbo)
            {
                fullName += " TURBO";
            }

            if (HasAnte)
            {
                fullName += " ANTE";
            }

            return fullName;
        }

        public bool Equals(GameType other)
        {
            if (other == null) return false;

            return this.FullName() == other.FullName();
        }
    }
}

namespace RAYTracker.Domain.Model
{
    public class GameType
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
    }
}

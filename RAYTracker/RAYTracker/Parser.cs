using System;

namespace RAYTracker
{
    public class Parser
    {
        private Reader _reader;

        public Parser()
        {
        }

        public Parser(Reader reader)
        {
            _reader = reader;
        }

        //public IList<TableSession> GetTableSessions()
        //{
            
        //}

        //public IList<Session> GetSessions()
        //{
            
        //}

        public TableSession CreateTableSession(string[] tokens)
        {
            TableSession session = new TableSession();

            session.TableName = tokens[0];
            session.StartTime = Convert.ToDateTime(tokens[1]);
            session.SessionDuration = ParseDuration(tokens[2]);
            session.EndTime = session.StartTime + new TimeSpan(0, session.SessionDuration, 0);
            session.GameType = tokens[3];
            session.TotalBetsMade = ParseCurrency(tokens[4]);
            session.TotalWonAmount = ParseCurrency(tokens[5]);
            session.HandsPlayed = Int32.Parse(tokens[6]);
            session.ChipsBought = ParseCurrency(tokens[7]);
            session.ChipsCashedOut = ParseCurrency(tokens[8]);

            return session;
        }

        public string[] ParseLine(string line)
        {
            string[] tokens = line.Split('\t');

            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = tokens[i].Trim();
            }

            return tokens;
        }

        public int ParseDuration(string duration)
        {
            string[] temp = duration.Split(':');
            return Int32.Parse(temp[0]) * 60 + Int32.Parse(temp[1]);
        }

        private decimal ParseCurrency(string currency)
        {
            currency = currency.Remove(0, 1);
            currency = currency.Replace('.', ',');
            return Convert.ToDecimal(currency);
        }
    }
}

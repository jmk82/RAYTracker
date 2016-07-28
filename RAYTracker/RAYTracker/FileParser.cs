using RAYTracker.Model;
using System;

namespace RAYTracker
{
    public class FileParser
    {
        private Reader _reader;
        private readonly DataConverter _dataConverter;

        public FileParser()
        {
            _dataConverter = new DataConverter();
        }

        public FileParser(Reader reader)
        {
            _dataConverter = new DataConverter();
            _reader = reader;
        }

        public TableSession CreateTableSession(string[] tokens)
        {
            TableSession session = new TableSession();

            session.TableName = tokens[0];
            session.StartTime = Convert.ToDateTime(tokens[1]);
            session.SessionDuration = _dataConverter.ParseDuration(tokens[2]);
            session.EndTime = session.StartTime + session.SessionDuration;
            session.GameType = _dataConverter.AssignGameType(tokens[3]);
            session.TotalBetsMade = _dataConverter.ParseCurrency(tokens[4]);
            session.TotalWonAmount = _dataConverter.ParseCurrency(tokens[5]);
            session.HandsPlayed = int.Parse(tokens[6]);
            session.ChipsBought = _dataConverter.ParseCurrency(tokens[7]);
            session.ChipsCashedOut = _dataConverter.ParseCurrency(tokens[8]);
            session.Result = session.ChipsCashedOut - session.ChipsBought;
            
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
    }
}

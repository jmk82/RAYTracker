﻿using RAYTracker.Domain.Model;
using System;

namespace RAYTracker.Domain.Utils
{
    public class FileParser
    {
        private Reader _reader;

        public FileParser()
        {
        }

        public FileParser(Reader reader)
        {
            _reader = reader;
        }

        public Session CreateTableSession(string[] tokens)
        {
            Session session = new Session();

            session.TableName = tokens[0];
            session.StartTime = Convert.ToDateTime(tokens[1]);
            session.Duration = DataConverter.ParseDuration(tokens[2]);
            session.EndTime = session.StartTime + session.Duration;
            session.GameType = DataConverter.AssignGameType(tokens[3], tokens[0]);
            session.TotalBetsMade = DataConverter.ParseCurrency(tokens[4]);
            session.TotalWonAmount = DataConverter.ParseCurrency(tokens[5]);
            session.HandsPlayed = int.Parse(tokens[6]);
            session.ChipsBought = DataConverter.ParseCurrency(tokens[7]);
            session.ChipsCashedOut = DataConverter.ParseCurrency(tokens[8]);
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

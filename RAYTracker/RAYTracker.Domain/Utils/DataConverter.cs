using RAYTracker.Domain.Model;
using System;

namespace RAYTracker.Domain.Utils
{
    public static class DataConverter
    {
        public static TimeSpan ParseDuration(string duration)
        {
            string[] temp = duration.Split(':');
            return new TimeSpan(int.Parse(temp[0]), int.Parse(temp[1]), 0);
        }

        public static decimal ParseCurrency(string currency)
        {
            currency = currency.Remove(0, 1);

            if (currency.IndexOf(',') != -1)
            {
                currency = currency.Remove(currency.IndexOf(','));
            }
            currency = currency.Replace('.', ',');

            decimal convertedCurrency;

            convertedCurrency = Convert.ToDecimal(currency);
            
            return convertedCurrency;
        }

        public static GameType AssignGameType(string gameType, string tableName)
        {
            var hasAnte = tableName.Contains("ANTE");
            var isTurbo = tableName.Contains("TURBO");

            return new GameType { Name = gameType, BigBlind = FindBigBlindValue(gameType), HasAnte = hasAnte, IsTurbo = isTurbo };
        }

        public static decimal FindBigBlindValue(string s)
        {
            var tokens = s.Split(' ');
            var blinds = tokens[tokens.Length - 1].Split('€');
            var bb = blinds[blinds.Length - 1].Replace('.', ',');

            return Convert.ToDecimal(bb);
        }
    }
}
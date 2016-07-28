using RAYTracker.Model;
using System;

namespace RAYTracker
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
            return Convert.ToDecimal(currency);
        }

        public static GameType AssignGameType(string s)
        {
            return new GameType { Name = s, BigBlind = FindBigBlindValue(s) };
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
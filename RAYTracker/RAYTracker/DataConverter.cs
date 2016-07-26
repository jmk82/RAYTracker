using System;

namespace RAYTracker
{
    public class DataConverter
    {
        public TimeSpan ParseDuration(string duration)
        {
            string[] temp = duration.Split(':');
            return new TimeSpan(int.Parse(temp[0]), int.Parse(temp[1]), 0);
        }

        public decimal ParseCurrency(string currency)
        {
            currency = currency.Remove(0, 1);

            if (currency.IndexOf(',') != -1)
            {
                currency = currency.Remove(currency.IndexOf(','));
            }
            currency = currency.Replace('.', ',');
            return Convert.ToDecimal(currency);
        }
    }
}
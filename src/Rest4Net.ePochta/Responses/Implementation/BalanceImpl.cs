using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class BalanceImpl : IBalance
    {
        private double balance_currency;
        private string currency;

        public double Amount
        {
            get { return balance_currency; }
        }

        public Currency AmountCurrency
        {
            get { return (Currency)Enum.Parse(typeof(Currency), currency, true); }
        }
    }
}

using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class BalanceImpl : IBalance
    {
#pragma warning disable 649
        private double balance_currency;
        private string currency;
#pragma warning restore 649

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

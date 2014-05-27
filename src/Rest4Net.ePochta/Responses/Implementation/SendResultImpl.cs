using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendResultImpl : ISendResult
    {
        private int id;
        private double price;
        private string currency;

        public int Id
        {
            get { return id; }
        }

        public double Price
        {
            get { return price; }
        }
        
        public Currency PriceCurrency
        {
            get { return (Currency)Enum.Parse(typeof(Currency), currency, true); }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1} {2}", Id, Price, PriceCurrency);
        }
    }
}

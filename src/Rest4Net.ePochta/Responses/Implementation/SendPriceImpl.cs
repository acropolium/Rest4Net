using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendPriceImpl : ISendPrice
    {
        protected double price;
        protected string currency;

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
            return String.Format("{0} {1}", Price, PriceCurrency);
        }
    }
}

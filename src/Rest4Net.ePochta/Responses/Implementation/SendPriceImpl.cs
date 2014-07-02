using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendPriceImpl : ISendPrice
    {
#pragma warning disable 649
        protected double price;
        protected string currency;
#pragma warning restore 649

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

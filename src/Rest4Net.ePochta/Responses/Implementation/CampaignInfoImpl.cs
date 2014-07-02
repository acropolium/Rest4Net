using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class CampaignInfoImpl : ICampaignInfo
    {
#pragma warning disable 649
        private int sent;
        private int delivered;
        private int not_delivered;
        private int status;
        private double price;
        private string currency;
#pragma warning restore 649

        public int SentCount
        {
            get { return sent; }
        }

        public int DeliveredCount
        {
            get { return delivered; }
        }

        public int NonDeliveredCount
        {
            get { return not_delivered; }
        }

        public CampaignStatus Status
        {
            get { return (CampaignStatus)status; }
        }

        public double Price
        {
            get { return price; }
        }

        public Currency PriceCurrency
        {
            get { return (Currency)Enum.Parse(typeof(Currency), currency, true); }
        }
    }
}

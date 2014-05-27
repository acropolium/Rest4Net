using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SmsDeliveryInfoImpl : ISmsDeliveryInfo
    {
        private string phone;
        private string sentdate;
        private string donedate;
        private string status;

        public string Phone
        {
            get { return phone; }
        }

        public DateTime? SentAt
        {
            get { return sentdate.ToPochtaDateNullable(); }
        }

        public DateTime? FinalStatusAt
        {
            get { return donedate.ToPochtaDateNullable(); }
        }

        public SmsDeliveryStatus Status
        {
            get { return status.AsSmsDeliveryStatus(); }
        }

        public override string ToString()
        {
            return String.Format("{0} -> {1}", Phone, Status);
        }
    }
}

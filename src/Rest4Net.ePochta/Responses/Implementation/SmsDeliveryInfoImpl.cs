using System;
using Rest4Net.ePochta.Utils;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SmsDeliveryInfoImpl : ISmsDeliveryInfo
    {
#pragma warning disable 649
        private string phone;
        private string sentdate;
        private string donedate;
        private string status;
#pragma warning restore 649

        public string Phone
        {
            get { return phone; }
        }

        public DateTime? SentAt
        {
            get { return DateUtils.ToPochtaDateNullable(sentdate); }
        }

        public DateTime? FinalStatusAt
        {
            get { return DateUtils.ToPochtaDateNullable(donedate); }
        }

        public SmsDeliveryStatus Status
        {
            get { return SmsDeliveryStatusUtils.AsSmsDeliveryStatus(status); }
        }

        public override string ToString()
        {
            return String.Format("{0} -> {1}", Phone, Status);
        }
    }
}

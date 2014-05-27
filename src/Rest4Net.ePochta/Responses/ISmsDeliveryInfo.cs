using System;

namespace Rest4Net.ePochta.Responses
{
    public interface ISmsDeliveryInfo
    {
        string Phone { get; }
        DateTime? SentAt { get; }
        DateTime? FinalStatusAt { get; }
        SmsDeliveryStatus Status { get; }
    }
}

using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public enum SmsDeliveryStatus
    {
        InQueue,
        Sent,
        Delivered,
        NonDelivered,
        InvalidPhoneNumber,
        Spam
    }

    internal static class SmsDeliveryStatusUtils
    {
        private static readonly Dictionary<SmsDeliveryStatus, string> Items = new Dictionary<SmsDeliveryStatus, string>
        {
            {SmsDeliveryStatus.InQueue, "0"},
            {SmsDeliveryStatus.Sent, "SENT"},
            {SmsDeliveryStatus.Delivered, "DELIVERED"},
            {SmsDeliveryStatus.NonDelivered, "NOT_DELIVERED"},
            {SmsDeliveryStatus.InvalidPhoneNumber, "INVALID_PHONE_NUMBER"},
            {SmsDeliveryStatus.Spam, "SPAM"},
        };

        private static readonly Dictionary<string, SmsDeliveryStatus> ItemsReverse = new Dictionary<string, SmsDeliveryStatus>();

        static SmsDeliveryStatusUtils()
        {
            foreach (var item in Items)
                ItemsReverse[item.Value] = item.Key;
        }

        public static string AsString(this SmsDeliveryStatus item)
        {
            return Items[item];
        }

        public static SmsDeliveryStatus AsSmsDeliveryStatus(this string item)
        {
            return ItemsReverse[item];
        }
    }
}
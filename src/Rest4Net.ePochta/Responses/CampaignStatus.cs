namespace Rest4Net.ePochta.Responses
{
    public enum CampaignStatus
    {
        InQueue = 0,
        NotEnoughFunds = 1,
        InProcess = 2,
        Sent = 3,
        InvalidReceiverPhoneNumbers = 4,
        PartiallySent = 5,
        Spam = 6,
        InvalidSenderName = 7,
        Pause = 8,
        Planned = 9,
        OnModeration = 10
    }
}

namespace Rest4Net.ePochta.Responses
{
    public interface ICampaignInfo
    {
        int SentCount { get; }
        int DeliveredCount { get; }
        int NonDeliveredCount { get; }
        CampaignStatus Status { get; }
        double Price { get; }
        Currency PriceCurrency { get; }
    }
}

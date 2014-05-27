namespace Rest4Net.ePochta.Responses
{
    public interface ICampaign
    {
        int Id { get; }
        string Sender { get; }
        string Text { get; }
        CampaignStatus Status { get; }
    }
}

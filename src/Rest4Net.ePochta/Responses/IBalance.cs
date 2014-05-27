namespace Rest4Net.ePochta.Responses
{
    public interface IBalance
    {
        double Amount { get; }
        Currency AmountCurrency { get; }
    }
}

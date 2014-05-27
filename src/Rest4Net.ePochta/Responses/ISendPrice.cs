namespace Rest4Net.ePochta.Responses
{
    public interface ISendPrice
    {
        /// <summary>
        /// Price for the campaign
        /// </summary>
        double Price { get; }

        /// <summary>
        /// Price currency
        /// </summary>
        Currency PriceCurrency { get; }
    }
}

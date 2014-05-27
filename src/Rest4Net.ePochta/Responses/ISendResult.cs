namespace Rest4Net.ePochta.Responses
{
    public interface ISendResult
    {
        /// <summary>
        /// Created campaign id
        /// </summary>
        int Id { get; }

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

namespace Rest4Net.ePochta.Responses
{
    public interface ISendResult : ISendPrice
    {
        /// <summary>
        /// Created campaign id
        /// </summary>
        int Id { get; }
    }
}

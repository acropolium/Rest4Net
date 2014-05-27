namespace Rest4Net.ePochta.Responses
{
    public interface ISender
    {
        int Id { get; }
        string Name { get; }
        ReviewStatus Status { get; }
        Country Country { get; }
    }
}

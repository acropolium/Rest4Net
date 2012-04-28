namespace Rest4Net.BitLy.Responses
{
    public interface IBitlyItem
    {
        string LongUrl { get; }
        string ShortUrl { get; }
        string GlobalHash { get; }
        int UserClicks { get; }
        string UserHash { get; }
        int GlobalClicks { get; }
        string Hash { get; }
        string Error { get; }
    }
}

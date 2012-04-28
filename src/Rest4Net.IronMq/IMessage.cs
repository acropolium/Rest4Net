namespace Rest4Net.IronMq
{
    public interface IMessage
    {
        string ID { get; }
        string Body { get; }
        int Timeout { get; }
        int Delay { get; }
        int ExpiresIn { get; }
    }
}

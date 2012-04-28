namespace Rest4Net.IronMq
{
    public interface IQueue
    {
        string ID { get; }
        string ProjectID { get; }
        string Name { get; }
        int Size { get; }
    }
}

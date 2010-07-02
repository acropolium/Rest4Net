namespace Rest4Net
{
    public interface IRestSerializer
    {
        TObject Deserialize<TObject>(TObject obj, byte[] content);
    }
}

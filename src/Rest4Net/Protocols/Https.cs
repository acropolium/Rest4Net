namespace Rest4Net.Protocols
{
    public class Https : HttpBaseProtocol
    {
        public Https(string host, int port = -1) : base(host, port, true) { }
    }
}

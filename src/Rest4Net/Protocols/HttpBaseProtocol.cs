namespace Rest4Net.Protocols
{
    public class HttpBaseProtocol : CommonWebProtocol
    {
        private readonly bool _useHttps;

        public HttpBaseProtocol(string host, int port = -1, bool useHttps = false) : base(host, port > 0 ? port : (useHttps ? 443 : 80))
        {
            _useHttps = useHttps;
        }

        private const string UriSchemeHttps = "https";
        private const string UriSchemeHttp = "http";

        protected override string Scheeme
        {
            get { return _useHttps ? UriSchemeHttps : UriSchemeHttp; }
        }

        protected override int DefaultPort
        {
            get { return _useHttps ? 443 : 80; }
        }
    }
}

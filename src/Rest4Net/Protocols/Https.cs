using System;

namespace Rest4Net.Protocols
{
    public class Https : CommonWebProtocol
    {
        public Https(string host, int port = -1) : base(host, port) { }

        protected override string Scheeme
        {
            get { return Uri.UriSchemeHttps; }
        }

        protected override int DefaultPort
        {
            get { return 443; }
        }
    }
}

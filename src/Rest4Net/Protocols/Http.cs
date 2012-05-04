using System;

namespace Rest4Net.Protocols
{
    public class Http : CommonWebProtocol
    {
        public Http(string host, int port = -1) : base(host, port) { }

        protected override string Scheeme
        {
            get { return Uri.UriSchemeHttp; }
        }

        protected override int DefaultPort
        {
            get { return 80; }
        }
    }
}

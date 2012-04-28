namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ShortenImpl : IShorten
    {
        private string _url;
        private string _hash;
        private string _globalHash;
        private string _longUrl;
        private string _newHash;

        public string Url
        {
            get { return _url; }
        }

        public string Hash
        {
            get { return _hash; }
        }

        public string GlobalHash
        {
            get { return _globalHash; }
        }

        public string LongUrl
        {
            get { return _longUrl; }
        }

        public string NewHash
        {
            get { return _newHash; }
        }
    }
}

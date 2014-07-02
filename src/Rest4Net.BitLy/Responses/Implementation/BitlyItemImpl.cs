namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class BitlyItemImpl : IBitlyItem
    {
#pragma warning disable 649
        private string _longUrl;
        private string _shortUrl;
        private string _globalHash;
        private int _userClicks;
        private string _userHash;
        private int _globalClicks;
        private string _hash;
        private string _error;
#pragma warning restore 649

        public string LongUrl
        {
            get { return _longUrl; }
        }

        public string ShortUrl
        {
            get { return _shortUrl; }
        }

        public string GlobalHash
        {
            get { return _globalHash; }
        }

        public int UserClicks
        {
            get { return _userClicks; }
        }

        public string UserHash
        {
            get { return _userHash; }
        }

        public int GlobalClicks
        {
            get { return _globalClicks; }
        }

        public string Hash
        {
            get { return _hash; }
        }

        public string Error
        {
            get { return _error; }
        }
    }
}
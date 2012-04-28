using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class LookupImpl : ILookup
    {
        private readonly IList<LookupItemImpl> _lookup = new List<LookupItemImpl>();

        public IEnumerable<ILookupItem> Lookup
        {
            get { return _lookup; }
        }
    }

    internal class LookupItemImpl : ILookupItem
    {
        private string _globalHash;

        private string _shortUrl;

        private string _url;

        public string GlobalHash
        {
            get { return _globalHash; }
        }

        public string ShortUrl
        {
            get { return _shortUrl; }
        }

        public string Url
        {
            get { return _url; }
        }
    }
}

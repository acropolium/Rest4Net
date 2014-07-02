using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class LookupImpl : ILookup
    {
#pragma warning disable 649
        private readonly List<LookupItemImpl> _lookup;
#pragma warning restore 649

        public IEnumerable<ILookupItem> Lookup
        {
            get
            {
                foreach (var impl in _lookup)
                    yield return impl;
            }
        }
    }

    internal class LookupItemImpl : ILookupItem
    {
#pragma warning disable 649
        private string _globalHash;
        private string _shortUrl;
        private string _url;
#pragma warning restore 649

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

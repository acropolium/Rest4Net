using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class InfoImpl : IInfo
    {
#pragma warning disable 649
        private readonly List<InfoItemImpl> _info;
#pragma warning restore 649

        public IEnumerable<IInfoItem> Info
        {
            get
            {
                foreach (var impl in _info)
                    yield return impl;
            }
        }
    }

    internal class InfoItemImpl : IInfoItem
    {
#pragma warning disable 649
        private string _globalHash;
        private string _shortUrl;
        private string _hash;
        private string _userHash;
        private string _error;
        private string _title;
        private string _createdBy;
#pragma warning restore 649

        public string GlobalHash
        {
            get { return _globalHash; }
        }

        public string ShortUrl
        {
            get { return _shortUrl; }
        }

        public string Hash
        {
            get { return _hash; }
        }

        public string UserHash
        {
            get { return _userHash; }
        }

        public string Error
        {
            get { return _error; }
        }

        public string Title
        {
            get { return _title; }
        }

        public string CreatedBy
        {
            get { return _createdBy; }
        }
    }
}

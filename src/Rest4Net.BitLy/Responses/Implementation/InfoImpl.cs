using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class InfoImpl : IInfo
    {
        private readonly List<InfoItemImpl> _info;

        public IEnumerable<IInfoItem> Info
        {
            get { return _info; }
        }
    }

    internal class InfoItemImpl : IInfoItem
    {
        private string _globalHash;

        private string _shortUrl;

        private string _hash;

        private string _userHash;

        private string _error;

        private string _title;

        private string _createdBy;

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

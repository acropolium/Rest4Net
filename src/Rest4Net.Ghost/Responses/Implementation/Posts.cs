using System.Collections.Generic;

namespace Rest4Net.Ghost.Responses.Implementation
{
    internal class Posts : IPosts
    {
        private List<Post> _posts;
        private int _page;
        private int _limit;
        private int _pages;
        private int _total;

        public IEnumerable<IPost> Items
        {
            get { return _posts; }
        }

        public int Page
        {
            get { return _page; }
        }

        public int Limit
        {
            get { return _limit; }
        }

        public int Pages
        {
            get { return _pages; }
        }

        public int Total
        {
            get { return _total; }
        }
    }
}

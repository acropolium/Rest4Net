using System.Collections.Generic;

namespace Rest4Net.GoogleCustomSearch
{
    public class SearchResult
    {
        private string _kind;

        public string Kind
        {
            get { return _kind; }
        }
        
        private SearchResultUrl _url;

        public SearchResultUrl Url
        {
            get { return _url; }
        }

        private SearchResultQueries _queries;

        public SearchResultQueries Queries
        {
            get { return _queries; }
        }

        private SearchResultContext _context;

        public SearchResultContext Context
        {
            get { return _context; }
        }

        private List<SearchResultItem> _items;

        public IEnumerable<SearchResultItem> Items
        {
            get { return _items; }
        }
    }

    public class SearchResultQueries
    {
        private List<SearchResultQuery> _nextPage;
        private List<SearchResultQuery> _previousPage;
        private List<SearchResultQuery> _request;

        public IEnumerable<SearchResultQuery> NextPage
        {
            get { return _nextPage; }
        }

        public IEnumerable<SearchResultQuery> PreviousPage
        {
            get { return _previousPage; }
        }

        public IEnumerable<SearchResultQuery> Request
        {
            get { return _request; }
        }
    }

    public class SearchResultContext
    {
        private string _title;

        public string Title
        {
            get { return _title; }
        }
    }

    public class SearchResultUrl
    {
        private string _type;

        public string Type
        {
            get { return _type; }
        }

        private string _template;

        public string Template
        {
            get { return _template; }
        }
    }

    public class SearchResultQuery
    {
        private string _title;
        private string _searchTerms;
        private string _inputEncoding;
        private string _outputEncoding;
        private string _cx;
        private int _totalResults;
        private int _count;
        private int _startIndex;

        public string Title
        {
            get { return _title; }
        }

        public string SearchTerms
        {
            get { return _searchTerms; }
        }

        public string InputEncoding
        {
            get { return _inputEncoding; }
        }

        public string OutputEncoding
        {
            get { return _outputEncoding; }
        }

        public string Cx
        {
            get { return _cx; }
        }

        public int TotalResults
        {
            get { return _totalResults; }
        }

        public int Count
        {
            get { return _count; }
        }

        public int StartIndex
        {
            get { return _startIndex; }
        }
    }

    public class SearchResultItem
    {
        private string _title;
        private string _htmlTitle;
        private string _kind;
        private string _link;
        private string _displayLink;
        private string _snippet;
        private string _htmlSnippet;

        public string Title
        {
            get { return _title; }
        }

        public string HtmlTitle
        {
            get { return _htmlTitle; }
        }

        public string Kind
        {
            get { return _kind; }
        }

        public string Link
        {
            get { return _link; }
        }

        public string DisplayLink
        {
            get { return _displayLink; }
        }

        public string Snippet
        {
            get { return _snippet; }
        }

        public string HtmlSnippet
        {
            get { return _htmlSnippet; }
        }
    }
}

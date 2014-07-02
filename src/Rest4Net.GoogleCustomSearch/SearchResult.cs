using System.Collections.Generic;

namespace Rest4Net.GoogleCustomSearch
{
    public class SearchResult
    {
#pragma warning disable 649
        private string _kind;
        private SearchResultUrl _url;
        private SearchResultQueries _queries;
        private SearchResultContext _context;
        private List<SearchResultItem> _items;
#pragma warning restore 649

        public string Kind
        {
            get { return _kind; }
        }
        
        public SearchResultUrl Url
        {
            get { return _url; }
        }
        public SearchResultQueries Queries
        {
            get { return _queries; }
        }

        public SearchResultContext Context
        {
            get { return _context; }
        }

        public IEnumerable<SearchResultItem> Items
        {
            get { return _items; }
        }
    }

    public class SearchResultQueries
    {
#pragma warning disable 649
        private List<SearchResultQuery> _nextPage;
        private List<SearchResultQuery> _previousPage;
        private List<SearchResultQuery> _request;
#pragma warning restore 649

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
#pragma warning disable 649
        private string _title;
#pragma warning restore 649

        public string Title
        {
            get { return _title; }
        }
    }

    public class SearchResultUrl
    {
#pragma warning disable 649
        private string _type;
        private string _template;
#pragma warning restore 649

        public string Type
        {
            get { return _type; }
        }

        public string Template
        {
            get { return _template; }
        }
    }

    public class SearchResultQuery
    {
#pragma warning disable 649
        private string _title;
        private string _searchTerms;
        private string _inputEncoding;
        private string _outputEncoding;
        private string _cx;
        private int _totalResults;
        private int _count;
        private int _startIndex;
#pragma warning restore 649

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
#pragma warning disable 649
        private string _title;
        private string _htmlTitle;
        private string _kind;
        private string _link;
        private string _displayLink;
        private string _snippet;
        private string _htmlSnippet;
#pragma warning restore 649

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

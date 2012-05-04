using System.Json;
using Rest4Net.Exceptions;
using Rest4Net.Protocols;

namespace Rest4Net.GoogleCustomSearch
{
    public class GoogleCustomSearchProvider : RestApiProvider
    {
        private readonly bool _keyIsAccessToken;
        private readonly string _key;
        private readonly string _cx;

        public GoogleCustomSearchProvider(string key, string cxCustomSearchId, bool keyIsAccessToken = false)
            : base(new Https("www.googleapis.com"))
        {
            _key = key;
            _cx = cxCustomSearchId;
            _keyIsAccessToken = keyIsAccessToken;
        }

        private Command Run()
        {
            return Cmd("/customsearch/v1")
                .WithParameter(_keyIsAccessToken ? "access_token" : "key", _key)
                .WithParameter("cx", _cx)
                .WithParameter("alt", "json");
        }

        /// <summary>
        /// Makes a search with google API
        /// https://developers.google.com/custom-search/v1/using_rest
        /// </summary>
        /// <param name="dataToSearch">actually Query String</param>
        /// <param name="parameters">number of search parameters like described at https://developers.google.com/custom-search/v1/using_rest#query-params </param>
        /// <returns>Search summary. Throws ResultException if parameters are wrong</returns>
        public SearchResult Search(string dataToSearch, SearchParameters parameters = null)
        {
            var cmd = Run().WithParameter("q", dataToSearch);
            if (parameters != null)
                cmd = parameters.ProcessCommand(cmd);
            return cmd.Execute().To<SearchResult>(CheckForError);
        }

        private static JsonValue CheckForError(JsonValue arg)
        {
            if (arg == null || !arg.ContainsKey("error"))
                return arg;
            var e = arg["error"];
            throw new ResultException(e["message"].ReadAs<string>(), e["code"].ReadAs<int>(), e);
        }
    }
}

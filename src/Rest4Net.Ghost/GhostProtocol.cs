using System;
using System.Net;
using System.Text;
using Rest4Net.Ghost.Exceptions;
using Rest4Net.Protocols;

namespace Rest4Net.Ghost
{
    internal class GhostProtocol : HttpBaseProtocol
    {
        private string _baseUri;
        private string _relativePath;
        private string _token;
        private string _cookiePath;
        private Cookie _cookie;
        private readonly string _host;

        public GhostProtocol(string host, int port = -1, bool useHttps = false) : base(host, port, useHttps)
        {
            _host = host;
        }

        public string Login { get; set; }

        public string Password { get; set; }

        internal GhostProtocol BuildVariables(string urlPath, bool useHttps, string domain, int port)
        {
            _relativePath = (urlPath ?? "/").Trim('/');
            var sb = new StringBuilder(150);
            sb.Append("http");
            if (useHttps)
                sb.Append("s");
            sb.Append("://").Append(domain);
            if (port > 0)
                sb.Append(':').Append(port);
            sb.Append('/').Append(_relativePath);
            if (sb[sb.Length - 1] == '/')
                sb.Remove(sb.Length - 1, 1);
            _baseUri = sb.ToString();
            _cookiePath = new Uri(GetUrl("/ghost/")).AbsolutePath;
            return this;
        }

        private string GetUrl(string relative)
        {
            return _baseUri + relative;
        }

        private void PerformLogin()
        {
            try
            {
                using (var client = new GhostWebClient())
                {
                    client.Login(GetUrl("/ghost/signin/"), GetUrl("/ghost/"), Login, Password);
                    _token = client.Tag;
                    _cookie = client.ConnSid;
                    if (_cookie != null)
                        _cookie.Path = _cookiePath;
                }
            }
            catch (WebException exception)
            {
                if ((int)((HttpWebResponse)exception.Response).StatusCode == 403)
                    throw new GhostWrongUsernamePasswordException();
                throw;
            }
        }

        protected override HttpWebRequest RequestBeforeBodySend(HttpWebRequest request)
        {
            PerformLogin();
            request.Referer = GetUrl(@"/ghost/");
            request.UserAgent = @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.9; rv:30.0) Gecko/20100101 Firefox/30.0";
            if (request.Headers == null)
                request.Headers = new WebHeaderCollection();
            request.Headers.Add(@"X-Requested-With", @"XMLHttpRequest");
            if (!String.IsNullOrEmpty(_token))
                request.Headers.Add(@"X-CSRF-Token", _token);
            if (_cookie != null)
            {
                if (request.CookieContainer == null)
                    request.CookieContainer = new CookieContainer();
                _cookie.Path = request.RequestUri.AbsolutePath;
                request.CookieContainer.Add(_cookie);
            }
            return base.RequestBeforeBodySend(request);
        }
    }
}

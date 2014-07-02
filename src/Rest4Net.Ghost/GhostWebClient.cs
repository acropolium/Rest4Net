using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Rest4Net.Ghost
{
    internal class GhostWebClient : WebClient
    {
        private readonly IDictionary<string, Cookie> _cookies = new Dictionary<string, Cookie>();

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address) as HttpWebRequest;
            if (request == null)
                return null;
            if (!String.IsNullOrEmpty(Tag))
            {
                if (request.Headers == null)
                    request.Headers = new WebHeaderCollection();
                request.Headers.Add("X-CSRF-Token", Tag);
            }
            request.CookieContainer = new CookieContainer();
            foreach (var c in _cookies.Values)
                request.CookieContainer.Add(c);
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            if (response != null)
                FindDomainCookies(response);
            return response;
        }

        private void FindDomainCookies(WebResponse response)
        {
            var headers = response.Headers;
            if (headers == null)
                return;
            for (var i = 0; i < headers.Count; i++)
            {
                if ("Set-Cookie" != headers.Keys[i]) continue;
                var rawCookie = headers[i];
                var cookie = new Cookie {Path = "/", Domain = response.ResponseUri.Host};
                var cookieValues = rawCookie.Split(new[] {';'});
                foreach (var param in cookieValues)
                {
                    try
                    {
                        var paramNameVale = param.Trim().Split(new[] {'='});
                        paramNameVale[0] = paramNameVale[0].ToLower();
                        switch (paramNameVale[0])
                        {
                            case "domain":
                                cookie.Domain = param.Split(new[] {'='})[1];
                                break;
                            case "path":
                                cookie.Path = paramNameVale[1];
                                break;
                            case "expires":
                                cookie.Expires = DateTime.Parse(paramNameVale[1]);
                                break;
                            default:
                                if (paramNameVale.Length == 2)
                                {
                                    cookie.Name = paramNameVale[0];
                                    cookie.Value = paramNameVale[1];
                                }
                                else
                                {
                                    if (paramNameVale[0] == "HttpOnly")
                                        cookie.HttpOnly = true;
                                }
                                break;
                        }
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch { }
                }
                _cookies[cookie.Name] = cookie;
            }
        }

        private string MakeReqest(string url, string parameters = "", bool isGetRequest = true)
        {
            if (isGetRequest)
                return Encoding.UTF8.GetString(DownloadData(url + "?" + parameters));
            UploadString(url, "POST", parameters);
            return null;
        }

        private readonly Regex _metaTagRegEx = new Regex("<meta name=\"(.+?)\" content=\"(.+?)\" />");

        private void GetCsrfParamMetaValue(string html)
        {
            foreach (Match match in _metaTagRegEx.Matches(html))
            {
                if (match.Groups[1].Value != "csrf-param")
                    continue;
                Tag = match.Groups[2].Value;
                break;
            }
        }

        public void UpdateToken(string url)
        {
            GetCsrfParamMetaValue(MakeReqest(url));
        }

        private static string UrlEncode(string data)
        {
            return Uri.EscapeDataString(data);
        }

        public void Login(string url, string normalUrl, string login, string password)
        {
            UpdateToken(url);
            MakeReqest(url,
                String.Format("email={0}&password={1}", UrlEncode(login), UrlEncode(password)),
                false);
            UpdateToken(normalUrl);
        }

        public string Tag { get; private set; }

        public Cookie ConnSid
        {
            get
            {
                foreach (var x in _cookies.Values)
                {
                    if (x.Name == "connect.sid")
                        return x;
                }
                return null;
            }
        }
    }
}

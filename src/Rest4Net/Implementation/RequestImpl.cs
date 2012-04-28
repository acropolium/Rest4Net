using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace Rest4Net.Implementation
{
    internal class RequestImpl : IRequest
    {
        private string _path = "/";
        private string _method = "GET";
        private string _constantContentType;

        private readonly string _host;
        private readonly bool _useSsl;
        private readonly int _port;

        private readonly IList<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
        private readonly IList<KeyValuePair<string, string>> _parameters = new List<KeyValuePair<string, string>>();

        public RequestImpl(string host, bool useSsl, int port)
        {
            _host = host;
            _useSsl = useSsl;
            _port = port;
        }

        public IRequest AlwaysUseContentType(string contentType)
        {
            _constantContentType = contentType;
            return this;
        }

        public IRequest SetMethod(RequestType requestType)
        {
            _method = requestType.ToString().ToUpper();
            return this;
        }

        public IRequest SetPath(string path)
        {
            _path = String.IsNullOrEmpty(path) ? @"/" : path;
            if (!_path.StartsWith("/"))
                _path = "/" + _path;
            return this;
        }

        public IRequest AddHeader(string key, string value)
        {
            _headers.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public IRequest AddQueryParam(string key, string value)
        {
            _parameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public IRequest AddQueryParams(object parameters)
        {
            AddQueryParams(
                parameters.GetType().GetFields().Select(
                    pi => new KeyValuePair<string, string>(pi.Name, pi.GetValue(parameters).ToString())));
            AddQueryParams(
                parameters.GetType().GetProperties().Select(
                    pi => new KeyValuePair<string, string>(pi.Name, pi.GetValue(parameters, null).ToString())));
            return this;
        }

        public IRequest AddQueryParams(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            foreach (var keyValuePair in parameters)
            {
                AddQueryParam(keyValuePair.Key, keyValuePair.Value);
            }
            return this;
        }

        public IRequest AddQueryParams(Expression<Func<object, string>> parameters)
        {
            var objValue = ((ConstantExpression) (((MemberExpression) (parameters.Body)).Expression)).Value;
            var value = objValue == null ? null : objValue.GetType().GetFields()[0].GetValue(objValue).ToString();
            AddQueryParam(parameters.Parameters[0].Name, value);
            return this;
        }

        public event Request OnRequest;
        public event Response OnResponse;
        public event Parsing OnParsing;

        private string ParamsToUri()
        {
            var sb = new StringBuilder();
            foreach (var pair in _parameters)
            {
                if (sb.Length > 0)
                    sb.Append('&');
                sb.AppendFormat("{0}={1}", pair.Key, Uri.EscapeDataString(pair.Value));
            }
            return sb.ToString();
        }

        private Uri Url(bool passParamsInBody)
        {
            var sb = new StringBuilder(_useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);
            sb.Append("://");
            sb.Append(_host);
            if (_port > 0)
                sb.AppendFormat(":{0}", _port);
            sb.Append(_path);
            if (!passParamsInBody)
            {
                var parameters = ParamsToUri();
                if (!String.IsNullOrEmpty(parameters))
                {
                    sb.Append('?');
                    sb.Append(parameters);
                }
            }
            return new Uri(sb.ToString());
        }

        public IResponse Run(bool passParamsInBody)
        {
            if (!passParamsInBody)
                throw new InvalidOperationException("Can't fire this method if argument is false");
            if (String.Compare(_method, @"POST") != 0)
                throw new InvalidOperationException("Method must be 'POST' for this option");
            return Run(Url(true), Encoding.UTF8.GetBytes(ParamsToUri()));
        }

        public IResponse Run(string content)
        {
            return Run(Encoding.UTF8.GetBytes(content));
        }

        public IResponse Run(byte[] content = null)
        {
            return Run(Url(false), content);
        }

        private IResponse Run(Uri uri, byte[] content)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = _method;
                foreach (var pair in _headers)
                    request.Headers[pair.Key] = pair.Value;

                if (OnRequest != null)
                    OnRequest(request);

                if (content != null)
                    request.GetRequestStream().Write(content, 0, content.Length);

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var resp = (IResponse)(new ResponseImpl(response));
                    if (OnResponse != null)
                        OnResponse(this, response, ref resp);
                    return resp;
                }
            }
            catch (WebException exception)
            {
                return new ResponseImpl((HttpWebResponse)exception.Response, exception);
            }
        }

        private TSerializableResult PerformParsing<TSerializableResult>(IResponse response)
            where TSerializableResult : class, new()
        {
            if (OnParsing != null)
            {
                var cancel = false;
                OnParsing(response, Parse, ref cancel);
                if (cancel)
                    return null;
            }
            return Parse<TSerializableResult>(response);
        }

        public TSerializableResult Run<TSerializableResult>(bool passParamsInBody)
            where TSerializableResult : class, new()
        {
            return PerformParsing<TSerializableResult>(Run(passParamsInBody));
        }

        public TSerializableResult Run<TSerializableResult>(string content)
            where TSerializableResult : class, new()
        {
            return PerformParsing<TSerializableResult>(Run(content));
        }

        public TSerializableResult Run<TSerializableResult>(byte[] content = null)
            where TSerializableResult : class, new()
        {
            return PerformParsing<TSerializableResult>(Run(content));
        }

        private readonly IDictionary<string, Type> _parsers = new Dictionary<string, Type>();
        public IRequest MapParser<TParserType>(string contentType, params string[] additionalContentTypes) where TParserType : IRestSerializer
        {
            _parsers[contentType] = typeof(TParserType);
            if (additionalContentTypes == null || additionalContentTypes.Length == 0)
                return this;
            foreach (var type in additionalContentTypes)
                _parsers[type] = typeof(TParserType);
            return this;
        }

        private TSerializableResult Parse<TSerializableResult>(IResponse response)
            where TSerializableResult : class, new()
        {
            return (TSerializableResult) (Parse(response, typeof (TSerializableResult)));
        }

        private object Parse(IResponse response, Type resultType)
        {
            var item = Activator.CreateInstance(resultType);
            var ct = (_constantContentType ?? response.ContentType).ToLower();
            var key = _parsers.Keys.FirstOrDefault(k => ct.Contains(k.ToLower()));
            if (String.IsNullOrEmpty(key))
                return item;
            var attr =
                (resultType.GetCustomAttributes(typeof(RestApiSerializableAttribute), false).
                     FirstOrDefault() as RestApiSerializableAttribute) ?? new RestApiSerializableAttribute();
            return ((IRestSerializer) (Activator.CreateInstance(_parsers[key], attr))).Deserialize(item, response.Content);
        }

        public void Dispose()
        {
            if (OnRequest != null)
                foreach (Request @delegate in OnRequest.GetInvocationList())
                    OnRequest -= @delegate;
            if (OnResponse != null)
                foreach (Response @delegate in OnResponse.GetInvocationList())
                    OnResponse -= @delegate;
            if (OnParsing != null)
                foreach (Parsing @delegate in OnParsing.GetInvocationList())
                    OnParsing -= @delegate;
        }
    }
}

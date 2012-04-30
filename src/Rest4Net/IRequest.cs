using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;

namespace Rest4Net
{
    public delegate void Request(HttpWebRequest request);
    public delegate void Response(IRequest request, HttpWebResponse webResponse, ref IResponse response);
    public delegate object Parser(IResponse response, Type resultType);
    public delegate void Parsing(IResponse response, Parser parser, ref bool cancelParsing);

    public interface IRequest : IDisposable
    {
        IRequest AlwaysUseContentType(string contentType);
        IRequest SetMethod(RequestType requestType);
        IRequest SetPath(string path);
        IRequest AddHeader(string key, string value);
        IRequest AddQueryParam(string key, string value);
        IRequest AddQueryParams(object parameters);
        IRequest AddQueryParams(IEnumerable<KeyValuePair<string, string>> parameters);
        IRequest AddQueryParams(Expression<Func<dynamic, string>> parameters);
        IResponse Run(bool passParamsInBody);
        IResponse Run(string content);
        IResponse Run(byte[] content = null);

        event Request OnRequest;
        event Response OnResponse;
        event Parsing OnParsing;

        #region Generic response
        TSerializableResult Run<TSerializableResult>(bool passParamsInBody) where TSerializableResult : class, new();
        TSerializableResult Run<TSerializableResult>(string content) where TSerializableResult : class, new();
        TSerializableResult Run<TSerializableResult>(byte[] content = null) where TSerializableResult : class, new();

        IRequest MapParser<TParserType>(string contentType, params string[] additionalContentTypes) where TParserType : IRestSerializer;
        #endregion Generic response
    }
}

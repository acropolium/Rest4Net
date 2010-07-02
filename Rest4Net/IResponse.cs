using System;
using System.Collections.Generic;
using System.Net;

namespace Rest4Net
{
    public interface IResponse
    {
        HttpStatusCode Code { get; }
        string CodeDescription { get; }
        string ContentType { get; }
        long ContentLength { get; }
        byte[] Content { get; }
        IDictionary<string, string> Headers { get; }
        Exception Exception { get; }
    }
}

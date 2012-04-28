using System;
using System.Collections.Generic;
using System.Net;

namespace Rest4Net
{
    public interface IResponse
    {
        HttpStatusCode Code { get; set; }
        string CodeDescription { get; set; }
        string ContentType { get; set; }
        long ContentLength { get; set; }
        byte[] Content { get; set; }
        IDictionary<string, string> Headers { get; set; }
        Exception Exception { get; set; }
    }
}

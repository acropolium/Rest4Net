using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Rest4Net.Implementation
{
    public class ResponseImpl : IResponse
    {
        public HttpStatusCode Code { get; private set; }
        public string CodeDescription { get; private set; }

        public string ContentType { get; private set; }

        public long ContentLength { get; private set; }
        public byte[] Content { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public Exception Exception { get; private set; }

        public ResponseImpl(Exception exception)
        {
            Exception = exception;
        }

        public ResponseImpl(HttpWebResponse response)
        {
            Code = response.StatusCode;
            CodeDescription = response.StatusDescription;

            ContentType = response.ContentType;

            ContentLength = response.ContentLength;
            Content = ContentLength > 0 ? ReadFully(response.GetResponseStream()) : null;

            Headers = new Dictionary<string, string>();
            foreach (var headerKey in response.Headers.AllKeys)
                Headers.Add(headerKey, response.Headers.Get(headerKey));
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public override string ToString()
        {
            return (Content != null) ? Encoding.UTF8.GetString(Content) : String.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Rest4Net.Implementation
{
    public class ResponseImpl : IResponse
    {
        public HttpStatusCode Code { get; set; }
        public string CodeDescription { get; set; }

        public string ContentType { get; set; }

        public long ContentLength { get; set; }
        public byte[] Content { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public Exception Exception { get; set; }

        public ResponseImpl(HttpWebResponse response, Exception exception = null)
        {
            if (response != null)
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
            Exception = exception;
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

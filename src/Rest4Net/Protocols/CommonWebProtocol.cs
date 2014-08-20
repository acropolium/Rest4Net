using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Rest4Net.Protocols
{
	public abstract class CommonWebProtocol : BaseProtocol
	{
		protected CommonWebProtocol(string host, int port) : base(host, port) { }
		protected abstract string Scheeme { get; }

		private static string ParamsToUri(Command cmd)
		{
			var sb = new StringBuilder();
			foreach (var pair in cmd.Parameters)
			{
				if (sb.Length > 0)
					sb.Append('&');
				sb.AppendFormat("{0}={1}", pair.Key, Uri.EscapeDataString(pair.Value));
			}
			return sb.ToString();
		}

		private Uri CreateUri(Command cmd)
		{
			var sb = new StringBuilder(Scheeme);
			sb.Append("://");
			sb.Append(Host);
			if (Port != DefaultPort)
				sb.AppendFormat(":{0}", Port);
			sb.Append(cmd.Path);
			var parameters = ParamsToUri(cmd);
			if (!String.IsNullOrEmpty(parameters))
			{
				sb.Append('?');
				sb.Append(parameters);
			}
			return new Uri(sb.ToString());
		}

		private static HttpWebRequest CreateRequest(Uri uri, Command cmd)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = cmd.Type.ToString().ToUpper();
			foreach (var pair in cmd.Headers)
				request.Headers[pair.Key] = pair.Value;
			request.ContentType = "application/json";
			return request;
		}

        protected virtual HttpWebRequest RequestBeforeBodySend(HttpWebRequest request)
        {
            return request;
        }

	    private class SyncCallHelper
	    {
	        private ManualResetEvent _done1Event = new ManualResetEvent(false);
	        private ManualResetEvent _done2Event = new ManualResetEvent(false);
            private readonly HttpWebRequest _request;
            private readonly StreamWriter _streamWriter;

	        public delegate void StreamWriter(Stream s);

	        public SyncCallHelper(HttpWebRequest request, StreamWriter s)
	        {
	            _request = request;
	            _streamWriter = s;
	        }

	        public void WriteBody()
	        {
                if (_streamWriter == null)
                    return;
	            try
	            {
                    _request.BeginGetRequestStream(RequestCallback, _request);
                    _done1Event.WaitOne();
	            }
	            catch (Exception e)
	            {
                    LastException = e;
	            }
	        }

            private void RequestCallback(IAsyncResult asynchronousResult)
	        {
                try
                {
                    var request = (HttpWebRequest) asynchronousResult.AsyncState;
                    var postStream = request.EndGetRequestStream(asynchronousResult);
                    _streamWriter(postStream);
                    postStream.Dispose();
                }
                catch (Exception e)
                {
                    LastException = e;
                }
                finally
                {
                    _done1Event.Set();
                }
	        }

	        private HttpWebResponse _response;

            public Exception LastException { get; private set; }

	        public HttpWebResponse ReadResponse()
	        {
	            try
	            {
                    _request.BeginGetResponse(GetResponseCallback, _request);
                    _done2Event.WaitOne();
	            }
	            catch (Exception e)
	            {
                    LastException = e;
	            }
                return _response;
            }

            private void GetResponseCallback(IAsyncResult asynchronousResult)
	        {
                try
                {
                    var request = (HttpWebRequest) asynchronousResult.AsyncState;
                    _response = (HttpWebResponse) request.EndGetResponse(asynchronousResult);
                }
                catch (Exception e)
                {
                    LastException = e;
                }
                finally
                {
                    _done2Event.Set();
                }
	        }
	    }

		public override CommandResult Execute(Command command)
		{
			var uri = CreateUri(command);
			try
			{
                var request = RequestBeforeBodySend(CreateRequest(uri, command));
			    var sch = new SyncCallHelper(request, (command.BodyProvider != null) ? command.BodyProvider.Provide : (SyncCallHelper.StreamWriter) null);
                sch.WriteBody();
			    if (sch.LastException != null)
			        throw sch.LastException;
			    var rsp = sch.ReadResponse();
                if (sch.LastException != null)
                    throw sch.LastException;
			    return ToResult(rsp);
			}
			catch (WebException exception)
			{
				var r = (HttpWebResponse)exception.Response;
				if (r == null)
					throw new Exceptions.ConnectionException(uri.ToString(), exception);
				return ToResult(r);
			}
			catch (Exception ex)
			{
				throw new Exceptions.ConnectionException(uri.ToString(), ex);
			}
		}

		private static CommandResult ToResult(HttpWebResponse response)
		{
			var result = new CommandResult(response.GetResponseStream())
				.WithMeta("code", ((int)response.StatusCode).ToString(CultureInfo.InvariantCulture))
				.WithMeta("description", response.StatusDescription)
				.WithMeta("contentType", response.ContentType);
			foreach (var headerKey in response.Headers.AllKeys)
			{
				result.WithMeta("header:" + headerKey, response.Headers[headerKey]);
			}
			foreach (Cookie c in response.Cookies)
			{
				result.WithMeta("cookie:" + c.Name, c.ToString());
			}
			return result;
		}
	}
}
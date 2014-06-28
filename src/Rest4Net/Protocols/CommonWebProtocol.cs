using System;
using System.Globalization;
using System.Net;
using System.Text;

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

		public override CommandResult Execute(Command command)
		{
			var uri = CreateUri(command);
			try
			{
                var request = RequestBeforeBodySend(CreateRequest(uri, command));

				if (command.BodyProvider != null)
					command.BodyProvider.Provide(request.GetRequestStream());

				return ToResult((HttpWebResponse)request.GetResponse());
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
using System;

namespace Rest4Net.Exceptions
{
	public class ConnectionException : Rest4NetException
	{
		public ConnectionException (string uri, Exception innerException)
			: base("Connection failed to `"+uri+"`", innerException)
		{
		}
	}
}


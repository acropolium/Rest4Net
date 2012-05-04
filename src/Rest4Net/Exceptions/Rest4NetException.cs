using System;

namespace Rest4Net.Exceptions
{
	public class Rest4NetException : Exception {
		public Rest4NetException(string message, Exception innerException) : base(message, innerException) { }
	}
}


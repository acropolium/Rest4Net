using System.Globalization;
using System.Json;

namespace Rest4Net.Exceptions
{
	public class ResultException : Rest4NetException
	{
		public ResultException(string message, int code = 200, JsonValue response = null)
			: base(code.ToString(CultureInfo.InvariantCulture) + ": " + message, null)
		{
		    ResponseJson = response;
		}

		public JsonValue ResponseJson { get; private set; }
	}
}


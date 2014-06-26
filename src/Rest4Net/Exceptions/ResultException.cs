using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Rest4Net.Exceptions
{
	public class ResultException : Rest4NetException
	{
		public ResultException(string message, int code = 200, JToken response = null)
			: base(code.ToString(CultureInfo.InvariantCulture) + ": " + message, null)
		{
		    ResponseJson = response;
		}

        public JToken ResponseJson { get; private set; }
	}
}


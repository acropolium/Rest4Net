using System.Text;

namespace Rest4Net.IronMq
{
    public class Message : IMessage
    {
        private string id;
        private string body;
        private int timeout;
        private int delay;
        private int expires_in;

        public Message()
            :this(null)
        { }

        public Message(string body, int timeout = 60, int delay = 0, int expiresIn = 604800)
        {
            this.body = body;
            this.timeout = timeout;
            this.delay = delay;
            this.expires_in = expiresIn;
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public int ExpiresIn
        {
            get { return expires_in; }
            set { expires_in = value; }
        }

        private string PrepareJson(string str)
        {
            return str.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        internal string ToJson()
        {
            var sb = new StringBuilder();
            sb.Append('{');

            sb.Append('"').Append("body").Append('"').Append(':').Append('"').Append(PrepareJson(body)).Append('"').Append(',');
            sb.Append('"').Append("timeout").Append('"').Append(':').Append(timeout).Append(',');
            sb.Append('"').Append("delay").Append('"').Append(':').Append(delay).Append(',');
            sb.Append('"').Append("expires_in").Append('"').Append(':').Append(expires_in);

            sb.Append('}');
            return sb.ToString();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} ({2}, {3}, {4})", ID ?? "null", Body ?? "null", Timeout, Delay, ExpiresIn);
        }
    }
}

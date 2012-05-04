using System;

namespace Rest4Net.Protocols
{
    public abstract class BaseProtocol
    {
        protected BaseProtocol(string host, int port = -1)
        {
            WithHost(host).WithPort(port);
        }

        protected string Host { get; private set; }
        protected int Port { get; private set; }

        protected abstract int DefaultPort { get; }

        public BaseProtocol WithHost(string host)
        {
            if (String.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException("host");
            Host = host;
            return this;
        }

        public BaseProtocol WithPort(int port = -1)
        {
            Port = (port < 1) ? DefaultPort : port;
            return this;
        }

        public abstract CommandResult Execute(Command command);
    }
}

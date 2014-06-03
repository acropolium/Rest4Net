using Rest4Net.Protocols;

namespace Rest4Net.ePochta.Utils
{
    internal class AtomParkProtocolHttp : Http
    {
        public AtomParkProtocolHttp(string host, int port = -1)
            : base(host, port)
        {
        }

        public override CommandResult Execute(Command command)
        {
            return command.PrepareRequestAndExecute(PrivateKey, base.Execute);
        }

        internal string PrivateKey { private get; set; }
    }
}

﻿using Rest4Net.Protocols;

namespace Rest4Net.ePochta.Utils
{
    internal class AtomParkProtocolHttps : Https
    {
        public AtomParkProtocolHttps(string host, int port = -1)
            : base(host, port)
        {
        }

        public override CommandResult Execute(Command command)
        {
            return CommandUtils.PrepareRequestAndExecute(command, PrivateKey, base.Execute);
        }

        internal string PrivateKey { private get; set; }
    }
}

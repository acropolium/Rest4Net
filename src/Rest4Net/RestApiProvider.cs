using Rest4Net.Protocols;

namespace Rest4Net
{
    public abstract class RestApiProvider
    {
        private readonly BaseProtocol _protocol;

        protected RestApiProvider(BaseProtocol protocol)
        {
            _protocol = protocol;
        }

        protected virtual Command Cmd(string path, RequestType requestType = RequestType.Get)
        {
            return Command.Create(path, requestType, this);
        }

        internal CommandResult Execute(Command cmd)
        {
            return _protocol.Execute(cmd);
        }
    }
}

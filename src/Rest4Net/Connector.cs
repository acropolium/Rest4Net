using System;
using Rest4Net.Implementation;
using Rest4Net.Parsers;

namespace Rest4Net
{
    public abstract class Connector : IDisposable
    {
        protected string Host
        {
            get; private set;
        }

        protected bool UseSsl
        {
            get; private set;
        }

        protected int Port
        {
            get; private set;
        }

        protected Connector(string host, bool useSsl = false, int port = 0)
        {
            Host = host;
            UseSsl = useSsl;
            Port = port;
        }

        protected delegate void RequestCreation(IRequest request);

        protected RequestCreation OnRequestCreation;

        protected event Request OnRequest;
        protected event Response OnResponse;

        private TRequestImplementation ModifyRequest<TRequestImplementation>(TRequestImplementation request)
            where TRequestImplementation : IRequest
        {
            if (OnRequestCreation != null)
                OnRequestCreation(request);
            if (OnRequest != null)
                request.OnRequest += OnRequest;
            if (OnResponse != null)
                request.OnResponse += OnResponse;
            return request;
        }

        protected virtual IRequest Build
        {
            get
            {
                return ModifyRequest(new RequestImpl(Host, UseSsl, Port))
                    .MapParser<ParserXml>("application/xml", "text/xml")
                    .MapParser<ParserJson>("application/json", "text/json", "text/javascript");
            }
        }

        public virtual void Dispose()
        {
            if (OnRequest != null)
                foreach (Request @delegate in OnRequest.GetInvocationList())
                    OnRequest -= @delegate;
            if (OnResponse != null)
                foreach (Response @delegate in OnResponse.GetInvocationList())
                    OnResponse -= @delegate;
        }
    }
}

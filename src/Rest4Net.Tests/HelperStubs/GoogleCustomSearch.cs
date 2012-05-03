using System;
using Rest4Net.Protocols;

namespace Rest4Net.Tests.HelperStubs
{
	internal class GoogleCustomSearch : RestServiceAccessProvider
    {
        public GoogleCustomSearch() : base(new Https("www.googleapis.com")) {}

        public CommandResult Get()
        {
            return Cmd("/customsearch/v1")
                .WithParameter("cx", "017576662512468239146:omuauf_lfve")
                .WithParameter("q", "lectures")
                .Execute();
        }
    }
}


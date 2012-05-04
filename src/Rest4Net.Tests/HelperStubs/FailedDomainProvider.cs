using Rest4Net.Protocols;

namespace Rest4Net.Tests.HelperStubs
{
	internal class FailedDomainProvider : RestApiProvider
	{
		public FailedDomainProvider() : base(new Https("www.some-nonono-domain.com")) {}

		public CommandResult Get()
		{
			return Cmd("/v1")
				.WithParameter("q", "lectures")
				.Execute();
		}
	}
}


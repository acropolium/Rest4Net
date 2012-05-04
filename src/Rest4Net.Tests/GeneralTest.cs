using NUnit.Framework;
using Rest4Net.Tests.HelperStubs;

namespace Rest4Net.Tests
{
	[TestFixture]
	public class GeneralTest
	{
		[Test(Description = "Failed Connection Test")]
		[ExpectedException(typeof(Exceptions.ConnectionException))]
		public void FailedConnection()
		{
			using (var p = new FailedDomainProvider())
				p.Get ();
		}
	}
}

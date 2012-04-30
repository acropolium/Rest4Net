using NUnit.Framework;
using Rest4Net.Protocols;
using Rest4Net.Tests.HelperStubs;

namespace Rest4Net.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string ApiKey = "";
        #endregion

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
			var p = new GoogleCustomSearch();
            using (var s = p.Get())
            {
                var o = s.ToObject();
				if (o == null)
					throw new System.ArgumentNullException();
            }
        }

        [Test(Description = "Failed Connection Test")]
		[ExpectedException(typeof(Rest4Net.Exceptions.ConnectionException))]
        public void FailedConnection()
        {
            var p = new FailedDomainProvider();
            p.Get ();
        }
    }
}

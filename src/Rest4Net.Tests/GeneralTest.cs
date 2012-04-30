using NUnit.Framework;
using Rest4Net.Protocols;

namespace Rest4Net.Tests
{
    public class GoogleCustomSearch : RestServiceAccessProvider
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

    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string ApiKey = "";
        #endregion

        private GoogleCustomSearch _client;

        [SetUp]
        public void Init()
        {
            _client = new GoogleCustomSearch();
        }

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            using (var s = _client.Get())
            {
                var d = s.ToJson();
            }
        }
    }
}

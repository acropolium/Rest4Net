using System.Linq;
using NUnit.Framework;

namespace Rest4Net.BitLy.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string Login = "";
        private const string ApiKey = "";
        private const string WrongPassword = "";
        #endregion

        private BitLyProvider _client;

        [SetUp]
        public void Init()
        {
            _client = new BitLyProvider(Login, ApiKey);
        }

        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
        }

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            var shorten = _client.Shorten("http://google.com.ua/").Data;
            Assert.NotNull(shorten);
            var expanded1 = _client.Expand(shorten.Url);
            Assert.True(expanded1.Data.Expand.Any());
            var expanded2 = _client.ExpandWithHashes(shorten.Hash);
            Assert.True(expanded2.Data.Expand.Any());
            Assert.True(_client.Validate(Login, ApiKey).Data.Valid);
            Assert.True(_client.IsBitlyProDomain("nyti.ms").Data.BitlyProDomain);
            Assert.Null(_client.Authenticate(Login, WrongPassword).Data);

            // Simple sample
            var info = _client.Info("http://google.com.ua/");
            var lookup = _client.Lookup("http://betaworks.com/");
        }
    }
}

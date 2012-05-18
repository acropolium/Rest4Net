using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Rest4Net.IronCache.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string Token = "1_Zyel0wcox-naWO_5B9IuxtyGc";
        private const string ProjectId = "4fb6434d68a01974c8001780";
        #endregion

        private IronCacheProvider _client;
        private string _cacheName;

        [SetUp]
        public void Init()
        {
            _client = new IronCacheProvider(Token, ProjectId);
            _cacheName = 10.RandomString();
        }

        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
        }

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            Assert.True(_client.Put(_cacheName, "k1", "v1"));
            Assert.True(_client.Put(_cacheName, "k1", "v1a", replace:true));
            Assert.False(_client.Put(_cacheName, "k2", "v2", replace:true));
            Assert.False(_client.Put(_cacheName, "k1", "v1b", add:true));
            Assert.True(_client.Put(_cacheName, "k5", 6));
            Assert.AreEqual(_client.Increment(_cacheName, "k5", 7), 13);
            Assert.AreEqual(_client.Increment(_cacheName, "k5", -7), 6);
            Assert.AreEqual(_client.Get(_cacheName, "k1"), "v1a");
            
            Assert.True(_client.Delete(_cacheName, "k1"));
            //Assert.False(_client.Delete(_cacheName, "k2")); API spec wrong here, always return true
            Assert.True(_client.Delete(_cacheName, "k5"));
            //Assert.False(_client.Delete(_cacheName, "k17")); API problem here

            Assert.True(_client.Caches().Any(x => x.Name == _cacheName));
        }
    }
}

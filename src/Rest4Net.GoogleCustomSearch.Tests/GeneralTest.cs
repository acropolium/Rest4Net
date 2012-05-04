using NUnit.Framework;

namespace Rest4Net.GoogleCustomSearch.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string Key = "";
        private const string Cx = "";
        private const string SearchPhrase = "";
        #endregion

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            using (var client = new GoogleCustomSearchProvider(Key, Cx))
            {
                var q = client.Search(SearchPhrase, new SearchParameters { Start = 1 });
                Assert.Null(q.Queries.PreviousPage);
            }
        }
    }
}
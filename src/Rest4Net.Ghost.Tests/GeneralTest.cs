using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace Rest4Net.Ghost.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        private readonly dynamic _a = new ExpandoObject();

        [TestFixtureSetUp]
        public void LoadConfigurationValuesAndConstants()
        {
            var configPath = Path.GetFullPath("./../../Assets/Config.xml");
            if (!File.Exists(configPath))
                throw new FileNotFoundException("Test configuration file not found. Prepare it from _Config.xml", configPath);
            var w = (IDictionary<string, object>)_a;
            var doc = XDocument.Load(configPath);
            foreach (var item in doc.Descendants("Item"))
                w[item.Attribute("name").Value] = item.Attribute("value").Value;
        }

        private GhostProvider CreateProvider()
        {
            return new GhostProvider(_a.Domain, _a.Login, _a.Password, true);
        }

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            using (var client = CreateProvider())
            {
                var tags = client.GetTags();
                var posts = client.GetPosts(1, 1000);
                var items = posts.Items.ToArray();
                foreach (var item in items)
                {
                    if (item.HasTag("CustomTag"))
                        client.DeletePost(item.Id);
                }
                //var s = client.GetPosts();
                //var items = s.Items.ToArray();
                //var itemD = items.First().CreatedAt;
                //var p = client.GetPost(items.First().Id);
                //var r = client.DeletePost(items.Last().Id+1000);
                //var slug = client.GenerateSlug("Some title to be made");
                //var post = items.Single(x => x.Id == 121);
                //post.Markdown = "New text";
                //post = client.SavePost(post);

                for (var i = 0; i < 10; i++)
                {
                    client.SavePost(client.CreatePost("NewTitle" + i, "NewText" + i).WithTag("CustomTag"));
                }
            }
        }
    }
}
